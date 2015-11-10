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

    public interface Caster
    {
        void Cast(Spell spell, List<XNAObject> targets);
    }

    public class Character : XNAObject
    {
        
        public string Name;

        public string TextureName;
        public Texture2D Texture;

        public List<Spell> Spellbook = new List<Spell>();

        public Rectangle Bounds = new Rectangle(0, 0, 64, 64);

        public stat HP;
        public stat MP;

        public delegate void MouseEventHandler(object sender, MouseState mouseState);

        public event MouseEventHandler Click;

        public Character()
        {
            Engine.Characters.Add(this);

            Click += Character_Click;
        }

        private void Character_Click(object sender, MouseState mouseState)
        {
            
            Debug.WriteLine("You clicked me!");
            Debug.WriteLine("X, Y: " + mouseState.X + " " + mouseState.Y);
        }

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

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point();
            mousePoint.X = mouseState.X - UI.GameView.X;
            mousePoint.Y = mouseState.Y - UI.GameView.Y;

            UI.MouseCoordinates = mouseState.X + ", " + mouseState.Y;
            if (Bounds.Contains(mousePoint) && mouseState.LeftButton == ButtonState.Pressed )
            {
                Click(this, mouseState);
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

        public void OnGameStart(object sender, EventArgs e)
        {
            Debug.WriteLine("No ways, dude! I heard it too!" + Name);
        }


    }

    public class Player : Character, Caster
    {
        
        public stat XP;

        public Player()
        {
            Engine.Start += OnGameStart;
        }


    }

    public class Creature : Character, Caster
    {
        public Creature()
        {
            Engine.Start += OnGameStart;
        }


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
