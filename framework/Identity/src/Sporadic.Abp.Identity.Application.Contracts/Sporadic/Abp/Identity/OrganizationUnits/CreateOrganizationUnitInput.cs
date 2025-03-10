using System;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public class CreateOrganizationUnitInput : OrganizationCreateOrUpdateDtoBase
    {
        /// <summary>
        /// 父机构id
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
