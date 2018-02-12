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
using Wots.UI;

namespace Wots.Entities
{
    public class SlimeAI : AI
    {
        private bool useGravity = true;

        public SlimeAI()
            : base(new Vector2(64), new Vector2(96 * 5, 96 * 2))
        {
            Sprite.Animations.Add("jump", new Animation(
                new Frame(AssetManager.GetTexture("slime1"))
                ));
            Sprite.Animations.Add("move", new Animation(
                new Frame(AssetManager.GetTexture("slime0"))
                ));

            Sprite.CurrentAnimation = "move";
        }


        bool jumping = false;
        double jumpBuildTime = 0;
        double attackTimer = 0;

        public override void Update(GameTime gameTime, SpriteBatch sp)
        {
            attackTimer += gameTime.ElapsedGameTime.TotalSeconds;
            jumpBuildTime += gameTime.ElapsedGameTime.TotalSeconds;

            bool canDown = World.isSpaceOpen(this.Sprite.Position + new Vector2(0, this.Sprite.Size.Y), null, new Vector2(64, 1)).Item1;
            bool canUp = World.isSpaceOpen(this.Sprite.Position, null, new Vector2(66, 1)).Item1;
            bool canLeft = World.isSpaceOpen(Sprite.Position + new Vector2(-1, 8), null, new Vector2(16, 32)).Item1;

            if (this.Sprite.Position.X > GameScreen.Player.PlayerSprite.Position.X
                && canLeft)
                this.Sprite.Position.X -= 3;
            else if (this.Sprite.Position.X < GameScreen.Player.PlayerSprite.Position.X
                && World.isSpaceOpen(Sprite.Position + new Vector2(this.Sprite.Size.X, 8), null, new Vector2(16, 32)).Item1)
                this.Sprite.Position.X += 3;
            if (canDown && useGravity)
                this.Sprite.Position.Y += (UniversalInputManager.Manager.Speed * GameManager.GAMESPEED);

            if (canUp && this.Sprite.Position.Y > GameScreen.Player.PlayerSprite.Position.Y && !canDown)
            {
                jumping = true;
                jumpBuildTime = 0;
            }
            // Code for jumping
            if (jumping && jumpBuildTime < 0.25)
            {
                useGravity = false;

                if (canUp)
                {
                    Sprite.CurrentAnimation = "jump";
                    this.Sprite.Position.Y -= UniversalInputManager.Manager.Speed * 3.5f;
                }
            }
            else
            {
                Sprite.CurrentAnimation = "move";
                useGravity = true;
                jumping = false;
            }

            if (GameScreen.Player.Bounds.Contains(this.Sprite.Position) && attackTimer > 0.1)
            {
                attackTimer = 0;
                GameScreen.Stats.Health.Value--;
            }

            Sprite.Update(gameTime);
        }
    }
}