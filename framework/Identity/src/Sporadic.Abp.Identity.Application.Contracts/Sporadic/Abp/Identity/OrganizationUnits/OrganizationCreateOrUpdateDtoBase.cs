using Volo.Abp.Validation;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public abstract class OrganizationCreateOrUpdateDtoBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DynamicStringLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxNameLength))]
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DynamicStringLength(typeof(OrganizationUnitConsts), nameof(OrganizationUnitConsts.MaxAddressLength))]
        public string Address { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }

        protected OrganizationCreateOrUpdateDtoBase()
        {
        }
    }
}
