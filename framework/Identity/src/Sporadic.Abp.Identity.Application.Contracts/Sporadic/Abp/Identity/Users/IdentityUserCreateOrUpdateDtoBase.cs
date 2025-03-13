using JetBrains.Annotations;
using Microsoft.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Sporadic.Abp.Identity.Users
{
    public abstract class IdentityUserCreateOrUpdateDtoBase : IValidatableObject
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 是否外部用户
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// 强制下次登录时更改密码
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
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
