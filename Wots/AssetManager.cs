using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Wots
{
    public static class AssetManager
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();

        /// Loads a texture into memmory
        public static void AddTexture(string key, string path)
        {
            Textures.Add(key, GameManager.Game.Content.Load<Texture2D>(path));
        }
        
        public static Texture2D LoadImage(string name)
        {
            return GameManager.Game.Content.Load<Texture2D>("Assets/"+name);
        }
        public static void LoadXml()
        {
            XmlDocument doc = new XmlDocument();

            string xml = "";
            using (StreamReader sr = new StreamReader(Activity1.ASSETS.Open("assets.xml")))
            {
                xml = sr.ReadToEnd();
            }

            doc.LoadXml(xml);

            foreach (XmlNode node in doc["textures"].ChildNodes)
            {
                string file, name, type;
                file = node["file"].InnerText.Split('.')[0];
                name = node["name"].InnerText;
                type = node["type"].InnerText;

                if (node.Attributes["xnb"].InnerText == "true")
                {
                    AddTexture(name, file);
                }
                else
                {
                    AddTexture(name, LoadImage(file));
                }
            }
        }

        public static Texture2D CreateTexture(int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(GameManager.Game.Graphics.GraphicsDevice, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Length; pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }
        /// Loads a texture into memmory
        public static void AddTexture(string key, Texture2D texture)
        {
            try
            {
                Textures.Add(key, texture);
            }
            catch { };
        }


        ///// Loads a font into memmory
        //public static void AddFont(string key, string path)
        //{
        //    Fonts.Add(key, GameManager.Game.Content.Load<SpriteFont>( path));
        //}

        /// Gets a texure from memory
        public static Texture2D GetTexture(string key)
        {
            return Textures[key];
        }

        /// Gets a texure from memory
        public static SpriteFont GetFont(string key)
        {
            return Fonts[key];
        }
        
    }

    public class Assets
    {
        public static Dictionary<string, string> Textures = new Dictionary<string, string>();
    }
}