using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Roles
{
    public class IdentityRoleDto : EntityDto<Guid>, IHasConcurrencyStamp
    {
        /// <summary>
        /// 显示的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string NormalizedName { get; set; }

        /// <summary>
        /// 默认角色
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 静态角色无法删除或更改
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 非公共角色其他人不可见
        /// </summary>
        public bool IsPublic { get; set; }


        public string ConcurrencyStamp { get; set; }
    }
}
