namespace Minesweeper.Core.ValueObjects
{
    public static class NeighborsPosition
    {
        /// <summary>
        /// Returns an array of tuples of the offsets of the Neighbors position
        /// </summary>
        public static readonly (int rowDisplacement, int columnDisplacement)[] NeighborsDisplacement =
            new[] {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),           (0, 1),
                (1, -1), (1, 0), (1, 1)
            };
    }
}
