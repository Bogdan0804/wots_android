using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Wots.GamePlay;

namespace Wots.UI
{
    public class Bar
    {
        // Dimentions
        public float MaxWidth = 200;
        public float Height = 30;

        // The value
        public int Value = 100;

        // Its positions
        public Vector2 Position;

        // some spice (coloring)
        public Color Color = Color.White;

        public Bar(Vector2 pos)
        {
            this.Position = pos;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameScreen.white, new Rectangle(Position.ToPoint(), new Point((int)Extentions.Map(Value, 0, 100, 0, MaxWidth + 2), (int)Height)), Color);
        }
    }
}