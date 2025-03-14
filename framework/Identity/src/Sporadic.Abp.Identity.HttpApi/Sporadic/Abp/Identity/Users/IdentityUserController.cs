﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp;
using Sporadic.Abp.Identity.Roles;

namespace Sporadic.Abp.Identity.Users
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area(IdentityRemoteServiceConsts.ModuleName)]
    [ControllerName("User")]
    [Route("api/identity/users")]
    public class IdentityUserController : AbpControllerBase, IIdentityUserAppService
    {
        protected IIdentityUserAppService UserAppService { get; }

        public IdentityUserController(IIdentityUserAppService userAppService)
        {
            UserAppService = userAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<IdentityUserDto> GetAsync(Guid id)
        {
            return UserAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual Task<PagedResultDto<IdentityUserWithRoleNamesDto>> GetListAsync([FromQuery]GetIdentityUsersInput input)
        {
            return UserAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual Task<IdentityUserDto> CreateAsync([FromBody]IdentityUserCreateDto input)
        {
            return UserAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<IdentityUserDto> UpdateAsync(Guid id,[FromBody] IdentityUserUpdateDto input)
        {
            return UserAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return UserAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}/roles")]
        public virtual Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
        {
            return UserAppService.GetRolesAsync(id);
        }

        [HttpGet]
        [Route("assignable-roles")]
        public Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
        {
            return UserAppService.GetAssignableRolesAsync();
        }

        [HttpPut]
        [Route("{id}/roles")]
        public virtual Task UpdateRolesAsync(Guid id,[FromBody] IdentityUserUpdateRolesDto input)
        {
            return UserAppService.UpdateRolesAsync(id, input);
        }

        [HttpGet]
        [Route("by-username/{userName}")]
        public virtual Task<IdentityUserDto> FindByUsernameAsync(string userName)
        {
            return UserAppService.FindByUsernameAsync(userName);
        }

        [HttpGet]
        [Route("by-email/{email}")]
        public virtual Task<IdentityUserDto> FindByEmailAsync(string email)
        {
            return UserAppService.FindByEmailAsync(email);
        }

        [HttpGet]
        [Route("by-phone/{phoneNumber}")]
        public virtual Task<IdentityUserDto> FindByPhoneNumberAsync(string phoneNumber)
        {
            return UserAppService.FindByPhoneNumberAsync(phoneNumber);
        }
    }

}
