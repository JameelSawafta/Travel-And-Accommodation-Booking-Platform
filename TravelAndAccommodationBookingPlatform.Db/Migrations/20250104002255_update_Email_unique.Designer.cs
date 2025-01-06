﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TravelAndAccommodationBookingPlatform.Db.DbContext;

#nullable disable

namespace TravelAndAccommodationBookingPlatform.Db.Migrations
{
    [DbContext(typeof(TravelAndAccommodationBookingPlatformDbContext))]
    [Migration("20250104002255_update_Email_unique")]
    partial class update_Email_unique
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Booking", b =>
                {
                    b.Property<Guid>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("CheckOutDate")
                        .HasColumnType("timestamp");

                    b.Property<Guid>("DiscountId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("BookingId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.BookingDetail", b =>
                {
                    b.Property<Guid>("BookingDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DiscountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("BookingDetailsId");

                    b.HasIndex("BookingId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("RoomId");

                    b.ToTable("BookingDetails");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.City", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PostCode")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PostOfficeCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("CityId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Discount", b =>
                {
                    b.Property<Guid>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("DiscountType")
                        .HasColumnType("integer");

                    b.Property<double>("DiscountValue")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("timestamp");

                    b.HasKey("DiscountId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", b =>
                {
                    b.Property<Guid>("HotelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Owner")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StarRating")
                        .HasColumnType("integer");

                    b.HasKey("HotelId");

                    b.HasIndex("CityId");

                    b.ToTable("Hotels");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uuid");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp");

                    b.HasKey("PaymentId");

                    b.HasIndex("BookingId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", b =>
                {
                    b.Property<Guid>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AdultsCapacity")
                        .HasColumnType("integer");

                    b.Property<bool>("Availability")
                        .HasColumnType("boolean");

                    b.Property<int>("ChildrenCapacity")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("RoomType")
                        .HasColumnType("integer");

                    b.HasKey("RoomId");

                    b.HasIndex("HotelId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Booking", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.BookingDetail", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Discount");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Payment", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", "Hotel")
                        .WithMany()
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });
#pragma warning restore 612, 618
        }
    }
}
