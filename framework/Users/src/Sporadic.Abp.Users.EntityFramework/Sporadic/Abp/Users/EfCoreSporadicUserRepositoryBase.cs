using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sporadic.Abp.Users
{
    public class EfCoreSporadicUserRepositoryBase<TDbContext, TUser>(IDbContextProvider<TDbContext> dbContextProvider) : EfCoreRepository<TDbContext, TUser, Guid>(dbContextProvider), IUserRepository<TUser>
        where TDbContext : IEfCoreDbContext
        where TUser : class, IUser
    {
        public async Task<TUser> FindByEmailAsync(string email, bool confirmed = false , CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(
                u => u.Email == email 
                && u.EmailConfirmed == confirmed, 
                GetCancellationToken(cancellationToken));
        }

        public async Task<TUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(
                u => u.UserName == userName, 
                GetCancellationToken(cancellationToken));
        }

        public async Task<TUser> FindByPhoneNumberAsync(string phoneNumber, bool confirmed = false, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(
                u => u.PhoneNumber == phoneNumber && 
                u.PhoneNumberConfirmed == confirmed, 
                GetCancellationToken(cancellationToken));
        }
    }
}
