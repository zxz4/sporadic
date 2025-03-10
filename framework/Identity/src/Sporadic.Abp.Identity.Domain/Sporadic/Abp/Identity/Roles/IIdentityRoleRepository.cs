using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Sporadic.Abp.Identity.Roles
{
    public interface IIdentityRoleRepository : IBasicRepository<IdentityRole, Guid>
    {
        Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedName,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRoleWithUserCount>> GetListWithUserCountAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRole>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default
        );
        Task<List<IdentityRole>> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRole>> GetDefaultOnesAsync(
            CancellationToken cancellationToken = default
        );

        Task<int> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default
        );
    }

}
