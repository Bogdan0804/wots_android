using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wots.Screens
{
	public class LoadingScreen : IGameScreen
	{
        double timeBeforeDone = 0.0;
		Sprite name;
		public Color background;


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
            name.Animations.Add("copywrite", new Animation(
                new Frame(WOTS, 2f),
                new Frame(AssetManager.LoadImage("art/ui/copy/thejeff1"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/thejeff2"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/thejeff3"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/thejeff4"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/thejeff5"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/aidan1"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/aidan2"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/aidan3"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/aidan4"), 0.75f),
                new Frame(AssetManager.LoadImage("art/ui/copy/tim/1"), 0.85f),
                new Frame(AssetManager.LoadImage("art/ui/copy/tim/2"), 0.85f),
                new Frame(AssetManager.LoadImage("art/ui/copy/tim/3"), 0.85f)
            ));
			name.CurrentAnimation = "copywrite";
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
            timeBeforeDone += gameTime.ElapsedGameTime.TotalSeconds;

			if (name != null)
				name.Update(gameTime);

            if (timeBeforeDone > 2.3)
                background = Color.Black;

			// CHeck if 10 seconds past
			if (timeBeforeDone > 11.2f)
				GameManager.Game.ChangeScreen(new MenuGameScreen());
		}
    }
}
