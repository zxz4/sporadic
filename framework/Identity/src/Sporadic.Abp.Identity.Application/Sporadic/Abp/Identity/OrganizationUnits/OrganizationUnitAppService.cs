using Microsoft.AspNetCore.Authorization;
using Sporadic.Abp.Identity.Users;
using Sporadic.Abp.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    [Authorize(IdentityPermissions.OrganizationUnits.Default)]
    public class OrganizationUnitAppService : IdentityAppServiceBase, IOrganizationUnitAppService
    {
        public OrganizationUnitAppService(
            IIdentityUserRepository userRepository,
            IOrganizationUnitRepository organizationUnitRepository,
            OrganizationUnitManager organizationUnitManager)
        {
            UserRepository = userRepository;
            OrganizationUnitRepository = organizationUnitRepository;
            OrganizationUnitManager = organizationUnitManager;
        }

        protected IIdentityUserRepository UserRepository { get; }
        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

        protected OrganizationUnitManager OrganizationUnitManager { get; }

        public async Task<OrganizationUnitDto> CreateAsync(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnit(
                GuidGenerator.Create(),
                input.Name,
                input.ParentId)
            {
                Address = input.Address,
                Longitude = input.Longitude,
                Latitude = input.Latitude,
            };

            await OrganizationUnitManager.CreateAsync(organizationUnit);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(organizationUnit);
        }

        public async Task DeleteAsync(Guid id)
        {
            await OrganizationUnitManager.DeleteAsync(id);
        }

        public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(GetOrganizationUnitsInput input)
        {
            var result = new PagedResultDto<OrganizationUnitDto>()
            {
                TotalCount = await OrganizationUnitRepository.GetCountAsync(input.Filter),
            };

            if (result.TotalCount > input.SkipCount)
            {
                var list = await OrganizationUnitRepository.GetListAsync(
                    input.Sorting,
                    input.MaxResultCount,
                    input.SkipCount,
                    input.Filter
                );

                result.Items = ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list);
            }

            return result;
        }


        public virtual async Task<List<OrganizationUnitNodeDto>> GetOrganizationUnitNodesAsync()
        {
            // 获取所有机构(根据Code排序)
            var orderedList = await OrganizationUnitRepository.GetListAsync(
                sorting: nameof(OrganizationUnit.Code),
                maxResultCount: int.MaxValue);

            var roots = new List<OrganizationUnitNodeDto>();
            var nodeMap = new Dictionary<Guid, OrganizationUnitNodeDto>();

            foreach (var item in orderedList)
            {
                var node = new OrganizationUnitNodeDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Children = []
                };

                // 将当前节点添加到字典中
                nodeMap[item.Id] = node;

                // 如果当前节点是根节点
                if (!item.ParentId.HasValue)
                {
                    roots.Add(node);
                }
                else
                {
                    // 找到父节点并将当前节点添加到父节点的子节点列表中
                    if (nodeMap.TryGetValue(item.ParentId.Value, out var parentNode))
                    {
                        parentNode.Children.Add(node);
                    }
                }
            }

            nodeMap.Clear();

            return roots;
        }

        public async Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid organizationUnitId, GetIdentityUsersInput input)
        {
            var result = new PagedResultDto<IdentityUserDto>()
            {
                TotalCount = await OrganizationUnitRepository.GetUsersCountAsync(
                  organizationUnitId,
                  input.Filter),
            };

            if (result.TotalCount > input.SkipCount)
            {
                result.Items = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(
                    await OrganizationUnitRepository.GetUsersAsync(
                        organizationUnitId,
                        input.Sorting,
                        input.MaxResultCount,
                        input.SkipCount,
                        input.Filter
                    )
                );
            }

            return result;
        }

        public async Task<PagedResultDto<IdentityUserDto>> GetUnaddedUsersAsync(Guid organizationUnitId, GetIdentityUsersInput input)
        {
            var result = new PagedResultDto<IdentityUserDto>()
            {
                TotalCount = await OrganizationUnitRepository.GetUnaddedUsersCountAsync(
                    organizationUnitId,
                    input.Filter),
            };

            if (result.TotalCount > input.SkipCount)
            {
                result.Items = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(
                    await OrganizationUnitRepository.GetUnaddedUsersAsync(
                        organizationUnitId,
                        input.Sorting,
                        input.MaxResultCount,
                        input.SkipCount,
                        input.Filter
                    )
                );
            }

            return result;
        }

        public async Task<OrganizationUnitDto> UpdateAsync(Guid id, UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id)
                ?? throw new EntityNotFoundException(typeof(OrganizationUnit), id);

            if (!string.Equals(organizationUnit.Name, input.Name))
            {
                await OrganizationUnitManager.SetNameAsync(organizationUnit, input.Name);
            }

            organizationUnit.Address = input.Address;
            organizationUnit.Latitude = input.Latitude;
            organizationUnit.Longitude = input.Longitude;

            await OrganizationUnitManager.UpdateAsync(organizationUnit);

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(organizationUnit);
        }

        public async Task AddUserToOrganizationUnitAsync(Guid organizationUnitId, Guid userId)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(organizationUnitId);
            var identityUser = await UserRepository.GetAsync(userId);
            await OrganizationUnitManager.AddUserToOrganizationUnitAsync(organizationUnit, identityUser);
        }

        public async Task RemoveUserFromOrganizationUnitAsync(Guid organizationUnitId, Guid userId)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(organizationUnitId);
            var identityUser = await UserRepository.GetAsync(userId);
            await OrganizationUnitManager.RemoveUserFromOrganizationUnitAsync(organizationUnit, identityUser);
        }
    }
}
