using System.Security.Cryptography;

namespace Application.GameSessions.Services.SessionCodeGenerator
{
    public class SessionCodeGenerator : ISessionCodeGenerator
    {
        private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private const int CodeLenght = 6;

        public string Generate()
        {
            Span<byte> bytes = stackalloc byte[CodeLenght];
            RandomNumberGenerator.Fill(bytes);

            var chars = new char[CodeLenght];

            for (int i = 0; i < CodeLenght; i++)
            {
                chars[i] = Alphabet[bytes[i] % Alphabet.Length];
            }

            return new string(chars);
        }
    }
}
