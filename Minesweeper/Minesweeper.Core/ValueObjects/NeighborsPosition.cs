namespace Minesweeper.Core.ValueObjects
{
    public static class NeighborsPosition
    {
        public static readonly (int rowDisplacement, int columnDisplacement)[] NeighborsDisplacement =
            new[] {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),           (0, 1),
                (1, -1), (1, 0), (1, 1)
            };
    }
}
