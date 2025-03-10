using Sporadic.Abp.Identity.Localization;
using Volo.Abp.Application.Services;

namespace Sporadic.Abp.Identity
{
    public abstract class IdentityAppServiceBase : ApplicationService
    {
        protected IdentityAppServiceBase()
        {
            ObjectMapperContext = typeof(SporadicIdentityApplicationModule);
            LocalizationResource = typeof(IdentityResource);
        }
    }

}
