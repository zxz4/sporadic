using Volo.Abp.Application.Dtos;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public class GetOrganizationUnitsInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 根据机构名称过滤
        /// </summary>
        public string Filter { get; set; }
    }
}
