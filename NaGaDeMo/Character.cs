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

    public interface Caster
    {
        void Cast(Spell spell, List<XNAObject> targets);
    }

    public class Character : XNAObject
    {
        public string TextureName;
        public Texture2D Texture;

        public List<Spell> Spellbook = new List<Spell>();

        public Rectangle Bounds = new Rectangle(0, 0, 64, 64);

        public stat HP;
        public stat MP;
        
        public void Cast(Spell spell, List<XNAObject> targets)
        {
            if (this.MP.Current < spell.BaseManaCost)
            {
                throw new Exception("Not enough mana");
            }
            else
            {
                this.MP.Current -= spell.BaseManaCost;
                spell.Resolve(this, targets);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(TextureName);
        }
    }

    public class Player : Character, Caster
    {
        public string Name;

        public stat XP;
        
        public Texture2D PlayerTexture;


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
                else if (value < 0)
                {
                    current = 0;
                }
                else
                {
                    current = value;
                }
            }
        }
    }
}
