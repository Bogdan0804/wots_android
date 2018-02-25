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
    class Ladder : Tile
    {
        public override void Initialize(Vector2 position, string state)
        {
            this.Position = position;
            this.Collidable = false;
            this.Prefs.usePrefUp = true;
            this.Prefs.usePrefJump = true;
            this.Prefs.OnJump += new Func<Player, bool>(e => {
                e.useGravity = false;
                return false;
            });
            this.Prefs.OnUp = new Func<Player, bool>(e => {
                e.useGravity = false;
                e.PlayerSprite.Position.Y -= 3;
                return false;
            });
        }
    }
}