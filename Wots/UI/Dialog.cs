using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wots.UI
{
    public class Dialog : UIComponent
    {
        public string Title = "";
        public string Message = "";
        private Texture2D Texture;

        Button okBtn;

        public bool Enabled = true;

        public Vector2 Size, Position;
        public Dialog(Vector2 size)
        {
            this.Size = size;
        }
        public delegate void Toggled(bool state);
        public event Toggled StateChanged;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Enabled)
            {
                spriteBatch.Draw(Texture, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.White);
                spriteBatch.DrawString(AssetManager.GetFont("24"), Title, Position + new Vector2(Size.X / 2 - AssetManager.GetFont("24").MeasureString(Title).X / 2, 20), Color.Black);
                spriteBatch.DrawString(AssetManager.GetFont("12"), Message, Position + new Vector2(20, 60), Color.Black);
                okBtn.Draw(gameTime, spriteBatch);

                okBtn.Position = this.Position + (this.Size - okBtn.Dimentions) - (new Vector2(20, 30));
            }
        }

        public override void Update(GameTime gameTime)
        {
            okBtn.Update(gameTime);
        }
        public void  LoadContent()
        {
            Texture = AssetManager.LoadImage("art/ui/dialoguebox");

            okBtn = new Button(AssetManager.GetTexture("button_1"), Vector2.Zero, new Vector2(100, 50));
            okBtn.HoverColor = Color.Gray;
            okBtn.Text = "Ok";
            okBtn.RenderText = true;
            okBtn.Font = AssetManager.GetFont("12");
            okBtn.HoverEffect = true;

            okBtn.Pressed += OkBtn_Pressed;
        }

        private void OkBtn_Pressed(object sender)
        {
            Enabled = false;

            StateChanged(Enabled);
        }
    }
}
