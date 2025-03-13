using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
    {
        [DisableAuditing]
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }
    }
}
