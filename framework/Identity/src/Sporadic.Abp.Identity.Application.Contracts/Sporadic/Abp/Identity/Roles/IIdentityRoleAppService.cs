using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sporadic.Abp.Identity.Roles
{
    public interface IIdentityRoleAppService : ICrudAppService<
        IdentityRoleDto,
        Guid,
        GetIdentityRolesInput,
        IdentityRoleCreateDto,
        IdentityRoleUpdateDto>
    {
        Task<ListResultDto<IdentityRoleDto>> GetAllListAsync();
    }
}
