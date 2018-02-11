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

namespace Wots.Entities
{
    public abstract class AI
    {
        public Sprite Sprite { get; set; }
        

        public AI(Vector2 size, Vector2 position)
        {
            Sprite = new Sprite(size, position);
        }

        public abstract void Update(GameTime gameTime);
    }
}