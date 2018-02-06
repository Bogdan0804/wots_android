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
        DPad pad = new DPad();
        Button pause;

        // Player cameras
        public static Camera2D Camera;

        // Splitscreen
        public bool SplitScreen = false;
        //pyblic  string name = "";
        private Texture2D textureBlank;

        public GameScreen()
        {
            pad.Position = new Vector2(1, GameManager.Game.ScreenSize.Y - 265);
            // Setup our player and the world
            Player = new Player();
            // main camera
            Camera = new Camera2D(GameManager.Game.Graphics.GraphicsDevice);

            // pause button
            pause = new Button(AssetManager.LoadImage("art/ui/pause"), new Vector2(GameManager.Game.ScreenSize.X - 50, 3), new Vector2(48));
            pause.Pressed += (e) =>
            {
                GameManager.Game.Paused = !GameManager.Game.Paused;
            };
            this.UI.Add(pause);

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
            GameManager.Game.Graphics.GraphicsDevice.Clear(new Color(20, 20, 20));

            // Draw the player
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());

            #region Serverside Stuff
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
            #endregion

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
            // Draw the health
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Player.HealthBar.Draw(gameTime, spriteBatch);
            pad.Draw(spriteBatch);
            spriteBatch.End();

        }

        int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }

        public override void Update(GameTime gameTime)
        {
            pad.Update(gameTime);
            UpdatePlayerCameras(gameTime);


            // Update the world
            World.Update(gameTime);
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
