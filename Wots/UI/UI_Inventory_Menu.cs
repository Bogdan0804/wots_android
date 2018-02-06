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

namespace Wots.UI
{
    public class UI_Inventory_Menu : UIComponent
    {
        public Keys UseKey = Keys.E;
        private KeyboardState oldKeyboardState;

        public class InventoryItem
        {
            public Texture2D Texture;
            public string Name;
            public int Amount;
            private Func<int, int> UseItem;

            public InventoryItem(string name, int amount, Func<int, int> useTheItem)
            {
                this.Texture = AssetManager.GetTexture(name);
                this.Amount = amount;
                this.Name = name.ToUpper().ToCharArray()[0] + name.Remove(0, 1);

                UseItem = useTheItem;
            }

            public void UseTheItem()
            {
                this.Amount = this.UseItem(Amount);
            }
        }

        public struct Inventory_Hotbar_Items
        {
            public InventoryItem Tile0;
            public InventoryItem Tile1;
            public InventoryItem Tile2;
            public InventoryItem Tile3;
            public InventoryItem Tile4;
            public InventoryItem Tile5;
            public InventoryItem Tile6;
            public InventoryItem Tile7;
            public InventoryItem Tile8;

            public InventoryItem this[int index]
            {
                get
                {
                    if (index == 0)
                        return Tile0;
                    else if (index == 1)
                        return Tile1;
                    else if (index == 2)
                        return Tile2;
                    else if (index == 3)
                        return Tile3;
                    else if (index == 4)
                        return Tile4;
                    else if (index == 5)
                        return Tile5;
                    else if (index == 6)
                        return Tile6;
                    else if (index == 7)
                        return Tile7;
                    else if (index == 8)
                        return Tile8;
                    else throw new Exception($"No inventory item at that index {index}!!!");
                }
            }
        }
        public static Inventory_Hotbar_Items Inventory = new Inventory_Hotbar_Items();

        public static Texture2D texture_ui_selector;
        public static Texture2D texture_ui_selected;
        private Rectangle rect_gui_selector;
        public int SelectedItemIndex = 1;

