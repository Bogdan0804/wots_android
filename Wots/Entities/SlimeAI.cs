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
using Microsoft.Xna.Framework.Input.Touch;
using Wots.GamePlay;
using Wots.UI;

namespace Wots.Entities
{
    public class SlimeAI : AI
    {
        Physics coli = new Physics();
        private bool useGravity = true;

        public SlimeAI(Vector2 pos)
            : base(new Vector2(64), pos)
        {
            Sprite.Animations.Add("jump", new Animation(
                new Frame(AssetManager.GetTexture("slime1"))
                ));
            Sprite.Animations.Add("move", new Animation(
                new Frame(AssetManager.GetTexture("slime0"))
                ));

            Sprite.CurrentAnimation = "move";
            coli.GeneratePoints(Sprite);
        }


        bool jumping = false;
        double jumpBuildTime = 0;
        double attackTimer = 0;
        bool canDown, canUp, canLeft, canRight;
        public override void Update(GameTime gameTime)
        {
            attackTimer += gameTime.ElapsedGameTime.TotalSeconds;
            jumpBuildTime += gameTime.ElapsedGameTime.TotalSeconds;
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            coli.UpdateCollitions(Sprite, null);

            canDown = coli.CanDown;
            canUp = coli.CanUp;
            canLeft = coli.CanLeft;
            canRight = coli.CanRight;

            if (this.Sprite.Position.X > GameScreen.Player.PlayerSprite.Position.X
                && canLeft)
                this.Sprite.Position.X -= 3;
            else if (this.Sprite.Position.X < GameScreen.Player.PlayerSprite.Position.X
                && canRight)
                this.Sprite.Position.X += 3;

            // Code for jumping
            if (jumping && jumpBuildTime < 0.25)
            {
                useGravity = false;

                if (canUp)
                {
                    Sprite.CurrentAnimation = "jump";
                    this.Sprite.Position.Y -= UniversalInputManager.Manager.Speed * 2;
                }
            }
            else
            {
                Sprite.CurrentAnimation = "move";
                useGravity = true;
                jumping = false;
            }

            if (canUp && this.Sprite.Position.Y > GameScreen.Player.PlayerSprite.Position.Y && !canDown && this.coli.Collitions.Up.Point1.Item2.State != "fast4")
            {
                jumping = true;
                jumpBuildTime = 0;
            }
            else if (canUp && this.Sprite.Position.Y > GameScreen.Player.PlayerSprite.Position.Y && this.coli.Collitions.Up.Point1.Item2.State == "fast4")
            {
                this.Sprite.Position.Y -= GameManager.GAMESPEED;
                useGravity = false;
            }
            else if (coli.Collitions.Up.Point1.Item1 && coli.Collitions.Down.Point1.Item2.State == "fast4" && canUp && this.Sprite.Position.Y > GameScreen.Player.PlayerSprite.Position.Y)
            {
                jumping = true;
                jumpBuildTime = 0;
                useGravity = false;
            }

            bool oldState = useGravity;


            if (canDown && useGravity)
                this.Sprite.Position.Y += (UniversalInputManager.Manager.Speed * GameManager.GAMESPEED);

            if (GameScreen.Player.Bounds.Contains(this.Sprite.Position) && attackTimer > 0.1)
            {
                attackTimer = 0;
                if (GameScreen.Stats.Health.Value > 0)
                    GameScreen.Stats.Health.Value--;
            }

            Sprite.Update(gameTime);
        }
        double timer = 0;
        public override void Damage(int damage, Vector2 gestureDelta)
        {
            this.Health -= damage * 2;

            if (GameScreen.Stats.EXP.Value < 100)
                GameScreen.Stats.EXP.Value++;
            if (gestureDelta.X < 0)
            {
                if (canUp)
                    this.Sprite.Position.Y -= 100;
                if (canLeft)
                    this.Sprite.Position.X += 100;
            }
            else if (gestureDelta.X <= 0)
            {
                if (canUp)
                    this.Sprite.Position.Y -= 100;
                if (canRight)
                    this.Sprite.Position.X -= 100;
            }
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gestures)
        {

        }

        public override void Draw(SpriteBatch sp)
        {
            DrawHealth(sp);
        }
    }
}