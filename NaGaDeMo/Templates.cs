using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaGaDeMo
{
    public static class Templates
    {
        public static class Creatures
        {
            public static Creature Goblin(int X, int Y)
            {
                Creature creature = new Creature();
                creature.Name = "Goblin";
                creature.TextureName = "Enemy";
                creature.HP.Max = 10;
                creature.X = X;
                creature.Y = Y;
                creature.Bounds.X = X * 64;
                creature.Bounds.Y = Y * 64;
                return creature;
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

                return spell;
            }
        }

        public static class Methods
        {
            public static void Fireball(Character Caster = null, List<XNAObject> Targets = null)
            {

            }

            public static void Spark(Character Caster, List<XNAObject> Targets)
            {
                foreach (Creature Creature in Targets)
                {
                    Creature.HP.Current -= 5;
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

        }

        public static class Battles
        {
            public static Battle DefaultBattle()
            {
                Battle battle = new Battle();

                battle.GameMap = Templates.Maps.DefaultMap();

                battle.Creatures.Add(Templates.Creatures.Goblin(3, 4));

                battle.Creatures.Add(Templates.Creatures.Goblin(5, 7));

                battle.Creatures.Add(Templates.Creatures.Goblin(8, 2));

                return battle;
            }

        }

    }
}
