using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NaGaDeMo
{

    public interface Targetable
    {

    }

    public interface Caster
    {
        void Cast(Spell spell, Targetable target);
    }

    public class Character : Targetable
    {
        private stat hp;

        public stat HP
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
            }
        }

        public void Cast(Spell spell, Targetable target)
        {
            throw new NotImplementedException();
        }
    }

    public class Player : Character, Caster

    {

    }

    public class Creature : Character, Caster
    {

    }

    public struct stat
    {
        private int max;
        private int current;

        public int Max
        {
            get
            {
                return max;
            }

            set
            {
                max = value;
                if (current > max)
                {
                    current = max;
                }
            }
        }

        public int Current
        {
            get
            {
                return current;
            }

            set
            {
                if (value > max)
                {
                    current = max;
                }
                else
                {
                    current = value;
                }
            }
        }
    }
}
