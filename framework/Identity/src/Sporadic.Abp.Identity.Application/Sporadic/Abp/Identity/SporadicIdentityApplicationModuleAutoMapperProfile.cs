using AutoMapper;
using Sporadic.Abp.Identity.OrganizationUnits;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporadic.Abp.Identity
{
    public class SporadicIdentityApplicationModuleAutoMapperProfile : Profile
    {
        public SporadicIdentityApplicationModuleAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the application layer.
            CreateMap<IdentityUser, IdentityUserDto>();
            CreateMap<IdentityRole, IdentityRoleDto>();
            CreateMap<OrganizationUnit, OrganizationUnitDto>();
        }
    }
}
