﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ClubEventManagementContext))]
    partial class ClubEventManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApplicationCore.AdminAccount", b =>
                {
                    b.Property<int>("AdminAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserIdentityEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AdminAccountId");

                    b.HasIndex("UserIdentityEmail");

                    b.ToTable("AdminAccounts");
                });

            modelBuilder.Entity("ApplicationCore.ClubProfile", b =>
                {
                    b.Property<int>("ClubProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClubName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FoundationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SocialAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalMember")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ClubProfileId");

                    b.ToTable("ClubProfiles");
                });

            modelBuilder.Entity("ApplicationCore.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedClub")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatedClubId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EventEndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EventStartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Place")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalFollow")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EventCategoryId");

                    b.HasIndex("EventTypeId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ApplicationCore.EventActivity", b =>
                {
                    b.Property<int>("EventActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventActivityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("EventActivityId");

                    b.HasIndex("EventId");

                    b.ToTable("EventActivities");
                });

            modelBuilder.Entity("ApplicationCore.EventCategory", b =>
                {
                    b.Property<int>("EventCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventCategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventCategoryId");

                    b.ToTable("EventCategories");
                });

            modelBuilder.Entity("ApplicationCore.EventPost", b =>
                {
                    b.Property<int>("EventPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("EventPostId");

                    b.HasIndex("EventId");

                    b.HasIndex("StudentAccountId");

                    b.ToTable("EventPosts");
                });

            modelBuilder.Entity("ApplicationCore.EventType", b =>
                {
                    b.Property<int>("EventTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventTypeId");

                    b.ToTable("EventTypes");
                });

            modelBuilder.Entity("ApplicationCore.PostReaction", b =>
                {
                    b.Property<int>("PostReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventPostId")
                        .HasColumnType("int");

                    b.Property<bool>("IsLike")
                        .HasColumnType("bit");

                    b.HasKey("PostReactionId");

                    b.HasIndex("EventPostId");

                    b.ToTable("PostReactions");
                });

            modelBuilder.Entity("ApplicationCore.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ApplicationCore.StudentAccount", b =>
                {
                    b.Property<int>("StudentAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserIdentityEmail")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StudentAccountId");

                    b.HasIndex("UserIdentityEmail");

                    b.ToTable("StudentAccounts");
                });

            modelBuilder.Entity("ApplicationCore.UserIdentity", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Email");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClubProfileEvent", b =>
                {
                    b.Property<int>("ClubProfilesClubProfileId")
                        .HasColumnType("int");

                    b.Property<int>("EventsId")
                        .HasColumnType("int");

                    b.HasKey("ClubProfilesClubProfileId", "EventsId");

                    b.HasIndex("EventsId");

                    b.ToTable("ClubProfileEvent");
                });

            modelBuilder.Entity("ClubProfileStudentAccount", b =>
                {
                    b.Property<int>("ClubProfilesClubProfileId")
                        .HasColumnType("int");

                    b.Property<int>("StudentAccountsStudentAccountId")
                        .HasColumnType("int");

                    b.HasKey("ClubProfilesClubProfileId", "StudentAccountsStudentAccountId");

                    b.HasIndex("StudentAccountsStudentAccountId");

                    b.ToTable("ClubProfileStudentAccount");
                });

            modelBuilder.Entity("ApplicationCore.AdminAccount", b =>
                {
                    b.HasOne("ApplicationCore.UserIdentity", "UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserIdentityEmail");

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("ApplicationCore.Event", b =>
                {
                    b.HasOne("ApplicationCore.EventCategory", "EventCategory")
                        .WithMany()
                        .HasForeignKey("EventCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApplicationCore.EventType", "EventType")
                        .WithMany()
                        .HasForeignKey("EventTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventCategory");

                    b.Navigation("EventType");
                });

            modelBuilder.Entity("ApplicationCore.EventActivity", b =>
                {
                    b.HasOne("ApplicationCore.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("ApplicationCore.EventPost", b =>
                {
                    b.HasOne("ApplicationCore.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApplicationCore.StudentAccount", "StudentAccount")
                        .WithMany()
                        .HasForeignKey("StudentAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("StudentAccount");
                });

            modelBuilder.Entity("ApplicationCore.PostReaction", b =>
                {
                    b.HasOne("ApplicationCore.EventPost", "EventPost")
                        .WithMany()
                        .HasForeignKey("EventPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventPost");
                });

            modelBuilder.Entity("ApplicationCore.StudentAccount", b =>
                {
                    b.HasOne("ApplicationCore.UserIdentity", "UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserIdentityEmail");

                    b.Navigation("UserIdentity");
                });

            modelBuilder.Entity("ApplicationCore.UserIdentity", b =>
                {
                    b.HasOne("ApplicationCore.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ClubProfileEvent", b =>
                {
                    b.HasOne("ApplicationCore.ClubProfile", null)
                        .WithMany()
                        .HasForeignKey("ClubProfilesClubProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApplicationCore.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClubProfileStudentAccount", b =>
                {
                    b.HasOne("ApplicationCore.ClubProfile", null)
                        .WithMany()
                        .HasForeignKey("ClubProfilesClubProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApplicationCore.StudentAccount", null)
                        .WithMany()
                        .HasForeignKey("StudentAccountsStudentAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
