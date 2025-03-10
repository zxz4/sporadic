using Microsoft.EntityFrameworkCore;
using Sporadic.Abp.Identity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sporadic.Abp.Identity.Roles
{
    public class EfCoreIdentityRoleRepository : EfCoreRepository<IIdentityDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<IdentityRole> FindByNormalizedNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync())
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, 
                GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(),x => x.Name.Contains(filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetDefaultOnesAsync(CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(r => r.IsDefault)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityRole.Id) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRoleWithUserCount>> GetListWithUserCountAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var roles = await GetListAsync(sorting, maxResultCount, skipCount, filter, cancellationToken: cancellationToken);

            var roleIds = roles.Select(x => x.Id).ToList();
            var userCount = await (await GetDbContextAsync()).Set<IdentityUserRole>()
                .Where(userRole => roleIds.Contains(userRole.RoleId))
                .GroupBy(userRole => userRole.RoleId)
                .Select(x => new
                {
                    RoleId = x.Key,
                    Count = x.Count()
                })
                .ToListAsync(GetCancellationToken(cancellationToken));

            return roles.Select(role => new IdentityRoleWithUserCount(role, userCount.FirstOrDefault(x => x.RoleId == role.Id)?.Count ?? 0)).ToList();
        }
    }
}
