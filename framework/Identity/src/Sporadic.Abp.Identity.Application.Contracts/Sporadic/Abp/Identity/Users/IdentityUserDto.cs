using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserDto : EntityDto<Guid>, IHasConcurrencyStamp
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 是否外部用户
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮箱是否已确认
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机号码是否已确认
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 强制下次登录时更改密码
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        /// <summary>
        /// ConcurrencyStamp
        /// </summary>
        public string ConcurrencyStamp { get; set; }
    }
}
