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
    public interface XNAObject
    {
        void Draw(SpriteBatch spritebatch);
        //void Update(GameTime gameTime);
        void LoadContent(ContentManager Content);
    }

    /// <summary>
    /// The Game class handles the game actions and events
    /// </summary>
    public static class Engine
    {
        // Event stuff
        public static event EventHandler Start;

        public delegate void EventHandler(object sender, EventArgs e);

        public delegate void MouseEventHandler(object sender, MouseState mouseState);

        public static List<Character> Characters = new List<Character>();

        public static RenderTarget2D MapBuffer;

        public static Player Player = new Player();

        public static Battle CurrentBattle = new Battle();

        public static State GameState = State.GameStopped;
        private static EventArgs e = null;

        public static void Initialize(Game game)
        {
            // Setup Buffer
            MapBuffer = new RenderTarget2D(game.GraphicsDevice, 640, 640);
            
            // Subscribe to event handler
            Start += new EventHandler(OnGameStart);
            
            Player.HP.Current = Player.HP.Max;
            Player.TextureName = "Player";
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
                    Start(null, e);
                    
                    break;
                case State.StartPlayerTurn:
                    StartPlayerTurn();
                    break;
                case State.WaitingForPlayer:

                    break;
                case State.EndPlayerTurn:
                    EndPlayerTurn();
                    break;
                case State.StartEnemyTurn:
                    StartEnemyTurn();
                    break;
                case State.WaitingForEnemy:

                    break;
                case State.EndEnemyTurn:
                    EndEnemyTurn();
                    break;
                case State.GameStopped:
                    Environment.Exit(0);
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
            spriteBatch.Draw(MapBuffer, UI.GameView, Color.White );
        }

        /// <summary>
        /// Renders the complete Battle to the MapBuffer to be rendered to the screen later
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="spriteBatch"></param>
        public static void BufferMap(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(Engine.MapBuffer);
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

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

        public static void OnGameStart(object sender, EventArgs e)
        {
            Debug.WriteLine("Game has started");
        }

        public static void OnPlayerTurn()
        {

        }

        

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
        public Player Player;

        public int TurnNumber;

        public Map GameMap = new Map();

        public Opponent Opponent = new Opponent();

        public List<Creature> Creatures = new List<Creature>();

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the map
            GameMap.Draw(spriteBatch);

            //Draw the creatures
            foreach (Creature Creature in Creatures)
            {
                Creature.Draw(spriteBatch);
            }

            //Draw the Player
            Player.Draw(spriteBatch);

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
