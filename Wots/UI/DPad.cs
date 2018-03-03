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
using SwordRush.Components;

namespace Wots.UI
{
    public class DPad : UIComponent
    {
        public Vector2 Position;
        public Texture2D keyL, keyR, keyJ;

        Rectangle left, right, jump;

        public void LoadContent()
        {
            keyL = AssetManager.GetTexture("dpad_key_left");
            keyR = AssetManager.GetTexture("dpad_key_right");
            keyJ = AssetManager.GetTexture("dpad_key_up");
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 leftKey, rightKey, jumpKey;
            leftKey = Position;
            rightKey = Position + new Vector2(256, 0);
            jumpKey = GameManager.Game.ScreenSize - new Vector2(256);

            spriteBatch.Draw(keyL, new Rectangle(leftKey.ToPoint(), new Point(256)), Color.White);
            spriteBatch.Draw(keyR, new Rectangle(rightKey.ToPoint(), new Point(256)), Color.White);
            spriteBatch.Draw(keyJ, new Rectangle(jumpKey.ToPoint(), new Point(256)), Color.White);

            left = new Rectangle(leftKey.ToPoint(), new Point(256));
            right = new Rectangle(rightKey.ToPoint(), new Point(256));
            jump = new Rectangle(jumpKey.ToPoint(), new Point(256));
        }
        public override void Update(GameTime gameTime)
        {
            if (InputManager.Singleton.TouchIntersects(left))
                UniversalInputManager.Manager.MoveVector.X = -1;
            else if (InputManager.Singleton.TouchIntersects(right))
                UniversalInputManager.Manager.MoveVector.X = 1;
            else
                UniversalInputManager.Manager.MoveVector.X = 0;


            if (InputManager.Singleton.TouchIntersects(jump))
                UniversalInputManager.Manager.MoveVector.Y = 1;
            else
                UniversalInputManager.Manager.MoveVector.Y = 0;
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
        }
    }
}