        public UI_Inventory_Menu()
        {
            Inventory.Tile0 = new InventoryItem("grass", 21, e =>
            {
                int amount = e;
                amount -= amount > 1 ? 1 : 0;
                if (amount - 1 == 0)
                    Inventory.Tile0 = null;
                //if (Player.HealthBar.Value > 0)
                //    Player.HealthBar.Value -= 10;
                return amount;
            });
            Inventory.Tile1 = new InventoryItem("grass_1", 53, e =>
            {
                int amount = e;
                amount -= amount > 1 ? 1 : 0;
                if (amount - 1 == 0)
                    Inventory.Tile1 = null;
                //if (Player.HealthBar.Value < 100)
                //    Player.HealthBar.Value += 10;
                return amount;
            });
            Inventory.Tile2 = new InventoryItem("ladder", 8, e =>
            {
                int amount = e;
                if (amount - 1 == 0)
                    Inventory.Tile2 = null;
                amount -= amount > 1 ? 1 : 0;

                return amount;
            });

            texture_ui_selector = AssetManager.GetTexture("inv_gui_items");
            texture_ui_selected = AssetManager.GetTexture("selected_ui_inv");
            rect_gui_selector = new Rectangle((int)GameManager.Game.ScreenSize.X / 2 - texture_ui_selector.Width / 2, (int)GameManager.Game.ScreenSize.Y - texture_ui_selector.Height * 2, texture_ui_selector.Width, texture_ui_selector.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            Rectangle selected_rect = new Rectangle(
                rect_gui_selector.X + (SelectedItemIndex * 32),
                rect_gui_selector.Y,
                32,
                32
                );

            #region Draw Items
            if (Inventory.Tile0 != null)
            {
                Rectangle posRect = new Rectangle(
                   rect_gui_selector.X + (32 * 0),
                   rect_gui_selector.Y,
                   32,
                   32
                   );
                uiSpriteBatch.Draw(Inventory.Tile0.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile0.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }
            if (Inventory.Tile1 != null)
            {
                Rectangle posRect = new Rectangle(
                   rect_gui_selector.X + (32 * 1),
                   rect_gui_selector.Y,
                   32,
                   32
                   );
                uiSpriteBatch.Draw(Inventory.Tile1.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile1.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }
            if (Inventory.Tile2 != null)
            {
                Rectangle posRect = new Rectangle(
                      rect_gui_selector.X + (32 * 2),
                      rect_gui_selector.Y,
                      32,
                      32
                      );
                uiSpriteBatch.Draw(Inventory.Tile2.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile2.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }
            if (Inventory.Tile3 != null)
            {
                Rectangle posRect = new Rectangle(
                   rect_gui_selector.X + (32 * 3),
                   rect_gui_selector.Y,
                   32,
                   32
                   );
                uiSpriteBatch.Draw(Inventory.Tile3.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile3.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }
            if (Inventory.Tile4 != null)
            {
                Rectangle posRect = new Rectangle(
                      rect_gui_selector.X + (32 * 4),
                      rect_gui_selector.Y,
                      32,
                      32
                      );
                uiSpriteBatch.Draw(Inventory.Tile4.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile4.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }
            if (Inventory.Tile5 != null)
            {
                Rectangle posRect = new Rectangle(
                      rect_gui_selector.X + (32 * 5),
                      rect_gui_selector.Y,
                      32,
                      32
                      );
                uiSpriteBatch.Draw(Inventory.Tile5.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile5.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }
            if (Inventory.Tile6 != null)
            {
                Rectangle posRect = new Rectangle(
                   rect_gui_selector.X + (32 * 6),
                   rect_gui_selector.Y,
                   32,
                   32
                   );
                uiSpriteBatch.Draw(Inventory.Tile6.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile6.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }
            if (Inventory.Tile7 != null)
            {
                Rectangle posRect = new Rectangle(
                   rect_gui_selector.X + (32 * 7),
                   rect_gui_selector.Y,
                   32,
                   32
                   );
                uiSpriteBatch.Draw(Inventory.Tile7.Texture, posRect, Color.White);
                uiSpriteBatch.DrawString(AssetManager.GetFont("12"), Inventory.Tile7.Amount.ToString(), new Vector2(posRect.X + 4, posRect.Y + 4), Color.White);
            }

            uiSpriteBatch.Draw(texture_ui_selector, rect_gui_selector, Color.White * 0.7f);
            uiSpriteBatch.Draw(texture_ui_selected, selected_rect, Color.Gray);
            #endregion
        }
        
        public override void Update(GameTime gameTime)
        {
            //var mouseState = Mouse.GetState();
            var state = Keyboard.GetState();

            #region Mouse Wheel
            //if (mouseState.ScrollWheelValue < oldMouseWheelValue)
            //    if (SelectedItemIndex < 7)
            //        SelectedItemIndex++;
            //if (mouseState.ScrollWheelValue > oldMouseWheelValue)
            //    if (SelectedItemIndex > 0)
            //        SelectedItemIndex--;
            #endregion

            #region Keyboard Items
            if (state.IsKeyDown(Keys.D1))
                SelectedItemIndex = 0;
            else if (state.IsKeyDown(Keys.D2))
                SelectedItemIndex = 1;
            else if (state.IsKeyDown(Keys.D3))
                SelectedItemIndex = 2;
            else if (state.IsKeyDown(Keys.D4))
                SelectedItemIndex = 3;
            else if (state.IsKeyDown(Keys.D5))
                SelectedItemIndex = 4;
            else if (state.IsKeyDown(Keys.D6))
                SelectedItemIndex = 5;
            else if (state.IsKeyDown(Keys.D7))
                SelectedItemIndex = 6;
            else if (state.IsKeyDown(Keys.D8))
                SelectedItemIndex = 7;
            #endregion

            #region Use items
            var touches = TouchPanel.GetState();

            if (state.IsKeyDown(UseKey) && oldKeyboardState.IsKeyUp(UseKey))
                if (Inventory[SelectedItemIndex] != null)
                    UseItem(Inventory[SelectedItemIndex]);
            #endregion

            oldKeyboardState = state;
            //this.oldMouseWheelValue = mouseState.ScrollWheelValue;
        }

        /// <summary>
        /// Usess the item when the item use key is pressed
        /// </summary>
        /// <param name="p"></param>
        private void UseItem(InventoryItem p)
        {
            p.UseTheItem();
        }
    }
}
