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

        public Bar Mana, Health, EXP;

        public Texture2D MainTexture;
        public Rectangle Overview;
        private int _width, _height;

        public BasicStats()
        {
            MainTexture = AssetManager.LoadImage("art/ui/gameui/bars");
            Overview = new Rectangle(0, 0, MainTexture.Width * 4, MainTexture.Height * 4);
            Health = new Bar(new Vector2(12));
            Mana = new Bar(new Vector2(12, 48));
            EXP = new Bar(new Vector2(12, 84));

            _width = MainTexture.Width * 4;
            _height = MainTexture.Height * 4;

            Health.Height = 16;
            Mana.Height = 16;
            EXP.Height = 16;

            Health.Color = Color.OrangeRed;
            Mana.Color = Color.Aqua;
            EXP.Color = Color.GreenYellow;


            Health.MaxWidth = 340;
            Mana.MaxWidth = 340;
            EXP.MaxWidth = 340;

            EXPValue = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            uiSpriteBatch.Draw(MainTexture, Overview, Color.White);
            Mana.Draw(gameTime, uiSpriteBatch);
            Health.Draw(gameTime, uiSpriteBatch);
            EXP.Draw(gameTime, uiSpriteBatch);
        }

        private double healTimer = 0;
        private double manaTimer = 0;
        public override void Update(GameTime gameTime)
        {
            healTimer += gameTime.ElapsedGameTime.TotalSeconds;
            manaTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (healTimer > 1 && HealthValue < 100 && ManaValue > 0)
            {
                healTimer = 0;
                HealthValue++;

                ManaValue -= 5;
            }

            if (manaTimer > 2 && ManaValue < 100)
            {
                manaTimer = 0;
                ManaValue += 5;
            }
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {

        }
    }
}