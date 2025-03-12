using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public class OrganizationUnit : Entity<Guid>
    {
        /// <summary>
        /// 父亲机构id
        /// </summary>
        public Guid? ParentId { get; protected set; }

        /// <summary>
        /// 名称(唯一)
        /// </summary>
        public string Name { get; protected internal set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 经度(小数后六位)
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 维度(小数后六位)
        /// </summary>
        public decimal Latitude { get;  set; }

        public virtual string Code { get; internal set; }

        public virtual ICollection<IdentityUserOrganizationUnit> Users { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUnit"/> class.
        /// </summary>
        public OrganizationUnit()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUnit"/> class.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">Display name.</param>
        /// <param name="parentId">Parent's Id or null if OU is a root.</param>
        public OrganizationUnit(Guid id, string name, Guid? parentId = null)
            : base(id)
        {
            Name = name;
            ParentId = parentId;
        }





        /// <summary>
        /// Creates code for given numbers.
        /// Example: if numbers are 4,2 then returns "00004.00002";
        /// </summary>
        /// <param name="numbers">Numbers</param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', OrganizationUnitConsts.CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        /// Appends a child code to a parent code.
        /// Example: if parentCode = "00001", childCode = "00042" then returns "00001.00042".
        /// </summary>
        /// <param name="parentCode">Parent code. Can be null or empty if parent is a root.</param>
        /// <param name="childCode">Child code.</param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        /// Gets relative code to the parent.
        /// Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="parentCode">The parent code.</param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }

        /// <summary>
        /// Calculates next code for given code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055.00002".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        /// Gets the last unit code.
        /// Example: if code = "00019.00055.00001" returns "00001".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        /// Gets parent code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }

        public virtual void AddUser(Guid userId)
        {
            Check.NotNull(userId, nameof(userId));

            Users.Add(new IdentityUserOrganizationUnit(userId,Id));
        }

        public virtual void RemoveUser(Guid userId)
        {
            Check.NotNull(userId, nameof(userId));

            Users.RemoveAll(r => r.UserId == userId);
        }
    }
}
