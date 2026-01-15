namespace Application.GameSessions.Requests
{
    public class MoveDto
    {
        public int From { get; init; }
        public int To { get; init; }
        public int Die { get; init; }

        public MoveDto(int from, int to, int die)
        {
            From = from;
            To = to;
            Die = die;
        }
    }
}
