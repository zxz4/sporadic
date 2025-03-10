using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sporadic.Abp.Users
{
    public static class SporadicUsersDbContextModelCreatingExtensions
    {
        public static void ConfigureSporadicUser<TUser>(this EntityTypeBuilder<TUser> b)
            where TUser : class, IUser
        {
            b.Property(u => u.Email).HasMaxLength(SporadicUserConsts.MaxEmailLength).HasColumnName(nameof(IUser.Email));
            b.Property(u => u.UserName).HasMaxLength(SporadicUserConsts.MaxNameLength).HasColumnName(nameof(IUser.UserName));
            b.Property(u => u.Name).HasMaxLength(SporadicUserConsts.MaxNameLength).HasColumnName(nameof(IUser.Name));

            b.Property(u => u.EmailConfirmed).HasDefaultValue(false).HasColumnName(nameof(IUser.EmailConfirmed));
            b.Property(u => u.PhoneNumber).IsUnicode().HasMaxLength(SporadicUserConsts.MaxPhoneNumberLength).HasColumnName(nameof(IUser.PhoneNumber));
            b.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false).HasColumnName(nameof(IUser.PhoneNumberConfirmed));
            b.Property(u => u.IsActive).HasDefaultValue(true).HasColumnName(nameof(IUser.IsActive));
        }
    }
}
