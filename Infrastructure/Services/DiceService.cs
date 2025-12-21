using Application.Interfaces;

namespace Infrastructure.Services
{
    public class DiceService : IDiceService
    {
        private static readonly Random _random = new();

        public int Roll() => _random.Next(1, 7);

        public (int, int) RollDistinctPair()
        {
            int r1, r2;
            do
            {
                r1 = Roll();
                r2 = Roll();
            }
            while (r1 == r2);

            return (r1, r2);
        }
    }
}
