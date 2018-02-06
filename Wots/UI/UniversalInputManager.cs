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
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots.UI
{
    public class UniversalInputManager
    {
        private static UniversalInputManager inputs;
        public static UniversalInputManager Manager
        {
            get
            {
                if (inputs == null)
                {
                    inputs = new UniversalInputManager();
                    inputs.MoveVector = Vector2.Zero;
                }

                return inputs;
            }
        }
        public Vector2 MoveVector;

        public float GetAxis(string name)
        {
            if (name == "Vertical")
            {
                return MoveVector.Y;
            }
            else if (name == "Horizontal")
            {
                return MoveVector.X;
            }

            return 0.0f;
        }

        public GestureSample ReadGesture()
        {
            if (TouchPanel.IsGestureAvailable)
                return TouchPanel.ReadGesture();

            return new GestureSample();
        }
    }

}