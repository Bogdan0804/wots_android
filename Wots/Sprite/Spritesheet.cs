using System;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework;
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

		public Vector2 TileSize
		{
			get;
			set;
		}

		public Spritesheet(string path, int tileSize)
		{
			this.Path = path;
			this.TileSize = new Vector2(tileSize);
		}
		public Spritesheet(string path, int x, int y)
		{
			this.Path = path;
			this.TileSize = new Vector2(x,y);
		}

		//public Texture2D GetSprite(int xPos, int yPos)
		//{
		//	var img = (Bitmap)Image.FromFile(GameManager.Game.Content.RootDirectory + "/" + this.Path);

		//	Bitmap g = new Bitmap((int)TileSize.X - xPos , (int)TileSize.Y - yPos, PixelFormat.Format64bppArgb);

		//	for (int x = xPos; x < (int)TileSize.X - xPos; x++)
		//	{
		//		for (int y = yPos; y < (int)TileSize.Y - yPos; y++)
		//		{
		//			g.SetPixel(x, y, img.GetPixel(x, y));
		//		}
		//	}

		//	using (Stream s = g.ToStream(ImageFormat.Bmp))
		//		return Texture2D.FromStream(GameManager.Game.Graphics.GraphicsDevice, s);
		//}
	}
}
