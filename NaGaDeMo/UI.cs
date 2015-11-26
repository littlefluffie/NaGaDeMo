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
    /// The UI static class manages all User Interface related events and drawing
    /// </summary>
    public static class UI 
    {
        public static void DrawLine(SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(Pixel, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        public static class Overlay
        {
            public static void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.DrawString(UI.UIFont, "HP: " + Engine.CurrentBattle.Player.HP.Current, new Vector2(10, 10), Color.Black);
                spriteBatch.DrawString(UI.UIFont, "MP: " + Engine.CurrentBattle.Player.MP.Current, new Vector2(10, 25), Color.Black);
                spriteBatch.DrawString(UI.UIFont, "AP: " + Engine.CurrentBattle.Player.AP.Current, new Vector2(10, 40), Color.Black);

                spriteBatch.DrawString(UIFont, (Engine.GameState == Engine.State.PlayersTurn) ? "YOUR TURN" : "OPPONENTS TURN", new Vector2(500, 20), Color.Red);

                foreach (XNAObject Element in Elements)
                {
                    Element.Draw(spriteBatch);
                }

                //DrawLine(spriteBatch, 5f, Color.Red, new Vector2(0, 0), new Vector2(500, 300));
            }

            public static void Update(GameTime gameTime)
            {
                foreach (XNAObject Element in Elements)
                {
                    Element.Update(gameTime);
                }
            }
        }

        public static Texture2D Pixel;

        public static MouseState CurrentMouseState;
        public static MouseState PreviousMouseState;

        public static KeyboardState CurrentKeyboardState;
        public static KeyboardState PreviousKeyboardState;

        public static Point MapPoint = new Point(0, 0);

        public static Point MousePoint = new Point();

        public static SpriteFont UIFont;

        public static void Initialize()
        {

        }

        public static Rectangle GameView = new Rectangle(0, 0, 640, 640);

        public static List<XNAObject> Elements = new List<XNAObject>();

        public static void Draw(SpriteBatch spriteBatch)
        {
            // TODO Draw the UI

        }

        public static void LoadContent(ContentManager Content)
        {
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

    public class Textbox : XNAObject
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

    public class Button : XNAObject
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
            spriteBatch.Draw(UI.Pixel, Bounds, Color.White);
            spriteBatch.DrawString(UI.UIFont, Label, new Vector2(Bounds.X + (Bounds.Width / 2 - measurement.X / 2), Bounds.Y + (Bounds.Height / 2 - measurement.Y / 2)), Color.Black);

        }
    }
}
