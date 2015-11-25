using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace NaGaDeMo
{
    public enum TileType
    {
        Water,
        Grass,
        Rock,
    }

    /// <summary>
    /// A Tile is a single element of a Map
    /// </summary>
    public class Tile : XNAObject
    {
        public int MapIndex;

        public int X;
        public int Y;

        public Tile()
        {
            Click += Tile_Click;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D textureMap)
        {
            spriteBatch.Draw(textureMap, this.Bounds, new Rectangle(MapIndex * 64, 0, 64, 64), Color.White);
        }

        #region Events

        public void Tile_Click(object sender, MouseState mouseState)
        {
            Debug.WriteLine("You have clicked on a Tile at " + X + ", " + Y);

            if (Engine.GameState != Engine.State.PlayersTurn)
            {
                return;
            }

            if (Engine.CurrentCommandInput is MoveCommand)
            {
                MoveCommand move = (MoveCommand)Engine.CurrentCommandInput;
                move.MapPoint.X = Bounds.X;
                move.MapPoint.Y = Bounds.Y;

                if (move.CanExecute())
                {
                    move.Execute();

                    Engine.CommandList.Add(move);
                    Engine.CommandQueue.Remove(move);
                    Engine.CurrentCommandInput = null;
                }
                else
                {
                    return;
                }
            }

            if (Engine.CurrentCommandInput is CastSpellCommand)
            {
                CastSpellCommand spell = (CastSpellCommand)Engine.CurrentCommandInput;

                switch (spell.Spell.TargetType)
                {
                    case TargetType.None:
                        break;

                    case TargetType.Self:
                        break;

                    case TargetType.Single:
                        break;

                    case TargetType.Multiple:
                        if (!spell.CanExecute())
                        {
                            return;
                        }

                        foreach (Character character in Engine.Characters)
                        {
                            if(character.InRange(new Point (Bounds.X+32,Bounds.Y+32), spell.Spell.Range))
                            {
                                spell.Targets.Add(character);
                            }
                        }

                        spell.Execute();
                        Engine.CommandList.Add(spell);
                        Engine.CommandQueue.Remove(spell);
                        Engine.CurrentCommandInput = null;

                        break;

                    default:
                        break;
                }
                

            }

        }

        #endregion
    }

    /// <summary>
    /// A Map is a collection of tiles
    /// </summary>
    public class Map : XNAObject
    {
        public List<Tile> Tiles = new List<Tile>();
        
        public int Width;
        public int Height;

        public void GenerateTiles()
        {
            string[] lines = System.IO.File.ReadAllLines(@"../../../Maps/" + MapFile);

            Width = lines[0].Length;
            Height = lines.Length;

            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    Tile tile = new Tile();
                    tile.X = i;
                    tile.Y = j;
                    tile.Bounds = new Rectangle(i * 64, j * 64, 64, 64);
                    tile.MapIndex = int.Parse(lines[j].Substring(i, 1));
                    Tiles.Add(tile);
                }
            }
        }

        public void UpdateTiles()
        {
            foreach (Tile tile in Tiles)
            {
                tile.Bounds.X = UI.GameView.X + tile.X * 64;
                tile.Bounds.Y = UI.GameView.Y + tile.Y * 64;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles)
            {
                tile.Draw(spriteBatch, TextureMap);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            TextureMap = content.Load<Texture2D>("terrain");
        }

        public Texture2D TextureMap;

        public string MapFile;

     }
}

