using Microsoft.EntityFrameworkCore;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Ticket;
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
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<EventCategory> EventCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().OwnsMany(x => x.SocialLinks);
            modelBuilder.Entity<Event>().OwnsOne(x => x.Address);
            modelBuilder.Entity<Event>().OwnsMany(x => x.Images);
            
            // Many to many event category
            modelBuilder.Entity<EventCategory>()
                .HasKey(t => new { t.EventId, t.CategoryId });

            modelBuilder.Entity<EventCategory>()
                .HasOne(pt => pt.Event)
                .WithMany(p => p.EventCategories)
                .HasForeignKey(pt => pt.EventId);

            modelBuilder.Entity<EventCategory>()
                .HasOne(pt => pt.Category)
                .WithMany(t => t.Events)
                .HasForeignKey(pt => pt.CategoryId);
        }
    }
}