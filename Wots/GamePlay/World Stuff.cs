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
using Wots.GamePlay.Tiles;

namespace Wots.GamePlay
{
    public class WorldPrefs
    {
        public Bag<Tile> Floor = new Bag<Tile>();
        public Bag<Tile> Tiles = new Bag<Tile>();
        public Bag<AI> Entities = new Bag<AI>();
        public Bag<GameObject> GameObjects = new Bag<GameObject>();
        public Vector2 Position;
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

        }
        public static int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }

        public static Dictionary<string, Vector2> Spawns = new Dictionary<string, Vector2>();
        public static Bag<GameObject> GOQue = new Bag<GameObject>();
        public static Bag<AI> EQue = new Bag<AI>();
        public static Vector2 TempSpawn;
        public static void LoadWorld(string name, bool load = true)
        {
            if (Worlds.ContainsKey(name))
            {
                WorldName = name;
                XmlDocument docl = new XmlDocument();

                string xmll = "";
                using (StreamReader sr = new StreamReader(Activity1.ASSETS.Open($"{name}.xml")))
                {
                    xmll = sr.ReadToEnd();
                }
                try
                {
                    GameScreen.BackdropName = docl["world"].Attributes["backdrop"].InnerText;
                }
                catch
                {

                    GameScreen.BackdropName = "art/BG";
                }
                return;
            }

            if (load == true)
                WorldName = name;

            var Floor = new Bag<Tile>();
            var Tiles = new Bag<Tile>();
            WorldPrefs p = new WorldPrefs();

            XmlDocument doc = new XmlDocument();

            string xml = "";
            using (StreamReader sr = new StreamReader(Activity1.ASSETS.Open($"{name}.xml")))
            {
                xml = sr.ReadToEnd();
            }

            doc.LoadXml(xml);
            GameScreen.BackdropName = doc["world"].Attributes["backdrop"].InnerText;
            {
                int spawnX = int.Parse(doc["world"]["prefs"]["spawn"]["x"].InnerText) * 96;
                int spawnY = int.Parse(doc["world"]["prefs"]["spawn"]["y"].InnerText) * 96;
                p.Position = new Vector2(spawnX, spawnY);
                GameScreen.Player.PlayerSprite.Position = p.Position;
                Spawns.Add(WorldName, p.Position);
            }
            foreach (XmlNode tile in doc["world"]["tiles"])
            {
                Tile t = default(Tile);

                string type    = tile["type"].InnerText.ToLower();
                string state   = tile["state"].InnerText;
                string texture = tile["texture"].InnerText;

                int x = int.Parse(tile["position"]["x"].InnerText) * 96;
                int y = int.Parse(tile["position"]["y"].InnerText) * 96;

                switch (type)
                {
                    case "grass":
                        t = new Grass();
                        break;
                    case "cobble":
                        t = new Cobble();
                        break;
                }
                t.Texture = texture;
                t.Initialize( new Vector2(x, y), state);
                Tiles.Add(t);
            }

            p.Floor = Floor;
            p.Tiles = Tiles;
            p.GameObjects.AddRange(GOQue);
            p.Entities.AddRange(EQue);
            Worlds.Add(name, p);

            EQue.Clear();
            GOQue.Clear();

            hasWorld = true;
        }
        /*
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
            else if (t.State.ToLower() == "stairesleft")
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
            else if (t.State.ToLower() == "stairesright")
            {
                t.Prefs.usePrefRight = true;
                t.Collidable = false;
                t.Prefs.OnRight = new Func<Player, bool>((p) =>
                {
                    p.PlayerSprite.Position.X += 96;
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
            else if (t.State.ToLower().StartsWith("entity:"))
            {
                string type = t.State.Split(':')[1];
                if (type == "woodwatcher0")
                {
                    EQue.Add(new WoodWatcherAI(t.Position));
                }
                else if (type == "slime0")
                {
                    EQue.Add(new SlimeAI(t.Position));
                }

                t = null;
            }
            else if (t.State.ToLower() == "spawn")
            {
                TempSpawn = t.Position;
                GameScreen.Player.PlayerSprite.Position = t.Position;
                t = null;
            }

            return t;
        }
        */
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
                if (item.BoundingBox.Intersects(rect))
                    spriteBatch.Draw(AssetManager.GetTexture(item.Texture), new Rectangle(item.Position.ToPoint(), origin.ToPoint()), Color.White);
            }
            // Loop throug them
            for (int i = 0; i < Tiles.Count; i++)
            {
                // Get the tile are are current on
                Tile item = Tiles[i];
                // The center of the tile (32, 32)
                var origin = new Vector2(96);
                var rect = new Rectangle((int)GameScreen.Camera.Position.X, (int)GameScreen.Camera.Position.Y, (int)GameManager.Game.ScreenSize.X, (int)GameManager.Game.ScreenSize.Y);
                if (item.BoundingBox.Intersects(rect))
                    spriteBatch.Draw(AssetManager.GetTexture(item.Texture), new Rectangle(item.Position.ToPoint(), origin.ToPoint()), Color.White);
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
                return new Tuple<bool, Tile>(false, new Air());

            var Floor = Worlds[WorldName].Floor;
            var Tiles = Worlds[WorldName].Tiles;

            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
            bool isOpen = true;
            s?.Draw(black, rect, Color.Red);
            Tile t = new Air();
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i].BoundingBox.Intersects(rect))
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
