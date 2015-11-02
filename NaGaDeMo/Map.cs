using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NaGaDeMo
{
    public interface MonoGraphic
    {
        Rectangle Bounds { get; set; }
        Texture2D Image { get; set; }

        void Draw(SpriteBatch spriteBatch);

    }

    
    /// <summary>
    /// A Tile is a single element of a Map
    /// </summary>
    public class Tile: MonoGraphic
    {
        private Rectangle bounds = new Rectangle(0, 0, 64, 64);
        public Rectangle Bounds
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        private Texture2D image;
        public Texture2D Image
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.image, this.bounds, Color.White);
        }

    }

    /// <summary>
    /// A Map is a collection of tiles
    /// </summary>
    public class Map
    {
        private int[][] mapArray;
        private Tile[] tiles;

        private Texture2D mapImage;

        public void renderMap()
        {
            foreach (Tile tile in this.tiles)
            {

            }
        }
    }
}

