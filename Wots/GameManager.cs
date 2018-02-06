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
using Wots.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Wots.GamePlay;

namespace Wots
{
    public class GameManager
    {
        #region Singleten
        private static GameManager _manager;
        public static GameManager Game
        {
            get
            {
                if (_manager == null)
                    _manager = new GameManager();

                return _manager;
            }
        }
        #endregion

        #region Private Variables
        private bool SkipRender = false;
        private bool isChangingScreen = false;
        private FrameCounter _frameCounter = new FrameCounter();
        private IGameScreen GameScreen;
        private double timeUpdateTimer = 0;
        #endregion

        #region Public Variables
        public ContentManager Content;
        public static bool DEBUG = false;
        public static bool MODE32 = false;
        public static string sessionID = null;
        public InGameTime CurrentGameTime;
        public GraphicsDeviceManager Graphics;
        public PauseScreen PauseScreen;
        public Vector2 ScreenSize
        {
            get
            {
                return new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            }
        }
        #endregion

        public static float GAMESPEED = 1;


        public void Initialize()
        {
            CurrentGameTime = new InGameTime();
            CurrentGameTime.Day = 0;
            CurrentGameTime.Hour = 22;
            CurrentGameTime.Minute = 55;
            GameScreen = new Screens.LoadingScreen();
            GameScreen.Intialize();
            PauseScreen = new PauseScreen();
            PauseScreen.Intialize();
        }
        public void LoadContent(ContentManager content)
        {
            // Load in all out fonts globaly
            AssetManager.Fonts.Add("12", GameManager.Game.Content.Load<SpriteFont>("Assets/fonts/12"));
            AssetManager.Fonts.Add("24", GameManager.Game.Content.Load<SpriteFont>("Assets/fonts/24"));
            AssetManager.Fonts.Add("36", GameManager.Game.Content.Load<SpriteFont>("Assets/fonts/36"));
            AssetManager.Fonts.Add("ConsoleFont", GameManager.Game.Content.Load<SpriteFont>("Assets/fonts/ConsoleFont"));
            AssetManager.LoadXml();
            GameScreen.LoadContent(content);
            PauseScreen.LoadContent(content);
        }
        public void Update(GameTime gameTime)
        {
            this.timeUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds * GameManager.GAMESPEED;

            if (this.timeUpdateTimer >= 0.3)
            {
                if (CurrentGameTime.Minute < 60)
                    CurrentGameTime.Minute++;
                else if (CurrentGameTime.Hour < 23 && CurrentGameTime.Minute >= 58)
                {
                    CurrentGameTime.Minute = 0;
                    CurrentGameTime.Hour++;
                }
                else
                {
                    CurrentGameTime.Minute = 0;
                    CurrentGameTime.Hour = 0;
                    CurrentGameTime.Day++;
                }

                this.timeUpdateTimer = 0;
            }
            

            if (!Paused)
                GameScreen.Update(gameTime);
            else
                PauseScreen.Update(gameTime);
            
        }

        public struct InGameTime
        {
            public float Day { get; set; }
            public float Minute { get; set; }
            public float Hour { get; set; }
        }
        public bool Paused = false;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch uiSpriteBatch)
        {
            if (!isChangingScreen && !SkipRender)
            {
                uiSpriteBatch.Begin(samplerState: SamplerState.PointWrap);
                GameScreen.Draw(gameTime, spriteBatch);
                for (int i = 0; i < GameScreen.UI.Count; i++)
                {
                    GameScreen.UI[i].Draw(gameTime, uiSpriteBatch);
                }
                uiSpriteBatch.End();
                for (int i = 0; i < GameScreen.UI.Count; i++)
                {
                    GameScreen.UI[i].Update(gameTime);
                }
            }

            spriteBatch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend);

            if (Paused)
            {
                PauseScreen.Draw(gameTime, spriteBatch);
                PauseScreen.Update(gameTime);
            }
            


            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * GameManager.GAMESPEED;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", Math.Round(_frameCounter.AverageFramesPerSecond));
            spriteBatch.DrawString(AssetManager.GetFont("36"), fps, Vector2.One, Color.Black);
            #region Date system
            //string minute = "";
            //if (CurrentGameTime.Minute.ToString().Split().Length == 0)
            //    minute = "0" + CurrentGameTime.Minute;
            //else
            //    minute = CurrentGameTime.Minute.ToString();
            //spriteBatch.DrawString(AssetManager.GetFont("ConsoleFont"), $"Day {CurrentGameTime.Day} [{CurrentGameTime.Hour}:{minute}]", new Vector2(1, 20), Color.Black);
            #endregion
            spriteBatch.End();
        }

        public void ChangeScreen(IGameScreen screen)
        {
            isChangingScreen = true;
            GameScreen.Unload();
            GameScreen = null;
            GameScreen = screen;
            GameScreen.Intialize();
            GameScreen.LoadContent(Content);
            isChangingScreen = false;
        }
    }

    public class CantDrawException : Exception
    {
        private string data = "";

        public CantDrawException(string name)
        {
            this.data = name;
        }

        public override string Message => $"You cannot draw a {data}. It has to be drawn through its correct handler.";
    }
}