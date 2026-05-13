using Application.Interfaces.Repository;
using Domain.GroupMembership;

namespace Application.Groups.Helpers
{
    public static class MembershipHelper
    {
        public static async Task<GroupMembership> GetOrCreateMembershipAsync(
            IUnitOfWork uow,
            Guid userId,
            Guid groupId,
            DateTimeOffset now,
            CancellationToken cancellationToken)
        {
            var existing = await uow.GroupMembershipsWrite
                .GetAnyAsync(userId, groupId, cancellationToken);

            if (existing != null)
            {
                existing.IsActive = true;
                existing.JoinedAt = now;
                existing.DisabledAt = null;

                return existing;
            }

            var membership = new GroupMembership
            {
                UserId = userId,
                GroupId = groupId,
                JoinedAt = now,
                IsActive = true
            };

            await uow.GroupMembershipsWrite.AddAsync(membership, cancellationToken);

            return membership;
        }
    }
}