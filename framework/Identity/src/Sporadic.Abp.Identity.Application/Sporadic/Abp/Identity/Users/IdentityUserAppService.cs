using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sporadic.Abp.Identity.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;

namespace Sporadic.Abp.Identity.Users
{
    [Authorize(IdentityPermissions.Users.Default)]
    public class IdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions,
        IPermissionChecker permissionChecker) : IdentityAppServiceBase, IIdentityUserAppService
    {
        protected IdentityUserManager UserManager { get; } = userManager;
        protected IIdentityUserRepository UserRepository { get; } = userRepository;
        protected IIdentityRoleRepository RoleRepository { get; } = roleRepository;
        protected IOptions<IdentityOptions> IdentityOptions { get; } = identityOptions;
        protected IPermissionChecker PermissionChecker { get; } = permissionChecker;

        public virtual async Task<IdentityUserDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await UserManager.GetByIdAsync(id)
            );
        }


        public virtual async Task<PagedResultDto<IdentityUserWithRoleNamesDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var result = new PagedResultDto<IdentityUserWithRoleNamesDto>()
            {
                TotalCount = await UserRepository.GetCountAsync(input.Filter)
            };

            if (result.TotalCount > input.SkipCount)
            {
                var userList = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(
                    await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter));

                var userIdWithRoleNames = await UserRepository.GetNormalizedRoleNamesAsync(userList.Select(x => x.Id));

                var resultList = from u in userList 
                                 join r in userIdWithRoleNames on u.Id equals r.Id into ur
                                 from urGroup in ur.DefaultIfEmpty()
                                 select new IdentityUserWithRoleNamesDto
                                 {
                                     User = u,
                                     RoleNames = urGroup?.RoleNames
                                 };

                result.Items = [.. resultList];
            }


            return result;
        }

        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            await IdentityOptions.SetAsync();

            var user = new IdentityUser(
                GuidGenerator.Create(),
                phoneNumber: input.PhoneNumber,
                email: input.Email,
                userName: input.UserName)
            {
                IsActive = input.IsActive,
                IsExternal = input.IsExternal,
                Name = input.Name,
                ShouldChangePasswordOnNextLogin = input.ShouldChangePasswordOnNextLogin,
            };

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
            await UpdateUserByInput(user, input);
            (await UserManager.UpdateAsync(user)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return;
            }

            (await UserManager.DeleteAsync(user)).CheckErrors();
        }

        public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            await IdentityOptions.SetAsync();

            var user = await UserManager.GetByIdAsync(id);

            user.Name = input.Name;

            user.IsExternal = input.IsExternal;

            user.IsActive = input.IsActive;

            user.ShouldChangePasswordOnNextLogin = input.ShouldChangePasswordOnNextLogin;

            user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

            if (string.Compare(user.UserName,input.UserName) != 0)
            {
                (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
            }

            await UpdateUserByInput(user, input);

            (await UserManager.UpdateAsync(user)).CheckErrors();

            if (!input.Password.IsNullOrEmpty())
            {
                (await UserManager.RemovePasswordAsync(user)).CheckErrors();
                (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }


        public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            var roles = await UserRepository.GetRolesAsync(id);

            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
            );
        }


        public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
        {
            var list = await RoleRepository.GetListAsync();
            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
        }

        public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
        {
            await IdentityOptions.SetAsync();
            var user = await UserManager.GetByIdAsync(id);
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            await UserRepository.UpdateAsync(user);
        }


        public virtual async Task<IdentityUserDto> FindByUsernameAsync(string userName)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await UserManager.FindByNameAsync(userName)
            );
        }

        public virtual async Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
                await UserManager.FindByEmailAsync(email)
            );
        }
        public virtual async Task<IdentityUserDto> FindByPhoneNumberAsync(string phoneNumber)
        {
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
               await UserRepository.FindByPhoneNumberAsync(phoneNumber)
           );
        }
        protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
        {
            if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
            }

            if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            }

            if (!string.Equals(user.UserName, input.UserName))
            {
                (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();
            }

            user.Name = input.Name;

            user.IsExternal = input.IsExternal;

            user.IsActive = input.IsActive;

            (await UserManager.UpdateAsync(user)).CheckErrors();

            if (input.RoleNames != null)
            {
                (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
            }
        }
    }

}
