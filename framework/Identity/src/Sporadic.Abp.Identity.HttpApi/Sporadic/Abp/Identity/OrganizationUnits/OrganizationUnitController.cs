using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area(IdentityRemoteServiceConsts.ModuleName)]
    [ControllerName("OrganizationUnit")]
    [Route("api/identity/organizationunits")]
    public class OrganizationUnitController : AbpControllerBase, IOrganizationUnitAppService
    {
        protected IOrganizationUnitAppService OrganizationUnitAppService { get; }

        public OrganizationUnitController(IOrganizationUnitAppService organizationUnitAppService)
        {
            OrganizationUnitAppService = organizationUnitAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<OrganizationUnitDto>> GetListAsync([FromQuery]GetOrganizationUnitsInput input)
        {
            return OrganizationUnitAppService.GetListAsync(input);
        }

        [HttpGet("nodes")]
        public Task<List<OrganizationUnitNodeDto>> GetOrganizationUnitNodesAsync()
        {
            return OrganizationUnitAppService.GetOrganizationUnitNodesAsync();
        }

        [HttpPost]
        public async Task<OrganizationUnitDto> CreateAsync([FromBody]CreateOrganizationUnitInput input)
        {
            return await OrganizationUnitAppService.CreateAsync(input);
        }

        
        [HttpPut("{id}")]
        public async Task<OrganizationUnitDto> UpdateAsync(Guid id,[FromBody] UpdateOrganizationUnitInput input)
        {
            return await OrganizationUnitAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 移除机构（包括子机构）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await OrganizationUnitAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 获取机构成员
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("{organizationUnitId}/users")]
        public Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid organizationUnitId,[FromQuery] GetIdentityUsersInput input)
        {
            return OrganizationUnitAppService.GetUsersAsync(organizationUnitId, input);
        }

        /// <summary>
        /// 获取未加入机构成员
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{organizationUnitId}/unadded-users")]
        public Task<PagedResultDto<IdentityUserDto>> GetUnaddedUsersAsync(Guid organizationUnitId,[FromQuery] GetIdentityUsersInput input)
        {
            return OrganizationUnitAppService.GetUnaddedUsersAsync(organizationUnitId, input);
        }
    }
}
