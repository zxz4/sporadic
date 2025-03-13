using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Users;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sporadic.Abp.Identity.Users
{
    public interface IIdentityUserRepository : IUserRepository<IdentityUser>
    {
        /// <summary>
        /// 获取用户数量
        /// </summary>
        /// <param name="filter">用户名，手机号，邮箱</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="skipCount"></param>
        /// <param name="filter"></param>
        /// <param name="includeDetails"><  /param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityUser>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过外部登录关联信息获取用户
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
      
        /// <summary>
        /// 获取相关角色的用户id
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Guid>> GetUserIdListByRoleIdAsync(
            Guid roleId,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取用户角色名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<string>> GetNormalizedRoleNamesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取用户的角色名称
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityUserIdWithRoleNames>> GetNormalizedRoleNamesAsync(
            IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取角色下所有用户
        /// </summary>
        /// <param name="normalizedRoleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
            string normalizedRoleName,
            CancellationToken cancellationToken = default);
    }
}
