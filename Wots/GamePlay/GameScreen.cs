﻿// Copywrite Bogz. -)-
using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Collections;
using Wots.Screens;
using Wots.UI;
using Wots.GamePlay.Intro;
using Wots.Entities;
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots.GamePlay
{
    /// <summary>
    /// Description of GameScreen.
    /// </summary>
    public class GameScreen : IGameScreen
    {
        // our players
        public static Player Player { get; set; }
        public Bag<NetworkPlayer> MultiPlayers = new Bag<NetworkPlayer>(16);

        // Our ui things
        public static UI_Inventory_Menu ui_menu_inventory;
        DPad pad = new DPad();
        Button pause;
        public static BasicStats Stats;

        // Player cameras
        public static Camera2D Camera;

        // Splitscreen
        public bool SplitScreen = false;
        //pyblic  string name = "";
        public static Texture2D textureBlank;
        public static Texture2D white;

        // More game mechanics
        Combat combat;
        public static string BackdropName = "null";
        public static bool STOP = false;
        public GameScreen()
        {
            pad.Position = new Vector2(1, GameManager.Game.ScreenSize.Y - 265);
            // Setup our player and the world
            Player = new Player();
            // main camera
            Camera = new Camera2D(GameManager.Game.Graphics.GraphicsDevice);

            // mechanics
            combat = new Combat();

            // pause button
            pause = new Button(AssetManager.LoadImage("art/ui/pause"), new Vector2(GameManager.Game.ScreenSize.X - 69, 3), new Vector2(64));
            pause.Pressed += (e) =>
            {
                GameManager.Game.Paused = !GameManager.Game.Paused;
            };
            this.UI.Add(pause);

            white = new Texture2D(GameManager.Game.Graphics.GraphicsDevice, 4, 4);

            uint[] whiteU = new uint[4 * 4];
            for (int i = 0; i < 4 * 4; i++)
                whiteU[i] = (new Color(255, 255, 255) * 100).PackedValue;
            white.SetData<uint>(whiteU);

            textureBlank = new Texture2D(GameManager.Game.Graphics.GraphicsDevice, 4, 4);

            uint[] red = new uint[4 * 4];
            for (int i = 0; i < 4 * 4; i++)
                red[i] = (new Color(255, 0, 0) * 100).PackedValue;
            textureBlank.SetData<uint>(red);

            #region Serverside Stuff
            //if (client)
            //{
            //    var config = new NetPeerConfiguration("wots" + RPEngine.Program.major.ToString());
            //    this.netClient = new NetClient(config);
            //    netClient.Start();
            //    netClient.Connect(host: ip, port: 21211);

            //    useServer = true;
            //}
            //else
            //{
            //    var config = new NetPeerConfiguration(("wots" + RPEngine.Program.major.ToString()))
            //    {
            //        Port = 21211
            //    };
            //    SingleplayerServer = new NetServer(config);
            //    SingleplayerServer.Start();
            //}
            #endregion

        }

        SpeechDialog sp = new SpeechDialog();

        #region IGameScreen implementation

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameManager.Game.Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            if (BackdropName != "null")
            {
                spriteBatch.Begin(samplerState: SamplerState.PointWrap);
                spriteBatch.Draw(AssetManager.LoadImage(BackdropName), new Rectangle(0, (-(int)Player.PlayerSprite.Position.Y / 10) - 100, (int)GameManager.Game.ScreenSize.X, (int)GameManager.Game.ScreenSize.Y * 2), Color.White);
                spriteBatch.End();
            }
            // Draw the player
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());

            // Draw the world
            World.Draw(gameTime, spriteBatch);

            for (int i = 0; i < MultiPlayers.Count; i++)
            {
                MultiPlayers[i].Draw(gameTime, spriteBatch);
            }

            if (World.hasWorld)
                Player.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            //}
            // Draw the health, ect
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            pad.Draw(gameTime, spriteBatch);
            ui_menu_inventory.Draw(gameTime, spriteBatch);
            Stats.Draw(gameTime, spriteBatch);
            spriteBatch.End();

        }

        int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }

        public override void Update(GameTime gameTime)
        {
            if (!STOP)
            {
                Stats.Update(gameTime);
                pad.Update(gameTime);
                combat.Update(gameTime);
                // Update the world
                World.Update(gameTime);
                UpdatePlayerCameras(gameTime);
            }

            ui_menu_inventory.Update(gameTime);
        }
        void UpdatePlayerCameras(GameTime gameTime)
        {
            Vector2 playerPos = Player.PlayerSprite.Position;

            Camera.Position = (playerPos - (new Vector2((int)GameManager.Game.ScreenSize.X / 2, (int)GameManager.Game.ScreenSize.Y / 2))).Round(1);
            // Run our players update command
            if (World.hasWorld)
                Player.Update(gameTime);
        }

        public override void LoadContent(ContentManager content)
        {
            pad.LoadContent();
            // Load our players content and set an initial state
            Player.LoadContent(content);
            Stats = new BasicStats();
            World.Intialize();
        }


        public override void Intialize()
        {
            // Intialize the players
            Player.Intialize();
            ui_menu_inventory = new UI_Inventory_Menu();
            //this.UI.Add(ui_menu_inventory);
            // this.UI.Add(new GameIntro());
        }

        public override void Unload()
        {
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gestures)
        {
            World.UpdateGestures(touches, gestures);
            ui_menu_inventory.UpdateGestures(touches, gestures);
            pad.UpdateGestures(touches, gestures);
            Stats.UpdateGestures(touches, gestures);
        }

        #endregion
    }
}
