using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sporadic.Abp.Users
{
    public class EfCoreSporadicUserRepositoryBase<TDbContext, TUser> : EfCoreRepository<TDbContext, TUser, Guid>, IUserRepository<TUser>
        where TDbContext : IEfCoreDbContext
        where TUser : class, IUser
    {
        public EfCoreSporadicUserRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<TUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(
                u => u.Email == email 
                && u.EmailConfirmed, 
                GetCancellationToken(cancellationToken));
        }

        public async Task<TUser> FindByUserNameAsync(string userName, bool includeDetails, CancellationToken cancellationToken)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(
                u => u.UserName == userName, 
                GetCancellationToken(cancellationToken));
        }

        public async Task<TUser> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(
                u => u.PhoneNumber == phoneNumber && 
                u.PhoneNumberConfirmed, 
                GetCancellationToken(cancellationToken));
        }
    }
}
