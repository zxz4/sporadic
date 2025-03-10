using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Sporadic.Abp.Identity.Roles
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area(IdentityRemoteServiceConsts.ModuleName)]
    [ControllerName("Role")]
    [Route("api/identity/roles")]
    public class IdentityRoleController : AbpControllerBase, IIdentityRoleAppService
    {
        protected IIdentityRoleAppService RoleAppService { get; }

        public IdentityRoleController(IIdentityRoleAppService roleAppService)
        {
            RoleAppService = roleAppService;
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public virtual Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
        {
            return RoleAppService.GetAllListAsync();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<PagedResultDto<IdentityRoleDto>> GetListAsync([FromQuery]GetIdentityRolesInput input)
        {
            return RoleAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public virtual Task<IdentityRoleDto> GetAsync(Guid id)
        {
            return RoleAppService.GetAsync(id);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual Task<IdentityRoleDto> CreateAsync([FromBody]IdentityRoleCreateDto input)
        {
            return RoleAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public virtual Task<IdentityRoleDto> UpdateAsync(Guid id,[FromBody] IdentityRoleUpdateDto input)
        {
            return RoleAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return RoleAppService.DeleteAsync(id);
        }
    }
}
