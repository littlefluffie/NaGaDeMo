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
    /// <summary>
    /// The UI Element Class is the base class of all User Interface Elements
    /// </summary>
    public abstract class UIElement: XNAObject
    {
        public Rectangle Bounds = new Rectangle(0, 0, 64, 64);

        public event Engine.MouseEventHandler Click;

        public virtual void Draw(SpriteBatch spritebatch)
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Bounds.Contains(UI.CurrentMouseState.Position) && UI.CurrentMouseState.LeftButton == ButtonState.Released && UI.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                Click(this, UI.CurrentMouseState);
            }

        }

        public virtual void LoadContent(ContentManager Content)
        {
            
        }
    }

    /// <summary>
    /// The UI static class manages all User Interface related events and drawing
    /// </summary>
    public static class UI
    {
        public static class Overlay 
        {
            public static void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.DrawString(UI.UIFont, "X: " + UI.MousePoint.X + " Y: " + UI.MousePoint.Y + "Click: " + UI.CurrentMouseState.LeftButton, new Vector2(10, 10), Color.Black);

                foreach (UIElement Element in Elements)
                {
                    Element.Draw(spriteBatch);
                }
            }

            public static void Update(GameTime gameTime)
            {
                foreach (UIElement Element in Elements)
                {
                    Element.Update(gameTime);
                }
            }
        }

        // At the top of your class:
        public static Texture2D pixel;

        public static MouseState CurrentMouseState;
        public static MouseState PreviousMouseState;

        public static KeyboardState CurrentKeyboardState;
        public static KeyboardState PreviousKeyboardState;

        public static Point MapPoint = new Point(0, 0);

        public static Point MousePoint = new Point();

        public static Texture2D SpellBookTexture;
        public static SpriteFont UIFont;

        public static Rectangle GameView = new Rectangle(0, 0, 640, 640);

        public static List<UIElement> Elements = new List<UIElement>();

        public static void Draw(SpriteBatch spriteBatch)
        {
            // TODO Draw the UI
            spriteBatch.Draw(SpellBookTexture, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White);
        }

        public static void LoadContent(ContentManager Content)
        {
            // Somewhere in your LoadContent() method:
            SpellBookTexture = Content.Load<Texture2D>("Spellbook");
            UIFont = Content.Load<SpriteFont>("Centaur");
        }

        public static void Update(GameTime gameTime)
        {
            // Mouse
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            // This calculates the location of the mouse relative to the Game Window offset and the map viewpoint offset
            MousePoint.X = CurrentMouseState.X - GameView.X - MapPoint.X;
            MousePoint.Y = CurrentMouseState.Y - GameView.Y - MapPoint.Y;

            // Keyboard
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            // Cancel command
            if (CurrentMouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed)
            {
                Engine.CurrentCommandInput = null;
            }

        }
    }

    public class Button : UIElement
    {
        public string Label;
                
        public Button(int x, int y, int height, int width)
        {
            Bounds.X = x;
            Bounds.Y = y;
            Bounds.Height = height;
            Bounds.Width = width;


        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 measurement = UI.UIFont.MeasureString(Label);
            spriteBatch.Draw(UI.pixel, Bounds, Color.White);
            spriteBatch.DrawString(UI.UIFont, Label, new Vector2(Bounds.X + (Bounds.Width / 2 - measurement.X / 2), Bounds.Y + (Bounds.Height / 2 - measurement.Y / 2)), Color.Black);
            
        }
    }
}
