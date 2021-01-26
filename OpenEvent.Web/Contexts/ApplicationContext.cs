using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Models;

namespace OpenEvent.Web.Contexts
{
    public class ApplicationContext : ApiAuthorizationDbContext<User>
    {
        public ApplicationContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}