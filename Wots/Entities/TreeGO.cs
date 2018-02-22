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

namespace Wots.Entities
{
    public class TreeGO : GameObject
    {
        public TreeGO()
        {
            Texture = AssetManager.LoadImage("art/tree_1");
            Size = new Vector2(96 * 3, 96 * 4);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(((Position - Size) + new Vector2(192, 96)).ToPoint(), Size.ToPoint()), Color.White);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}