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
    public class WoodWatcherAI : AI
    {
        Physics coli = new Physics();
        private bool useGravity = true;

        public WoodWatcherAI(Vector2 pos)
            : base(new Vector2(96 * 3, 96 * 5), pos)
        {
            Sprite.Animations.Add("moveLeft", new Animation(
                new Frame(AssetManager.LoadImage("art/mobs/monsters/woodwatcher/woodwatcher_left1")),
                new Frame(AssetManager.LoadImage("art/mobs/monsters/woodwatcher/woodwatcher_left2")),
                new Frame(AssetManager.LoadImage("art/mobs/monsters/woodwatcher/woodwatcher_left3"))
                ));
            Sprite.Animations.Add("moveRight", new Animation(
                new Frame(AssetManager.LoadImage("art/mobs/monsters/woodwatcher/woodwatcher_right1")),
                new Frame(AssetManager.LoadImage("art/mobs/monsters/woodwatcher/woodwatcher_right2")),
                new Frame(AssetManager.LoadImage("art/mobs/monsters/woodwatcher/woodwatcher_right3"))
                ));

            Sprite.CurrentAnimation = "moveLeft";
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
            bool changeAnimation = true;


            if (this.Sprite.Position.X > GameScreen.Player.PlayerSprite.Position.X
                && canLeft)
            {
                //    if (changeAnimation)
                this.Sprite.CurrentAnimation = "moveLeft";
                this.Sprite.Position.X -= 1;
            }
            else if (this.Sprite.Position.X < GameScreen.Player.PlayerSprite.Position.X
            && canRight)
            {
                //  if (changeAnimation)
                this.Sprite.CurrentAnimation = "moveRight";
                this.Sprite.Position.X += 1;
            }
            // Code for jumping
            if (jumping && jumpBuildTime < 0.25)
            {
                useGravity = false;

                if (canUp)
                {
                    this.Sprite.Position.Y -= UniversalInputManager.Manager.Speed * 2;
                }
            }
            else
            {
                useGravity = true;
                jumping = false;
            }

            if (canUp && this.Sprite.Position.Y + 96 > GameScreen.Player.PlayerSprite.Position.Y && !canDown && this.coli.Collitions.Up.Point1.Item2.State != "fast4")
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

            if (GameScreen.Player.PlayerSprite.GetRectangle().Intersects(this.Sprite.GetRectangle()) && attackTimer > 0.1)
            {
                attackTimer = 0;
                if (GameScreen.Stats.Health.Value > 0)
                    GameScreen.Stats.Health.Value--;
            }

            Sprite.Update(gameTime);
            //sp.Draw(AssetManager.LoadImage("art/mobs/monsters/woodwatcher/woodwatcher_left1"), this.Sprite.GetRectangle(), Color.Red);

        }
        double timer = 0;
        public override void Damage(int damage, Vector2 gestureDelta)
        {
            this.Health -= damage;

            if (GameScreen.Stats.EXP.Value < 100)
                GameScreen.Stats.EXP.Value += 2;

            if (gestureDelta.X < 0)
            {
                if (canUp)
                    this.Sprite.Position.Y -= 10;
                if (canLeft)
                    this.Sprite.Position.X += 10;
            }
            else if (gestureDelta.X <= 0)
            {
                if (canUp)
                    this.Sprite.Position.Y -= 10;
                if (canRight)
                    this.Sprite.Position.X -= 10;
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