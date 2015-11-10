using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NaGaDeMo
{
    /// <summary>
    /// The Game class handles the game actions and events
    /// </summary>
    public static class Engine
    {
        private static Battle currentBattle;
        public static Battle CurrentBattle
        {
            get
            {
                return currentBattle;
            }
            set
            {
                currentBattle = value;
            }
        }

        private static State gameState = State.GameStopped;
        public static State GameState
        {
            get
            {
                return gameState;
            }
            set
            {
                gameState = value;
            }
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

        public static void StartNewGame(Battle battle = null)
        {
            Debug.Write("Starting New Game");
            GameState = State.GameStarted;


        }


    }

    public class Opponent
    {


    }

    public class Battle
    {
        private Map gameMap;
        public Map GameMap
        {
            get
            {
                return gameMap;
            }
            set
            {
                gameMap = value;
            }
        }

        private Opponent opponent;
        public Opponent Opponent
        {
            get
            {
                return opponent;
            }
            set
            {
                opponent = value;
            }
        }

        private List<Creature> creatures = new List<Creature>();
        public List<Creature> Creatures
        {
            get
            {
                return creatures;
            }
            set
            {
                creatures = value;
            }
        }

    }
}
