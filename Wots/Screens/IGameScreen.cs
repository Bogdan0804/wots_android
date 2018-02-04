using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;

namespace Wots.Screens
{
	/// <summary>
	/// The base of all game screens
	/// </summary>
	public abstract class IGameScreen
	{
        public Bag<UI.UIComponent> UI = new Bag<UI.UIComponent>();

		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent(ContentManager content);
        public abstract void Intialize();
        public abstract void Unload();
	}
}
