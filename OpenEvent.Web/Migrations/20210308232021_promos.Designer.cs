﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Web.Migrations
{
#pragma warning disable CS1591
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210308232021_promos")]
    partial class promos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("OpenEvent.Web.Models.Analytic.PageViewEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("PageViewEvents");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Analytic.SearchEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Params")
                        .HasColumnType("longtext");

                    b.Property<string>("Search")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SearchEvents");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Analytic.TicketVerificationEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("TicketId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("TicketId");

                    b.HasIndex("UserId");

                    b.ToTable("VerificationEvents");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.BankAccount.BankAccount", b =>
                {
                    b.Property<string>("StripeBankAccountId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Bank")
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<string>("Currency")
                        .HasColumnType("longtext");

                    b.Property<string>("LastFour")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("StripeBankAccountId");

                    b.HasIndex("UserId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Category.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Category.EventCategory", b =>
                {
                    b.Property<Guid>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.HasKey("EventId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("EventCategories");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Event.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndLocal")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndUTC")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("HostId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartLocal")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartUTC")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TicketsLeft")
                        .HasColumnType("int");

                    b.Property<bool>("isCanceled")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.PaymentMethod.PaymentMethod", b =>
                {
                    b.Property<string>("StripeCardId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Brand")
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<long>("ExpiryMonth")
                        .HasColumnType("bigint");

                    b.Property<long>("ExpiryYear")
                        .HasColumnType("bigint");

                    b.Property<string>("Funding")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastFour")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("NickName")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("StripeCardId");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Promo.Promo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Discount")
                        .HasColumnType("double");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Promos");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Recommendation.RecommendationScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<double>("Weight")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("RecommendationScores");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Ticket.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Available")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("QRCode")
                        .HasColumnType("longblob");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Uses")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Transaction.Transaction", b =>
                {
                    b.Property<string>("StripeIntentId")
                        .HasColumnType("varchar(255)");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Paid")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("StripeIntentId");

                    b.HasIndex("EventId");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("longblob");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDarkMode")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("StripeAccountId")
                        .HasColumnType("longtext");

                    b.Property<string>("StripeCustomerId")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Analytic.PageViewEvent", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.Event.Event", "Event")
                        .WithMany("PageViewEvents")
                        .HasForeignKey("EventId");

                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("PageViewEvents")
                        .HasForeignKey("UserId");

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Analytic.SearchEvent", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("SearchEvents")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Analytic.TicketVerificationEvent", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.Event.Event", "Event")
                        .WithMany("VerificationEvents")
                        .HasForeignKey("EventId");

                    b.HasOne("OpenEvent.Web.Models.Ticket.Ticket", "Ticket")
                        .WithMany("VerificationEvents")
                        .HasForeignKey("TicketId");

                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("VerificationEvents")
                        .HasForeignKey("UserId");

                    b.Navigation("Event");

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.BankAccount.BankAccount", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("BankAccounts")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Category.EventCategory", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.Category.Category", "Category")
                        .WithMany("Events")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenEvent.Web.Models.Event.Event", "Event")
                        .WithMany("EventCategories")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Event.Event", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.User.User", "Host")
                        .WithMany("HostedEvents")
                        .HasForeignKey("HostId");

                    b.OwnsOne("OpenEvent.Web.Models.Address.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("AddressLine1")
                                .HasColumnType("longtext");

                            b1.Property<string>("AddressLine2")
                                .HasColumnType("longtext");

                            b1.Property<string>("City")
                                .HasColumnType("longtext");

                            b1.Property<string>("CountryCode")
                                .HasColumnType("longtext");

                            b1.Property<string>("CountryName")
                                .HasColumnType("longtext");

                            b1.Property<double>("Lat")
                                .HasColumnType("double");

                            b1.Property<double>("Lon")
                                .HasColumnType("double");

                            b1.Property<string>("PostalCode")
                                .HasColumnType("longtext");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsMany("OpenEvent.Web.Models.Event.Image", "Images", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("char(36)");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<string>("Label")
                                .HasColumnType("longtext");

                            b1.Property<byte[]>("Source")
                                .HasColumnType("longblob");

                            b1.HasKey("EventId", "Id");

                            b1.ToTable("Events_Images");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsOne("OpenEvent.Web.Models.Event.Image", "Thumbnail", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("char(36)");

                            b1.Property<Guid>("Id")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Label")
                                .HasColumnType("longtext");

                            b1.Property<byte[]>("Source")
                                .HasColumnType("longblob");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsMany("OpenEvent.Web.Models.Event.SocialLink", "SocialLinks", b1 =>
                        {
                            b1.Property<Guid>("EventId")
                                .HasColumnType("char(36)");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("char(36)");

                            b1.Property<string>("Link")
                                .HasColumnType("longtext");

                            b1.Property<int>("SocialMedia")
                                .HasColumnType("int");

                            b1.HasKey("EventId", "Id");

                            b1.ToTable("SocialLink");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("Address");

                    b.Navigation("Host");

                    b.Navigation("Images");

                    b.Navigation("SocialLinks");

                    b.Navigation("Thumbnail");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.PaymentMethod.PaymentMethod", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Promo.Promo", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.Event.Event", "Event")
                        .WithMany("Promos")
                        .HasForeignKey("EventId");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Recommendation.RecommendationScore", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.Category.Category", "Category")
                        .WithMany("Scores")
                        .HasForeignKey("CategoryId");

                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("RecommendationScores")
                        .HasForeignKey("UserId");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Ticket.Ticket", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.Event.Event", "Event")
                        .WithMany("Tickets")
                        .HasForeignKey("EventId");

                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId");

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Transaction.Transaction", b =>
                {
                    b.HasOne("OpenEvent.Web.Models.Event.Event", "Event")
                        .WithMany("Transactions")
                        .HasForeignKey("EventId");

                    b.HasOne("OpenEvent.Web.Models.Ticket.Ticket", "Ticket")
                        .WithOne("Transaction")
                        .HasForeignKey("OpenEvent.Web.Models.Transaction.Transaction", "TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenEvent.Web.Models.User.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId");

                    b.Navigation("Event");

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.User.User", b =>
                {
                    b.OwnsOne("OpenEvent.Web.Models.Address.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("AddressLine1")
                                .HasColumnType("longtext");

                            b1.Property<string>("AddressLine2")
                                .HasColumnType("longtext");

                            b1.Property<string>("City")
                                .HasColumnType("longtext");

                            b1.Property<string>("CountryCode")
                                .HasColumnType("longtext");

                            b1.Property<string>("CountryName")
                                .HasColumnType("longtext");

                            b1.Property<double>("Lat")
                                .HasColumnType("double");

                            b1.Property<double>("Lon")
                                .HasColumnType("double");

                            b1.Property<string>("PostalCode")
                                .HasColumnType("longtext");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Category.Category", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Scores");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Event.Event", b =>
                {
                    b.Navigation("EventCategories");

                    b.Navigation("PageViewEvents");

                    b.Navigation("Promos");

                    b.Navigation("Tickets");

                    b.Navigation("Transactions");

                    b.Navigation("VerificationEvents");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Ticket.Ticket", b =>
                {
                    b.Navigation("Transaction");

                    b.Navigation("VerificationEvents");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.User.User", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("HostedEvents");

                    b.Navigation("PageViewEvents");

                    b.Navigation("PaymentMethods");

                    b.Navigation("RecommendationScores");

                    b.Navigation("SearchEvents");

                    b.Navigation("Tickets");

                    b.Navigation("Transactions");

                    b.Navigation("VerificationEvents");
                });
#pragma warning restore 612, 618
        }
    }
#pragma warning restore CS1591
#pragma warning restore CS1591
}
