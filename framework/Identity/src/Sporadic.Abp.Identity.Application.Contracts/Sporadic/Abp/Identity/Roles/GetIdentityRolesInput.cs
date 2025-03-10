using Volo.Abp.Application.Dtos;

namespace Sporadic.Abp.Identity.Roles
{
    public class GetIdentityRolesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
