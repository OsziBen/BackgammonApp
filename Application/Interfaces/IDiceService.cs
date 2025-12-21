namespace Application.Interfaces
{
    public interface IDiceService
    {
        int Roll();
        (int, int) RollDistinctPair();
    }
}
