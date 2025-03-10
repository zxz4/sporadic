using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
    {
        [DisableAuditing]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
