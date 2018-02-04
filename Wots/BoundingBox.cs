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

using System;
using Microsoft.Xna.Framework;

namespace Wots
{
    public class BoundingBox
    {
        public Rectangle Box { get; internal set; }
        public Rectangle ColBox { get; internal set; }
    }
}
