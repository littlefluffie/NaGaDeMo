using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NaGaDeMo
{
    public class Tile
    {
        public Rectangle bounds = new Rectangle(0, 0, 64, 64);

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, this.bounds, Color.White);
        }

    }

    public class Map
    {
        public Tile[] tiles;

        public Texture2D mapImage;

        public void renderMap()
        {
            foreach (Tile tile in this.tiles)
            {

            }
        }
    }
}

