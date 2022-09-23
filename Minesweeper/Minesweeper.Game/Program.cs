using Minesweeper.Game;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

VideoMode mode = new(GameConstants.WIDTH, GameConstants.HEIGHT);
RenderWindow window = new(mode, GameConstants.TITLE);
Game game = new();

window.SetVerticalSyncEnabled(true);

window.Closed += (sender, args) => window.Close();

window.MouseButtonPressed += (sender, args) =>
{
    if (args.Button == Mouse.Button.Left)
    {
        game.LeftClick = true;
    }

    if (args.Button == Mouse.Button.Right)
    {
        game.RightClick = true;
    }
};

while (window.IsOpen)
{
    window.DispatchEvents();

    Vector2i mousePos = Mouse.GetPosition((Window)window);
    game.Update(mousePos);

    window.Clear(Color.Blue);

    game.Draw(window);

    window.Display();
}
