﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TravelAndAccommodationBookingPlatform.Db.DbContext;

#nullable disable

namespace TravelAndAccommodationBookingPlatform.Db.Migrations
{
    [DbContext(typeof(TravelAndAccommodationBookingPlatformDbContext))]
    partial class TravelAndAccommodationBookingPlatformDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Amenity", b =>
                {
                    b.Property<Guid>("AmenityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AmenityName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("AmenityID");

                    b.ToTable("Amenities");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Booking", b =>
                {
                    b.Property<Guid>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("CheckOutDate")
                        .HasColumnType("timestamp");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("BookingId");

                    b.HasIndex("CheckInDate");

                    b.HasIndex("CheckOutDate");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.City", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PostCode")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("CityId");

                    b.HasIndex("CityName")
                        .IsUnique();

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Discount", b =>
                {
                    b.Property<Guid>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("DiscountType")
                        .HasColumnType("integer");

                    b.Property<double>("DiscountValue")
                        .HasColumnType("double precision");

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
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("HotelName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StarRating")
                        .HasColumnType("integer");

                    b.HasKey("HotelId");

                    b.HasIndex("HotelName");

                    b.HasIndex("OwnerId");

                    b.HasIndex("CityId", "HotelName");

                    b.ToTable("Hotels");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.HotelAmenity", b =>
                {
                    b.Property<Guid>("HotelAmenityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AmenityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uuid");

                    b.HasKey("HotelAmenityId");

                    b.HasIndex("AmenityId");

                    b.HasIndex("HotelId");

                    b.ToTable("HotelAmenities");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.HotelDiscount", b =>
                {
                    b.Property<Guid>("HotelDiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DiscountId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("HotelId")
                        .HasColumnType("uuid");

                    b.HasKey("HotelDiscountId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("HotelId");

                    b.ToTable("HotelDiscounts");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Owner", b =>
                {
                    b.Property<Guid>("OwnerId")
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

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("OwnerId");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uuid");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TransactionID")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("PaymentId");

                    b.HasIndex("BookingId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Review", b =>
                {
                    b.Property<Guid>("ReviewID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("HotelID")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uuid");

                    b.HasKey("ReviewID");

                    b.HasIndex("HotelID");

                    b.HasIndex("UserID");

                    b.ToTable("Reviews");
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

                    b.Property<decimal>("PricePerNight")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("RoomNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int>("RoomType")
                        .HasColumnType("integer");

                    b.HasKey("RoomId");

                    b.HasIndex("RoomNumber");

                    b.HasIndex("HotelId", "RoomNumber")
                        .IsUnique();

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.RoomAmenity", b =>
                {
                    b.Property<Guid>("RoomAmenityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AmenityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("RoomAmenityId");

                    b.HasIndex("AmenityId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomAmenities");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.RoomDiscount", b =>
                {
                    b.Property<Guid>("RoomDiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DiscountId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("RoomDiscountId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomDiscounts");
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
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

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
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", "Room")
                        .WithMany("Bookings")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.City", "City")
                        .WithMany("Hotels")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Owner", "Owner")
                        .WithMany("Hotels")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.HotelAmenity", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Amenity", "Amenity")
                        .WithMany("HotelAmenities")
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", "Hotel")
                        .WithMany("HotelAmenities")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenity");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.HotelDiscount", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Discount", "Discount")
                        .WithMany("HotelDiscounts")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", "Hotel")
                        .WithMany("HotelDiscounts")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Payment", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Booking", "Booking")
                        .WithMany("Payments")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Review", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", "Hotel")
                        .WithMany("Reviews")
                        .HasForeignKey("HotelID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", "Hotel")
                        .WithMany("Rooms")
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hotel");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.RoomAmenity", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Amenity", "Amenity")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", "Room")
                        .WithMany("RoomAmenities")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenity");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.RoomDiscount", b =>
                {
                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Discount", "Discount")
                        .WithMany("RoomDiscounts")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", "Room")
                        .WithMany("RoomDiscounts")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Amenity", b =>
                {
                    b.Navigation("HotelAmenities");

                    b.Navigation("RoomAmenities");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Booking", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.City", b =>
                {
                    b.Navigation("Hotels");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Discount", b =>
                {
                    b.Navigation("HotelDiscounts");

                    b.Navigation("RoomDiscounts");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Hotel", b =>
                {
                    b.Navigation("HotelAmenities");

                    b.Navigation("HotelDiscounts");

                    b.Navigation("Reviews");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Owner", b =>
                {
                    b.Navigation("Hotels");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.Room", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("RoomAmenities");

                    b.Navigation("RoomDiscounts");
                });

            modelBuilder.Entity("TravelAndAccommodationBookingPlatform.Domain.Entities.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
