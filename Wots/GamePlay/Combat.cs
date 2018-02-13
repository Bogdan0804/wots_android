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

namespace Wots.GamePlay
{
    public class Combat
    {
        public Rectangle HitArea
        {
            get
            {
                return new Rectangle((int)GameScreen.Player.PlayerSprite.Position.X - 100, (int)GameScreen.Player.PlayerSprite.Position.Y - 100, (int)GameScreen.Player.PlayerSprite.Size.X + 200, (int)GameScreen.Player.PlayerSprite.Size.Y + 200);
            }
        }

        public void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
            if (gesture.GestureType == GestureType.Flick)
            {
                if (GameScreen.ui_menu_inventory.Bar.HotbarItems[GameScreen.ui_menu_inventory.Bar.SelectedIndex].ItemType == UI.Type.Weapon)
                {
                    foreach (var entity in World.Worlds[World.WorldName].Entities)
                    {
                        if (this.HitArea.Contains(entity.Sprite.Position))
                            entity.Damage(10);
                    }
                }

            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}