using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sporadic.Abp.Identity.Users;
using System.Threading.Tasks;
using IdentityUser = Sporadic.Abp.Identity.Users.IdentityUser;

namespace Sporadic.Abp.Identity.AspNetCore;

public class SporadicSignInManager(
    IdentityUserManager userManager,
    IHttpContextAccessor contextAccessor,
    IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
    IOptions<IdentityOptions> optionsAccessor,
    ILogger<SignInManager<IdentityUser>> logger,
    IAuthenticationSchemeProvider schemes,
    IUserConfirmation<IdentityUser> confirmation) : SignInManager<IdentityUser>(
    userManager,
    contextAccessor,
    claimsFactory,
    optionsAccessor,
    logger,
    schemes,
    confirmation)
{
    protected async override Task<SignInResult> PreSignInCheck(IdentityUser user)
    {
        if (!user.IsActive)
        {
            Logger.LogWarning("该用户已被停用!(username: \"{UserName}\", id:\"{Id}\")",user.UserName,user.Id);
            return SignInResult.NotAllowed;
        }

        if (user.ShouldChangePasswordOnNextLogin)
        {
            Logger.LogWarning("该用户需要强制更新密码! (username: \"{UserName}\", id:\"{Id}\")", user.UserName, user.Id);
            return SignInResult.NotAllowed;
        }

        return await base.PreSignInCheck(user);
    }

    /// <summary>
    /// This is to call the protection method SignInOrTwoFactorAsync
    /// </summary>
    /// <param name="user"></param>
    /// <param name="isPersistent"></param>
    /// <param name="loginProvider"></param>
    /// <param name="bypassTwoFactor"></param>
    /// <returns></returns>
    public virtual async Task<SignInResult> CallSignInOrTwoFactorAsync(IdentityUser user, bool isPersistent, string loginProvider = null, bool bypassTwoFactor = false)
    {
        return await base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
    }
}
