using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RescueSystem.Domain.Entities;
using RescueSystem.Domain.Enums;

namespace RescueSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Location> Locations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RescueTeam> RescueTeams { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.Avatar)
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure Role entity
            builder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure Identity tables
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            // Configure Location entity
            builder.Entity<Location>(entity =>
            {
                entity.ToTable("Locations");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Latitude)
                    .IsRequired();

                entity.Property(e => e.Longitude)
                    .IsRequired();

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.Landmark)
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure Address entity
            builder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Street)
                    .HasMaxLength(255);

                entity.Property(e => e.City)
                    .HasMaxLength(100);

                entity.Property(e => e.District)
                    .HasMaxLength(100);

                entity.Property(e => e.GPS)
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure Contact entity
            builder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contacts");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(e => e.Relationship)
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure Request entity
            builder.Entity<Request>(entity =>
            {
                entity.ToTable("Requests");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.EmergencyType)
                    .IsRequired();

                entity.Property(e => e.Priority)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValue(RequestStatus.PENDING);

                entity.Property(e => e.LocationId)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(2000);

                entity.Property(e => e.MediaUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.SubmittedTime)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Foreign Key: User -> Request (RequestedBy)
                entity.HasOne(e => e.RequestedBy)
                    .WithMany(u => u.Requests)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Foreign Key: Location -> Request
                entity.HasOne(e => e.Location)
                    .WithMany()
                    .HasForeignKey(e => e.LocationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure RescueTeam entity
            builder.Entity<RescueTeam>(entity =>
            {
                entity.ToTable("RescueTeams");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TeamName)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(e => e.TeamLeaderId)
                    .IsRequired();

                entity.Property(e => e.BaseLocationId)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValue(TeamStatus.AVAILABLE);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Foreign Key: User -> RescueTeam (TeamLeader)
                entity.HasOne(e => e.TeamLeader)
                    .WithMany(u => u.LeadingTeams)
                    .HasForeignKey(e => e.TeamLeaderId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Foreign Key: Location -> RescueTeam (BaseLocation)
                entity.HasOne(e => e.BaseLocation)
                    .WithMany()
                    .HasForeignKey(e => e.BaseLocationId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Many-to-Many: RescueTeam <-> User (Members)
                entity.HasMany(e => e.Members)
                    .WithMany(u => u.TeamsAsMember)
                    .UsingEntity(j => j.ToTable("TeamMembers"));
            });

            // Configure Mission entity
            builder.Entity<Mission>(entity =>
            {
                entity.ToTable("Missions");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.RequestId)
                    .IsRequired();

                entity.Property(e => e.DispatcherId)
                    .IsRequired();

                entity.Property(e => e.RescueTeamId)
                    .IsRequired();

                entity.Property(e => e.StartTime)
                    .IsRequired();

                entity.Property(e => e.EndTime);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValue(MissionStatus.ASSIGNED);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Foreign Key: Request -> Mission
                entity.HasOne(e => e.Request)
                    .WithMany(r => r.Missions)
                    .HasForeignKey(e => e.RequestId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Foreign Key: User -> Mission (Dispatcher)
                entity.HasOne(e => e.Dispatcher)
                    .WithMany(u => u.DispatchedMissions)
                    .HasForeignKey(e => e.DispatcherId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Foreign Key: RescueTeam -> Mission
                entity.HasOne(e => e.RescueTeam)
                    .WithMany(rt => rt.Missions)
                    .HasForeignKey(e => e.RescueTeamId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Report entity
            builder.Entity<Report>(entity =>
            {
                entity.ToTable("Reports");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.MissionId)
                    .IsRequired();

                entity.Property(e => e.CreatedById)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .HasMaxLength(5000);

                entity.Property(e => e.AttachmentUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.Type)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Foreign Key: Mission -> Report
                entity.HasOne(e => e.Mission)
                    .WithMany(m => m.Reports)
                    .HasForeignKey(e => e.MissionId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Foreign Key: User -> Report (CreatedBy)
                entity.HasOne(e => e.CreatedBy)
                    .WithMany(u => u.Reports)
                    .HasForeignKey(e => e.CreatedById)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
