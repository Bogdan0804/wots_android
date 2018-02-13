﻿using System;
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
        Physics coli = new Physics();
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
            coli.GeneratePoints(Sprite);
        }


        bool jumping = false;
        double jumpBuildTime = 0;
        double attackTimer = 0;
        bool canDown, canUp, canLeft, canRight;
        public override void Update(GameTime gameTime, SpriteBatch sp)
        {
            attackTimer += gameTime.ElapsedGameTime.TotalSeconds;
            jumpBuildTime += gameTime.ElapsedGameTime.TotalSeconds;
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            coli.UpdateCollitions(Sprite, sp);

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
                this.Sprite.Position.Y -= GameManager.GAMESPEED ;
                useGravity = false;
            }
            else if (coli.Collitions.Up.Point1.Item1 && coli.Collitions.Down.Point1.Item2.State == "fast4" && canUp && this.Sprite.Position.Y > GameScreen.Player.PlayerSprite.Position.Y)
            {
                jumping = true;
                jumpBuildTime = 0;
                useGravity = false;
            }

            bool oldState = useGravity;
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
                useGravity = oldState;
                jumping = false;
            }

            if (canDown && useGravity)
                this.Sprite.Position.Y += (UniversalInputManager.Manager.Speed * GameManager.GAMESPEED);

            if (GameScreen.Player.Bounds.Contains(this.Sprite.Position) && attackTimer > 0.1)
            {
                attackTimer = 0;
                GameScreen.Stats.Health.Value--;
            }

            Sprite.Update(gameTime);
        }
        double timer = 0;
        public override void Damage(int damage)
        {
            this.Health -= damage;
            if (Player.FacingDirection == Player.Direction.Left)
            {
                this.Sprite.Position.Y -= 100;
                this.Sprite.Position.X += 100;
            }
            else if (Player.FacingDirection == Player.Direction.Right)
            {
                this.Sprite.Position.Y -= 100;
                this.Sprite.Position.X -= 100;
            }
        }
    }
}