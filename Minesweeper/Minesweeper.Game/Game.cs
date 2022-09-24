using Minesweeper.Core.Entities;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Minesweeper.Game
{
    public class Game
    {
        private readonly BoardUI _boardUI;
        private readonly Sprite _boardSprite;
        private readonly Sprite _gameOverMessage;
        private readonly Sprite _youWonMessage;
        private readonly RenderTexture renderTexture = new(GameConstants.BOARD_WIDTH, GameConstants.BOARD_HEIGHT);
        private readonly Music _losingMusicPlayer;
        private readonly Music _winningMusicPlayer;
        readonly int _leftPadding = GameConstants.WINDOW_WIDTH / 2 - GameConstants.BOARD_WIDTH / 2; //Spacing from the left of the window
        readonly int _topPadding = GameConstants.WINDOW_HEIGHT / 2 - GameConstants.BOARD_HEIGHT / 2; //Spacing from the top of the window

        private bool isWon;
        private bool isLose;

        public bool LeftClick { get; set; }
        public bool RightClick { get; set; }

        public Game()
        {
            Texture texture = new("Assets/gaming_SpriteSheet.png");

            _boardUI = new BoardUI(GameConstants.BOARD_SIZE, GameConstants.NUMBER_OF_BOMBS, texture);
            _boardUI.InitializeBoardUI();

            _boardSprite = new Sprite(renderTexture.Texture)
            {
                Position = new Vector2f(_leftPadding, _topPadding)
            };

            _gameOverMessage = new Sprite(new Texture("Assets/game-over.jpg"))
            {
                Position = new Vector2f(120, 200)
            };

            _youWonMessage = new Sprite(new Texture("Assets/you-win.jpg"))
            {
                Position = new Vector2f(120, 140)
            };

            _losingMusicPlayer = new Music("Assets/losing-sound.wav")
            {
                Volume = 100
            };

            _winningMusicPlayer = new Music("Assets/winning-sound.wav")
            {
                Volume = 100
            };
        }

        public void Draw(RenderTarget window)
        {
            renderTexture.Clear();

            foreach (var key in _boardUI.CellsDictionary.Keys)
            {
                renderTexture.Draw(_boardUI.CellsDictionary[key].UIBox);
            }

            if (isLose)
            {
                renderTexture.Draw(_gameOverMessage);
            }
            else if (isWon)
            {
                renderTexture.Draw(_youWonMessage);
            }

            renderTexture.Display();
            window.Draw(_boardSprite);
        }

        public void Update(Vector2i mouseCoords)
        {
            int row = Math.DivRem(mouseCoords.Y - _topPadding, GameConstants.CELL_SIZE, out int _);
            int column = Math.DivRem(mouseCoords.X - _leftPadding, GameConstants.CELL_SIZE, out int _);

            if (_boardUI.CellsDictionary.TryGetValue((row, column), out CellUI cellUI))
            {
                if (LeftClick)
                {
                    HandleLeftCLick(cellUI);
                }
                else if (RightClick)
                {
                    HandleRightCLick(cellUI);
                }
            }
        }

        private void HandleLeftCLick(CellUI cellUI)
        {
            LeftClick = false;

            UserPlayResult userPlayResult = _boardUI.BoardEngine.Play(new UserPlay(cellUI.Cell.Row, cellUI.Cell.Column, false));
            if (userPlayResult != null && userPlayResult.IsSuccessful)
            {
                cellUI.ReRenderCell();

                //To re-render the cells exposed on a click on a blank cell
                if (userPlayResult.ResultingGameState == Core.Enums.GameStateEnum.StillAlive)
                {
                    foreach (var affectedCell in userPlayResult.AffectedCells)
                    {
                        if (_boardUI.CellsDictionary.TryGetValue((affectedCell.Row, affectedCell.Column), out CellUI cellUIToReRender))
                        {
                            cellUIToReRender.ReRenderCell();
                        }
                    }
                }

                if (userPlayResult.ResultingGameState == Core.Enums.GameStateEnum.Won)
                {
                    isWon = true;
                    _boardUI.ExposeAllCellsAndReRender();
                    _winningMusicPlayer.Play();
                }
            }
            else
            {
                cellUI.ReRenderCell();
                _boardUI.ExposeAllCellsAndReRender();
                isLose = true;

                _losingMusicPlayer.Play();
            }
        }

        private void HandleRightCLick(CellUI cellUI)
        {
            RightClick = false;

            if (!cellUI.Cell.IsExposed)
            {
                _boardUI.BoardEngine.Play(new UserPlay(cellUI.Cell.Row, cellUI.Cell.Column, true));
                cellUI.ReRenderCell();
            }
        }
    }
}
