﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pomodoro.DataAccess.EF;

#nullable disable

namespace Pomodoro.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("PomoIdentityUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex(new[] { "PomoIdentityUserId" }, "IX_Users_AspNetUserId")
                        .IsUnique();

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.Completed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ActualDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<bool>("IsDone")
                        .HasColumnType("bit");

                    b.Property<float>("PomodorosCount")
                        .HasColumnType("real");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TimeSpent")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("CompletedTasks");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.Frequency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("Every")
                        .HasColumnType("smallint");

                    b.Property<Guid>("FrequencyTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCustom")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("FrequencyTypeId");

                    b.ToTable("Frequencies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000002-0001-0003-0001-020304050607"),
                            Every = (short)0,
                            FrequencyTypeId = new Guid("00000001-0002-0003-0001-020304050607"),
                            IsCustom = false
                        },
                        new
                        {
                            Id = new Guid("00000002-0002-0003-0001-020304050607"),
                            Every = (short)1,
                            FrequencyTypeId = new Guid("00000002-0002-0003-0001-020304050607"),
                            IsCustom = false
                        },
                        new
                        {
                            Id = new Guid("00000002-0003-0003-0001-020304050607"),
                            Every = (short)1,
                            FrequencyTypeId = new Guid("00000003-0002-0003-0001-020304050607"),
                            IsCustom = false
                        },
                        new
                        {
                            Id = new Guid("00000002-0004-0003-0001-020304050607"),
                            Every = (short)1,
                            FrequencyTypeId = new Guid("00000004-0002-0003-0001-020304050607"),
                            IsCustom = false
                        },
                        new
                        {
                            Id = new Guid("00000002-0005-0003-0001-020304050607"),
                            Every = (short)1,
                            FrequencyTypeId = new Guid("00000005-0002-0003-0001-020304050607"),
                            IsCustom = false
                        },
                        new
                        {
                            Id = new Guid("00000002-0006-0003-0001-020304050607"),
                            Every = (short)1,
                            FrequencyTypeId = new Guid("00000006-0002-0003-0001-020304050607"),
                            IsCustom = false
                        },
                        new
                        {
                            Id = new Guid("00000002-0007-0003-0001-020304050607"),
                            Every = (short)1,
                            FrequencyTypeId = new Guid("00000007-0002-0003-0001-020304050607"),
                            IsCustom = false
                        });
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.FrequencyType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("varchar(7)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("FrequencyTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000001-0002-0003-0001-020304050607"),
                            Value = "None"
                        },
                        new
                        {
                            Id = new Guid("00000002-0002-0003-0001-020304050607"),
                            Value = "Day"
                        },
                        new
                        {
                            Id = new Guid("00000003-0002-0003-0001-020304050607"),
                            Value = "Week"
                        },
                        new
                        {
                            Id = new Guid("00000004-0002-0003-0001-020304050607"),
                            Value = "Month"
                        },
                        new
                        {
                            Id = new Guid("00000005-0002-0003-0001-020304050607"),
                            Value = "Year"
                        },
                        new
                        {
                            Id = new Guid("00000006-0002-0003-0001-020304050607"),
                            Value = "Workday"
                        },
                        new
                        {
                            Id = new Guid("00000007-0002-0003-0001-020304050607"),
                            Value = "Weekend"
                        });
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.PomoIdentityUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.Settings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AutostartEnabled")
                        .HasColumnType("bit");

                    b.Property<byte>("LongBreak")
                        .HasColumnType("tinyint");

                    b.Property<byte>("PomodoroDuration")
                        .HasColumnType("tinyint");

                    b.Property<byte>("PomodorosBeforeLongBreak")
                        .HasColumnType("tinyint");

                    b.Property<byte>("ShortBreak")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("AllocatedTime")
                        .HasColumnType("smallint");

                    b.Property<Guid>("FrequencyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("InitialDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FrequencyId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.PomoIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.PomoIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pomodoro.DataAccess.Entities.PomoIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.PomoIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.AppUser", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.PomoIdentityUser", "PomoIdentityUser")
                        .WithOne("AppUser")
                        .HasForeignKey("Pomodoro.DataAccess.Entities.AppUser", "PomoIdentityUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PomoIdentityUser");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.Completed", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.TaskEntity", "Task")
                        .WithMany("CompletedTasks")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.Frequency", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.FrequencyType", "FrequencyType")
                        .WithMany("Frequencies")
                        .HasForeignKey("FrequencyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FrequencyType");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.Settings", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.AppUser", "User")
                        .WithOne("Settings")
                        .HasForeignKey("Pomodoro.DataAccess.Entities.Settings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.TaskEntity", b =>
                {
                    b.HasOne("Pomodoro.DataAccess.Entities.Frequency", "Frequency")
                        .WithMany()
                        .HasForeignKey("FrequencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pomodoro.DataAccess.Entities.AppUser", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Frequency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.AppUser", b =>
                {
                    b.Navigation("Settings");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.FrequencyType", b =>
                {
                    b.Navigation("Frequencies");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.PomoIdentityUser", b =>
                {
                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("Pomodoro.DataAccess.Entities.TaskEntity", b =>
                {
                    b.Navigation("CompletedTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
