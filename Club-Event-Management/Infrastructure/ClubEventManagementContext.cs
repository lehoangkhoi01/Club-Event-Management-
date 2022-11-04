using ApplicationCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ClubEventManagementContext : DbContext
    {
        public ClubEventManagementContext() { }
        public ClubEventManagementContext(DbContextOptions<ClubEventManagementContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("ClubEventManagement");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventClubProfile>()
                .HasKey(x => new { x.EventId, x.ClubProfileId });

            modelBuilder.Entity<ClubProfileStudentAccount>()
                .HasKey(x => new { x.StudentAccountId, x.ClubProfileId });
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<AdminAccount> AdminAccounts { get; set; }
        public DbSet<StudentAccount> StudentAccounts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ClubProfile> ClubProfiles { get; set; }
        public DbSet<EventActivity> EventActivities { get; set; }
        public DbSet<EventPost> EventPosts { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<EventClubProfile> EventClubProfile { get; set; }
        public DbSet<ClubProfileStudentAccount> ClubProfileStudentAccount { get; set; }

    }
}
