using Microsoft.Extensions.Logging;
using Sporadic.Abp.Identity.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public class OrganizationUnitManager : DomainService
    {
        public OrganizationUnitManager(
            IOrganizationUnitRepository organizationUnitRepository,
            IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache)
        {
            OrganizationUnitRepository = organizationUnitRepository;
            DynamicClaimCache = dynamicClaimCache;
        }

        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
        protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; }



        [UnitOfWork]
        public virtual async Task CreateAsync(OrganizationUnit organizationUnit)
        {
            organizationUnit.Code = await GetNextChildCodeAsync(organizationUnit.ParentId);
          
            await ValidateOrganizationUnitAsync(organizationUnit);
           
            await OrganizationUnitRepository.InsertAsync(organizationUnit);
        }

        public virtual async Task UpdateAsync(OrganizationUnit organizationUnit)
        {
            await OrganizationUnitRepository.UpdateAsync(organizationUnit);

            await RemoveDynamicClaimCacheAsync(organizationUnit);
        }

        public virtual async Task SetNameAsync(OrganizationUnit organizationUnit , string name)
        {
            organizationUnit.Name = name;

            await ValidateOrganizationUnitAsync(organizationUnit);
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await RemoveDynamicClaimCacheAsync(child);
                await OrganizationUnitRepository.RemoveAllMembersAsync(child);
                await OrganizationUnitRepository.DeleteAsync(child);
            }

            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

            await RemoveDynamicClaimCacheAsync(organizationUnit);
            await OrganizationUnitRepository.RemoveAllMembersAsync(organizationUnit);
            await OrganizationUnitRepository.DeleteAsync(id);
        }

        public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null)
            {
                return OrganizationUnit.CalculateNextCode(lastChild.Code);
            }

            var parentCode = parentId != null
                ? await GetCodeOrDefaultAsync(parentId.Value)
                : null;

            return OrganizationUnit.AppendCode(
                parentCode,
                OrganizationUnit.CreateCode(1)
            );
        }

        public virtual async Task<OrganizationUnit> GetLastChildOrNullAsync(Guid? parentId)
        {
            var children = await OrganizationUnitRepository.GetChildrenAsync(parentId);

            return children.OrderBy(c => c.Code).LastOrDefault();
        }


        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var ou = await OrganizationUnitRepository.FindAsync(id);
            return ou?.Code;
        }

        protected virtual async Task ValidateOrganizationUnitAsync(OrganizationUnit organizationUnit)
        {
            var siblings = (await FindChildrenAsync(organizationUnit.ParentId))
                .Where(ou => ou.Id != organizationUnit.Id)
                .ToList();

            if (siblings.Any(ou => ou.Name == organizationUnit.Name))
            {
                throw new BusinessException("已存在名为 {0} 的机构. 无法在同一级别创建相同名称。")
                    .WithData("0", organizationUnit.Name);
            }
        }

        /// <summary>
        /// 获取所有子机构
        /// </summary>
        /// <param name="parentId">父机构id</param>
        /// <param name="recursive">是否递归子机构</param>
        /// <returns></returns>
        public async Task<List<OrganizationUnit>> FindChildrenAsync(Guid? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await OrganizationUnitRepository.GetChildrenAsync(parentId);
            }

            if (!parentId.HasValue)
            {
                return await OrganizationUnitRepository.GetListAsync();
            }

            var code = await GetCodeOrDefaultAsync(parentId.Value);

            return await OrganizationUnitRepository.GetAllChildrenWithParentCodeAsync(code, parentId);
        }

        public virtual async Task RemoveDynamicClaimCacheAsync(OrganizationUnit organizationUnit)
        {
            Logger.LogDebug($"Remove dynamic claims cache for users of organization: {organizationUnit.Id}");
            var userIds = await OrganizationUnitRepository.GetUserIdsAsync(organizationUnit.Id);
            await DynamicClaimCache.RemoveManyAsync(userIds.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, null)));
        }
    }
}
