using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NaGaDeMo
{
    public enum TileType
    {
        Water,
        Grass,
        Rock
    }
    /// <summary>
    /// A Tile is a single element of a Map
    /// </summary>
    public class Tile 
    {
        private int mapIndex;
        public int MapIndex
        {
            get
            {
                return mapIndex;
            }
            set
            {
                mapIndex = value;
            }
        }

        private Rectangle bounds = new Rectangle(0, 0, 64, 64);
        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                bounds = value;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D textureMap)
        {
            spriteBatch.Draw(textureMap, this.bounds, new Rectangle(mapIndex * 64, 0, 64, 64), Color.White);
        }

    }

    /// <summary>
    /// A Map is a collection of tiles
    /// </summary>
    public class Map
    {
        private List<Tile> tiles = new List<Tile>();
        public List<Tile> Tiles
        {
            get
            {
                return tiles;
            }
            set
            {
                tiles = value;
            }
        }

        public void GenerateTiles()
        {
            tiles.Clear();
            string[] lines = System.IO.File.ReadAllLines(@"../../../Maps/"+ mapFile);

            for (var j = 0; j < lines.Length; j++)
            {
                for (var i = 0; i < lines[j].Length; i++)
                {
                    Tile tile = new Tile();
                    tile.Bounds = new Rectangle(i * 64, j * 64, 64, 64);
                    tile.MapIndex = int.Parse(lines[j].Substring(i, 1));
                    tiles.Add(tile);
                }
            }
        }

        private string textureName;
        public string TextureName
        {
            get
            {
                return textureName;
            }

            set
            {
                textureName = value;
            }
        }

        private Texture2D textureMap;
        public Texture2D TextureMap
        {
            get
            {
                return textureMap;
            }

            set
            {
                textureMap = value;
            }
        }

        private string mapFile;
        public string MapFile
        {
            get
            {
                return mapFile;
            }

            set
            {
                mapFile = value;
            }
        }

    }
}

