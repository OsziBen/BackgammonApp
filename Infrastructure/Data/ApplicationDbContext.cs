using Common.Models;
using Domain.AppRole;
using Domain.Comment;
using Domain.Group;
using Domain.GroupMembership;
using Domain.GroupMembershipRole;
using Domain.GroupRole;
using Domain.Post;
using Domain.Reaction;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        static readonly DateTimeOffset SeedTimestamp = new DateTimeOffset(2025, 01, 01, 0, 0, 0, TimeSpan.Zero);
        static readonly DateOnly SeedDate = new DateOnly(1990, 1, 1);

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<AppRole> AppRoles { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<GroupRole> GroupRoles { get; set; } = null!;
        public DbSet<GroupMembership> GroupMemberships { get; set; } = null!;
        public DbSet<GroupMembershipRole> GroupMembershipRoles { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Reaction> Reactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);

                user.HasIndex(u => u.EmailAddress)
                    .IsUnique();

                user.HasOne(u => u.AppRole)
                    .WithMany(ar => ar.Users)
                    .HasForeignKey(u => u.AppRoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                user.Property(u => u.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                user.Property(u => u.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                user.Property(u => u.UserName)
                    .HasMaxLength(50)
                    .IsRequired();

                user.Property(u => u.EmailAddress)
                    .HasMaxLength(50)
                    .IsRequired();

                user.Property(u => u.PasswordHash)
                    .HasMaxLength(200)
                    .IsRequired();

                user.Property(u => u.ProfilePictureUrl)
                    .HasMaxLength(200)
                    .IsRequired();

                user.Property(u => u.Rating)
                    .HasDefaultValue(0);

                user.Property(u => u.ExperiencePoints)
                    .HasDefaultValue(0);

                user.Property(u => u.IsBanned)
                    .HasDefaultValue(false);

                user.Property(u => u.IsDeleted)
                    .HasDefaultValue(false);

                user.Property(u => u.IsSystemUser)
                    .HasDefaultValue(false);

                user.HasData(
                    new User
                    {
                        Id = SystemUsers.System,
                        FirstName = "system",
                        LastName = "system",
                        UserName = "System",
                        EmailAddress = "system@local",
                        PasswordHash = "hash_this_later",
                        DateOfBirth = SeedDate,
                        ProfilePictureUrl = "",
                        AppRoleId = DefaultAppRoles.AdminRoleId,
                        Rating = 0,
                        ExperiencePoints = 0,
                        IsBanned = false,
                        IsSystemUser = true,
                        IsDeleted = false,
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp,
                        DeletedAt = null
                    },
                    new User
                    {
                        Id = SystemUsers.DeletedUser,
                        FirstName = "deleted",
                        LastName = "deleted",
                        UserName = "Deleted User",
                        EmailAddress = "deleted@local",
                        PasswordHash = "hash_this_later",
                        DateOfBirth = SeedDate,
                        ProfilePictureUrl = "",
                        AppRoleId = DefaultAppRoles.UserRoleId,
                        Rating = 0,
                        ExperiencePoints = 0,
                        IsBanned = false,
                        IsSystemUser = false,
                        IsDeleted = false,
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp,
                        DeletedAt = null
                    },
                    new User
                    {
                        Id = SystemUsers.SystemAdmin,
                        FirstName = "Admin",
                        LastName = "User",
                        UserName = "admin",
                        EmailAddress = "admin@backgammonapp.com",
                        PasswordHash = "hash_this_later",
                        DateOfBirth = SeedDate,
                        ProfilePictureUrl = "",
                        AppRoleId = DefaultAppRoles.AdminRoleId,
                        Rating = 0,
                        ExperiencePoints = 0,
                        IsBanned = false,
                        IsSystemUser = false,
                        IsDeleted = false,
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp,
                        DeletedAt = null
                    });
            });

            modelBuilder.Entity<AppRole>(appRole =>
            {
                appRole.HasKey(ar => ar.Id);

                appRole.HasIndex(ar => ar.Name)
                       .IsUnique();

                appRole.Property(ar => ar.Name)
                       .HasMaxLength(20)
                       .IsRequired();

                appRole.Property(ar => ar.Description)
                       .HasMaxLength(200)
                       .IsRequired();

                appRole.HasData(
                    new AppRole
                    {
                        Id = DefaultAppRoles.AdminRoleId,
                        Name = "Admin",
                        Description = "Default administrator role",
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp
                    },
                    new AppRole
                    {
                        Id = DefaultAppRoles.UserRoleId,
                        Name = "User",
                        Description = "Default user role",
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp
                    });
            });

            modelBuilder.Entity<Group>(group =>
            {
                group.HasKey(g => g.Id);

                group.HasOne(g => g.Creator)
                     .WithMany(u => u.CreatedGroups)
                     .HasForeignKey(g => g.CreatorId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Restrict);

                group.Property(g => g.Name)
                     .HasMaxLength(100)
                     .IsRequired();

                group.Property(g => g.Description)
                     .HasMaxLength(200)
                     .IsRequired();

                group.Property(g => g.GroupType)
                     .HasConversion<int>()
                     .IsRequired();

                group.Property(g => g.MaxMembers)
                     .HasDefaultValue(50);

                group.Property(g => g.IsDeleted)
                     .HasDefaultValue(false);
            });

            modelBuilder.Entity<GroupRole>(groupRole =>
            {
                groupRole.HasKey(gr => gr.Id);

                groupRole.HasIndex(gr => new { gr.GroupId, gr.Name })
                         .IsUnique();

                groupRole.HasOne(gr => gr.Group)
                         .WithMany(g => g.GroupRoles)
                         .HasForeignKey(gr => gr.GroupId)
                         .IsRequired(false)
                         .OnDelete(DeleteBehavior.Cascade);

                groupRole.Property(gr => gr.Name)
                         .HasMaxLength(20)
                         .IsRequired();

                groupRole.Property(gr => gr.Description)
                         .HasMaxLength(200)
                         .IsRequired();

                groupRole.Property(g => g.IsDeleted)
                         .HasDefaultValue(false);

                groupRole.HasData(
                    new GroupRole
                    {
                        Id = DefaultGroupRoles.Owner,
                        Name = "Owner",
                        Description = "Group creator and top-level admin",
                        GroupId = null,
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp
                    },
                    new GroupRole
                    {
                        Id = DefaultGroupRoles.Moderator,
                        Name = "Moderator",
                        Description = "Can manage content and members",
                        GroupId = null,
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp
                    },
                    new GroupRole
                    {
                        Id = DefaultGroupRoles.Member,
                        Name = "Member",
                        Description = "Standard group participant",
                        GroupId = null,
                        CreatedAt = SeedTimestamp,
                        LastUpdatedAt = SeedTimestamp
                    });
            });

            modelBuilder.Entity<GroupMembership>(groupMembership =>
            {
                groupMembership.HasKey(gm => gm.Id);

                groupMembership.HasIndex(gm => new { gm.UserId, gm.GroupId })
                               .IsUnique();

                groupMembership.HasOne(gm => gm.User)
                               .WithMany(u => u.GroupMemberships)
                               .HasForeignKey(gm => gm.UserId)
                               .IsRequired()
                               .OnDelete(DeleteBehavior.Restrict);

                groupMembership.HasOne(gm => gm.Group)
                               .WithMany(g => g.GroupMemberships)
                               .HasForeignKey(gm => gm.GroupId)
                               .IsRequired()
                               .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GroupMembershipRole>(groupMembershipRole =>
            {
                groupMembershipRole.HasKey(gmr => new { gmr.GroupMembershipId, gmr.GroupRoleId });

                groupMembershipRole.HasOne(gmr => gmr.GroupMembership)
                                   .WithMany(gm => gm.GroupRoles)
                                   .HasForeignKey(gmr => gmr.GroupMembershipId)
                                   .OnDelete(DeleteBehavior.Cascade);

                groupMembershipRole.HasOne(gmr => gmr.GroupRole)
                                   .WithMany(gm => gm.GroupMembershipRoles)
                                   .HasForeignKey(gmr => gmr.GroupRoleId)
                                   .OnDelete(DeleteBehavior.Cascade);

                groupMembershipRole.HasOne(gmr => gmr.GrantedByUser)
                                   .WithMany(u => u.GrantedRoles)
                                   .HasForeignKey(gmr => gmr.GrantedBy)
                                   .OnDelete(DeleteBehavior.Restrict);

                groupMembershipRole.Property(gmr => gmr.IsActive)
                                   .HasDefaultValue(true);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(p => p.Id);

                post.HasOne(p => p.Author)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.AuthorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                post.HasOne(p => p.Group)
                    .WithMany(g => g.Posts)
                    .HasForeignKey(p => p.GroupId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                post.Property(p => p.Title)
                    .HasMaxLength(100)
                    .IsRequired();

                post.Property(p => p.Content)
                    .HasMaxLength(1000)
                    .IsRequired();

                post.Property(p => p.Type)
                    .HasConversion<int>()
                    .IsRequired();

                post.Property(p => p.VisibilityType)
                    .HasConversion<int>()
                    .IsRequired();

                post.Property(p => p.IsPinned)
                    .HasDefaultValue(false);

                post.Property(p => p.IsDeleted)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<Comment>(comment =>
            {
                comment.HasKey(c => c.Id);

                comment.HasOne(c => c.Author)
                       .WithMany(u => u.Comments)
                       .HasForeignKey(c => c.AuthorId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Restrict);

                comment.HasOne(c => c.Post)
                       .WithMany(p => p.Comments)
                       .HasForeignKey(c => c.PostId)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Restrict);

                comment.HasOne(c => c.ParentComment)
                       .WithMany(c => c.Replies)
                       .HasForeignKey(c => c.ParentCommentId)
                       .IsRequired(false)
                       .OnDelete(DeleteBehavior.SetNull);

                comment.Property(c => c.Content)
                       .HasMaxLength(500)
                       .IsRequired();

                comment.Property(c => c.IsDeleted)
                       .HasDefaultValue(false);

            });

            modelBuilder.Entity<Reaction>(reaction =>
            {
                reaction.HasKey(r => r.Id);

                reaction.HasIndex(r => new { r.AuthorId, r.TargetId, r.TargetType })
                        .IsUnique();

                reaction.HasOne(r => r.Author)
                        .WithMany(u => u.Reactions)
                        .HasForeignKey(r => r.AuthorId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                reaction.Property(r => r.Type)
                        .HasConversion<int>()
                        .IsRequired();

                reaction.Property(r => r.TargetType)
                        .HasConversion<int>()
                        .IsRequired();

                reaction.Property(r => r.IsDeleted)
                        .HasDefaultValue(false);
            });
        }
    }
}
