﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Web.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210203184115_eventsAndTickets")]
    partial class eventsAndTickets
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.2");

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

                    b.Property<decimal>("Price")
                        .HasColumnType("money(65,30)");

                    b.Property<DateTime>("StartLocal")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartUTC")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Ticket.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("QRCode")
                        .HasColumnType("longblob");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
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

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
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

                    b.OwnsOne("OpenEvent.Web.Models.Event.Address", "Address", b1 =>
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

                            b1.ToTable("Image");

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

            modelBuilder.Entity("OpenEvent.Web.Models.Category.Category", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.Event.Event", b =>
                {
                    b.Navigation("EventCategories");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("OpenEvent.Web.Models.User.User", b =>
                {
                    b.Navigation("HostedEvents");

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
