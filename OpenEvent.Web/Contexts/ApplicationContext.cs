using Microsoft.EntityFrameworkCore;
using OpenEvent.Data.Models.Analytic;
using OpenEvent.Data.Models.BankAccount;
using OpenEvent.Data.Models.Category;
using OpenEvent.Data.Models.Event;
using OpenEvent.Data.Models.PaymentMethod;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Data.Models.Recommendation;
using OpenEvent.Data.Models.Ticket;
using OpenEvent.Data.Models.Transaction;
using OpenEvent.Data.Models.User;

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

#pragma warning disable CS1591
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
        public virtual DbSet<TicketVerificationEvent> VerificationEvents { get; set; }
        public virtual DbSet<RecommendationScore> RecommendationScores { get; set; }
        public virtual DbSet<Promo> Promos { get; set; }
#pragma warning restore CS1591

        /// <summary>
        /// Configure extra relationships between entities
        /// </summary>
        /// <param name="modelBuilder"></param>
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