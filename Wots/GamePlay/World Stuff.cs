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
        public Bag<Tile> Tiles = new Bag<Tile>();
        public Bag<AI> Entities = new Bag<AI>();
        public string Background;
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

        public static Bag<GameObject> GOQue = new Bag<GameObject>();
        public static Bag<AI> EQue = new Bag<AI>();
        public static Vector2 TempSpawn;
        public static void LoadWorld(string name, bool load = true)
        {
            if (Worlds.ContainsKey(name))
            {
                WorldName = name;
                GameScreen.BackdropName = Worlds[WorldName].Background;
                return;
            }


            var Tiles = new Bag<Tile>();
            WorldPrefs p = new WorldPrefs();

            XmlDocument doc = new XmlDocument();

            string xml = "";
            using (StreamReader sr = new StreamReader(Activity1.ASSETS.Open($"{name}.xml")))
            {
                xml = sr.ReadToEnd();
            }

            doc.LoadXml(xml);
            p.Background = doc["world"].Attributes["backdrop"].InnerText;
            {
                int spawnX = int.Parse(doc["world"]["prefs"]["spawn"]["x"].InnerText) * 96;
                int spawnY = int.Parse(doc["world"]["prefs"]["spawn"]["y"].InnerText) * 96;
                p.Position = new Vector2(spawnX, spawnY);
                GameScreen.Player.PlayerSprite.Position = p.Position;
            }
            foreach (XmlNode tile in doc["world"]["tiles"])
            {
                Tile t = default(Tile);

                string type = tile["type"].InnerText.ToLower();
                string state = tile["state"].InnerText;
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
                    case "water":
                        t = new Water();
                        break;
                    case "door":
                        t = new Door();
                        break;
                    case "ladder":
                        t = new Ladder();
                        break;
                    case "bush":
                        t = new Bush();
                        break;
                    case "luna":
                        t = new LunaSoil();
                        break;
                    case "wall":
                        t = new Wall();
                        break;
                    case "wood":
                        t = new Wood();
                        break;
                }
                t.Texture = texture;
                t.Type = type;
                t.Initialize(new Vector2(x, y), state);
                Tiles.Add(t);
            }
            foreach (XmlNode tile in doc["world"]["entitys"])
            {
                AI ai = default(AI);

                string type = tile["type"].InnerText;
                string data = tile["data"].InnerText;
                int x = int.Parse(tile["position"]["x"].InnerText);
                int y = int.Parse(tile["position"]["y"].InnerText);
                Vector2 pos = new Vector2(x * 96, y * 96);
                switch (type)
                {
                    case "woodwatcher":
                        ai = new WoodWatcherAI(pos)
                        {
                            Data = data
                        };
                        break;
                    case "slime":
                        ai = new SlimeAI(pos)
                        {
                            Data = data
                        };
                        break;
                    case "npc":
                        ai = new NPCAI(pos)
                        {
                            Data = data
                        };
                        break;

                    default:
                        break;
                }
                if (ai != default(AI))
                    EQue.Add(ai);
            }
            foreach (XmlNode tile in doc["world"]["objects"])
            {
                string type = tile["type"].InnerText;
                int x = int.Parse(tile["position"]["x"].InnerText);
                int y = int.Parse(tile["position"]["y"].InnerText);

                Vector2 pos = new Vector2(x * 96, y * 96);
                GameObject go = default(GameObject);
                switch (type)
                {
                    case "tree":
                        go = new TreeGO() { Position = pos };
                        break;

                    default:
                        break;
                }

                if (go != default(GameObject))
                    GOQue.Add(go);
            }

            p.Tiles = Tiles;
            p.GameObjects.AddRange(GOQue);
            p.Entities.AddRange(EQue);
            GameScreen.BackdropName = p.Background;
            Worlds.Add(name, p);

            EQue.Clear();
            GOQue.Clear();

            hasWorld = true;
            if (load == true)
                WorldName = name;
        }
        public static void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
            //for the fraction of a second where no world is loaded
            if (hasWorld)
                foreach (var ent in Worlds[WorldName].Entities)
                    ent.UpdateGestures(touches, gesture);
        }


        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var Tiles = Worlds[WorldName].Tiles;

            if (GameManager.DEBUG)
                spriteBatch.Draw(black, new Rectangle(0, 0, (int)blackWorld.X, (int)blackWorld.Y), Color.Black * 0.1f);

            // Loop throug them
            for (int i = 0; i < Tiles.Count; i++)
            {
                // Get the tile are are current on
                Tile item = Tiles[i];
                // The center of the tile (32, 32)
                var origin = new Vector2(96);
                var rect = new Rectangle((int)GameScreen.Camera.Position.X, (int)GameScreen.Camera.Position.Y, (int)GameManager.Game.ScreenSize.X, (int)GameManager.Game.ScreenSize.Y);
                if (item.BoundingBox.Intersects(rect))
                    spriteBatch.Draw(AssetManager.Textures[item.Texture], new Rectangle(item.Position.ToPoint(), origin.ToPoint()), item.Color);
            }

            foreach (var entity in Worlds[WorldName].Entities)
            {
                entity.Sprite.Draw(spriteBatch);
                entity.Draw(spriteBatch);
            }

            foreach (var go in Worlds[WorldName].GameObjects)
                go.Draw(gameTime, spriteBatch);
        }

        public static Tuple<bool, Tile> isSpaceOpen(Vector2 pos, SpriteBatch s, Vector2 size)
        {
            // Safety for when we temporarily have no world loaded
            if (!hasWorld)
                return new Tuple<bool, Tile>(false, new Air());

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
            if (hasWorld)
            {
                if (!GameManager.Game.Paused)
                    foreach (var entity in Worlds[WorldName].Entities)
                        if (entity.Health <= 0)
                            Worlds[WorldName].Entities.Remove(entity);
                        else
                            entity.Update(gameTime);

                if (!GameManager.Game.Paused)
                    foreach (var go in Worlds[WorldName].GameObjects)
                        go.Update(gameTime);
            }

        }
    }
}
