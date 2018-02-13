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
    public abstract class AI
    {
        public int Health=100;

        public Rectangle HitBox
        {
            get
            {
                return new Rectangle(Sprite.Size.ToPoint(), Sprite.Position.ToPoint());
            }
        }
        public Sprite Sprite { get; set; }

        public AI(Vector2 size, Vector2 position)
        {
            Sprite = new Sprite(size, position);
        }

        public abstract void Damage(int damage);
        public abstract void Update(GameTime gameTime, SpriteBatch  sp);
    }
}