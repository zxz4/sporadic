using Sporadic.Abp.Identity.Roles;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sporadic.Abp.Identity.Users
{
    public interface IIdentityUserAppService : IApplicationService
    {
        Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

        Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

        Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);

        Task<IdentityUserDto> FindByUsernameAsync(string userName);

        Task<IdentityUserDto> FindByEmailAsync(string email);

        Task<IdentityUserDto> FindByPhoneNumberAsync(string phoneNumber);

        Task<IdentityUserDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input);

        Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input);

        Task<PagedResultDto<IdentityUserWithRoleNamesDto>> GetListAsync(GetIdentityUsersInput input);
    }
}
