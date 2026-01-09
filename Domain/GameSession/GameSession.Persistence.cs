namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void UpdateBoardStateJson(string boardStateJson)
        {
            CurrentBoardStateJson = boardStateJson;
        }
    }
}
