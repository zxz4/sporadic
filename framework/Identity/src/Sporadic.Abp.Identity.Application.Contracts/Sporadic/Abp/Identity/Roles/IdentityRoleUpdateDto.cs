using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Roles
{
    public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
}
