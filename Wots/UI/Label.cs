using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace Wots.UI
{
    public class Label : UIComponent
    {
        public string Text { get; set; }
        public string Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            uiSpriteBatch.DrawString(AssetManager.GetFont(Font), Text, Position, Color);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
