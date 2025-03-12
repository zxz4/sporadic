using Microsoft.EntityFrameworkCore;
using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public class EfCoreOrganizationUnitRepository : EfCoreRepository<IIdentityDbContext, OrganizationUnit, Guid>,
        IOrganizationUnitRepository
    {
        public EfCoreOrganizationUnitRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
            string code, 
            Guid? parentId, 
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.Code.StartsWith(code))
                .OrderBy(x => x.Code)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<OrganizationUnit> GetAsync(
            string name, 
            CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync())
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(
                ou => ou.Name == name,
                GetCancellationToken(cancellationToken)
           );
        }

        public virtual async Task<List<OrganizationUnit>> GetChildrenAsync(
            Guid? parentId,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => x.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<int> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(filter),x => x.Name.Contains(filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetListAsync(
            string sorting = null,
            int maxResultCount = 200, 
            int skipCount = 0, 
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(filter),x => x.Name.Contains(filter))
                .OrderBy(string.IsNullOrWhiteSpace(sorting)? nameof(OrganizationUnit.Id) :sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }


        public virtual async Task<List<Guid>> GetUserIdsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return await (from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                          join user in dbContext.Users on userOu.UserId equals user.Id
                          where userOu.OrganizationUnitId == id
                          select user.Id).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<IdentityUser>> GetUsersAsync(
            Guid organizationUnitId,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = await CreateGetMembersFilteredQueryAsync(organizationUnitId, filter);

            return await query.OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityUser.UserName) : sorting)
                        .PageBy(skipCount, maxResultCount)
                        .ToListAsync(GetCancellationToken(cancellationToken));
        }


        public virtual async Task<int> GetUsersCountAsync(
            Guid organizationUnitId,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = await CreateGetMembersFilteredQueryAsync(organizationUnitId, filter);

            return await query.CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUnaddedUsersAsync(
            Guid organizationUnitId,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var userIdsInOrganizationUnit = dbContext.Set<IdentityUserOrganizationUnit>()
                .Where(uou => uou.OrganizationUnitId == organizationUnitId)
                .Select(uou => uou.UserId);

            var query = dbContext.Users
                .Where(u => !userIdsInOrganizationUnit.Contains(u.Id));

            if (!filter.IsNullOrWhiteSpace())
            {
                query = query.Where(u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                );
            }

            return await query
                .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityUser.Name) : sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetUnaddedUsersCountAsync(
            Guid organizationUnitId,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var userIdsInOrganizationUnit = dbContext.Set<IdentityUserOrganizationUnit>()
                .Where(uou => uou.OrganizationUnitId == organizationUnitId)
                .Select(uou => uou.UserId);

            return await dbContext.Users
                .Where(u => !userIdsInOrganizationUnit.Contains(u.Id))
                .WhereIf(!filter.IsNullOrWhiteSpace(), u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter)))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task RemoveAllMembersAsync(
            OrganizationUnit organizationUnit,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var ouMembersQuery = await dbContext.Set<IdentityUserOrganizationUnit>()
                .Where(q => q.OrganizationUnitId == organizationUnit.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            dbContext.Set<IdentityUserOrganizationUnit>().RemoveRange(ouMembersQuery);
        }


        protected virtual async Task<IQueryable<IdentityUser>> CreateGetMembersFilteredQueryAsync(Guid organizationUnitId, string filter = null)
        {
            var dbContext = await GetDbContextAsync();

            var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                        join user in dbContext.Users on userOu.UserId equals user.Id
                        where userOu.OrganizationUnitId == organizationUnitId
                        select user;

            if (!filter.IsNullOrWhiteSpace())
            {
                query = query.Where(u =>
                    u.PhoneNumber.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    u.UserName.Contains(filter)
                );
            }

            return query;
        }

    }
}
