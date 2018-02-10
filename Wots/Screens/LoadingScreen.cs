using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wots.Screens
{
	public class LoadingScreen : IGameScreen
	{
		private double timeBeforeMenu = 0;
		private double timeBeforeCopy = 0;
		private double timeBeforeNames = 0;

		Sprite name;
		public Color background;
		bool stage1,stage2;


		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			GameManager.Game.Graphics.GraphicsDevice.Clear(background);

			spriteBatch.Begin(samplerState:SamplerState.PointWrap);
			name.Draw(spriteBatch);
			spriteBatch.End();
		}

		public override void Intialize()
		{
        }

		public override void LoadContent(ContentManager content)
		{
            Texture2D WOTS = AssetManager.LoadImage("art/title_l");

			name = new Sprite(new Vector2(500, 300), GameManager.Game.ScreenSize / 2 - (new Vector2((int)WOTS.Width, (int)WOTS.Height) / 2));
			name.Animations.Add("default", new Animation(new Frame(WOTS)));
			name.Animations.Add("copywrite", new Animation(
				new Frame(AssetManager.LoadImage("art/ui/copy/thejeff1")),
				new Frame(AssetManager.LoadImage("art/ui/copy/thejeff2")),
				new Frame(AssetManager.LoadImage("art/ui/copy/thejeff3")),
				new Frame(AssetManager.LoadImage("art/ui/copy/thejeff4")),
				new Frame(AssetManager.LoadImage("art/ui/copy/thejeff5"))
			));
			name.Animations.Add("names", new Animation(
				new Frame(AssetManager.LoadImage("art/ui/copy/tim/1")),
				new Frame(AssetManager.LoadImage("art/ui/copy/tim/2")),
				new Frame(AssetManager.LoadImage("art/ui/copy/tim/3")),
                new Frame(AssetManager.LoadImage("art/ui/copy/tim/3")),
                new Frame(AssetManager.LoadImage("art/ui/copy/tim/3")),
                new Frame(AssetManager.LoadImage("art/ui/copy/tim/3"))

            ));
			name.CurrentAnimation = "default";
            LoadBlocks();
			background = new Color(102, 57, 49);
            
#if DEBUG
            GameManager.Game.ChangeScreen(new MenuGameScreen());
#endif

            if (GameManager.DEBUG)
                GameManager.Game.ChangeScreen(new MenuGameScreen());
        }

        void LoadBlocks()
        {
            // Load in the assets

            //GameManager.Game.ChangeScreen(new MenuGameScreen());
        }
        public override void Unload()
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			if (name != null)
				name.Update(gameTime);

			// Add every second to this
			timeBeforeMenu += gameTime.ElapsedGameTime.TotalSeconds  * GameManager.GAMESPEED;
			timeBeforeCopy += gameTime.ElapsedGameTime.TotalSeconds  * GameManager.GAMESPEED;
			timeBeforeNames += gameTime.ElapsedGameTime.TotalSeconds  * GameManager.GAMESPEED;

			if (timeBeforeCopy > 3 && !stage1)
			{
				background = Color.Black;
				stage1 = true;
				name.FrameTime = 0.6f;
				name.CurrentAnimation = "copywrite";
			}

			if (timeBeforeNames > 6 && !stage2)
			{
				stage2 = true;
				name.FrameTime = 0.8f;
				name.CurrentAnimation = "names";
			}

			// CHeck if 10 seconds past
			if (timeBeforeMenu > 10)
				GameManager.Game.ChangeScreen(new MenuGameScreen());
		}
    }
}
