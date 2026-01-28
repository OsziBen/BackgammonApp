using Common.Enums.CubeActionType;
using Common.Models;

namespace Domain.CubeAction
{
    public class CubeAction : BaseEntity
    {
        public Guid PlayerTurnId { get; set; }
        public int OrderWithinTurn { get; set; }
        public CubeActionType ActionType { get; set; }
        // Offer, Take, Drop, Center (első duplázás)

        public Guid? PreviousOwnerId { get; set; }
        public Guid? NewOwnerId { get; set; }

        public int PreviousCubeValue { get; set; }
        public int NewCubeValue { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public PlayerTurn.PlayerTurn PlayerTurn { get; set; } = null!;
        public User.User? PreviousOwner { get; set; }
        public User.User? NewOwner { get; set; }
    }
}
