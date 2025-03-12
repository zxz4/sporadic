using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public interface IOrganizationUnitRepository : IBasicRepository<OrganizationUnit, Guid>
    {
        /// <summary>
        /// 获取机构数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default
            );

        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <param name="maxResultCount"></param>
        /// <param name="skipCount"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<OrganizationUnit>> GetListAsync(
            string sorting = null,
            int maxResultCount = 200,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default
            );

        Task<List<OrganizationUnit>> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default
        );

        Task<List<OrganizationUnit>> GetChildrenAsync(
            Guid? parentId,
            CancellationToken cancellationToken = default
            );

        Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            CancellationToken cancellationToken = default
        );

        Task<OrganizationUnit> GetAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 获取机构成员
        /// </summary>
        /// <param name="organizationUnit"></param>
        /// <param name="sorting"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="skipCount"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityUser>> GetUsersAsync(
             Guid organizationUnitId,
            string sorting = null,
            int maxResultCount = 100,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 获取机构成员数量
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetUsersCountAsync(
            Guid organizationUnitId,
            string filter = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取机构成员ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Guid>> GetUserIdsAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 获取未添加的用户
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="sorting"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="skipCount"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityUser>> GetUnaddedUsersAsync(
            Guid organizationUnitId,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 获取未添加的用户数量
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetUnaddedUsersCountAsync(
            Guid organizationUnitId,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 移除该机构所有成员
        /// </summary>
        /// <param name="organizationUnit"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveAllMembersAsync(
            OrganizationUnit organizationUnit,
            CancellationToken cancellationToken = default
        );
    }
}
