using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots.UI
{
    public class DPad
    {
        public Vector2 Position;
        public Texture2D keyL,keyR;

        Rectangle left, right;

        public void LoadContent()
        {
            keyL = AssetManager.LoadImage("art/ui/dpad_key_left");
            keyR = AssetManager.LoadImage("art/ui/dpad_key_right");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 leftKey, rightKey;
            leftKey = Position - new Vector2(0);
            rightKey = Position + new Vector2(256,0);

            spriteBatch.Draw(keyL, new Rectangle(leftKey.ToPoint(), new Point(256)), Color.White);
            spriteBatch.Draw(keyR, new Rectangle(rightKey.ToPoint(), new Point(256)), Color.White);

            left = new Rectangle(leftKey.ToPoint(), new Point(256));
            right = new Rectangle(rightKey.ToPoint(), new Point(256));
        }
        public void Update(GameTime gameTime)
        {
            var touches = TouchPanel.GetState();
            foreach (var touch in touches)
            {
                if (touch.State != TouchLocationState.Moved)
                {
                    UniversalInputManager.Manager.MoveVector.X = 0;
                    continue;
                }

                if (left.Contains(touch.Position))
                    UniversalInputManager.Manager.MoveVector.X = -1;
                else if (right.Contains(touch.Position))
                    UniversalInputManager.Manager.MoveVector.X = 1;
                else 
                    UniversalInputManager.Manager.MoveVector.X = 0;
            }
        }
    }
}