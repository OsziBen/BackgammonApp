using Common.Enums.BoardState;
using System.Text.Json.Serialization;

namespace Domain.GameLogic
{
    public class CheckerPosition
    {
        [JsonPropertyName("owner")]
        public PlayerColor? Owner { get; }

        [JsonPropertyName("count")]
        public int Count { get; }

        [JsonConstructor]
        public CheckerPosition(
            PlayerColor? owner,
            int count)
        {
            Owner = owner;
            Count = count;
        }

        public CheckerPosition Clone()
            => new CheckerPosition(Owner, Count);
    }
}
