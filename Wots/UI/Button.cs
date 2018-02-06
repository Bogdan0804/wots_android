// Copywrite Bogz. -)-
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
//

namespace Wots.UI
{
    /// <summary>
    /// Description of Button.
    /// </summary>
    public class Button : UIComponent
    {
        public Vector2 Dimentions;
        public Vector2 Position;
        public Texture2D Texture;

        public string Text = "";
        public SpriteFont Font = AssetManager.GetFont("24");
        public bool RenderText = false;

        public delegate void Clicked(object sender);
        public event Clicked Pressed;

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Dimentions.X, (int)Dimentions.Y);
            }
        }

        public Color HoverColor = Color.Gray;
        public bool HoverEffect = true;

        private bool isHover = false;

        public Button(Texture2D texture, Vector2 pos, Vector2 dimentions)
        {
            this.Dimentions = dimentions;
            this.Position = pos;
            this.Texture = texture;
        }

        double time = 0;


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (HoverEffect && isHover)
            {
                spriteBatch.Draw(Texture, Hitbox, HoverColor);
            }
            else
            {
                spriteBatch.Draw(Texture, Hitbox, Color.White);
            }

            if (RenderText)
            {
                Vector2 centerButton = new Vector2(Hitbox.X, Hitbox.Y);
                Vector2 centerText = Font.MeasureString(Text);

                spriteBatch.DrawString(Font, Text, centerButton + (centerText / 2), Color.Black);
            }

        }
        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds * GameManager.GAMESPEED;


            var state = TouchPanel.GetState();
            foreach (var touch in state)
            {
                if (this.Hitbox.Contains(touch.Position))
                    isHover = true;
                else
                    isHover = false;
            }


            GestureSample gesture = UniversalInputManager.Manager.ReadGesture();

            if (this.Hitbox.Contains(gesture.Position) && gesture.GestureType == GestureType.Tap)
            {
                Pressed?.Invoke(this);
            }

        }
    }
}