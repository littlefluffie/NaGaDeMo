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
    public static class UI
    {
        public static class Overlay
        {
            public static void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.DrawString(UIFont, "X: " + MousePoint.X + " Y: " + MousePoint.Y + "Click: " + CurrentMouseState.LeftButton, new Vector2(10, 10), Color.Black);
                
            }
        }

        // At the top of your class:
        public static Texture2D pixel;

        public static MouseState CurrentMouseState;
        public static MouseState PreviousMouseState;

        public static Point MousePoint = new Point();

        public static Texture2D SpellBookTexture;
        public static SpriteFont UIFont;

        public static Rectangle GameView = new Rectangle(0, 0, 640, 640);

        //Temp
        public static bool CharClick;

        //Temp

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

        public static void Update()
        {
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            MousePoint.X = CurrentMouseState.X - GameView.X;
            MousePoint.Y = CurrentMouseState.Y - GameView.Y;
        }
    }

    public class Button
    {
        public Rectangle Bounds = new Rectangle(0, 0, 64, 64);

        public Button(int x, int y, int height, int width)
        {
            Bounds.X = x;
            Bounds.Y = y;
            Bounds.Height = height;
            Bounds.Width = width;
        }
    }
}
