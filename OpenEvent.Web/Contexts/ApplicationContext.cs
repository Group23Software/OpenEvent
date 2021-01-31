using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Contexts
{
    /// <summary>
    /// Main db context for application
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public ApplicationContext(DbContextOptions<ApplicationContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}