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

        public int X;
        public int Y;

        public stat HP;
        public stat MP;

        public delegate void MouseEventHandler(object sender, MouseState mouseState);

        public event MouseEventHandler Click;

        public Character()
        {
            Engine.Characters.Add(this);

            Click += Character_Click;
        }

        public virtual void Cast(Spell spell, List<XNAObject> targets)
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

        public virtual void Update()
        {
            if (Bounds.Contains(UI.MousePoint) && UI.CurrentMouseState.LeftButton == ButtonState.Released && UI.PreviousMouseState.LeftButton == ButtonState.Pressed )
            {
                Click(this, UI.CurrentMouseState);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color.White);
        }

        public virtual void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(TextureName);
        }

        public void OnGameStart(object sender, EventArgs e)
        {
            Debug.WriteLine("No ways, dude! I heard it too!" + Name);
        }

        private void Character_Click(object sender, MouseState mouseState)
        {
            Debug.WriteLine("You clicked me!");
            Debug.WriteLine("X, Y: " + mouseState.X + " " + mouseState.Y);
        }
    }

    public class Player : Character, Caster
    {
        public stat XP;

        public delegate void KeyboardEventHandler(object sender, KeyboardState keyboardState);

        public event KeyboardEventHandler KeyPress;

        public Player()
        {
            Engine.Start += OnGameStart;

            KeyPress += Player_KeyPress;

        }

        public override void Update()
        {
            base.Update();

            if (UI.CurrentKeyboardState.GetPressedKeys().Length == 0 && UI.PreviousKeyboardState.GetPressedKeys().Length != 0)
            {
                KeyPress(this, UI.PreviousKeyboardState);
            }
        }

        public void Player_KeyPress(object sender, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.S))
            {
                Bounds.Y += 64;
                UI.MapPoint.Y -= 64;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                Bounds.Y -= 64;
                UI.MapPoint.Y += 64;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                Bounds.X -= 64;
                UI.MapPoint.X += 64;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                Bounds.X += 64;
                UI.MapPoint.X -= 64;
            }
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
