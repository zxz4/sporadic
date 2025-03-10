using JetBrains.Annotations;
using Microsoft.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Sporadic.Abp.Identity.Users
{
    public abstract class IdentityUserCreateOrUpdateDtoBase : IValidatableObject
    {
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string UserName { get; set; }

        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool IsExternal { get; set; }

        [CanBeNull]
        public string[] RoleNames { get; set; }

        protected IdentityUserCreateOrUpdateDtoBase()
        {

        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(PhoneNumber) && !PhoneNumber.IsChinaPhonePattern())
            {
                yield return new ValidationResult("手机号格式不正确", new[] { nameof(PhoneNumber) });
            }
        }
    }
}
