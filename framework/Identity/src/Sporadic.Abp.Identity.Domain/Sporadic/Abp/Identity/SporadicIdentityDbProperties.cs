namespace Sporadic.Abp.Identity
{
    public class SporadicIdentityDbProperties
    {
        public static string DbTablePrefix { get; set; } = "SYS_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "SprIdentity";
    }
}
