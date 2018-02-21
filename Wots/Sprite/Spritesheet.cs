using System;
using System.IO;
using Android.Graphics;
using Android.Media;
using Microsoft.Xna.Framework.Graphics;
//

namespace Wots
{
	public class Spritesheet
	{
		public string Path
		{
			get;
			set;
		}

		public Microsoft.Xna.Framework.Vector2 TileSize
		{
			get;
			set;
		}

		public Spritesheet(string path, int tileSize)
		{
			this.Path = path;
			this.TileSize = new Microsoft.Xna.Framework.Vector2(tileSize);
		}
		public Spritesheet(string path, int x, int y)
		{
			this.Path = path;
			this.TileSize = new Microsoft.Xna.Framework.Vector2(x,y);
		}

		public Texture2D GetSprite(int xPos, int yPos)
		{
			var img = AssetManager.LoadImage(GameManager.Game.Content.RootDirectory + "/" + this.Path);

            /// Get the data from the original texture and place it in an array
            Bitmap o = Bitmap.CreateBitmap((int)img.Width, (int)img.Height, Bitmap.Config.Argb4444);
            Color[] colorData = new Color[img.Width * img.Height];
            img.GetData<Color>(colorData);
            for (int x = 0; x < o.Width; x++)
            {
                for (int y = 0; y < o.Height; y++)
                {
                    o.SetPixel(x, y, colorData[x * o.Height + y]);
                }
            }
            

            Bitmap g = Bitmap.CreateBitmap((int)TileSize.X, (int)TileSize.Y, Bitmap.Config.Argb4444);
            for (int x = xPos; x < (int)TileSize.X - xPos; x++)
        	{
        		for (int y = yPos; y < (int)TileSize.Y - yPos; y++)
        		{
        			g.SetPixel(x, y, new Color(o.GetPixel(x, y)));
        		}
        	}

            return Texture2D.FromStream(GameManager.Game.Graphics.GraphicsDevice, g);
        }
    }
}
