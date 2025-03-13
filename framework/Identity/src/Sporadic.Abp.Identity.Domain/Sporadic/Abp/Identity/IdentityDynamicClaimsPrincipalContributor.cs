using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Security.Claims;

namespace Sporadic.Abp.Identity
{
    public class IdentityDynamicClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
    {
        public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
            var userId = identity?.FindUserId();
            if (userId == null)
            {
                return;
            }

            var dynamicClaimsCache = context.GetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();
            AbpDynamicClaimCacheItem dynamicClaims;
            try
            {
                dynamicClaims = await dynamicClaimsCache.GetAsync(userId.Value);
            }
            catch (EntityNotFoundException e)
            {
                //如果用户不存在，清空用户身份信息
                context.ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
                var logger = context.GetRequiredService<ILogger<IdentityDynamicClaimsPrincipalContributor>>();
                logger.LogWarning(e, "User not found: {Value}", userId.Value);
                return;
            }

            if (dynamicClaims.Claims.IsNullOrEmpty())
            {
                return;
            }

            await AddDynamicClaimsAsync(context, identity, dynamicClaims.Claims);
        }
    }

}
