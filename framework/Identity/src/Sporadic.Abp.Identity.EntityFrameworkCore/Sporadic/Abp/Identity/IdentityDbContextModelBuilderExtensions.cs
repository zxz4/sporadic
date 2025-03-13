using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Sporadic.Abp.Identity.OrganizationUnits;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Identity.Users;
using Sporadic.Abp.Users;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sporadic.Abp.Identity
{
    public static class IdentityDbContextModelBuilderExtensions
    {
        public static void ConfigureIdentity([NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(SporadicIdentityDbProperties.DbTablePrefix + "Users", SporadicIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();
                b.ConfigureSporadicUser();

                b.Property(u => u.PasswordHash).HasMaxLength(IdentityUserConsts.MaxPasswordHashLength)
                    .HasColumnName(nameof(IdentityUser.PasswordHash));

                b.Property(u => u.SecurityStamp).IsRequired().HasMaxLength(IdentityUserConsts.MaxSecurityStampLength)
                    .HasColumnName(nameof(IdentityUser.SecurityStamp));

                b.Property(u => u.ShouldChangePasswordOnNextLogin).HasDefaultValue(false)
                    .HasColumnName(nameof(IdentityUser.ShouldChangePasswordOnNextLogin));

                b.Property(u => u.IsExternal).IsRequired().HasDefaultValue(true)
                    .HasColumnName(nameof(IdentityUser.IsExternal));

                b.Property(u=>u.LockoutEnd).HasColumnName(nameof(IdentityUser.LockoutEnd));

                b.Property(u => u.LockoutEnabled).IsRequired().HasDefaultValue(false).HasColumnName(nameof(IdentityUser.LockoutEnabled));

                b.Property(u => u.AccessFailedCount).IsRequired().HasDefaultValue(0).HasColumnName(nameof(IdentityUser.AccessFailedCount));

                b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.OrganizationUnits).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(u => new {u.PhoneNumber,u.Email,u.UserName });
            });

            builder.Entity<IdentityUserLogin>(b =>
            {
                b.ToTable(SporadicIdentityDbProperties.DbTablePrefix + "UserLogins", SporadicIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.HasKey(x => new { x.UserId, x.LoginProvider });

                b.Property(ul => ul.LoginProvider).HasMaxLength(IdentityUserLoginConsts.MaxLoginProviderLength)
                    .IsRequired();
                b.Property(ul => ul.ProviderKey).HasMaxLength(IdentityUserLoginConsts.MaxProviderKeyLength)
                    .IsRequired();
                b.Property(ul => ul.ProviderDisplayName)
                    .HasMaxLength(IdentityUserLoginConsts.MaxProviderDisplayNameLength);

                b.HasIndex(l => new { l.LoginProvider, l.ProviderKey });
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable(SporadicIdentityDbProperties.DbTablePrefix + "Roles", SporadicIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(r => r.Name).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNameLength);
                b.Property(r => r.NormalizedName).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNameLength);

                b.Property(r => r.IsDefault).HasColumnName(nameof(IdentityRole.IsDefault));
                b.Property(r => r.IsStatic).HasColumnName(nameof(IdentityRole.IsStatic));
                b.Property(r => r.IsPublic).HasColumnName(nameof(IdentityRole.IsPublic));

                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<IdentityRoleClaim>(b =>
            {
                b.ToTable(SporadicIdentityDbProperties.DbTablePrefix + "RoleClaims", SporadicIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.ClaimType).HasMaxLength(IdentityRoleClaimConsts.MaxClaimTypeLength).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(IdentityRoleClaimConsts.MaxClaimValueLength);

                b.HasIndex(uc => uc.RoleId);
            });

            builder.Entity<IdentityUserRole>(b =>
            {
                b.ToTable(SporadicIdentityDbProperties.DbTablePrefix + "UserRoles", SporadicIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne<IdentityRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasOne<IdentityUser>().WithMany(u => u.Roles).HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(ur => new { ur.RoleId, ur.UserId });
            });

            builder.Entity<OrganizationUnit>(b =>
            {
                b.ToTable(SporadicIdentityDbProperties.DbTablePrefix + "OrganizationUnits", SporadicIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(ou => ou.Code).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxCodeLength)
                .HasColumnName(nameof(OrganizationUnit.Code));

                b.Property(ou => ou.Name).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxNameLength)
                .HasColumnName(nameof(OrganizationUnit.Name));

                b.Property(ou=>ou.Address).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxAddressLength)
                .HasColumnName(nameof(OrganizationUnit.Address));

                b.HasMany<OrganizationUnit>().WithOne().HasForeignKey(ou => ou.ParentId);

                b.HasIndex(ou => ou.Code);
            });

            builder.Entity<IdentityUserOrganizationUnit>(b =>
            {
                b.ToTable(SporadicIdentityDbProperties.DbTablePrefix + "UserOrganizationUnits", SporadicIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.HasKey(ou => new { ou.OrganizationUnitId, ou.UserId });

                b.HasOne<OrganizationUnit>().WithMany(ou=>ou.Users).HasForeignKey(ou => ou.OrganizationUnitId).IsRequired();
                
                b.HasIndex(ou => new { ou.UserId, ou.OrganizationUnitId });
            });
        }
    }
}
