using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wots.UI
{
    public abstract class UIComponent
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void UpdateGestures(TouchCollection touches, GestureSample gesture);
    }
}
