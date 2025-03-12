using System;

namespace Sporadic.Abp.Identity
{
    /// <summary>
    /// Options for caching <see cref="IdentityDynamicClaimsPrincipalContributor"/>.
    /// </summary>
    public class IdentityDynamicClaimsPrincipalContributorCacheOptions
    {
        public TimeSpan CacheAbsoluteExpiration { get; set; }

        public IdentityDynamicClaimsPrincipalContributorCacheOptions()
        {
            CacheAbsoluteExpiration = TimeSpan.FromHours(1);
        }
    }
}
