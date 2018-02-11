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

namespace Wots.UI
{
    public class BasicStats : UIComponent
    {
        public int HealthValue
        {
            set
            {
                Health.Value = value;
            }
            get
            {
                return Health.Value;
            }
        }
        public int ManaValue
        {
            set
            {
                Mana.Value = value;
            }
            get
            {
                return Mana.Value;
            }
        }
        public int EXPValue
        {
            set
            {
                EXP.Value = value;
            }
            get
            {
                return EXP.Value;
            }
        }

        Bar Mana, Health, EXP;

        public Texture2D MainTexture;
        public Rectangle Overview;

        public BasicStats()
        {
            MainTexture = AssetManager.LoadImage("art/ui/gameui/bars");
            Overview = new Rectangle(0, 0, MainTexture.Width * 4, MainTexture.Height * 4);
            Health = new Bar(new Vector2(12));
            Mana = new Bar(new Vector2(12, 48));
            EXP = new Bar(new Vector2(12, 84));

            Health.Height = 16;
            Mana.Height = 16;
            EXP.Height = 16;

            Health.Color = Color.OrangeRed;
            Mana.Color = Color.Aqua;
            EXP.Color = Color.GreenYellow;
            

            Health.MaxWidth = 340;
            Mana.MaxWidth = 340;
            EXP.MaxWidth = 340;
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            uiSpriteBatch.Draw(MainTexture, Overview, Color.White);
            Mana.Draw(gameTime, uiSpriteBatch);
            Health.Draw(gameTime, uiSpriteBatch);
            EXP.Draw(gameTime, uiSpriteBatch);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}