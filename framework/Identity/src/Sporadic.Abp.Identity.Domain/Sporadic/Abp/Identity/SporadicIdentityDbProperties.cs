namespace Sporadic.Abp.Identity
{
    public class SporadicIdentityDbProperties
    {
        /// <summary>
        /// 表前缀
        /// </summary>
        public static string DbTablePrefix { get; set; } = "SYS_";

        /// <summary>
        /// 表模式
        /// </summary>
        public static string DbSchema { get; set; } = null;

        /// <summary>
        /// 连接字符串名称
        /// </summary>
        public const string ConnectionStringName = "Identity";
    }
}
