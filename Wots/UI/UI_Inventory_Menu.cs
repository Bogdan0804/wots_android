using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended.Collections;
using SwordRush.Components;
using Wots.GamePlay;

namespace Wots.UI
{
    public enum Type
    {
        Item,
        Weapon
    }

    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public Type ItemType { get; set; }
        public Func<int> OnUse { get; set; }
    }

    public class InventoryUI
    {
        Inventory_MENU menu = new Inventory_MENU();
        public int SelectedIndex = 0;
        public Vector2 Position = Vector2.Zero;
        public Bag<Item> HotbarItems = new Bag<Item>(7);
        // public Rectangle UseArea;

        public InventoryUI()
        {
            menu.init();
            // UseArea = new Rectangle((int)GameManager.Game.ScreenSize.X / 2, 64, (int)GameManager.Game.ScreenSize.X / 2, (int)GameManager.Game.ScreenSize.Y - 296);
            Position = new Vector2(GameManager.Game.ScreenSize.X / 2 - AssetManager.GetTexture("inv_gui_items").Width / 2, 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isInvOpen)
                //spriteBatch.Draw(GameScreen.textureBlank, UseArea, new Color(Color.Black, 20));
                for (int i = 0; i < HotbarItems.Count; i++)
                {
                    if (i == 7)
                        continue;

                    var item = HotbarItems[i];
                    if (item.ItemType == Type.Weapon)
                    {
                        Vector2 tempPos = new Vector2(Position.X + (64 * i), Position.Y + 5);
                        spriteBatch.Draw(AssetManager.GetTexture(item.Type), new Rectangle(tempPos.ToPoint(), new Point(55, 90)), Color.White);
                    }
                    else
                    {
                        Vector2 tempPos = new Vector2(Position.X + (64 * i), Position.Y);
                        spriteBatch.Draw(AssetManager.GetTexture(item.Type), new Rectangle(tempPos.ToPoint(), new Point(64)), Color.White);
                        spriteBatch.DrawString(AssetManager.GetFont("12"), item.Amount.ToString(), tempPos + new Vector2(64) - AssetManager.GetFont("12").MeasureString(item.Amount.ToString()) - new Vector2(5, 7), Color.Black);
                    }

                }
            else
                menu.Draw(spriteBatch);

            spriteBatch.Draw(AssetManager.GetTexture("inv_gui_items"), new Rectangle(Position.ToPoint(), new Point(AssetManager.GetTexture("inv_gui_items").Width * 2, AssetManager.GetTexture("inv_gui_items").Height * 2)), Color.White);
            int x = (SelectedIndex * 64) + (int)Position.X;
            spriteBatch.Draw(AssetManager.GetTexture("selected_ui_inv"), new Rectangle(new Point(x, (int)Position.Y), new Point(64)), Color.Gray);
            // Draw open btn
            {
                Vector2 tempPos = new Vector2(Position.X + (64 * 7), Position.Y);
                spriteBatch.Draw(AssetManager.GetTexture("open_inv"), new Rectangle(tempPos.ToPoint(), new Point(64)), Color.White);
            }
        }

        public static bool isInvOpen = false;
        public void Update(GameTime gameTime)
        {
            //updare open btn
            {
                Vector2 tempPos = new Vector2(Position.X + (64 * 7), Position.Y);
                Rectangle itemRect = new Rectangle(tempPos.ToPoint(), new Point(64));
                if (InputManager.Singleton.TouchIntersects(itemRect))
                {
                    isInvOpen = true;
                }
            }

            GameScreen.STOP = isInvOpen;

            if (!isInvOpen)
                for (int i = 0; i < HotbarItems.Count; i++)
                {
                    if (i == 7)
                        continue;

                    var item = HotbarItems[i];

                    Vector2 tempPos = new Vector2(Position.X + (64 * i), Position.Y);
                    Rectangle itemRect = new Rectangle(tempPos.ToPoint(), new Point(64));
                    if (InputManager.Singleton.TouchIntersects(itemRect))
                    {
                        SelectedIndex = i;
                    }

                }
            else
                menu.Update(gameTime);
        }

    }

    public class Inventory_MENU
    {
        Texture2D menu_bg;
        Vector2 Size, Position;

        internal void init()
        {
            menu_bg = AssetManager.LoadImage("art/ui/gameui/open_inv_menu");
        }

        internal void Draw(SpriteBatch spriteBatch)
        {            
            spriteBatch.Draw(menu_bg, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.White);
        }

        internal void Update(GameTime gameTime)
        {
            Size = new Vector2(menu_bg.Width * 6f, menu_bg.Height * 6);
            Position = new Vector2(250, 100);

            var rect = new Rectangle(Position.ToPoint(), Size.ToPoint());
            var closeBtnRect = new Rectangle((int)(rect.X - 66 + Size.X), rect.Y, 66, 66);

            if (InputManager.Singleton.TouchIntersects(closeBtnRect))
            {
                InventoryUI.isInvOpen = false;
            }
        }
    }

    public class UI_Inventory_Menu : UIComponent
    {
        Random r = new Random();
        public InventoryUI Bar = new InventoryUI();

        public UI_Inventory_Menu()
        {
            Bar.HotbarItems.Add(new Item
            {
                Amount = 10,
                Name = "Grass",
                Type = "grass",
                OnUse = new Func<int>(() =>
                {
                    return r.Next();
                })
            });
            Bar.HotbarItems.Add(new Item
            {
                Amount = 12,
                Name = "Grass",
                Type = "grass"
            });
            Bar.HotbarItems.Add(new Item
            {
                Amount = 14,
                Name = "Grass",
                Type = "grass"
            });
            Bar.HotbarItems.Add(new Item
            {
                Amount = 14,
                Name = "Sword",
                Type = "wooden_sword1",
                ItemType = Type.Weapon
            });
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            Bar.Draw(uiSpriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Bar.Update(gameTime);
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
        }
    }
}
