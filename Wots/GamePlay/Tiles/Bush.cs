﻿using System;
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
    class Bush : Tile
    {
        public override void Initialize(Vector2 position, string state)
        {
            this.Position = position;
            this.Collidable = false;
        }
    }
}