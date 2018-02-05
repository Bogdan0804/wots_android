using System;
using MonoGame.Extended.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml;

namespace Wots.GamePlay
{
    public class Tile
    {
        public Vector2 Position { get; set; }
        public string Texture { get; set; }
        public Color Color { get; set; }
        public float Rotation = 0;
        public string State { get; set; }
        public bool Collidable = true;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X - 5, (int)Position.Y - 5, 101, 101);
            }
        }
        public Rectangle ColitionBox
        {
            get
            {
                return new Rectangle(RoundNum((int)Position.X), RoundNum((int)Position.Y), 96, 96);
            }
        }

        private int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }
    }

    public static class World
    {
        public static int TileWidth, TileHeight;
        public static Bag<Tile> Floor = new Bag<Tile>();
        public static Bag<Tile> Tiles = new Bag<Tile>();
        internal static Vector2 blackWorld = Vector2.Zero;
        private static Texture2D black;
        public static string WorldName;
        public static void Intialize()
        {
            black = new Texture2D(GameManager.Game.Graphics.GraphicsDevice, 1, 1);
            uint pVal = Color.Black.PackedValue;

            black.SetData<uint>(new uint[] { pVal });

            {
                LoadWorld("main");
            }
        }
        public static int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }
        public static void LoadWorld(string name)
        {
            GameScreen.Player.PlayerSprite.Position = Vector2.Zero;
            WorldName = name;

            Floor = new Bag<Tile>();
            Tiles = new Bag<Tile>();

            XmlDocument doc = new XmlDocument();

            string xml = "";
            using (StreamReader sr = new StreamReader(Activity1.ASSETS.Open($"{name}.xml")))
            {
                xml = sr.ReadToEnd();
            }

            doc.LoadXml(xml);

            TileWidth = int.Parse(doc["world"].Attributes["tileWidth"].InnerText);
            TileHeight = int.Parse(doc["world"].Attributes["tileHeight"].InnerText);

            blackWorld.X = float.Parse(doc["world"].Attributes["width"].InnerText) * 96;
            blackWorld.Y = float.Parse(doc["world"].Attributes["height"].InnerText) * 96;

            foreach (XmlNode node in doc["world"].ChildNodes)
            {
                Tile t = new Tile();

                // The state
                t.State = (node["state"].InnerText);

                // The position
                t.Position = new Vector2(
                    float.Parse(node["position"]["x"].InnerText) * 96,
                    float.Parse(node["position"]["y"].InnerText) * 96
                );
                t.Color = Color.White;
                t.Texture = node["texture"].InnerText;
                t.Collidable = bool.Parse(node.Attributes["collidable"].InnerText);

                if (t.Collidable == false && t.State.ToLower() == "none")
                    Floor.Add(t);
                else
                    Tiles.Add(t);
            }
        }
        //internal static void OldLoad(string name)
        //{
        //    string worldText = File.ReadAllText($"Assets/worlds/{name}.ward");
        //    string[] worldLines = worldText.Split('\n');

        //    foreach (string item in worldLines)
        //    {
        //        try
        //        {
        //            string innerText = item.Split('[', ']')[1];
        //            string texture = innerText.Split(',')[0];
        //            int x = int.Parse(innerText.Split(',')[1]) * 96;
        //            int y = int.Parse(innerText.Split(',')[2]) * 96;
        //            string type = (innerText.Split(',')[3]);
        //            bool colidable = bool.Parse(innerText.Split(',')[4]);

        //            Tile t = new Tile();
        //            t.Position = new Vector2(x, y);
        //            t.State = type;
        //            t.Texture = texture;
        //            t.Color = Color.White;

        //            Console.WriteLine($"Creating tile @ {t.Position.ToString()}, collidable = {t.Collidable}");

        //            if (type.Contains("p_spawn"))
        //                GameScreen.Player.PlayerSprite.Position = t.Position - new Vector2(0, 200);
        //            else
        //                Tiles.Add(t);
        //        }
        //        catch
        //        {
        //            // Leave this alone
        //        }
        //    }
        //}
        
        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GameManager.DEBUG)
                spriteBatch.Draw(black, new Rectangle(0, 0, (int)blackWorld.X, (int)blackWorld.Y), Color.Black * 0.1f);
            
            // Loop throug them
            for (int i = 0; i < Floor.Count; i++)
            {
                // Get the tile are are current on
                Tile item = Floor[i];
                // The center of the tile (32, 32)
                var origin = new Vector2(96);
                // Draw the tile
                var rect = new Rectangle((int)GameScreen.Camera.Position.X, (int)GameScreen.Camera.Position.Y, (int)GameManager.Game.ScreenSize.X, (int)GameManager.Game.ScreenSize.Y);
                if (item.ColitionBox.Intersects(rect))
                    spriteBatch.Draw(AssetManager.GetTexture(item.Texture), new Rectangle(item.Position.ToPoint(), origin.ToPoint()), item.Color);
            }
            // Loop throug them
            for (int i = 0; i < Tiles.Count; i++)
            {
                // Get the tile are are current on
                Tile item = Tiles[i];
                // The center of the tile (32, 32)
                var origin = new Vector2(96);
                var rect = new Rectangle((int)GameScreen.Camera.Position.X, (int)GameScreen.Camera.Position.Y, (int)GameManager.Game.ScreenSize.X, (int)GameManager.Game.ScreenSize.Y);
                if (item.ColitionBox.Intersects(rect))
                    spriteBatch.Draw(AssetManager.GetTexture(item.Texture), new Rectangle(item.Position.ToPoint(), origin.ToPoint()), item.Color);


            }

        }

        public static Tuple<bool, Tile> isSpaceOpen(Vector2 pos, SpriteBatch s, Vector2 size)
        {
            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            bool isOpen = true;
            s?.Draw(black, rect, Color.Red);
            Tile t = new Tile();
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i].ColitionBox.Intersects(rect))
                {
                    t = Tiles[i];
                    if (Tiles[i].Collidable)
                        isOpen = false;
                    break;
                }
            }

            return new Tuple<bool, Tile>(isOpen, t);
        }
        public static void Update(GameTime gameTime)
        {  
        }
    }
}
