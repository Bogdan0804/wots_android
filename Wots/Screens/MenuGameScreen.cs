﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Wots.UI;
using Wots.GamePlay;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
using Android.App;
using Android.Gms.Games.Achievement;
using Android.Gms.Common.Apis;
using System.Reflection;

namespace Wots.Screens
{
    public class MenuGameScreen : IGameScreen
    {
        private Texture2D background;
        private Menu menu;
        private SoundEffect click_select;
        private Texture2D titleTexture;
        private Texture2D cloud;
        private Texture2D Slide1;
        private float slide1X = -20;
        private Texture2D sunSet;
        //  Effect glow;
        private float sunTime = 0.0f;
        private float moutainsDarkness = 1f;
        private Vector2 cloudPos;
        // Set us a fixed size
        Vector2 size = new Vector2(550, 320);

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Enable pixely graphics
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Immediate);

            
            // This ofset is a asmall value that can be added to a objects posotion to give it mouse feedback
            var offset = new Vector2(UniversalInputManager.Manager.Accelerometer.CurrentValue.Acceleration.Y * 50, UniversalInputManager.Manager.Accelerometer.CurrentValue.Acceleration.Z * 25);
            // Draw the background and the first slide
            {
                Rectangle sky = new Rectangle((int)offset.X - 30, (int)offset.Y - 30, (int)GameManager.Game.ScreenSize.X * 20, (int)GameManager.Game.ScreenSize.Y + 120);
                spriteBatch.Draw(background, sky, Color.White);
                spriteBatch.Draw(sunSet, sky, Color.White * sunTime);
                //spriteBatch.Draw(cloud, new Rectangle(cloudPos.ToPoint(), new Point(5000, 400)), Color.White);

                // Draw the mountains witha darrkness
                Color desaturatedColor= Color.Lerp(Color.White, Color.Black, moutainsDarkness / 10);
                spriteBatch.Draw(Slide1, new Rectangle((int)slide1X, 175, (int)GameManager.Game.ScreenSize.X * 15, (int)GameManager.Game.ScreenSize.Y - 150), desaturatedColor);
            }
            // Draw the title
            {
                // Create a dynamicly chaging position for our title, using our ofset variable
                Vector2 position = new Vector2(((GameManager.Game.ScreenSize.X / 2) - size.X / 2) + offset.X , -offset.Y + 50);

                // Draw it
                spriteBatch.Draw(titleTexture, new Rectangle(position.ToPoint(), (size).ToPoint()), Color.White * 0.8f);
            }

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string buildNo = String.Format("V{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

            string copyText = "Copyright 2018 Bogz, Tim & Aidan\n" + buildNo + " (Public Beta)";
            spriteBatch.DrawString(AssetManager.GetFont("24"), copyText, GameManager.Game.ScreenSize - AssetManager.GetFont("24").MeasureString(copyText) - new Vector2(1), Color.White);
            // End the spritebatch (sends all date to the graphics card to be rendered).
            spriteBatch.End();
        }

        double time = 0;
        bool reverseSunTime = false;
        double sunTimeDelay = 0;
        KeyboardState oldState;
        double cloudMoveTimer = 0;
        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            time += gameTime.ElapsedGameTime.TotalSeconds * GameManager.GAMESPEED;
            sunTimeDelay += gameTime.ElapsedGameTime.TotalSeconds * GameManager.GAMESPEED;
            cloudMoveTimer += gameTime.ElapsedGameTime.TotalSeconds * GameManager.GAMESPEED;

            // Move the clouds..
            if (cloudMoveTimer > 0.1)
            {
                cloudMoveTimer = 0;
                if (cloudPos.X < GameManager.Game.ScreenSize.X)
                    cloudPos.X++;
            }


            // Change the color of the sky and the dimness of the mountains
            if (sunTimeDelay > 0.3)
            {
                sunTimeDelay = 0;
                if (reverseSunTime)
                {
                    if (moutainsDarkness > 1)
                        moutainsDarkness -= 0.2f;
                    else
                        reverseSunTime = false;
                    if (sunTime > 0.01)
                        sunTime -= 0.008f;
                }
                else
                {
                    if (moutainsDarkness < 8)
                        moutainsDarkness += 0.1f;
                    else if (moutainsDarkness >= 8)
                        reverseSunTime = true;
                    if (sunTime < 1)
                        sunTime += 0.008f;
                }
            }
            // Move the slide over slightley every 100ms
            if (time > 0.090)
            {
                slide1X--;
                time = 0;
            }


            oldState = state;
        }
        public override void LoadContent(ContentManager content)
        {
            // click_select = content.Load<SoundEffect>("audio/click2");
            //  this.glow = content.Load<Effect>("glow");
            background = AssetManager.LoadImage("art/bg1");
            Slide1 = AssetManager.LoadImage("art/slides/1");
            sunSet = AssetManager.LoadImage("art/sunset");
            cloud = AssetManager.LoadImage("art/cloud");
            AssetManager.AddTexture("sunset", AssetManager.LoadImage("art/sunset"));
            this.menu = new Menu();

            // Create a button
            {
                Vector2 size = new Vector2(200, 75);
                var startBtn = new MenuItem(AssetManager.LoadImage("art/ui/buttonPlay"), ((GameManager.Game.ScreenSize / 2) - (size / 2)) + new Vector2(0, 100), size);

                startBtn.Pressed += startBtn_Pressed;

                this.menu.AddItem(startBtn);
            }
            // Create a button
            {
                Vector2 size = new Vector2(200, 75);
                var exitBtn = new MenuItem(AssetManager.LoadImage("art/ui/buttonExit"), ((GameManager.Game.ScreenSize / 2) - (size / 2) + new Vector2(6, 185)), size);

                exitBtn.Pressed += ExitBtn_Pressed;

                this.menu.AddItem(exitBtn);
            }
       
            titleTexture = AssetManager.LoadImage("art/title_1");
            this.UI.Add(menu);
        }

        private void Options_Pressed(object sender)
        {
            //GameManager.Game.ChangeScreen(new RPEngine.Screens.Options.MainScreen());
        }

        public override void Intialize()
        {
            cloudPos = new Vector2(-2100, 10);
            
        }

        public override void Unload()
        {
            background.Dispose();
        }

        void startBtn_Pressed(object sender)
        {
            GameManager.Game.ChangeScreen(new GameScreen());
        }

        void multiplayer_Pressed(object sender)
        {
           // click_select.Play();
        }

        void ExitBtn_Pressed(object sender)
        {
            Game1.Activity.FinishAffinity();
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {

        }
    }

    internal class xButton
    {
    }
}
