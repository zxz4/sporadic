using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Identity.Users;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Sporadic.Abp.Identity.Data
{
    public class IdentityDataSeedContributor(IGuidGenerator guidGenerator, IIdentityRoleRepository roleRepository, IIdentityUserRepository userRepository, IdentityUserManager userManager, IdentityRoleManager roleManager, IOptions<IdentityOptions> identityOptions) : IDataSeedContributor, ITransientDependency
    {
        protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
        protected IIdentityRoleRepository RoleRepository { get; } = roleRepository;
        protected IIdentityUserRepository UserRepository { get; } = userRepository;
        protected IdentityUserManager UserManager { get; } = userManager;
        protected IdentityRoleManager RoleManager { get; } = roleManager;
        protected IOptions<IdentityOptions> IdentityOptions { get; } = identityOptions;

        public async Task SeedAsync(DataSeedContext context)
        {
            await IdentityOptions.SetAsync();

            var identityRole = await RoleRepository.FindByNormalizedNameAsync("ADMIN");

            if (identityRole == null)
            {
                identityRole = new IdentityRole(GuidGenerator.Create(), "Admin")
                {
                    IsDefault = false,
                    IsPublic = true,
                    IsStatic = true
                };

                identityRole.Name = "管理员";
                await RoleManager.CreateAsync(identityRole);
            }


            var identityUser = await UserRepository.FindByUserNameAsync("admin");

            if (identityUser == null)
            {
                identityUser = new IdentityUser(GuidGenerator.Create(), userName: "admin")
                {
                    IsActive = true,
                    IsExternal = false,
                    LockoutEnabled = true,
                };

                await UserManager.CreateAsync(identityUser, "123456", validatePassword: false);

                (await UserManager.AddToRoleAsync(identityUser, identityRole.NormalizedName)).CheckErrors();
            }
        }
    }
}
