using System.ComponentModel.DataAnnotations;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserUpdateRolesDto
    {
        [Required]
        public string[] RoleNames { get; set; }
    }
}
