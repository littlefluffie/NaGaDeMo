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
    public abstract class UIElement : XNAObject
    {
        public UIElement()
        {
            Click += UIElement_Click;
        }

        private void UIElement_Click(object sender, MouseState mouseState)
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
                spriteBatch.DrawString(UI.UIFont, "Blah", new Vector2(10, 10), Color.Black);

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

        public static Texture2D pixel;

        public static Texture2D CircleOverlay;

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

            CircleOverlay = Content.Load<Texture2D>("Circle");
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

            if (CurrentMouseState.MiddleButton == ButtonState.Pressed)
            {

                Point Movement = new Point();
                Movement.X = (CurrentMouseState.X - PreviousMouseState.X);
                Movement.Y = (CurrentMouseState.Y - PreviousMouseState.Y);

                UI.MapPoint += Movement;

                if (UI.MapPoint.X > 0)
                {
                    UI.MapPoint.X = 0;
                }

                if (UI.MapPoint.Y > 0)
                {
                    UI.MapPoint.Y = 0;
                }


                if (UI.MapPoint.X < -Engine.CurrentBattle.GameMap.Width * 64 + UI.GameView.Width)
                {
                    UI.MapPoint.X = -Engine.CurrentBattle.GameMap.Width * 64 + UI.GameView.Width;
                }

                if (UI.MapPoint.Y < -Engine.CurrentBattle.GameMap.Height * 64 + UI.GameView.Height)
                {
                    UI.MapPoint.Y = -Engine.CurrentBattle.GameMap.Height * 64 + UI.GameView.Height;
                }


            }

        }
    }

    public class Textbox : UIElement
    {
        public event EventHandler OnUpdate;

        public string Text;

        public Textbox(int x, int y, int height, int width, string text)
        {
            Bounds.X = x;
            Bounds.Y = y;
            Bounds.Height = height;
            Bounds.Width = width;
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 measurement = UI.UIFont.MeasureString(Text);

            spriteBatch.DrawString(UI.UIFont, Text, new Vector2(Bounds.X + (Bounds.Width / 2 - measurement.X / 2), Bounds.Y + (Bounds.Height / 2 - measurement.Y / 2)), Color.Black);
        }
    }

    public class Button : UIElement
    {
        public string Label;

        public Button(int x, int y, int height, int width, string label)
        {
            Bounds.X = x;
            Bounds.Y = y;
            Bounds.Height = height;
            Bounds.Width = width;
            Label = label;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 measurement = UI.UIFont.MeasureString(Label);
            spriteBatch.Draw(UI.pixel, Bounds, Color.White);
            spriteBatch.DrawString(UI.UIFont, Label, new Vector2(Bounds.X + (Bounds.Width / 2 - measurement.X / 2), Bounds.Y + (Bounds.Height / 2 - measurement.Y / 2)), Color.Black);

        }
    }
}
