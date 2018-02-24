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
using Wots.UI;

namespace Wots.Entities
{
    public abstract class AI
    {
        private Bar healthBar;
        public int Health = 100;

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
            Sprite = new Sprite(size, position - size);
            healthBar = new Bar(position);
            healthBar.Height = 7;
            healthBar.MaxWidth = size.X;
            healthBar.Color = Color.Red;
        }

        public abstract void Damage(int damage, Vector2 gestureDelta);
        public abstract void Update(GameTime gameTime, SpriteBatch sp);
        public void DrawHealth(SpriteBatch spriteBatch)
        {
            healthBar.Value = Health;
            healthBar.Position = Sprite.Position;
            healthBar.Draw(null, spriteBatch);
        }
    }
}