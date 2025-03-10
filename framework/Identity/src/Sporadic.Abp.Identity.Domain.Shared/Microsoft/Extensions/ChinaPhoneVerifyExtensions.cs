using System.Text.RegularExpressions;

namespace Microsoft.Extensions
{
    public static class ChinaPhoneVerifyExtensions
    {
        public const string ChinaPhonePattern = @"^(?:\+?86)?1(?:3\d{3}|4[5-9]\d{2}|5[0-35-9]\d{2}|6[2567]\d{2}|7[0-8]\d{2}|8\d{3}|9[189]\d{2})\d{6}$";

        public static bool IsChinaPhonePattern(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            phoneNumber = phoneNumber.Trim();

            var cleanNumber = Regex.Replace(phoneNumber, @"[-\s]", "");

            // 移除可能包含的国际区号前缀
            cleanNumber = Regex.Replace(cleanNumber, @"^(\+?86)", "");

            if (!Regex.IsMatch(cleanNumber, ChinaPhonePattern))
            {
                return false;
            }
            else
            {
                phoneNumber = cleanNumber;
                return true;
            }
        }
    }
}
