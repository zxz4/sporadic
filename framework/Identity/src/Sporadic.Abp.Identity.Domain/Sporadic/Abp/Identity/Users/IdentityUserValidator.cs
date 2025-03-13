using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Sporadic.Abp.Identity.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sporadic.Abp.Identity.Users
{
    public class IdentityUserValidator : IUserValidator<IdentityUser>
    {
        protected IStringLocalizer<IdentityResource> Localizer { get; }

        protected IIdentityUserRepository IdentityUserRepository { get; }

        public IdentityUserValidator(IStringLocalizer<IdentityResource> localizer ,IIdentityUserRepository identityUserRepository)
        {
            Localizer = localizer;
            IdentityUserRepository = identityUserRepository;
        }

        public virtual async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var errors = new List<IdentityError>();

            var userName = await manager.GetUserNameAsync(user);

            var email = await manager.GetEmailAsync(user);

            var phone = await manager.GetPhoneNumberAsync(user);

            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
            {
                errors.Add(new IdentityError
                {
                    Code = "EmptyUserName",
                    Description = Localizer["EmptyUserName"]
                });
            }
            else if (!string.IsNullOrWhiteSpace(userName))
            {
                var owner = await manager.FindByEmailAsync(userName);

                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidUserName",
                        Description = Localizer["Sporadic.Abp.Identity:DuplicateUserName", userName]
                    });
                }
            }
            else if (!string.IsNullOrWhiteSpace(email))
            {
                var owner = await manager.FindByNameAsync(email);
                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidEmail",
                        Description = Localizer["Sporadic.Abp.Identity:DuplicateEmail", email]
                    });
                }
            }
            else
            {
                var owner = await IdentityUserRepository.FindByPhoneNumberAsync(phone);

                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidPhoneNumber",
                        Description = Localizer["Sporadic.Abp.Identity:DuplicatePhoneNumber", userName]
                    });
                }
            }

            return errors.Count > 0 ? IdentityResult.Failed([.. errors]) : IdentityResult.Success;
        }
    }
}
