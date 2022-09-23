using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Game
{
    public class Box
    {
        Vector2f position;
        public bool isOpened;
        public bool isMine;
        public int mineCount;
        public bool isMarked;

        // ui
        public RectangleShape uiBox;
        Texture texture;
        public int type;
        public IntRect rect;

        public Box(Vector2f position, bool isMine, Texture texture)
        {
            this.position = position;
            this.isMine = isMine;

            this.uiBox = new RectangleShape(new Vector2f(16, 16));
            this.uiBox.Texture = texture;
            this.uiBox.Position = this.position;
            this.uiBox.TextureRect = new IntRect(type, 0, 10, 10);
            this.rect = new IntRect(160 + (int)this.position.X, 80 + (int)this.position.Y, 16, 16);
        }
    }
}
