using Microsoft.EntityFrameworkCore;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Sporadic.Abp.Identity.Users
{
    public class EfCoreIdentityUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : EfCoreSporadicUserRepositoryBase<IIdentityDbContext, IdentityUser>(dbContextProvider), IIdentityUserRepository
    {
        public virtual async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return await(await GetDbSetAsync())
                .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(filter),x => x.PhoneNumber == filter || x.Email == filter || x.UserName == filter)
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListAsync(string sorting = null, int maxResultCount = 50, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync())
               .WhereIf(!string.IsNullOrWhiteSpace(filter),x => x.PhoneNumber == filter || x.Email == filter || x.UserName == filter)
               .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityUser.Id) : sorting)
               .PageBy(skipCount, maxResultCount)
               .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
            string normalizedRoleName,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var role = await dbContext.Roles
                .Where(x => x.NormalizedName == normalizedRoleName)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            if (role == null)
            {
                return [];
            }

            return await dbContext.Users
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityUserIdWithRoleNames>> GetNormalizedRoleNamesAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var userRoles = await (from userRole in dbContext.Set<IdentityUserRole>()
                                   join role in dbContext.Roles on userRole.RoleId equals role.Id
                                   where userIds.Contains(userRole.UserId)
                                   group new
                                   {
                                       userRole.UserId,
                                       role.Name
                                   } by userRole.UserId
            into gp
                                   select new IdentityUserIdWithRoleNames
                                   {
                                       Id = gp.Key,
                                       RoleNames = gp.Select(x => x.Name).ToArray()
                                   }).ToListAsync(cancellationToken: cancellationToken);

            return userRoles;
        }

        public virtual async Task<List<string>> GetNormalizedRoleNamesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = from userRole in dbContext.Set<IdentityUserRole>()
                        join role in dbContext.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role.NormalizedName;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }


        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = from userRole in dbContext.Set<IdentityUserRole>()
                        join role in dbContext.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role;

       
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Guid>> GetUserIdListByRoleIdAsync(
            Guid roleId,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == roleId)
                .Select(x => x.UserId).ToListAsync(GetCancellationToken(cancellationToken));
        }

    }
}
