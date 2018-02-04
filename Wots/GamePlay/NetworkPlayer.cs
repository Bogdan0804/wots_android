using Microsoft.Xna.Framework;
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Wots.GamePlay
{
    public class NetworkPlayer
    {
        public Sprite PlayerSprite;
        public string Name { get; set; }

        public NetworkPlayer(string v)
        {
            this.Name = v;
            this.PlayerSprite = new Sprite(new Vector2(100, 200), GameScreen.Player.PlayerSprite.Position - new Vector2(0,100));

            // Create our animations
            PlayerSprite.Animations.Add("left", new Animation(
                new Frame(AssetManager.GetTexture("left_0")),
                new Frame(AssetManager.GetTexture("left_1")),
                new Frame(AssetManager.GetTexture("left_2"))
            ));
            PlayerSprite.Animations.Add("right", new Animation(
                new Frame(AssetManager.GetTexture("right_0")),
                new Frame(AssetManager.GetTexture("right_1")),
                new Frame(AssetManager.GetTexture("right_2"))
            ));

            PlayerSprite.CurrentAnimation = "left";
        }

        /// <summary>
        /// Our players position variable.
        /// </summary>
        public Vector2 Position
        {
            set
            {
                PlayerSprite.Position = value;
            }
            get
            {
                return PlayerSprite.Position;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)this.PlayerSprite.Position.X, (int)this.PlayerSprite.Position.Y, this.PlayerSprite.GetTexture().Width, this.PlayerSprite.GetTexture().Height);
            }
        }

        public void ChangeAnimation(string animationName)
        {
            if (animationName == null)
            {
                PlayerSprite.Animations[PlayerSprite.CurrentAnimation].Frame = 1;
                PlayerSprite.Animate = false;
            }
            else
            {
                PlayerSprite.Animate = true;
                PlayerSprite.CurrentAnimation = animationName;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(AssetManager.GetFont("12"), Name, PlayerSprite.Position - new Vector2(-5, 20), Color.Black);
            PlayerSprite.Draw(spriteBatch);
        }

        private Vector2 oldPosition = Vector2.Zero;

        public void Update(GameTime gameTime, Vector2 position)
        {
            PlayerSprite.Update(gameTime);

            if (this.oldPosition.X > position.X)
                ChangeAnimation("left");
            else if (this.Position.X < position.X)
                ChangeAnimation("right");
            else
                ChangeAnimation(null);

            oldPosition = position;

            this.Position = position;
        }
    }
}
