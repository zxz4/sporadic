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

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
