using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public interface IOrganizationUnitAppService : IApplicationService
    {
        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(GetOrganizationUnitsInput input);

        /// <summary>
        /// 获取机构节点
        /// </summary>
        /// <returns></returns>
        Task<List<OrganizationUnitNodeDto>> GetOrganizationUnitNodesAsync();

        /// <summary>
        /// 创建机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrganizationUnitDto> CreateAsync(CreateOrganizationUnitInput input);


        /// <summary>
        /// 更新机构
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrganizationUnitDto> UpdateAsync(Guid id, UpdateOrganizationUnitInput input);

        /// <summary>
        /// 移除机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);


        /// <summary>
        /// 获取机构成员
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid organizationUnitId,GetIdentityUsersInput input);


        /// <summary>
        /// 添加用户到机构
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task AddUserToOrganizationUnitAsync(Guid organizationUnitId,Guid userId);


        /// <summary>
        /// 从机构移除用户
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveUserFromOrganizationUnitAsync(Guid organizationUnitId, Guid userId);

        /// <summary>
        /// 获取未添加的成员
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityUserDto>> GetUnaddedUsersAsync(Guid organizationUnitId, GetIdentityUsersInput input);
    }
}
