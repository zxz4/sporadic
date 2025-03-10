using Sporadic.Abp.Users;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserConsts
    {
        /// <summary>
        /// Default value: 64
        /// </summary>
        public static int MaxNameLength { get; set; } = SporadicUserConsts.MaxNameLength;

        /// <summary>
        /// Default value: 250
        /// </summary>
        public static int MaxEmailLength { get; set; } = SporadicUserConsts.MaxEmailLength;

        /// <summary>
        /// Default value: 16
        /// </summary>
        public static int MaxPhoneNumberLength { get; set; } = SporadicUserConsts.MaxPhoneNumberLength;

        /// <summary>
        /// Default value: 128
        /// </summary>
        public static int MaxPasswordLength { get; set; } = 128;

        /// <summary>
        /// Default value: 256
        /// </summary>
        public static int MaxPasswordHashLength { get; set; } = 256;

        /// <summary>
        /// Default value: 256
        /// </summary>
        public static int MaxSecurityStampLength { get; set; } = 256;
    }
}
