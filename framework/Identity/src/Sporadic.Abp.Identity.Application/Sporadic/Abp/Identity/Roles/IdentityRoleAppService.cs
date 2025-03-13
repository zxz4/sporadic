using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Sporadic.Abp.Identity.Roles
{
    [Authorize(IdentityPermissions.Roles.Default)]
    public class IdentityRoleAppService(IdentityRoleManager roleManager, IIdentityRoleRepository roleRepository) : IdentityAppServiceBase, IIdentityRoleAppService
    {
        protected IdentityRoleManager RoleManager { get; } = roleManager;
        protected IIdentityRoleRepository RoleRepository { get; } = roleRepository;

        public virtual async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
        {
            var role = new IdentityRole(
                GuidGenerator.Create(),
                input.Name)
            {
                IsDefault = input.IsDefault,
                IsPublic = input.IsPublic
            };

            (await RoleManager.CreateAsync(role)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var role = await RoleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                return;
            }

            (await RoleManager.DeleteAsync(role)).CheckErrors();
        }

        public virtual async Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
        {
            var list = await RoleRepository.GetListAsync();

            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list)
            );
        }

        public virtual async Task<IdentityRoleDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(
                await RoleManager.GetByIdAsync(id)
            );
        }

        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
        {
            var result = new PagedResultDto<IdentityRoleDto>()
            {
                TotalCount = await RoleRepository.GetCountAsync(input.Filter)
            };

            if (result.TotalCount > input.SkipCount)
            {
                var list = await RoleRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

                result.Items = ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list);
            }

            return result;
        }

        public virtual async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        {
            var role = await RoleManager.GetByIdAsync(id);

            role.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);


            if (role.Name != input.Name)
            {
                (await RoleManager.SetRoleNameAsync(role, input.Name)).CheckErrors();
            }

            role.IsDefault = input.IsDefault;
            role.IsPublic = input.IsPublic;

            (await RoleManager.UpdateAsync(role)).CheckErrors();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);

        }
    }
}
