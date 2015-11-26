using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace NaGaDeMo
{
    public static class Templates
    {
        public static class Opponents
        {
            public static class AI
            {
                public static void EasyAI()
                {
                    Random r = new Random();

                    foreach (Creature Creature in Engine.CurrentBattle.Creatures)
                    {
                        if (Creature.InRange(Engine.CurrentBattle.Player, 1))
                        {
                            List<XNAObject> Target = new List<XNAObject>();
                            Target.Add(Engine.CurrentBattle.Player);
                            Creature.Actions[0].Resolve(Creature, Target);
                        }

                        Vector2 Direction = new Vector2(r.Next(-1, 2), r.Next(-1, 2));

                        Creature.Move(Direction, 64);
                    }
                }

            }

        }
        /*
        public static class Interfaces
        {
            public static class DefaultInterface
            {
                 

                public static void Init()
                {
                    Button EndTurnButton = new Button(300, 300, 30, 100, "End Turn");
                    Button ExitButton = new Button(300, 800, 30, 100, "Exit");
                    Textbox txtPlayerHP = new Textbox(300, 400, 30, 500, "HP: 10/10 ");

                    EndTurnButton.Click += EndTurnButton_Click;
                    ExitButton.Click += ExitButton_Click;

                    UI.Elements.Add(EndTurnButton);
                    UI.Elements.Add(ExitButton);
                    UI.Elements.Add(txtPlayerHP);
                }

                private static void TxtPlayerHP_OnUpdate(object sender, EventArgs e)
                {
                    Textbox textbox = (Textbox)sender;
                    textbox.Text = "HP: " + Engine.CurrentBattle.Player.HP.Current + " / " + Engine.CurrentBattle.Player.HP.Max;
                }

                private static void ExitButton_Click(object sender, Microsoft.Xna.Framework.Input.MouseState mouseState)
                {
                    Environment.Exit(0);
                }

                private static void EndTurnButton_Click(object sender, Microsoft.Xna.Framework.Input.MouseState mouseState)
                {
                    if (Engine.GameState != Engine.State.PlayersTurn)
                    {
                        return;
                    }

                    Engine.EndPlayerTurn();

                    Debug.WriteLine("Ending turn...");

                }
            }
        }
        */
        public static class Creatures
        {
            public static Creature Goblin(int X, int Y)
            {
                Creature creature = new Creature();
                creature.Name = "Goblin";
                creature.TextureName = "Enemy";
                creature.HP.Max = 10;
                creature.HP.Current = 10;
                creature.X = X;
                creature.Y = Y;
                creature.Bounds.X = X * 64;
                creature.Bounds.Y = Y * 64;
                creature.Actions.Add(Actions.Attacks.GoblinAttack());
                return creature;
            }
        }

        public static class Actions
        {
            public static class Attacks
            {
                public static Attack GoblinAttack()
                {
                    Attack attack = new Attack();
                    attack.APCost = 1;
                    attack.Method = Methods.BasicAttack;

                    return attack;
                }

                public static class Methods
                {
                    public static void BasicAttack(Character Attacker = null, List<XNAObject> Targets = null)
                    {
                        foreach (Character Character in Targets)
                        {
                            Character.Damage(4);
                        }
                    }
                }
            }

            public static class Spells
            {
                public static Spell Spark()
                {
                    Spell spell = new Spell();
                    spell.TargetType = TargetType.Single;
                    spell.SpellName = "Spark";
                    spell.Method = Methods.Spark;
                    spell.BaseManaCost = 3;
                    spell.APCost = 1;

                    return spell;
                }

                public static Spell Fireball()
                {
                    Spell spell = new Spell();
                    spell.TargetType = TargetType.Multiple;
                    spell.SpellName = "Fireball";
                    spell.Method = Methods.Fireball;
                    spell.Range = 1;
                    spell.BaseManaCost = 6;
                    spell.APCost = 1;

                    return spell;
                }

                public static Spell Heal()
                {
                    Spell spell = new Spell();
                    spell.TargetType = TargetType.Self;
                    spell.SpellName = "Heal";
                    spell.Method = Methods.Heal;
                    spell.BaseManaCost = 3;
                    spell.APCost = 1;

                    return spell;
                }
            }

            public static class Methods
            {
                public static void Fireball(Character Caster = null, List<XNAObject> Targets = null)
                {
                    foreach (Character Character in Targets)
                    {
                        Character.Damage(10);
                    }
                }

                public static void Spark(Character Caster, List<XNAObject> Targets)
                {
                    foreach (Creature Creature in Targets)
                    {
                        Creature.Damage(5);
                    }
                }

                public static void Heal(Character Caster, List<XNAObject> Targets)
                {
                    Caster.HP.Current += 5;
                    Debug.WriteLine("Blessed Healing!");
                }
            }

        }


        public static class Maps
        {
            public static Map DefaultMap()
            {
                Map map = new Map();

                map.TextureName = "terrain.png";
                map.MapFile = "Default.txt";

                map.GenerateTiles();

                return map;
            }

            public static Map PathFinder()
            {
                Map map = new Map();
                map.TextureName = "terrain.png";
                map.MapFile = "PathFinding.txt";

                map.GenerateTiles();

                return map;

            }
        }

        public static class Battles
        {
            public static Battle DefaultBattle()
            {
                Battle battle = new Battle();

                battle.GameMap = Maps.DefaultMap();

                battle.Creatures.Add(Creatures.Goblin(3, 4));
                battle.Creatures.Add(Creatures.Goblin(4, 5));
                battle.Creatures.Add(Creatures.Goblin(5, 4));

                battle.Creatures.Add(Creatures.Goblin(5, 7));

                battle.Creatures.Add(Creatures.Goblin(8, 2));

                return battle;
            }

            public static Battle PathFinding()
            {
                Battle battle = new Battle();

                battle.GameMap = Maps.PathFinder();
                
                return battle;
            }

        }

    }
}
