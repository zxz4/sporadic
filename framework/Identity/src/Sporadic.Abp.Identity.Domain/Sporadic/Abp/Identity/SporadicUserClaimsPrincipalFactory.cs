using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Identity.Users;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Sporadic.Abp.Identity
{
    /// <summary>
    /// 为用户创建声明
    /// </summary>
    public class SporadicUserClaimsPrincipalFactory(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory) : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>(
              userManager,
              roleManager,
              options),
        ITransientDependency
    {
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; } = currentPrincipalAccessor;
        protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; } = abpClaimsPrincipalFactory;

        [UnitOfWork]
        public async override Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identities.First();

            //添加abp相关的声明
            if (!user.Name.IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.Name, user.Name));
            }

            if (!user.PhoneNumber.IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber));
            }

            identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString()));

            if (!user.Email.IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim(AbpClaimTypes.Email, user.Email));
            }

            identity.AddIfNotContains(new Claim(AbpClaimTypes.EmailVerified, user.EmailConfirmed.ToString()));

            using (CurrentPrincipalAccessor.Change(identity))
            {
                //替换当前执行线程的principal
                await AbpClaimsPrincipalFactory.CreateAsync(principal);
            }

            return principal;
        }
    }
}
