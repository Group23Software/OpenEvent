using Microsoft.EntityFrameworkCore;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.BankAccount;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Models.Transaction;
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
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        
        public virtual DbSet<PageViewEvent> PageViewEvents { get; set; }
        public virtual DbSet<SearchEvent> SearchEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(x => x.Transaction)
                .WithOne(x => x.Ticket)
                .HasForeignKey<Transaction>(x => x.TicketId);
            

            modelBuilder.Entity<Event>().OwnsMany(x => x.SocialLinks);
            modelBuilder.Entity<Event>().OwnsOne(x => x.Address);
            modelBuilder.Entity<Event>().OwnsMany(x => x.Images);
            
            modelBuilder.Entity<User>().OwnsOne(x => x.Address);

            // Many to many event category
            modelBuilder.Entity<EventCategory>()
                .HasKey(t => new {t.EventId, t.CategoryId});

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