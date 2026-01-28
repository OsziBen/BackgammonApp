using Application.Interfaces;
using Domain.GameSession.Services;

namespace Infrastructure.Services
{
    public class StartingPlayerRoller : IStartingPlayerRoller
    {
        private readonly IDiceService _diceService;

        public StartingPlayerRoller(IDiceService diceService)
        {
            _diceService = diceService;
        }

        public StartingPlayerRoll Roll()
        {
            int r1, r2;

            do
            {
                r1 = _diceService.Roll();
                r2 = _diceService.Roll();
            }
            while (r1 == r2);

            return new StartingPlayerRoll(r1, r2);
        }
    }
}
