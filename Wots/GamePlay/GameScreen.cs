// Copywrite Bogz. -)-
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
        UI_Inventory_Menu ui_menu_inventory;

        //// Networking
        //NetClient netClient;
        //public static NetServer SingleplayerServer;
        //public static string Name;

        // Player cameras
        public static Camera2D Camera;
        Camera2D Camera2;

        // keyboard stuff
        KeyboardState oldState;

        // The player view ports
        Viewport v;
        Viewport v1;
        public static Viewport original;

        // Splitscreen
        public bool SplitScreen = false;
        private bool useTile = false;
        //pyblic  string name = "";
        private MouseState oldMouseState;
        private bool useTileCollition = true;
        private Texture2D textureBlank;

        public GameScreen(bool client = false, string ip = "127.0.0.1")
        {
            // Setup our player and their views
            original = GameManager.Game.Graphics.GraphicsDevice.Viewport;
            Player = new Player(false);
            Camera = new Camera2D(GameManager.Game.Graphics.GraphicsDevice);
            Camera2 = new Camera2D(GameManager.Game.Graphics.GraphicsDevice);
            v = new Viewport(GameManager.Game.Graphics.PreferredBackBufferWidth / 2, 0, GameManager.Game.Graphics.PreferredBackBufferWidth / 2, GameManager.Game.Graphics.PreferredBackBufferHeight);
            v1 = new Viewport(0, 0, GameManager.Game.Graphics.PreferredBackBufferWidth - (GameManager.Game.Graphics.PreferredBackBufferWidth / 2), GameManager.Game.Graphics.PreferredBackBufferHeight);


            //MainGame.Console.AddCommand("set", a =>
            //{
            //    return "Set";
            //}, "Sets game states. Ie. set DEBUG true");

            textureBlank = new Texture2D(GameManager.Game.Graphics.GraphicsDevice, 4, 4);

            uint[] red = new uint[4 * 4];
            for (int i = 0; i < 4 * 4; i++)
                red[i] = (new Color(255, 0, 0) * 100).PackedValue;
            textureBlank.SetData<uint>(red);

            //#region Serverside Stuff
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
            //#endregion

        }

        SpeechDialog sp = new SpeechDialog();

        public static bool useServer = false;
        bool hasClient = false;
        bool hasServer = false;
        #region IGameScreen implementation

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameManager.Game.Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            GameManager.Game.Graphics.GraphicsDevice.Viewport = original;

            // Draw the player
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());

            //#region Serverside Stuff
            //if (useServer)
            //{
            //    if (netClient != null)
            //    {
            //        NetIncomingMessage message;
            //        while ((message = netClient.ReadMessage()) != null)
            //        {
            //            if (message.MessageType == NetIncomingMessageType.Data)
            //            {
            //                var data = message.ReadString();

            //                if (!hasServer)
            //                {
            //                    var mg = netClient.CreateMessage();
            //                    mg.Write("con_" + Name);

            //                    netClient.SendMessage(mg,
            //                        NetDeliveryMethod.ReliableOrdered);

            //                    if (data.StartsWith("cons_"))
            //                    {
            //                        string multiplayerName = data.Split('_')[1];
            //                        MultiPlayers[0].Name = multiplayerName;
            //                        multiplayerName = null;

            //                        hasServer = true;
            //                    }
            //                }
            //                else
            //                {
            //                    try
            //                    {
            //                        var pos = new Vector2(float.Parse(data.Split('{', '}')[1].Split(' ')[0].Trim('Y', 'X', ':')), float.Parse(data.Split('{', '}')[1].Split(' ')[1].Trim('Y', 'X', ':')));

            //                        for (int i = 0; i < MultiPlayers.Count; i++)
            //                        {
            //                            MultiPlayers[i].Update(gameTime, pos);
            //                        }
            //                    }
            //                    catch { }
            //                }
            //            }
            //        }
            //        var msg = netClient.CreateMessage();
            //        msg.Write(Name + "-" + Player.PlayerSprite.Position.ToString());
            //        netClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            //    }
            //}
            //else
            //{
            //    if (SingleplayerServer != null)
            //    {
            //        {
            //            NetIncomingMessage mesg;
            //            while ((mesg = SingleplayerServer.ReadMessage()) != null)
            //            {
            //                if (mesg.MessageType == NetIncomingMessageType.Data)
            //                {
            //                    var data = mesg.ReadString();
            //                    if (!hasClient)
            //                    {
            //                        var mg = SingleplayerServer.CreateMessage();
            //                        mg.Write("cons_" + Name);
            //                        foreach (var item in SingleplayerServer.Connections)
            //                        {
            //                            item.SendMessage(mg, NetDeliveryMethod.ReliableOrdered, 0);
            //                        }

            //                        if (data.StartsWith("con_"))
            //                        {
            //                            hasClient = true;
            //                            string multiplayerName = data.Split('_')[1];
            //                            MultiPlayers[0].Name = multiplayerName;
            //                            multiplayerName = null;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        try
            //                        {
            //                            var pos = new Vector2(float.Parse(data.Split('{', '}')[1].Split(' ')[0].Trim('Y', 'X', ':')), float.Parse(data.Split('{', '}')[1].Split(' ')[1].Trim('Y', 'X', ':')));

            //                            for (int i = 0; i < MultiPlayers.Count; i++)
            //                            {
            //                                MultiPlayers[i].Update(gameTime, pos);
            //                            }
            //                        }
            //                        catch { }
            //                    }
            //                }
            //            }
            //        }

            //        var message = SingleplayerServer.CreateMessage();
            //        message.Write(Name + "-" + Player.PlayerSprite.Position.ToString());
            //        foreach (var item in SingleplayerServer.Connections)
            //        {
            //            item.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
            //        }
            //    }
            //}
            //#endregion

            // Draw the world
            World.Draw(gameTime, spriteBatch);

            for (int i = 0; i < MultiPlayers.Count; i++)
            {
                MultiPlayers[i].Draw(gameTime, spriteBatch);
            }

            Player.Draw(gameTime, spriteBatch);

            try
            {
                spriteBatch.End();
                //}
                // Draw the health
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                Player.HealthBar.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }
            catch (Exception)
            {

            }

        }

        int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }

        public override void Update(GameTime gameTime)
        {
            var newMouseState = Mouse.GetState();
            var newState = Keyboard.GetState();

            // Check if we pressed splitscreen
            if (newState.IsKeyDown(Keys.K) && oldState.IsKeyUp(Keys.K))
                SplitScreen = !SplitScreen;

            UpdatePlayerCameras(gameTime);

            // Keyboard & mouse state stuff
            oldState = newState;
            oldMouseState = newMouseState;

            // Update the world
            World.Update(gameTime);
        }
        void UpdatePlayerCameras(GameTime gameTime)
        {
            Vector2 playerPos = Player.PlayerSprite.Position;

            Camera.Position = (playerPos - (new Vector2((int)GameManager.Game.ScreenSize.X / 2, (int)GameManager.Game.ScreenSize.Y / 2))).Round(1);
            // Run our players update command
            Player.Update(gameTime);

        }

        public override void LoadContent(ContentManager content)
        {
            // Load our players content and set an initial state
            Player.LoadContent(content);
            MultiPlayers.Add(new NetworkPlayer("Jeff"));
            
            World.Intialize();

            ui_menu_inventory = new UI_Inventory_Menu();
            this.UI.Add(ui_menu_inventory);

            this.UI.Add(new GameIntro());
        }


        public override void Intialize()
        {
            // Intialize the players
            Player.Intialize();
        }

        public override void Unload()
        {
        }

        #endregion
    }
}
