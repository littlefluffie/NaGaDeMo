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

        public event EventHandler Death;

        public Character()
        {
            Engine.Characters.Add(this);
        }


        public void Damage(int damage)
        {
            HP.Current -= damage;
            if (HP.Current <= 0)
            {
                Death(this, null);
            }
        }

        public virtual void Update(GameTime gametime)
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
            Debug.WriteLine("No ways, dude! I heard it too! " + Name);
        }

      
    }

    public class Player : Character
    {
        public stat XP;

        public stat AP;

        public delegate void KeyboardEventHandler(object sender, KeyboardState keyboardState);

        public event KeyboardEventHandler KeyPress;

        public Player()
        {
            
            Engine.Start += OnGameStart;

            
            KeyPress += Player_KeyPress;
            Click += Player_Click;

            Init();

        }

        public void Init()
        {
            HP.Current = HP.Max;
            MP.Current = MP.Max;
            AP.Current = AP.Max;

        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            if (UI.CurrentKeyboardState.GetPressedKeys().Length == 0 && UI.PreviousKeyboardState.GetPressedKeys().Length != 0)
            {
                KeyPress(this, UI.PreviousKeyboardState);
            }
        }

        #region Events

        private void Player_Click(object sender, MouseState mouseState)
        {
            Debug.WriteLine("You clicked yourself!");

            if (Engine.CurrentCommandInput is CastSpellCommand)
            {
                CastSpellCommand castspell = (CastSpellCommand)Engine.CurrentCommandInput;
                if (castspell.Spell.TargetType == TargetType.Self && castspell.CanExecute())
                {
                    castspell.Targets.Add(this);
                    castspell.Execute();

                    Engine.CommandList.Add(castspell);
                    Engine.CommandQueue.Remove(castspell);
                    Engine.CurrentCommandInput = null;
                }
            }
        }

        public void Player_KeyPress(object sender, KeyboardState keyboardState)
        {
            // Using key command for actions

            // Move command
            if (keyboardState.IsKeyDown(Keys.M))
            {
                MoveCommand move = new MoveCommand();
                move.Player = this;

                Engine.CommandQueue.Add(move);

                Engine.CurrentCommandInput = move;
            }

            if (keyboardState.IsKeyDown(Keys.C))
            {
                CastSpellCommand castspell = new CastSpellCommand();
                castspell.Caster = this;
                castspell.Spell = Templates.Spells.Spark();

                Engine.CommandQueue.Add(castspell);

                Engine.CurrentCommandInput = castspell;
            }

            if (keyboardState.IsKeyDown(Keys.H))
            {
                CastSpellCommand castspell = new CastSpellCommand();
                castspell.Caster = this;
                castspell.Spell = Templates.Spells.Heal();

                Engine.CommandQueue.Add(castspell);

                Engine.CurrentCommandInput = castspell;
            }


            if (keyboardState.IsKeyDown(Keys.S))
            {
                MoveCommand move = new MoveCommand();
                move.Player = this;
                move.MapPoint.X = Bounds.X;
                move.MapPoint.Y = Bounds.Y + 64;
                if (move.CanExecute())
                {
                    move.Execute();
                }
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                MoveCommand move = new MoveCommand();
                move.Player = this;
                move.MapPoint.X = Bounds.X;
                move.MapPoint.Y = Bounds.Y - 64;
                if (move.CanExecute())
                {
                    move.Execute();
                }
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                MoveCommand move = new MoveCommand();
                move.Player = this;
                move.MapPoint.X = Bounds.X - 64;
                move.MapPoint.Y = Bounds.Y ;
                if (move.CanExecute())
                {
                    move.Execute();
                }
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {

                MoveCommand move = new MoveCommand();
                move.Player = this;
                move.MapPoint.X = Bounds.X + 64;
                move.MapPoint.Y = Bounds.Y ;
                if (move.CanExecute())
                {
                    move.Execute();
                }
            }


        }

        #endregion
    }

    public class Creature : Character
    {
        
        public Creature()
        {
            Engine.Start += OnGameStart;

            Click += Creature_Click;
            Death += Creature_Death;
        }

        private void Creature_Death(object sender, EventArgs e)
        {
            Debug.WriteLine("Alas! I die!");
            Engine.CurrentBattle.Creatures.Remove(this);

        }



        private void Creature_Click(object sender, MouseState mouseState)
        {
            Debug.WriteLine("Megh! You clicked me!");
            Debug.WriteLine("X, Y: " + mouseState.X + " " + mouseState.Y);

            // Target of spell
            if (Engine.CurrentCommandInput is CastSpellCommand)
            {
                CastSpellCommand castspell = (CastSpellCommand)Engine.CurrentCommandInput;
                if (castspell.Spell.TargetType == TargetType.Single && castspell.CanExecute())
                {
                    castspell.Targets.Add(this);
                    castspell.Execute();

                    Engine.CommandList.Add(castspell);
                    Engine.CommandQueue.Remove(castspell);
                    Engine.CurrentCommandInput = null;
                }
            }
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
