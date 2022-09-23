namespace Minesweeper.Core.ValueObjects
{
    public static class NeighboorsPosition
    {
        public static readonly (int rowDisplacement, int columnDisplacement)[] NeighboorsDisplacement =
            new[] {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),           (0, 1),
                (1, -1), (1, 0), (1, 1)
            };
    }
}
