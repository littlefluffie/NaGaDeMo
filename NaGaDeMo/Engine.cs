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
    /// <summary>
    /// The XNAObject abstract class is the base call for any object that is handled by XNA events/methods.
    /// 
    /// All objects that are drawn and updated by the Main program are included here.
    /// 
    /// </summary>
    /// 
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

            origin.X = origin.X / 64 * 64 + 32;
            origin.Y = origin.Y / 64 * 64 + 32;

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

        public bool InRange(Character Character, int range)
        {
            Point point = new Point(Bounds.X + 32, Bounds.Y + 32);

            Point origin = new Point(Character.Bounds.X + 32, Character.Bounds.Y + 32);

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
     // public abstract bool CanExecute();
    }

    public class CastSpellCommand : Command 
    {
        public Player Player;
        public Spell Spell;
        public List<XNAObject> Targets = new List<XNAObject>();

        public  bool CanExecute(Spell spell)
        {
            if (Player.AP.Current == 0)
            {
                return false;
            }

            if (Player.MP.Current < Spell.BaseManaCost)
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
            Player.MP.Current -= Spell.BaseManaCost;
            Player.AP.Current -= 1;
            Spell.Resolve(Player, Targets);
        }
    }

    public class MoveCommand : Command 
    {
        public Player Player;
        public Point MapPoint;

        public override void Execute()
        {
            Player.Move(MapPoint);
            Player.AP.Current -= 1;
        }

        public bool CanExecute(Point movePoint)
        {
            if (Player.AP.Current == 0)
            {
                return false;
            }

            if (Engine.GetColorAtPoint(movePoint.X, movePoint.Y) == Color.White)
            {
                // Collision detection

                return true;
                
                //if (Player.InRange(MapPoint, 3))
                //{
                //    return true;
                //}
                //else
                //{
                //    Debug.WriteLine("Too far! ");
                //    return false;
                //}
            }
            else
            {
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

        public static event EventHandler GameStateChange;

        private static EventArgs e = null;

        public delegate void EventHandler(object sender, EventArgs e);

        public delegate void MouseEventHandler(object sender, MouseState mouseState);

        // Characters in Game
        // Check if necessary
        public static List<Character> Characters = new List<Character>();

        // Map buffer
        // Check if necessary
        public static RenderTarget2D MapBuffer;
        public static RenderTarget2D CollisionBuffer;

        // The Player
        public static Player Player = new Player();

        // The Opponent
        public static Opponent Opponent = new Opponent(Templates.Opponents.AI.EasyAI);

        // The Current Battle
        public static Battle CurrentBattle = new Battle();

        // Game state
        private static State gameState = State.Upkeep;
        public static State GameState
        {
            get
            {
                return gameState;
            }
            set
            {
                gameState = value;
                GameStateChange(null, e);
            }
        }

        // Command stuff

        public static Command CurrentCommandInput;

        public static List<Command> CommandList = new List<Command>();

        public static List<Command> CommandQueue = new List<Command>();

        public static void Initialize(Game game)
        {
            // Setup Buffer
            MapBuffer = new RenderTarget2D(game.GraphicsDevice, UI.GameView.Width, UI.GameView.Height);

            CollisionBuffer = new RenderTarget2D(game.GraphicsDevice, UI.GameView.Width, UI.GameView.Height);

            // Initialize player variables
            Player.HP.Max = 10;
            Player.MP.Max = 10;
            Player.AP.Max = 2;
            Player.Initialize();

            Player.TextureName = "Player";
            Player.Bounds.X =3*64;
            Player.Bounds.Y = 3 * 64;

            // Setup Opponent

            // Setup Battle
            CurrentBattle = Templates.Battles.DefaultBattle();
            CurrentBattle.Player = Player;

            // Events registration
            GameStart += Engine_GameStart;
            RoundStart += Engine_RoundStart;
            PlayerTurnStart += Engine_PlayerTurnStart;
            PlayerTurnEnd += Engine_PlayerTurnEnd;
            OpponentTurnStart += Engine_OpponentTurnStart;
            OpponentTurnEnd += Engine_OpponentTurnEnd;
            RoundEnd += Engine_RoundEnd;
            GameEnd += Engine_GameEnd;

            GameStateChange += Engine_GameStateChange;

            // Start game
            GameStart(null, e);
        }

        public static void EndPlayerTurn()
        {
            PlayerTurnEnd(null, e);
        }

        public static void EndOpponentTurn()
        {
            OpponentTurnEnd(null, e);
        }

        public enum State
        {
            Upkeep,
            PlayersTurn,
            OpponentsTurn
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MapBuffer, UI.GameView, Color.White);
            
            // spriteBatch.Draw(UI.Pixel, new Rectangle (UI.CurrentMouseState.X,UI.CurrentMouseState.Y,5,5), Color.White);

            // spriteBatch.Draw(CollisionBuffer, UI.GameView, Color.White * 0.5f);
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

        public static Color GetColorAtPoint(int x, int y)
        {
            Color[] lightPixel = new Color[1];

            Rectangle sourceRectangle = new Rectangle(x, y, 1, 1); //rendertarget2D

            CollisionBuffer.GetData(0, sourceRectangle, lightPixel, 0, 1);

            return lightPixel[0];
        }

        public static void BufferCollisionMap(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(CollisionBuffer);

            graphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(UI.MapPoint.X, UI.MapPoint.Y, 0));

            CurrentBattle.GameMap.DrawCollisionMap(spriteBatch);

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

        private static void Engine_GameStart(object sender, EventArgs e)
        {
            RoundStart(null, e);

        }

        private static void Engine_RoundStart(object sender, EventArgs e)
        {
            GameState = State.PlayersTurn;

            // Transfer functionality to classes?
            Player.MP.Current += 3;
            Player.AP.Current = Player.AP.Max;
        }

        private static void Engine_PlayerTurnStart(object sender, EventArgs e)
        {

        }

        private static void Engine_PlayerTurnEnd(object sender, EventArgs e)
        {
            GameState = State.OpponentsTurn;

        }

        private static void Engine_OpponentTurnStart(object sender, EventArgs e)
        {

        }

        private static void Engine_OpponentTurnEnd(object sender, EventArgs e)
        {
            RoundEnd(null, e);
        }

        private static void Engine_RoundEnd(object sender, EventArgs e)
        {
            RoundStart(null, e);
        }

        private static void Engine_GameEnd(object sender, EventArgs e)
        {
            Debug.WriteLine("The game has ended");
            Environment.Exit(0);
        }


        private static void Engine_GameStateChange(object sender, EventArgs e)
        {
            if (gameState == State.OpponentsTurn)
            {
                OpponentTurnStart(null, e);
            }

            if (gameState == State.PlayersTurn)
            {
                PlayerTurnStart(null, e);
            }
        }
        
        #endregion
    }

    public class Opponent
    {
        public delegate void DecisionDelegate();

        public DecisionDelegate MakeDecision;

        public Opponent(DecisionDelegate AI)
        {
            Engine.OpponentTurnStart += Engine_OpponentTurnStart;
            Engine.OpponentTurnEnd += Engine_OpponentTurnEnd;
            MakeDecision = AI;
        }

        private void Engine_OpponentTurnEnd(object sender, EventArgs e)
        {

        }

        private void Engine_OpponentTurnStart(object sender, EventArgs e)
        {
            Debug.WriteLine("Opponent: Mmmm...");
            MakeDecision();
            Engine.EndOpponentTurn();
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
                            spriteBatch.Draw(UI.Pixel, new Rectangle((UI.MousePoint.X / 64) * 64, (UI.MousePoint.Y / 64) * 64, 64, 64), Color.Red * 0.5f);
                            break;

                        case TargetType.Multiple:
                            // Draws the Area of Effect
                            for (var i = (UI.MousePoint.X / 64 * 64 - Command.Spell.Range * 64); i < (UI.MousePoint.X / 64 * 64 + Command.Spell.Range * 64 + 64); i += 64)
                            {
                                for (var j = (UI.MousePoint.Y / 64 * 64 - Command.Spell.Range * 64); j < (UI.MousePoint.Y / 64 * 64 + Command.Spell.Range * 64 + 64); j += 64)
                                {
                                    Point point = new Point(i + 32, j + 32);

                                    Point origin = new Point(UI.MousePoint.X / 64 * 64 + 32, UI.MousePoint.Y / 64 * 64 + 32);

                                    spriteBatch.Draw(UI.Pixel, new Rectangle(origin.X, origin.Y, 5, 5), Color.Aqua);

                                    int radius = (Command.Spell.Range * 64 + 32) * (Command.Spell.Range * 64 + 32);

                                    int distance = ((point.X - origin.X) * (point.X - origin.X) + (point.Y - origin.Y) * (point.Y - origin.Y));

                                    if (distance < radius)
                                    {
                                        spriteBatch.Draw(UI.Pixel, new Rectangle(i / 64 * 64, j / 64 * 64, 64, 64), Color.Red * 0.5f);
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
                    MoveCommand move = (MoveCommand)Engine.CurrentCommandInput;
                    UI.DrawLine(spriteBatch, 2f, (move.CanExecute(new Point (UI.MousePoint.X/64*64+32, UI.MousePoint.Y/64*64+32))) ? Color.Green * 0.5f : Color.Red * 0.5f, new Vector2(move.Player.Bounds.X + 32, move.Player.Bounds.Y + 32), new Vector2(UI.MousePoint.X / 64 * 64 + 32, UI.MousePoint.Y / 64 * 64 + 32));
                    spriteBatch.Draw(UI.Pixel, new Rectangle((UI.MousePoint.X / 64) * 64, (UI.MousePoint.Y / 64) * 64, 64, 64), (move.CanExecute(new Point(UI.MousePoint.X / 64 * 64 + 32, UI.MousePoint.Y / 64 * 64 + 32))) ? Color.Green * 0.5f : Color.Red * 0.5f);
                }
            }
        }

        public void GetPixel()
        {
            
        }

        public Player Player;

        public int TurnNumber;

        public Map GameMap = new Map();

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
