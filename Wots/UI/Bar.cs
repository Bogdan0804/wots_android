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


        public Bar(Vector2 pos)
        {
            this.Position = pos;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.GetTexture("sunset"), new Rectangle(Position.ToPoint(), new Point((int)Extentions.Map(Value, 1, 100, 5, MaxWidth), (int)Height)), new Color(255, Extentions.Map(Value, 1, 100, 0, 255), 255));
        }
    }
}