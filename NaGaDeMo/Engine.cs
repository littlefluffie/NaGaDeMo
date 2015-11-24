using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace NaGaDeMo
{
    public abstract class XNAObject
    {
        public Rectangle Bounds = new Rectangle(0, 0, 64, 64);

        public Texture2D Texture;

        public string TextureName;

        public event Engine.MouseEventHandler Click;
        public event Engine.MouseEventHandler MouseOver;

        public virtual void Update(GameTime gametime)
        {
            if (Bounds.Contains(UI.MousePoint))
            {
                if (MouseOver != null)
                {
                    MouseOver(this, UI.CurrentMouseState);
                }
            }

            if (Bounds.Contains(UI.MousePoint) && UI.CurrentMouseState.LeftButton == ButtonState.Released && UI.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (Click != null)
                {
                    Click(this, UI.CurrentMouseState);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color.White);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D textureMap)
        {

        }

        public virtual void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(TextureName);
        }

        public bool InRange(Point origin, int range)
        {
            Point point = new Point(Bounds.X + 32, Bounds.Y + 32);

            int radius = (range * 64 + 32) * (range * 64 + 32);

            int distance = ((point.X - origin.X) * (point.X - origin.X) + (point.Y - origin.Y) * (point.Y - origin.Y));

            if (distance < radius)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public abstract class Command
    {
        public abstract void Execute();
        public abstract bool CanExecute();
    }

    public class CastSpellCommand : Command
    {
        public Character Caster;
        public Spell Spell;
        public List<XNAObject> Targets = new List<XNAObject>();

        public override bool CanExecute()
        {
            if (Caster.MP.Current < Spell.BaseManaCost)
            {
                Debug.WriteLine("Not enough Mana!");
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void Execute()
        {
            Caster.MP.Current -= Spell.BaseManaCost;
            Spell.Resolve(Caster, Targets);
        }
    }

    public class MoveCommand : Command
    {
        public Player Player;
        public Point MapPoint;

        public override void Execute()
        {
            Player.Bounds.X = MapPoint.X;
            Player.Bounds.Y = MapPoint.Y;
        }

        public override bool CanExecute()
        {
            float distance = ((Player.Bounds.X - MapPoint.X) * (Player.Bounds.X - MapPoint.X) + (Player.Bounds.Y - MapPoint.Y) * (Player.Bounds.Y - MapPoint.Y));

            if (distance < (64 * 3) * (64 * 3))
            {
                Debug.WriteLine("Within range " + distance);
                return true;
            }
            else
            {
                Debug.WriteLine("Too far! " + distance);
                return false;
            }
        }
    }

    /// <summary>
    /// The Game class handles the game actions and events
    /// </summary>
    public static class Engine
    {
        // Event stuff

        public static event EventHandler GameStart;

        public static event EventHandler RoundStart;

        public static event EventHandler PlayerTurnStart;
        public static event EventHandler PlayerTurnEnd;

        public static event EventHandler OpponentTurnStart;
        public static event EventHandler OpponentTurnEnd;

        public static event EventHandler RoundEnd;

        public static event EventHandler GameEnd;

        private static EventArgs e = null;

        public delegate void EventHandler(object sender, EventArgs e);

        public delegate void MouseEventHandler(object sender, MouseState mouseState);

        // Characters in Game
        // Check if necessary
        public static List<Character> Characters = new List<Character>();

        // Map buffer
        // Check if necessary
        public static RenderTarget2D MapBuffer;

        // The Player
        public static Player Player = new Player();

        // The Current Battle
        public static Battle CurrentBattle = new Battle();

        // Game state
        public static State GameState = State.GameStopped;

        // Command stuff

        public static Command CurrentCommandInput;

        public static List<Command> CommandList = new List<Command>();

        public static List<Command> CommandQueue = new List<Command>();

        public static void Initialize(Game game)
        {
            // Setup Buffer
            MapBuffer = new RenderTarget2D(game.GraphicsDevice, UI.GameView.Width, UI.GameView.Height);

            Player.HP.Max = 10;
            Player.MP.Max = 10;
            Player.AP.Max = 2;
            Player.Initialize();

            Player.TextureName = "Player";
            Player.Bounds.X = 320;
            Player.Bounds.Y = 128;

            CurrentBattle = Templates.Battles.DefaultBattle();
            CurrentBattle.Player = Player;

            GameStateChange(State.GameStarted);
        }

        public enum State
        {
            GameStarted,
            StartPlayerTurn,
            WaitingForPlayer,
            EndPlayerTurn,
            StartEnemyTurn,
            WaitingForEnemy,
            EndEnemyTurn,
            GameStopped
        }

        public static void GameStateChange(State state)
        {
            GameState = state;

            switch (state)
            {
                case State.GameStarted:
                    if (GameStart != null)
                    {
                        GameStart(null, e);
                    }
                    break;

                case State.StartPlayerTurn:
                                        if (GameStart != null)
                    {
                        GameStart(null, e);
                    }
                    break;

                case State.WaitingForPlayer:
                    break;
                case State.EndPlayerTurn:
                    break;

                case State.StartEnemyTurn:
                    break;

                case State.WaitingForEnemy:
                    break;

                case State.EndEnemyTurn:
                    break;

                case State.GameStopped:
                    break;

                default:
                    break;
            }

        }

        private static void EndEnemyTurn()
        {
            throw new NotImplementedException();
        }

        private static void StartEnemyTurn()
        {
            throw new NotImplementedException();
        }

        private static void EndPlayerTurn()
        {
            throw new NotImplementedException();
        }

        private static void StartPlayerTurn()
        {
            Debug.WriteLine("Player's Turn");

            // TODO Player Start stuff

            GameStateChange(State.WaitingForPlayer);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MapBuffer, UI.GameView, Color.White);
        }

        /// <summary>
        /// Renders the complete Battle to the MapBuffer to be rendered to the screen later
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="spriteBatch"></param>
        public static void BufferMap(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(Engine.MapBuffer);

            graphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(UI.MapPoint.X, UI.MapPoint.Y, 0));

            CurrentBattle.Draw(spriteBatch);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }

        public static void LoadContent(ContentManager Content)
        {
            if (CurrentBattle == null)
            {
                throw new Exception("No battle has been loaded");
            }
            else
            {
                // Load player content
                Player.LoadContent(Content);

                // Load Map content
                CurrentBattle.GameMap.LoadContent(Content);

                //Load enemy content
                CurrentBattle.LoadContent(Content);
            }
        }

        #region Events

        // TODO Engine Events

        #endregion
    }

    public class Opponent
    {
        public void MakeDecision(Battle battle)
        {

        }
    }

    public class Battle
    {
        public static class Overlay
        {
            public static void Draw(SpriteBatch spriteBatch)
            {

                foreach (Creature creature in Engine.CurrentBattle.Creatures)
                {
                    spriteBatch.DrawString(UI.UIFont, "HP: " + creature.HP.Current, new Vector2(creature.Bounds.X, creature.Bounds.Y), Color.Red);

                }

                spriteBatch.DrawString(UI.UIFont, "MP: " + Engine.CurrentBattle.Player.MP.Current + "/" + Engine.CurrentBattle.Player.MP.Max, new Vector2(Engine.CurrentBattle.Player.Bounds.X, Engine.CurrentBattle.Player.Bounds.Y), Color.Blue, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 1.0f);

                // Spell target single
                if (Engine.CurrentCommandInput is CastSpellCommand)
                {
                    CastSpellCommand Command = (CastSpellCommand)Engine.CurrentCommandInput;

                    switch (Command.Spell.TargetType)
                    {
                        case TargetType.None:
                            break;

                        case TargetType.Self:
                            break;

                        case TargetType.Single:
                            spriteBatch.Draw(UI.pixel, new Rectangle((UI.MousePoint.X / 64) * 64, (UI.MousePoint.Y / 64) * 64, 64, 64), Color.Red * 0.5f);
                            break;

                        case TargetType.Multiple:
                            // Draws the Area of Effect
                            for (var i = (UI.MousePoint.X / 64 * 64 - Command.Spell.Range * 64); i < (UI.MousePoint.X / 64 * 64 + Command.Spell.Range * 64 + 64); i += 64)
                            {
                                for (var j = (UI.MousePoint.Y / 64 * 64 - Command.Spell.Range * 64); j < (UI.MousePoint.Y / 64 * 64 + Command.Spell.Range * 64 + 64); j += 64)
                                {
                                    Point point = new Point(i + 32, j + 32);

                                    Point origin = new Point(UI.MousePoint.X / 64 * 64 + 32, UI.MousePoint.Y / 64 * 64 + 32);

                                    spriteBatch.Draw(UI.pixel, new Rectangle(origin.X, origin.Y, 5, 5), Color.Aqua);

                                    int radius = (Command.Spell.Range * 64 + 32) * (Command.Spell.Range * 64 + 32);

                                    int distance = ((point.X - origin.X) * (point.X - origin.X) + (point.Y - origin.Y) * (point.Y - origin.Y));

                                    if (distance < radius)
                                    {
                                        spriteBatch.Draw(UI.pixel, new Rectangle(i / 64 * 64, j / 64 * 64, 64, 64), Color.Red * 0.5f);
                                    }
                                }
                            }

                            break;

                        default:
                            break;
                    }


                }

                // Movement target
                if (Engine.CurrentCommandInput is MoveCommand)
                {
                    spriteBatch.Draw(UI.pixel, new Rectangle((UI.MousePoint.X / 64) * 64, (UI.MousePoint.Y / 64) * 64, 64, 64), Color.Green * 0.5f);
                }

            }

        }
        public Player Player;

        public int TurnNumber;

        public Map GameMap = new Map();

        public Opponent Opponent = new Opponent();

        public List<Creature> Creatures = new List<Creature>();

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the map
            GameMap.Draw(spriteBatch);

            // Draw the creatures
            foreach (Creature Creature in Creatures)
            {
                Creature.Draw(spriteBatch);
            }

            // Draw the Player
            Player.Draw(spriteBatch);

            // Draw the overlay
            Overlay.Draw(spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Creature Creature in Creatures)
            {
                Creature.LoadContent(content);
            }
        }
    }
}
