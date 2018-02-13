using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots.UI
{
    public class Menu : UIComponent
    {
        public Bag<MenuItem> Items = new Bag<MenuItem>();

        public void AddItem(MenuItem item)
        {
            this.Items.Add(item);
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                uiSpriteBatch.Draw(Items[i].Texture, Items[i].Hitbox, Items[i].isHover ? Color.Gray : Color.White);
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var item in Items)
            {
                item.Update(gameTime);
            }
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
            foreach (var item in Items)
            {
                item.UpdateGestures(touches, gesture);
            }
        }
    }
}
