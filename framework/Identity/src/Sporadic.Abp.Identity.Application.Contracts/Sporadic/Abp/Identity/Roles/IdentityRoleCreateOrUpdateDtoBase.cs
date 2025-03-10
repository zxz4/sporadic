using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Sporadic.Abp.Identity.Roles
{
    public class IdentityRoleCreateOrUpdateDtoBase
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
        [Display(Name = "RoleName")]
        public string Name { get; set; }

        /// <summary>
        /// 是否默认角色
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 是否公开角色
        /// </summary>
        public bool IsPublic { get; set; }
    }
}
