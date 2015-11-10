using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public int MapIndex;

        public Rectangle Bounds = new Rectangle(0, 0, 64, 64);

        public void Draw(SpriteBatch spriteBatch, Texture2D textureMap)
        {
            spriteBatch.Draw(textureMap, this.Bounds, new Rectangle(MapIndex * 64, 0, 64, 64), Color.White);
        }
    }

    /// <summary>
    /// A Map is a collection of tiles
    /// </summary>
    public class Map: XNAObject
    {
        private List<Tile> Tiles = new List<Tile>();

        public void GenerateTiles()
        {
            Tiles.Clear();
            string[] lines = System.IO.File.ReadAllLines(@"../../../Maps/" + MapFile);

            for (var j = 0; j < lines.Length; j++)
            {
                for (var i = 0; i < lines[j].Length; i++)
                {
                    Tile tile = new Tile();
                    tile.Bounds = new Rectangle(i * 64, j * 64, 64, 64);
                    tile.MapIndex = int.Parse(lines[j].Substring(i, 1));
                    Tiles.Add(tile);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles)
            {
                tile.Draw(spriteBatch, TextureMap);
            }
        }

        public string TextureName;
        
        public Texture2D TextureMap;
        
        public string MapFile;

        public void LoadContent(ContentManager content)
        {
            TextureMap = content.Load<Texture2D>(TextureName);
        }
    }
}

