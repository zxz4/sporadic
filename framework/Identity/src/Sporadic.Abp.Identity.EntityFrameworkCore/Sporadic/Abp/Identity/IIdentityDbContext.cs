using Microsoft.EntityFrameworkCore;
using Sporadic.Abp.Identity.OrganizationUnits;
using Sporadic.Abp.Identity.Roles;
using Sporadic.Abp.Identity.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Sporadic.Abp.Identity
{
    [ConnectionStringName(SporadicIdentityDbProperties.ConnectionStringName)]
    public interface IIdentityDbContext : IEfCoreDbContext
    {
        DbSet<IdentityUser> Users { get;  }

        DbSet<IdentityRole> Roles { get;  }

        DbSet<OrganizationUnit> OrganizationUnits { get;  }
    }
}
