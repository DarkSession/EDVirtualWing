﻿// <auto-generated />
using System;
using ED_Virtual_Wing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ED_Virtual_Wing.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes", (string)null);
                });

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Key", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Algorithm")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("DataProtected")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsX509Certificate")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Use")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Use");

                    b.ToTable("Keys");
                });

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime?>("ConsumedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Key");

                    b.HasIndex("ConsumedTime");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.HasIndex("SubjectId", "SessionId", "Type");

                    b.ToTable("PersistedGrants", (string)null);
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.Commander", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("CommanderId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ExtraFlags")
                        .HasColumnType("int");

                    b.Property<int>("GameActivity")
                        .HasColumnType("int");

                    b.Property<short>("GameMode")
                        .HasColumnType("smallint");

                    b.Property<string>("GameModeGroupName")
                        .HasColumnType("varchar(256)");

                    b.Property<short>("GameVersion")
                        .HasColumnType("smallint");

                    b.Property<DateTimeOffset>("JournalLastEventDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<int?>("Ship")
                        .HasColumnType("int");

                    b.Property<decimal>("ShipHullHealth")
                        .HasColumnType("decimal(14,8)");

                    b.Property<short>("Suit")
                        .HasColumnType("smallint");

                    b.Property<int?>("TargetId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<long>("VehicleStatusFlags")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("TargetId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Commander");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.CommanderLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Altitude")
                        .HasColumnType("decimal(14,6)");

                    b.Property<int>("CommanderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(14,6)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(14,6)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256)");

                    b.Property<long?>("StarSystemId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StationId")
                        .HasColumnType("bigint");

                    b.Property<long?>("SystemBodyId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CommanderId");

                    b.HasIndex("StarSystemId");

                    b.HasIndex("StationId");

                    b.HasIndex("SystemBodyId");

                    b.ToTable("CommanderLocation");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.CommanderTarget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long?>("BodyId")
                        .HasColumnType("bigint");

                    b.Property<int>("CommanderId")
                        .HasColumnType("int");

                    b.Property<int>("FallbackBodyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<int?>("ShipTarget")
                        .HasColumnType("int");

                    b.Property<string>("ShipTargetName")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<long?>("StarSystemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BodyId");

                    b.HasIndex("CommanderId");

                    b.HasIndex("StarSystemId");

                    b.ToTable("CommanderTarget");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.StarSystem", b =>
                {
                    b.Property<long>("SystemAddress")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<decimal>("LocationX")
                        .HasColumnType("decimal(14,6)");

                    b.Property<decimal>("LocationY")
                        .HasColumnType("decimal(14,6)");

                    b.Property<decimal>("LocationZ")
                        .HasColumnType("decimal(14,6)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(512)");

                    b.HasKey("SystemAddress");

                    b.ToTable("StarSystem");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.StarSystemBody", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<int>("BodyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<long>("StarSystemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StarSystemId");

                    b.ToTable("StarSystemBody");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.Station", b =>
                {
                    b.Property<long>("MarketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<decimal>("DistanceFromStarLS")
                        .HasColumnType("decimal(14,6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NameAddon")
                        .HasColumnType("varchar(256)");

                    b.Property<long?>("StarSystemId")
                        .HasColumnType("bigint");

                    b.Property<short>("StationType")
                        .HasColumnType("smallint");

                    b.HasKey("MarketId");

                    b.HasIndex("StarSystemId");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.TranslationsPending", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("LocalizedExample")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NonLocalized")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.ToTable("TranslationsPending");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.Wing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<short>("JoinRequirement")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("varchar(255)");

                    b.Property<short>("Status")
                        .HasColumnType("smallint");

                    b.Property<Guid>("WingId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("WingId")
                        .IsUnique();

                    b.ToTable("Wing");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.WingMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Joined")
                        .HasColumnType("datetime(6)");

                    b.Property<short>("Status")
                        .HasColumnType("smallint");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("WingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WingId");

                    b.ToTable("WingMember");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.Commander", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.CommanderLocation", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("ED_Virtual_Wing.Models.CommanderTarget", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId");

                    b.HasOne("ED_Virtual_Wing.Models.ApplicationUser", "User")
                        .WithOne("Commander")
                        .HasForeignKey("ED_Virtual_Wing.Models.Commander", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Target");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.CommanderLocation", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.Commander", "Commander")
                        .WithMany()
                        .HasForeignKey("CommanderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ED_Virtual_Wing.Models.StarSystem", "StarSystem")
                        .WithMany()
                        .HasForeignKey("StarSystemId");

                    b.HasOne("ED_Virtual_Wing.Models.Station", "Station")
                        .WithMany()
                        .HasForeignKey("StationId");

                    b.HasOne("ED_Virtual_Wing.Models.StarSystemBody", "SystemBody")
                        .WithMany()
                        .HasForeignKey("SystemBodyId");

                    b.Navigation("Commander");

                    b.Navigation("StarSystem");

                    b.Navigation("Station");

                    b.Navigation("SystemBody");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.CommanderTarget", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.StarSystemBody", "Body")
                        .WithMany()
                        .HasForeignKey("BodyId");

                    b.HasOne("ED_Virtual_Wing.Models.Commander", "Commander")
                        .WithMany()
                        .HasForeignKey("CommanderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ED_Virtual_Wing.Models.StarSystem", "StarSystem")
                        .WithMany()
                        .HasForeignKey("StarSystemId");

                    b.Navigation("Body");

                    b.Navigation("Commander");

                    b.Navigation("StarSystem");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.StarSystemBody", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.StarSystem", "StarSystem")
                        .WithMany("Bodies")
                        .HasForeignKey("StarSystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StarSystem");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.Station", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.StarSystem", "StarSystem")
                        .WithMany()
                        .HasForeignKey("StarSystemId");

                    b.Navigation("StarSystem");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.Wing", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.WingMember", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.ApplicationUser", "User")
                        .WithMany("WingMemberships")
                        .HasForeignKey("UserId");

                    b.HasOne("ED_Virtual_Wing.Models.Wing", "Wing")
                        .WithMany("Members")
                        .HasForeignKey("WingId");

                    b.Navigation("User");

                    b.Navigation("Wing");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ED_Virtual_Wing.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ED_Virtual_Wing.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.ApplicationUser", b =>
                {
                    b.Navigation("Commander");

                    b.Navigation("WingMemberships");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.StarSystem", b =>
                {
                    b.Navigation("Bodies");
                });

            modelBuilder.Entity("ED_Virtual_Wing.Models.Wing", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
