using System;
using System.Collections.Generic;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public class OrganizationUnitNodeDto
    {
        /// <summary>
        /// 机构id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子机构
        /// </summary>
        public List<OrganizationUnitNodeDto> Children { get; set; }
    }
}
