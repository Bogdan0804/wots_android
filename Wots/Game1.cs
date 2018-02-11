using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        public static SpriteBatch MainSpriteBatch;
        SpriteBatch UISpriteBatch;


        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Graphics.PreferredBackBufferWidth = 1080;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            Graphics.ApplyChanges();

            Graphics.GraphicsProfile = GraphicsProfile.HiDef;

            GameManager.Game.Content = this.Content;
            GameManager.Game.Graphics = Graphics;
        }
        protected override void Initialize()
        {
            TouchPanel.EnabledGestures =
           GestureType.Tap | GestureType.Flick;
            GameManager.Game.Initialize();
        }
        protected override void LoadContent()
        {
            MainSpriteBatch = new SpriteBatch(this.GraphicsDevice);
            UISpriteBatch = new SpriteBatch(this.GraphicsDevice);

            Services.AddService(typeof(SpriteBatch), MainSpriteBatch);
            GameManager.Game.LoadContent(this.Content);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            GameManager.Game.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (MainSpriteBatch == null)
                this.LoadContent();

            GameManager.Game.Draw(gameTime, MainSpriteBatch, UISpriteBatch);

            base.Draw(gameTime);
        }
    }
}
