using Minesweeper.Core;
using Minesweeper.Core.Entities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Game
{
    public class BoardUI
    {
        public Board BoardEngine { get; private set; }
        public Dictionary<(int, int), Cell> CellsDictionary { get; private set; }

        public BoardUI(int boardSize, int numberOfBombs, Texture texture)
        {
            BoardEngine = new Board(boardSize, numberOfBombs, texture);
            CellsDictionary = new Dictionary<(int, int), Cell>();
        }

        public void CreateDictionary()
        {
            for (int index = 0; index < BoardEngine.NumberOfCells; index++)
            {
                int row = index / BoardEngine.BoardSize;
                int column = (index - row * BoardEngine.BoardSize) % BoardEngine.BoardSize;
                Cell cell = BoardEngine.Cells[row, column];
                cell.RenderUiBox();
                CellsDictionary.Add((row, column), cell);
            }
        }
    }
}
