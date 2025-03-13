using System;
using Volo.Abp.Application.Dtos;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public class OrganizationUnitDto : EntityDto<Guid>
    {
        /// <summary>
        /// 上级机构id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 名称(唯一)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 经度(小数后六位)
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 维度(小数后六位)
        /// </summary>
        public decimal Latitude { get; set; }
    }
}
