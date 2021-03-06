﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Wots.UI;
using Wots;
using Wots.GamePlay.Tiles;

namespace Wots.GamePlay
{
    public class Player
    {
        public Vector2 Velocity;

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public static Bar HealthBar;
        //public static int Health = 100;

        public static Direction FacingDirection;
        public class CollitionPoint
        {
            public Tuple<bool, Tile> Point1, Point2;

            public CollitionPoint(Tuple<bool, Tile> p1, Tuple<bool, Tile> p2)
            {
                Point1 = p1;
                Point2 = p2;
            }
        }
        // Gravity
        public float GravitySpeed = 10.0f;
        public bool useGravity = true;
        public static bool noClip = false;
        public bool d3 = false;

        public struct CollitionDetection
        {
            public CollitionPoint Up;
            public CollitionPoint Down;
            public CollitionPoint Left;
            public CollitionPoint Right;
        }
        public CollitionDetection Collitions = new CollitionDetection();

        bool canUp = true, canDown = true, canLeft = true, canRight = true;
        // Store all of our player textures in variables
        public Texture2D CurrentDirection
        {
            get
            {
                return PlayerSprite.GetTexture();
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)this.PlayerSprite.Position.X, (int)this.PlayerSprite.Position.Y, this.PlayerSprite.GetTexture().Width, this.PlayerSprite.GetTexture().Height);
            }
        }

        RenderTarget2D TargetForShaders;

        // Store our sprite
        public Sprite PlayerSprite;

        // Store our direction in a vector2
        public Vector2 TextureDirection;


        /// <summary>
        /// Intialize this instance.
        /// </summary>
        public void Intialize()
        {
            TextureDirection = new Vector2(-1, 0);
            PlayerSprite = new Sprite(new Vector2(100, 200), Vector2.One);
            HealthBar = new Bar(new Vector2(10, 10));
            Velocity = new Vector2(0);

            this.TargetForShaders = new RenderTarget2D(GameManager.Game.Graphics.GraphicsDevice, (int)this.PlayerSprite.Size.X, (int)this.PlayerSprite.Size.Y);
        }
        #region Load content
        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="content">Content.</param>
        public void LoadContent(ContentManager content)
        {

            AssetManager.AddTexture("down_0", AssetManager.LoadImage("art/player/playerDown/1"));
            AssetManager.AddTexture("down_1", AssetManager.LoadImage("art/player/playerDown/2"));
            AssetManager.AddTexture("down_2", AssetManager.LoadImage("art/player/playerDown/3"));

            AssetManager.AddTexture("up_0", AssetManager.LoadImage("art/player/playerUp/1"));
            AssetManager.AddTexture("up_1", AssetManager.LoadImage("art/player/playerUp/2"));
            AssetManager.AddTexture("up_2", AssetManager.LoadImage("art/player/playerUp/3"));

            AssetManager.AddTexture("right_0", AssetManager.LoadImage("art/player/playerRight/1"));
            AssetManager.AddTexture("right_1", AssetManager.LoadImage("art/player/playerRight/2"));
            AssetManager.AddTexture("right_2", AssetManager.LoadImage("art/player/playerRight/3"));

            AssetManager.AddTexture("left_0", AssetManager.LoadImage("art/player/playerLeft/1"));
            AssetManager.AddTexture("left_1", AssetManager.LoadImage("art/player/playerLeft/2"));
            AssetManager.AddTexture("left_2", AssetManager.LoadImage("art/player/playerLeft/3"));



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
            PlayerSprite.Animations.Add("up", new Animation(
                new Frame(AssetManager.GetTexture("up_0")),
                new Frame(AssetManager.GetTexture("up_1")),
                new Frame(AssetManager.GetTexture("up_2"))
            ));
            PlayerSprite.Animations.Add("down", new Animation(
                new Frame(AssetManager.GetTexture("down_0")),
                new Frame(AssetManager.GetTexture("down_1")),
                new Frame(AssetManager.GetTexture("down_2"))
            ));

            PlayerSprite.CurrentAnimation = "left";
            PlayerSprite.Position = new Vector2(0, -10);

        }
        #endregion
        public void Update(GameTime gameTime)
        {
            ChangeAnimation();
            MovePlayer(gameTime);
            PlayerSprite.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawPlayer(spriteBatch);
        }

        private void UpdateColitions(SpriteBatch spriteBatch)
        {
            Collitions.Up = new CollitionPoint(
                World.isSpaceOpen(this.PlayerSprite.Position + new Vector2(12, 1), null, new Vector2(15, 10)),
                World.isSpaceOpen(this.PlayerSprite.Position + new Vector2(76, 1), null, new Vector2(15, 10))
                );
            Collitions.Down = new CollitionPoint(
                World.isSpaceOpen(this.PlayerSprite.Position + new Vector2(6, 192), null, new Vector2(15, 10)),
                World.isSpaceOpen(this.PlayerSprite.Position + new Vector2(72, 192), null, new Vector2(15, 10))
                );
            Collitions.Left = new CollitionPoint(
                World.isSpaceOpen(this.PlayerSprite.Position - new Vector2(10, -8f), null, new Vector2(10, 80)),
                World.isSpaceOpen(this.PlayerSprite.Position - new Vector2(10, -100f), null, new Vector2(10, 80))
                );
            Collitions.Right = new CollitionPoint(
                World.isSpaceOpen(this.PlayerSprite.Position + new Vector2(90, 8), null, new Vector2(10, 80)),
                World.isSpaceOpen(this.PlayerSprite.Position + new Vector2(90, 100), null, new Vector2(10, 80))
                );

            canUp = Collitions.Up.Point1.Item1 & Collitions.Up.Point2.Item1;
            canLeft = Collitions.Left.Point1.Item1 & Collitions.Left.Point2.Item1;
            canRight = Collitions.Right.Point1.Item1 & Collitions.Right.Point2.Item1;
            canDown = Collitions.Down.Point1.Item1 & Collitions.Down.Point2.Item1;

        }

        /// <summary>
        /// Draws the player.
        /// </summary>
        public void DrawPlayer(SpriteBatch spriteBatch)
        {
            string currenttext = "";
            int x = (int)TextureDirection.X;
            int y = (int)TextureDirection.Y;

            if (y == 1)
            {
                currenttext = "down";
                PlayerSprite.Animate = true;
                PlayerSprite.CurrentAnimation = currenttext;
            }
            if (x == 1)
            {
                currenttext = "left";
                PlayerSprite.Animate = true;
                PlayerSprite.CurrentAnimation = currenttext;
            }
            else if (x == -1)
            {
                currenttext = "right";
                PlayerSprite.Animate = true;
                PlayerSprite.CurrentAnimation = currenttext;
            }

            else if (x == 0 && y == 0)
                PlayerSprite.Animate = false;

            PlayerSprite.Draw(spriteBatch);
            //UpdateColitions(spriteBatch);
        }

        Vector2 oldDir = Vector2.Zero;
        /// <summary>
        /// Changes the direction.
        /// </summary>
        public void ChangeAnimation()
        {
            var newDir = PlayerSprite.Position;


            if (UniversalInputManager.Manager.GetAxis("Horizontal") == 1 || UniversalInputManager.Manager.GetAxis("Horizontal") == 1 && UniversalInputManager.Manager.GetAxis("Vertical") == 1)
            {
                TextureDirection = new Vector2(-1, 0);
                FacingDirection = Direction.Left;
            }
            else if (UniversalInputManager.Manager.GetAxis("Horizontal") == -1 || UniversalInputManager.Manager.GetAxis("Horizontal") == -1 && UniversalInputManager.Manager.GetAxis("Vertical") == 1)
            {
                TextureDirection = new Vector2(1, 0);
                FacingDirection = Direction.Right;
            }

            if (UniversalInputManager.Manager.GetAxis("Horizontal") == 0 && UniversalInputManager.Manager.GetAxis("Vertical") == 0 || UniversalInputManager.Manager.GetAxis("Vertical") == 1 && UniversalInputManager.Manager.GetAxis("Horizontal") == 0)
            {
                PlayerSprite.Animations[PlayerSprite.CurrentAnimation].Frame = 1;
                PlayerSprite.Animate = false;
            }
            this.oldDir = newDir;
        }

        bool jumping = false;
        double jumpBuildTime = 0;

        /// <summary>
        /// Moves the player.
        /// </summary>
        public void MovePlayer(GameTime gameTime)
        {
            jumpBuildTime += gameTime.ElapsedGameTime.TotalSeconds;

            HandleMovements(gameTime);
            UpdateColitions(null);
        }


        private double fallingTimer = 0;
        private void HandleMovements(GameTime gameTime)
        {
            if (canDown)
                fallingTimer += gameTime.ElapsedGameTime.TotalSeconds;
            else
            {
                Velocity.Y = 0;
                fallingTimer = 0;
            }
            if (fallingTimer > 0.2 && Velocity.Y <= 10)
            {
                fallingTimer = 0;
                Velocity.Y += 0.5f;
            }
            // Make sure that collitions are enabled.
            if (!noClip)
            {
                // Check if we pressed jump key and if we can jump
                if (UniversalInputManager.Manager.GetAxis("Vertical") == 1 && !canDown && canUp)
                {
                    jumpBuildTime = 0;
                    if (FacingDirection == Direction.Left)
                    {
                        if (Collitions.Up.Point1.Item2.Prefs.usePrefJump)
                        {
                            jumping = Collitions.Up.Point1.Item2.Prefs.OnJump(this);

                        }
                        else jumping = true;
                    }
                    else if (FacingDirection == Direction.Right)
                    {
                        if (Collitions.Up.Point2.Item2.Prefs.usePrefJump)
                        {
                            jumping = Collitions.Up.Point2.Item2.Prefs.OnJump(this);

                        }
                        else jumping = true;
                    }
                }

                bool oldGravity = useGravity;
                // Code for jumping
                if (jumping && jumpBuildTime < 0.25)
                {
                    useGravity = false;

                    if (canUp)
                        this.PlayerSprite.Position.Y -= UniversalInputManager.Manager.Speed * 3.5f;
                }
                else
                {
                    useGravity = !jumping;
                    jumping = false;
                }

                if (UniversalInputManager.Manager.GetAxis("Vertical") == 1)
                {
                    if (canDown)
                    {
                        if (FacingDirection == Direction.Left)
                        {
                            if (Collitions.Down.Point1.Item2.Prefs.usePrefUp)
                                useGravity = Collitions.Down.Point1.Item2.Prefs.OnUp(this);
                        }
                        else if (FacingDirection == Direction.Right)
                        {
                            if (Collitions.Down.Point2.Item2.Prefs.usePrefUp)
                                useGravity = Collitions.Down.Point2.Item2.Prefs.OnUp(this);
                        }
                    }
                    else
                    {
                        if (FacingDirection == Direction.Left)
                        {
                            if (Collitions.Up.Point1.Item2.Prefs.usePrefUp)
                                useGravity = Collitions.Up.Point1.Item2.Prefs.OnUp(this);
                        }
                        else if (FacingDirection == Direction.Right)
                        {
                            if (Collitions.Up.Point2.Item2.Prefs.usePrefUp)
                                useGravity = Collitions.Up.Point2.Item2.Prefs.OnUp(this);
                        }
                    }
                }
                
                GravitySpeed = 10.0f;

                try
                {
                    if (Collitions.Down.Point1.Item2.Prefs.usePrefDown)
                        Collitions.Down.Point1.Item2.Prefs.OnDown(this);
                }
                catch { };

                // our psuedo gravity
                if (useGravity && canDown)
                    PlayerSprite.Position.Y += GravitySpeed + Velocity.Y;

                if ((UniversalInputManager.Manager.GetAxis("Horizontal") == -1 || GamePad.GetState(0).Triggers.Left > 100) && canLeft)
                {
                    if (Collitions.Left.Point1.Item2.Prefs.usePrefLeft)
                        Collitions.Left.Point1.Item2.Prefs.OnLeft(this);
                    else if (Collitions.Left.Point2.Item2.Prefs.usePrefLeft)
                        Collitions.Left.Point2.Item2.Prefs.OnLeft(this);
                    else
                        PlayerSprite.Position.X -= UniversalInputManager.Manager.Speed * GameManager.GAMESPEED;
                    //oldPos.X += 3;
                }
                else if ((UniversalInputManager.Manager.GetAxis("Horizontal") == 1 || GamePad.GetState(0).Triggers.Right > 100) && canRight)
                {
                    if (Collitions.Right.Point1.Item2.Prefs.usePrefRight)
                        Collitions.Right.Point1.Item2.Prefs.OnRight(this);
                    else if (Collitions.Right.Point2.Item2.Prefs.usePrefRight)
                        Collitions.Right.Point2.Item2.Prefs.OnRight(this);
                    else
                        PlayerSprite.Position.X += UniversalInputManager.Manager.Speed * GameManager.GAMESPEED;
                    //oldPos.X -= 3;
                }

            }
        }

    }
}