using Common.Models;
using Domain.AppRole;
using Domain.BoardStateSnapshot;
using Domain.CheckerMove;
using Domain.Comment;
using Domain.CubeAction;
using Domain.DiceRollSnapshot;
using Domain.Game;
using Domain.GamePlayer;
using Domain.GameSession;
using Domain.Group;
using Domain.GroupMembership;
using Domain.GroupMembershipRole;
using Domain.GroupRole;
using Domain.Match;
using Domain.PlayerTurn;
using Domain.Post;
using Domain.Reaction;
using Domain.RulesTemplate;
using Domain.Tournament;
using Domain.TournamentPairing;
using Domain.TournamentParticipant;
using Domain.TournamentRegistration;
using Domain.TournamentRound;
using Domain.TournamentStanding;
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
        public DbSet<BoardStateSnapshot> BoardStateSnapshots { get; set; } = null!;
        public DbSet<CheckerMove> CheckerMoves { get; set; } = null!;
        public DbSet<CubeAction> CubeActions { get; set; } = null!;
        public DbSet<DiceRollSnapshot> DiceRollSnapshots { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<PlayerTurn> PlayerTurns { get; set; } = null!;
        public DbSet<RulesTemplate> RulesTemplates { get; set; } = null!;
        public DbSet<Tournament> Tournaments { get; set; } = null!;
        public DbSet<TournamentPairing> TournamentPairings { get; set; } = null!;
        public DbSet<TournamentParticipant> TournamentParticipants { get; set; } = null!;
        public DbSet<TournamentRegistration> TournamentRegistrations { get; set; } = null!;
        public DbSet<TournamentRound> TournamentRounds { get; set; } = null!;
        public DbSet<TournamentStanding> TournamentStandings { get; set; } = null!;
        public DbSet<GameSession> GameSessions { get; set; } = null!;
        public DbSet<GamePlayer> GamePlayers { get; set; } = null!;
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

            modelBuilder.Entity<RulesTemplate>(rulesTemplate =>
            {
                rulesTemplate.HasKey(r => r.Id);

                rulesTemplate.HasOne(rt => rt.Author)
                             .WithMany(u => u.RulesTemplates)
                             .HasForeignKey(rt => rt.AuthorId)
                             .IsRequired(false)
                             .OnDelete(DeleteBehavior.Restrict);

                rulesTemplate.Property(rt => rt.Name)
                             .HasMaxLength(50)
                             .IsRequired();

                rulesTemplate.Property(rt => rt.Description)
                             .HasMaxLength(300)
                             .IsRequired(false);

                rulesTemplate.Property(rt => rt.IsPublic)
                             .HasDefaultValue(false);

                rulesTemplate.Property(rt => rt.UseClock)
                             .HasDefaultValue(false);

                rulesTemplate.Property(rt => rt.CrawfordRuleEnabled)
                             .HasDefaultValue(true);

                rulesTemplate.Property(rt => rt.IsDeleted)
                             .HasDefaultValue(false);
            });

            modelBuilder.Entity<Match>(match =>
            {
                match.HasKey(m => m.Id);

                match.HasIndex(g => new { g.Id, g.CurrentGameNumber }).IsUnique();

                match.HasOne(m => m.CreatedByUser)
                     .WithMany(u => u.CreatedMatches)
                     .HasForeignKey(m => m.CreatedByUserId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Restrict);

                match.HasOne(m => m.WhitePlayer)
                     .WithMany(u => u.MatchesAsWhite)
                     .HasForeignKey(m => m.WhitePlayerId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Restrict);

                match.HasOne(m => m.BlackPlayer)
                     .WithMany(u => u.MatchesAsBlack)
                     .HasForeignKey(m => m.BlackPlayerId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Restrict);

                match.HasOne(m => m.Winner)
                     .WithMany()
                     .HasForeignKey(m => m.WinnerId)
                     .IsRequired(false)
                     .OnDelete(DeleteBehavior.Restrict);

                match.HasOne(m => m.RulesTemplate)
                     .WithMany(rt => rt.Matches)
                     .HasForeignKey(m => m.RulesTemplateId)
                     .IsRequired(false)
                     .OnDelete(DeleteBehavior.Restrict);

                match.Property(rt => rt.Type)
                     .HasConversion<int>()
                     .IsRequired();

                match.Property(rt => rt.StatusType)
                     .HasConversion<int>()
                     .IsRequired();

                match.Property(m => m.MatchCode)
                     .HasMaxLength(20)
                     .IsRequired(false);

                match.Property(m => m.Notes)
                     .HasMaxLength(200)
                     .IsRequired(false);

                match.Property(m => m.IsResigned)
                     .HasDefaultValue(false);

                match.Property(m => m.IsDeleted)
                     .HasDefaultValue(false);
            });

            modelBuilder.Entity<Game>(game =>
            {
                game.HasKey(g => g.Id);

                game.HasOne(g => g.Match)
                    .WithMany(m => m.Games)
                    .HasForeignKey(g => g.MatchId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                game.HasOne(g => g.Winner)
                    .WithMany()
                    .HasForeignKey(g => g.WinnerId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                game.HasOne(g => g.StartingPlayer)
                    .WithMany()
                    .HasForeignKey(g => g.StartingPlayerId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                game.HasOne(g => g.DoublingCubeOwner)
                    .WithMany()
                    .HasForeignKey(g => g.DoublingCubeOwnerId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                game.Property(g => g.WhitePlayerScore)
                    .HasConversion<int>()
                    .IsRequired(false);

                game.Property(g => g.BlackPlayerScore)
                    .HasConversion<int>()
                    .IsRequired(false);

                game.Property(g => g.DoublingCubeValue)
                    .HasDefaultValue(1);

                game.Property(g => g.IsCrawfordActive)
                    .HasDefaultValue(false);

                game.Property(g => g.IsFinished)
                    .HasDefaultValue(false);

                game.Property(g => g.IsTimeOutLoss)
                    .HasDefaultValue(false);

                game.Property(g => g.IsDeleted)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<PlayerTurn>(playerMove =>
            {
                playerMove.HasKey(pm => pm.Id);

                playerMove.HasOne(pm => pm.Game)
                          .WithMany(g => g.PlayerMoves)
                          .HasForeignKey(pm => pm.GameId)
                          .IsRequired()
                          .OnDelete(DeleteBehavior.Restrict);

                playerMove.HasOne(pm => pm.Player)
                          .WithMany()
                          .HasForeignKey(pm => pm.PlayerId)
                          .IsRequired()
                          .OnDelete(DeleteBehavior.Restrict);

                playerMove.Property(pm => pm.ResultType)
                          .HasConversion<int>()
                          .IsRequired();
            });

            modelBuilder.Entity<CheckerMove>(checkerMove =>
            {
                checkerMove.HasKey(cm => cm.Id);

                checkerMove.HasIndex(cm => new { cm.PlayerTurnId, cm.OrderWithinTurn })
                           .IsUnique();

                checkerMove.HasOne(cm => cm.PlayerTurn)
                           .WithMany(pm => pm.CheckerMoves)
                           .HasForeignKey(cm => cm.PlayerTurnId)
                           .IsRequired()
                           .OnDelete(DeleteBehavior.Restrict);

                checkerMove.Property(cm => cm.IsHit)
                           .HasDefaultValue(false);

                checkerMove.Property(cm => cm.IsBearOff)
                           .HasDefaultValue(false);
            });

            modelBuilder.Entity<DiceRollSnapshot>(diceRoll =>
            {
                diceRoll.HasKey(dr => dr.Id);

                diceRoll.HasOne(dr => dr.PlayerTurn)
                        .WithOne(pm => pm.DiceRollSnapshot)
                        .HasForeignKey<DiceRollSnapshot>(dr => dr.PlayerTurnId)
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.Cascade);

                diceRoll.Property(dr => dr.IsDouble)
                        .HasDefaultValue(false);
            });

            modelBuilder.Entity<CubeAction>(cubeAction =>
            {
                cubeAction.HasKey(ca => ca.Id);

                cubeAction.HasIndex(ca => new { ca.PlayerTurnId, ca.OrderWithinTurn })
                          .IsUnique();

                cubeAction.HasOne(ca => ca.PlayerTurn)
                          .WithMany(pt => pt.CubeActions)
                          .HasForeignKey(ca => ca.PlayerTurnId)
                          .IsRequired()
                          .OnDelete(DeleteBehavior.Restrict);

                cubeAction.HasOne(ca => ca.PreviousOwner)
                          .WithMany()
                          .HasForeignKey(ca => ca.PreviousOwnerId)
                          .IsRequired(false)
                          .OnDelete(DeleteBehavior.Restrict);

                cubeAction.HasOne(ca => ca.NewOwner)
                          .WithMany()
                          .HasForeignKey(ca => ca.NewOwnerId)
                          .IsRequired(false)
                          .OnDelete(DeleteBehavior.Restrict);

                cubeAction.Property(ca => ca.ActionType)
                          .HasConversion<int>()
                          .IsRequired();
            });

            modelBuilder.Entity<BoardStateSnapshot>(boardState =>
            {
                boardState.HasKey(bs => bs.Id);

                boardState.HasIndex(bs => new { bs.GameId, bs.Order });

                boardState.Property(bs => bs.CurrentPlayer)
                          .HasConversion<int>()
                          .IsRequired();
            });

            modelBuilder.Entity<Tournament>(tournament =>
            {
                tournament.HasKey(t => t.Id);

                tournament.HasOne(t => t.OrganizerUser)
                          .WithMany(u => u.OrganizedTournaments)
                          .HasForeignKey(t => t.OrganizerUserId)
                          .IsRequired()
                          .OnDelete(DeleteBehavior.Restrict);

                tournament.HasOne(t => t.RulesTemplate)
                          .WithMany()
                          .HasForeignKey(t => t.RulesTemplateId)
                          .IsRequired()
                          .OnDelete(DeleteBehavior.Restrict);

                tournament.Property(t => t.Name)
                          .HasMaxLength(100)
                          .IsRequired();

                tournament.Property(t => t.Description)
                          .HasMaxLength(500)
                          .IsRequired(false);

                tournament.Property(t => t.Type)
                          .HasConversion<int>()
                          .IsRequired();

                tournament.Property(t => t.Status)
                          .HasConversion<int>()
                          .IsRequired();

                tournament.Property(t => t.IsDeleted)
                          .HasDefaultValue(false);
            });

            modelBuilder.Entity<TournamentPairing>(tournamentPairing =>
            {
                tournamentPairing.HasKey(tp => tp.Id);

                tournamentPairing.HasOne(tp => tp.TournamentRound)
                                 .WithMany(tr => tr.Pairings)
                                 .HasForeignKey(tp => tp.TournamentRoundId)
                                 .IsRequired()
                                 .OnDelete(DeleteBehavior.Restrict);

                tournamentPairing.HasOne(tp => tp.WhiteParticipant)
                                 .WithMany()
                                 .HasForeignKey(tp => tp.WhiteParticipantId)
                                 .IsRequired()
                                 .OnDelete(DeleteBehavior.Restrict);

                tournamentPairing.HasOne(tp => tp.BlackParticipant)
                                 .WithMany()
                                 .HasForeignKey(tp => tp.BlackParticipantId)
                                 .IsRequired()
                                 .OnDelete(DeleteBehavior.Restrict);

                tournamentPairing.Property(tp => tp.Result)
                                 .HasConversion<int>()
                                 .IsRequired(false);

                tournamentPairing.Property(tp => tp.RecordingUrl)
                                 .HasMaxLength(200)
                                 .IsRequired(false);

                tournamentPairing.Property(tp => tp.IsDeleted)
                                 .HasDefaultValue(false);
            });

            modelBuilder.Entity<TournamentParticipant>(tournamentParticipant =>
            {
                tournamentParticipant.HasKey(tp => tp.Id);

                tournamentParticipant.HasOne(tp => tp.Tournament)
                                     .WithMany(t => t.Participants)
                                     .HasForeignKey(tp => tp.TournamentId)
                                     .IsRequired()
                                     .OnDelete(DeleteBehavior.Restrict);

                tournamentParticipant.HasOne(tp => tp.User)
                                     .WithMany(u => u.TournamentParticipations)
                                     .HasForeignKey(tp => tp.UserId)
                                     .IsRequired(false)
                                     .OnDelete(DeleteBehavior.Restrict);

                tournamentParticipant.Property(tp => tp.Status)
                                     .HasConversion<int>()
                                     .IsRequired();

                tournamentParticipant.Property(tp => tp.DisplayName)
                                     .HasMaxLength(50)
                                     .IsRequired();

                tournamentParticipant.Property(tp => tp.Email)
                                     .HasMaxLength(50)
                                     .IsRequired(false);

                tournamentParticipant.Property(tp => tp.Notes)
                                     .HasMaxLength(200)
                                     .IsRequired(false);

                tournamentParticipant.Property(tp => tp.IsDeleted)
                                     .HasDefaultValue(false);
            });

            modelBuilder.Entity<TournamentRegistration>(tournamentRegistration =>
            {
                tournamentRegistration.HasKey(tr => tr.Id);

                tournamentRegistration.HasOne(tr => tr.Tournament)
                                      .WithMany()
                                      .HasForeignKey(tr => tr.TournamentId)
                                      .IsRequired()
                                      .OnDelete(DeleteBehavior.Restrict);

                tournamentRegistration.HasOne(tr => tr.Participant)
                                      .WithMany(tp => tp.Registrations)
                                      .HasForeignKey(tr => tr.ParticipantId)
                                      .IsRequired()
                                      .OnDelete(DeleteBehavior.Restrict);

                tournamentRegistration.Property(tr => tr.Status)
                                      .HasConversion<int>()
                                      .IsRequired();

                tournamentRegistration.Property(tr => tr.IsDeleted)
                                      .HasDefaultValue(false);
            });

            modelBuilder.Entity<TournamentRound>(tournamentRound =>
            {
                tournamentRound.HasKey(tr => tr.Id);

                tournamentRound.HasOne(tr => tr.Tournament)
                               .WithMany(t => t.Rounds)
                               .HasForeignKey(tr => tr.TournamentId)
                               .IsRequired()
                               .OnDelete(DeleteBehavior.Restrict);

                tournamentRound.Property(tr => tr.IsFinished)
                               .HasDefaultValue(false);

                tournamentRound.Property(tr => tr.IsDeleted)
                               .HasDefaultValue(false);
            });

            modelBuilder.Entity<TournamentStanding>(tournamentStanding =>
            {
                tournamentStanding.HasKey(ts => ts.Id);

                tournamentStanding.HasIndex(ts => new { ts.TournamentId, ts.ParticipantId })
                                  .IsUnique();

                tournamentStanding.HasOne(ts => ts.Tournament)
                                  .WithMany(t => t.Standings)
                                  .HasForeignKey(ts => ts.TournamentId)
                                  .IsRequired()
                                  .OnDelete(DeleteBehavior.Restrict);

                tournamentStanding.HasOne(ts => ts.Participant)
                                  .WithMany()
                                  .HasForeignKey(ts => ts.ParticipantId)
                                  .IsRequired()
                                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<GameSession>(gameSession =>
            {
                gameSession.HasKey(gs => gs.Id);

                gameSession.HasOne(gs => gs.Match)
                           .WithMany(m => m.GameSessions)
                           .HasForeignKey(gs => gs.MatchId)
                           .IsRequired()
                           .OnDelete(DeleteBehavior.Restrict);

                gameSession.HasOne(gs => gs.WinnerPlayer)
                           .WithMany()
                           .HasForeignKey(gs => gs.WinnerPlayerId)
                           .IsRequired(false)
                           .OnDelete(DeleteBehavior.Restrict);

                gameSession.HasOne(gs => gs.CurrentGame)
                           .WithOne()
                           .HasForeignKey<GameSession>(gs => gs.CurrentGameId)
                           .IsRequired()
                           .OnDelete(DeleteBehavior.Restrict);

                gameSession.Property(gs => gs.SessionCode)
                           .HasMaxLength(20)
                           .IsRequired();

                gameSession.Property(gs => gs.CurrentPhase)
                           .HasConversion<int>()
                           .IsRequired();

                gameSession.Property(gs => gs.IsFinished)
                           .HasDefaultValue(false);
            });

            modelBuilder.Entity<GamePlayer>(gamePlayer =>
            {
                gamePlayer.HasKey(gp => gp.Id);

                gamePlayer.HasIndex(gp => new { gp.GameSessionId, gp.UserId })
                          .IsUnique();

                gamePlayer.HasOne(gp => gp.GameSession)
                          .WithMany(gs => gs.Players)
                          .HasForeignKey(gp => gp.GameSessionId)
                          .IsRequired()
                          .OnDelete(DeleteBehavior.Cascade);

                gamePlayer.HasOne(gp => gp.User)
                          .WithMany(u => u.GamePlayers)
                          .HasForeignKey(gp => gp.UserId)
                          .IsRequired()
                          .OnDelete(DeleteBehavior.Restrict);

                gamePlayer.Property(gp => gp.Color)
                          .HasConversion<int>()
                          .IsRequired();

                gamePlayer.Property(gp => gp.IsHost)
                          .HasDefaultValue(false);

                gamePlayer.Property(gp => gp.IsConnected)
                          .HasDefaultValue(false);
            });
        }
    }
}
