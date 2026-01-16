using Application.Interfaces;
using Domain.GameLogic;

namespace Infrastructure.Services
{
    public class DiceRollerAdapter : IDiceRoller
    {
        private readonly IDiceService _diceService;

        public DiceRollerAdapter(IDiceService diceService)
        {
            _diceService = diceService;
        }

        public DiceRoll Roll()
        {
            var d1 = _diceService.Roll();
            var d2 = _diceService.Roll();

            return new DiceRoll(d1, d2);
        }
    }
}
