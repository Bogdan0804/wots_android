using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//
using System.Collections;
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots.UI
{
    public class MenuItem : UIComponent
    {
        public string Text { get; set; }
        public Texture2D Texture { get; set; }

        public Vector2 Dimentions;
        public Vector2 Position;

        public delegate void Clicked(object sender);
        public event Clicked Pressed;

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Dimentions.X, (int)Dimentions.Y);
            }
        }

        public bool isHover = false;

        public MenuItem(Texture2D texture, Vector2 pos, Vector2 dimentions)
        {
            this.Dimentions = dimentions;
            this.Position = pos;
            this.Texture = texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            throw new CantDrawException("MenuItem");
        }

        MouseState oldState;
        public override void Update(GameTime gameTime)
        {
            var newState = Mouse.GetState();

            if (Hitbox.Contains(Mouse.GetState().Position))
                isHover = true;
            else
                isHover = false;
            var touchCol = TouchPanel.GetState();

            foreach (var touch in touchCol)
            {
                if (this.Hitbox.Contains(touch.Position))
                    Pressed?.Invoke(this);
            }
            oldState = newState;
        }
    }
}
