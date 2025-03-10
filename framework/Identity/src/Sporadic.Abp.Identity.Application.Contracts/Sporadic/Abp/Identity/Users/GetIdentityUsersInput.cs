using Volo.Abp.Application.Dtos;

namespace Sporadic.Abp.Identity.Users
{
    public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
