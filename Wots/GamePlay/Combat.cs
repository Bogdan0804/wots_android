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

namespace Wots.GamePlay
{
    public class Combat
    {
        public double CoolDown = 0.5;
        public double AttackTimer = 0;
        public static Rectangle HitArea
        {
            get
            {
                return new Rectangle((int)GameScreen.Player.PlayerSprite.Position.X - 100, (int)GameScreen.Player.PlayerSprite.Position.Y - 100, (int)GameScreen.Player.PlayerSprite.Size.X + 200, (int)GameScreen.Player.PlayerSprite.Size.Y + 200);
            }
        }
        public void Update(GameTime gameTime)
        {
            AttackTimer += gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in World.Worlds[World.WorldName].Entities)
            {
                if (GameScreen.ui_menu_inventory.Bar.HotbarItems[GameScreen.ui_menu_inventory.Bar.SelectedIndex].ItemType == UI.Type.Weapon)
                {
                    if (InputManager.Singleton.TouchIntersects(entity.Sprite.GetScreenRectangle(), 100))
                    {
                        if (AttackTimer >= CoolDown)
                        {
                            AttackTimer = 0;
                            entity.Damage(2, new Vector2(1));
                        }
                    }
                }
            }
        }
    }
}