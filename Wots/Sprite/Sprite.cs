using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Wots.GamePlay;

namespace Wots
{
    public class Sprite
    {
        public Vector2 Size { get; set; }

        public Vector2 Position;

        public float Alpha = 255;

        public Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();
        public string CurrentAnimation = "";

        public Color Color = Color.White;

        public bool Animate = true;

        public Sprite(Vector2 size, Vector2 position)
        {
            this.Size = size;
            this.Position = position;
        }

        public Frame CurrentAnimationFrame
        {
            get
            {
                return Animations[CurrentAnimation].GetFrame();
            }
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        public Rectangle GetScreenRectangle()
        {
            return new Rectangle(GameScreen.Camera.WorldToScreen(Position).ToPoint(), Size.ToPoint());
        }
        public Texture2D GetTexture()
        {
            return CurrentAnimationFrame.Texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(GetTexture(), new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), new Color(this.Color.R, this.Color.G, this.Color.B, Alpha));
            }
            catch { /* We are still intializeing, wait */ }
        }
        public void Update(GameTime gameTime)
        {
            try
            {
                if (Animate)
                    Animations[CurrentAnimation].Update(gameTime, CurrentAnimationFrame.FrameTime);
            }
            catch
            {

            }

        }
    }
}