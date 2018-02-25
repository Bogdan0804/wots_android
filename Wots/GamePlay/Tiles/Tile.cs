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

namespace Wots.GamePlay.Tiles
{
    public class Prefs
    {
        public Func<Player, bool> OnJump;
        public Func<Player, bool> OnUp;
        public Func<Player, bool> OnDown;
        public Func<Player, bool> OnLeft;
        public Func<Player, bool> OnRight;
        public Func<Player, bool> OnClick;

        public bool usePrefJump = false;
        public bool usePrefUp = false;
        public bool usePrefDown = false;
        public bool usePrefLeft = false;
        public bool usePrefRight = false;
        public bool usePrefClick = false;
    }
    public abstract class Tile
    {
        public Prefs Prefs = new Prefs();
        public Vector2 Position { get; set; }
        public string Texture { get; set; }
        public string State { get; set; }
        public bool Collidable { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(RoundNum((int)Position.X), RoundNum((int)Position.Y), 96, 96);
            }
        }
        private int RoundNum(int num)
        {
            int rem = num % 96;
            return rem >= 96 / 2 ? (num - rem + 96) : (num - rem);
        }
        public abstract void Initialize(Vector2 position, string state);
    }
}