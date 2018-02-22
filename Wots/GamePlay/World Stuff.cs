using System;
using MonoGame.Extended.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Wots.UI;
using Wots.Entities;
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots.GamePlay
{
    public class Prefs
    {
        public Func<Player, bool> OnJump;
        public Func<Player, bool> OnUp;
        public Func<Player, bool> OnDown;
        public Func<Player, bool> OnLeft;
        public Func<Player, bool> OnRight;
        public Func<Player, bool> OnClick;

        public bool usePrefJump = false;
        public bool usePrefUp = false;
        public bool usePrefDown = false;
        public bool usePrefLeft = false;
        public bool usePrefRight = false;
        public bool usePrefClick = false;
    }

    public class Tile
    {
        public Prefs Prefs = new Prefs();
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

    public class WorldPrefs
    {
        public Bag<Tile> Floor = new Bag<Tile>();
        public Bag<Tile> Tiles = new Bag<Tile>();
        public Bag<AI> Entities = new Bag<AI>();
        public Bag<GameObject> GameObjects = new Bag<GameObject>();
    }

    public static class World
    {
        public static bool hasWorld = false;
        public static int TileWidth, TileHeight;

        public static Dictionary<string, WorldPrefs> Worlds = new Dictionary<string, WorldPrefs>();

        internal static Vector2 blackWorld = Vector2.Zero;
        private static Texture2D black;
        public static string WorldName;
        public static void Intialize()
        {
            black = new Texture2D(GameManager.Game.Graphics.GraphicsDevice, 1, 1);
            uint pVal = Color.Black.PackedValue;

            black.SetData<uint>(new uint[] { pVal });

            LoadWorld("main");

            World.Worlds[World.WorldName].Entities.Add(new SlimeAI());
        }
        public static int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }

        public static Bag<GameObject> GOQue = new Bag<GameObject>();

        public static void LoadWorld(string name, bool load = true)
        {
            GameScreen.Player.PlayerSprite.Position = Vector2.Zero;

            if (Worlds.ContainsKey(name))
            {
                WorldName = name;
                return;
            }

            if (load == true)
                WorldName = name;

            var Floor = new Bag<Tile>();
            var Tiles = new Bag<Tile>();

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
                t = ProcessPrefs(t);

                if (t == null)
                    continue;

                if (t.Collidable == false && t.State.ToLower() == "none")
                    Floor.Add(t);

                else
                    Tiles.Add(t);
            }

            WorldPrefs p = new WorldPrefs();
            p.Floor = Floor;
            p.Tiles = Tiles;

            Worlds.Add(name, p);
            Worlds[WorldName].GameObjects.AddRange(GOQue);
            GOQue.Clear();
            hasWorld = true;
        }

        private static Tile ProcessPrefs(Tile t)
        {
            if (t.State.ToLower() == "fast4")
            {
                t.Collidable = false;
                t.Prefs.usePrefJump = true;
                t.Prefs.usePrefUp = true;

                t.Prefs.OnJump = new Func<Player, bool>((e) =>
                {
                    return false;
                });
                t.Prefs.OnUp = new Func<Player, bool>((e) =>
                {
                    if (e.Collitions.Up.Point1.Item1)
                    {
                        e.PlayerSprite.Position.Y -= (UniversalInputManager.Manager.Speed * GameManager.GAMESPEED) / 1.5f;
                        e.useGravity = false;
                    }
                    return false;
                });
            }
            else if (t.State.ToLower() == "staires")
            {
                t.Prefs.usePrefLeft = true;
                t.Collidable = false;
                t.Prefs.OnLeft = new Func<Player, bool>((p) =>
                {
                    p.PlayerSprite.Position.X -= 96;
                    p.PlayerSprite.Position.Y -= 96;
                    return true;
                });
            }
            else if (t.State.ToLower() == "water")
            {
                t.Collidable = false;
                t.Prefs.usePrefJump = true;
                t.Prefs.OnJump = new Func<Player, bool>((e) =>
                {
                    return false;
                });
            }
            else if (t.State.ToLower().StartsWith("go:"))
            {
                string type = t.State.Split(':')[1];
                if (type == "tree")
                {
                    GOQue.Add(new TreeGO
                    {
                        Position = t.Position
                    });
                }

                t = null;
            }

            return t;
        }

        public static void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
            //if (gesture.GestureType == GestureType.Tap)
            //{
            //    var Tiles = Worlds[WorldName].Tiles;
            //    foreach (var tile in Tiles)
            //    {
            //        if (tile.BoundingBox.Contains(gesture.Position) && tile.Prefs.usePrefClick)
            //            throw new Exception("fuccckkkk");
            //    }
            //}
        }


        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var Floor = Worlds[WorldName].Floor;
            var Tiles = Worlds[WorldName].Tiles;

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

            if (!GameManager.Game.Paused)
                foreach (var entity in Worlds[WorldName].Entities)
                    if (entity.Health <= 0)
                        Worlds[WorldName].Entities.Remove(entity);
                    else
                        entity.Update(gameTime, spriteBatch);

            if (!GameManager.Game.Paused)
                foreach (var go in Worlds[WorldName].GameObjects)
                    go.Update(gameTime);

            foreach (var entity in Worlds[WorldName].Entities)
            {
                entity.Sprite.Draw(spriteBatch);
            }
            foreach (var go in Worlds[WorldName].GameObjects)
            {
                go.Draw(gameTime, spriteBatch);
            }
        }

        public static Tuple<bool, Tile> isSpaceOpen(Vector2 pos, SpriteBatch s, Vector2 size)
        {
            if (!hasWorld)
                return new Tuple<bool, Tile>(false, new Tile());

            var Floor = Worlds[WorldName].Floor;
            var Tiles = Worlds[WorldName].Tiles;

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
