using Sporadic.Abp.Identity.Roles;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sporadic.Abp.Identity.Users
{
    public interface IIdentityUserAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

        /// <summary>
        /// 获取可分配的角色列表
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);

        /// <summary>
        /// 通过用户名查找用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<IdentityUserDto> FindByUsernameAsync(string userName);

        /// <summary>
        /// 通过邮箱查找用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IdentityUserDto> FindByEmailAsync(string email);

        /// <summary>
        /// 通过手机号查找用户
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<IdentityUserDto> FindByPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// 通过id查找用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdentityUserDto> GetAsync(Guid id);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityUserWithRoleNamesDto>> GetListAsync(GetIdentityUsersInput input);
    }
}
