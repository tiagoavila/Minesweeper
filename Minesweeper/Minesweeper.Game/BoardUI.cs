using Minesweeper.Core.Entities;
using SFML.Graphics;

namespace Minesweeper.Game
{
    public class BoardUI
    {
        public Board BoardEngine { get; private set; }
        public Dictionary<(int, int), CellUI> CellsDictionary { get; private set; }
        public Texture Texture { get; private set; }

        public BoardUI(int boardSize, int numberOfBombs, Texture texture)
        {
            BoardEngine = new Board(boardSize, numberOfBombs);
            CellsDictionary = new Dictionary<(int, int), CellUI>();
            Texture = texture;
        }

        /// <summary>
        /// Initializes the BoardEngine and creates the Dictionary of Cells for the UI
        /// </summary>
        public void InitializeBoardUI()
        {
            BoardEngine.InitializeBoard();
            CreateDictionary();
        }

        public void ExposeAllCellsAndReRender()
        {
            foreach (var (row, column) in CellsDictionary.Keys)
            {
                CellUI cellUI = CellsDictionary[(row, column)];
                cellUI.Cell.MarkAsExposed();
                cellUI.ReRenderCell();
            }   
        }

        private void CreateDictionary()
        {
            for (int index = 0; index < BoardEngine.NumberOfCells; index++)
            {
                int row = index / BoardEngine.BoardSize;
                int column = (index - row * BoardEngine.BoardSize) % BoardEngine.BoardSize;

                CellUI cellUI = new(BoardEngine.Cells[row, column], Texture);
                cellUI.CreateUiBox();
                CellsDictionary.Add((row, column), cellUI);
            }
        }
    }
}
