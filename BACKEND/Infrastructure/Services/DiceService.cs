using Application.Interfaces;

namespace Infrastructure.Services
{
    public class DiceService : IDiceService
    {
        private readonly Random _random;

        public DiceService()
        {
         _random = new Random();   
        }

        public int Roll() => _random.Next(1, 7);
    }
}
