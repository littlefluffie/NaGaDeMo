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
        void Update(GameTime gameTime);
        void LoadContent(ContentManager Content);
    }

    /// <summary>
    /// The Game class handles the game actions and events
    /// </summary>
    public static class Engine
    {
        public static Player Player = new Player();

        public static Battle CurrentBattle = new Battle();

        public static State GameState = State.GameStopped;
        
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
                    StartGame();
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

        private static void StartGame()
        {
            Player.HP.Current = Player.HP.Max;
            Player.TextureName = "Player";
            CurrentBattle.Player = Player;

            GameStateChange(State.StartPlayerTurn);

        }

        public static void StartNewGame(Battle battle = null)
        {
            Debug.WriteLine("Starting New Game");
            CurrentBattle = battle;


            // Start the game
            GameStateChange(State.GameStarted);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentBattle.Draw(spriteBatch);
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
    }

    public class Opponent
    {
        public void MakeDecision(Battle battle)
        {

        }

    }

    public class Battle
    {
        public Player Player = new Player();
        
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

            //Draw the UI
            UI.Draw(spriteBatch);
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
