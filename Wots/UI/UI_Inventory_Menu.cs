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

        internal void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
            menu.UpdateGestures(touches, gesture);
        }
    }

    public class Inventory_MENU
    {
        public Item[,] Items;

        public Button useButton;

        public Vector2 SelectedSlot = new Vector2(0);
        Texture2D menu_bg;
        Vector2 Size, Position;
        private bool hasSelectedItem;

        internal void init()
        {
            menu_bg = AssetManager.LoadImage("art/ui/gameui/open_inv_menu");

            Size = new Vector2(menu_bg.Width * 6f, menu_bg.Height * 6);
            Position = new Vector2(250, 100);

            Items = new Item[5, 4];
            useButton = new Button(AssetManager.GetTexture("button_1"), new Vector2(Position.X + 550, Position.Y + Size.Y - 150), new Vector2(175,95));
            useButton.Text = "Use";
            useButton.RenderText = true;
            useButton.Pressed += UseButton_Pressed;

            Items[0, 0] = new Item { Amount = 2, ItemType = Type.Item, Name = "peice of grass", OnUse = new Func<int>(() => { GameScreen.Stats.HealthValue = 10;  return 0; }), Type = "grass" };
        }

        private void UseButton_Pressed(object sender)
        {
            try
            {
                var item = Items[(int)SelectedSlot.X, (int)SelectedSlot.Y];
                if (item.OnUse() <= 0)
                    Items[(int)SelectedSlot.X, (int)SelectedSlot.Y] = null;
            }
            catch { };
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menu_bg, new Rectangle(Position.ToPoint(), Size.ToPoint()), Color.White);
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Vector2 TempPosition = Position + new Vector2(100, 125) + new Vector2(x * 128, y * 128);
                    Vector2 Size = new Vector2(128);
                    Vector2 Margin = new Vector2(2);
                    
                    if (y == SelectedSlot.Y && x == SelectedSlot.X)
                    {
                        try
                        {
                            spriteBatch.Draw(AssetManager.GetTexture(Items[x, y].Type), new Rectangle((TempPosition + Margin).ToPoint(), (Size - Margin).ToPoint()), Color.LightGray);
                        }
                        catch { };

                        spriteBatch.Draw(AssetManager.GetTexture("selected_ui_inv"), new Rectangle((TempPosition + Margin).ToPoint(), (Size - Margin).ToPoint()), Color.Gray);
                    }
                    else
                    {
                        try
                        {
                            spriteBatch.Draw(AssetManager.GetTexture(Items[x, y].Type), new Rectangle((TempPosition + Margin).ToPoint(), (Size - Margin).ToPoint()), Color.White);
                        }
                        catch { };
                        spriteBatch.Draw(AssetManager.GetTexture("selected_ui_inv"), new Rectangle((TempPosition + Margin).ToPoint(), (Size - Margin).ToPoint()), Color.White);
                    }
                }
            }
            
            try
            {
                var item = Items[(int)SelectedSlot.X, (int)SelectedSlot.Y];

                spriteBatch.DrawString(AssetManager.GetFont("24"), item.Name, new Vector2(Position.X + 100, Position.Y + Size.Y - 200), Color.Black);
                useButton.Draw(null, spriteBatch);
                hasSelectedItem = true;
            }
            catch { hasSelectedItem = false; };
            
            
            DrawEquipables(spriteBatch);
        }

        internal void Update(GameTime gameTime)
        {
            var rect = new Rectangle(Position.ToPoint(), Size.ToPoint());
            var closeBtnRect = new Rectangle((int)(rect.X - 66 + Size.X), rect.Y, 66, 66);

            if (InputManager.Singleton.TouchIntersects(closeBtnRect))
            {
                InventoryUI.isInvOpen = false;
            }

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Vector2 TempPosition = Position + new Vector2(100, 125) + new Vector2(x * 128, y * 128);
                    Vector2 Size = new Vector2(128);
                    Vector2 Margin = new Vector2(2);

                    Rectangle tempRectangle = new Rectangle((TempPosition + Margin).ToPoint(), (Size - Margin).ToPoint());
                    if (SwordRush.Components.InputManager.Singleton.TouchIntersects(tempRectangle))
                        this.SelectedSlot = new Vector2(x, y);
                }
            }
            
            UpdateEquipables(gameTime);
        }

        private void DrawEquipables(SpriteBatch spriteBatch)
        {
            int X = (int)Position.X + 775;
            int margin = 32;

            spriteBatch.Draw(AssetManager.GetTexture("placeholder_helmet"), new Rectangle(X, 275, 128, 128), Color.White);
            spriteBatch.Draw(AssetManager.GetTexture("selected_ui_inv"), new Rectangle(X, 275, 128, 128), Color.White);

            spriteBatch.Draw(AssetManager.GetTexture("placeholder_chestplate"), new Rectangle(X, 275 + 128, 128, 128), Color.White);
            spriteBatch.Draw(AssetManager.GetTexture("selected_ui_inv"), new Rectangle(X, 275 + 128, 128, 128), Color.White);

            spriteBatch.Draw(AssetManager.GetTexture("placeholder_pants"), new Rectangle(X, 275 + 256, 128, 128), Color.White);
            spriteBatch.Draw(AssetManager.GetTexture("selected_ui_inv"), new Rectangle(X, 275 + 256, 128, 128), Color.White);
        }
        private void UpdateEquipables(GameTime gameTime)
        {

        }

        internal void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
            if (hasSelectedItem)
                useButton.UpdateGestures(touches, gesture);
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
            Bar.UpdateGestures(touches, gesture);
        }
    }
}
