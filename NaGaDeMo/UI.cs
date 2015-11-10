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
        public static Texture2D SpellBookTexture;
        public static SpriteFont UIFont;

        public static Rectangle GameView = new Rectangle(210, 100, 640, 640);

        public static string MouseCoordinates;

        public static void Draw(SpriteBatch spriteBatch)
        {
            // TODO Draw the UI
            spriteBatch.Draw(SpellBookTexture, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White);
            spriteBatch.DrawString(UIFont, MouseCoordinates, new Vector2(100, 100), Color.Black);
        }

        public static void LoadContent(ContentManager Content)
        {
            SpellBookTexture = Content.Load<Texture2D>("Spellbook");
            UIFont = Content.Load<SpriteFont>("Centaur");
        }
    }
}
