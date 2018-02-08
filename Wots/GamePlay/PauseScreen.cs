using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Wots.Screens;
using Wots.UI;

namespace Wots.GamePlay
{
	public class PauseScreen : IGameScreen
	{
        public bool Intialized = false;
        Texture2D texture;
        Dialog msgDialog;

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            spriteBatch.Draw(texture, new Rectangle(0, 0, (int)GameManager.Game.ScreenSize.X, (int)GameManager.Game.ScreenSize.Y), Color.Black * 0.5f);
            msgDialog.Draw(gameTime, spriteBatch);
		}

		public override void Intialize()
		{
            msgDialog = new Dialog(new Vector2(450,300));
            msgDialog.Position = new Vector2(100,100);
            msgDialog.Title = "Paused";
            
            msgDialog.Message = "The game has been paused.";
            msgDialog.StateChanged += MsgDialog_StateChanged;

            msgDialog.Position = GameManager.Game.ScreenSize / 2 - msgDialog.Size / 2;
        }

        private void MsgDialog_StateChanged(bool state)
        {
            msgDialog.Enabled = true;
            GameManager.Game.Paused = false;
        }

        public override void LoadContent(ContentManager content)
		{
            texture = AssetManager.CreateTexture(10, 10, pixels => Color.Red);
            msgDialog.LoadContent();
            Intialized = true;
		}

		public override void Unload()
		{
		}

		public override void Update(GameTime gameTime)
		{
            msgDialog.Update(gameTime);
        }
	}
}
