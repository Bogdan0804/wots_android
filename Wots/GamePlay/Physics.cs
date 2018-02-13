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
using static Wots.GamePlay.Player;

namespace Wots.GamePlay
{
    public class Physics
    {
        public bool CanUp
        {
            get
            {
                try
                {
                    return Collitions.Up.Point1.Item1 & Collitions.Up.Point2.Item1;
                }

                catch
                {
                    return false;
                }
            }
        }
        public bool CanDown
        {
            get
            {
                try
                {
                    return Collitions.Down.Point1.Item1 & Collitions.Down.Point2.Item1;
                }

                catch
                {
                    return false;
                }
            }
        }
        public bool CanLeft
        {
            get
            {
                try
                {
                    return Collitions.Left.Point1.Item1 & Collitions.Left.Point2.Item1;
                }

                catch
                {
                    return false;
                }
            }
        }
        public bool CanRight
        {
            get
            {
                try
                {
                    return Collitions.Right.Point1.Item1 & Collitions.Right.Point2.Item1;
                }
                catch
                {
                    return false;
                }
            }
        }

        public CollitionDetection Collitions = new CollitionDetection();

        public void GeneratePoints(Sprite sprite)
        {
            Collitions.Up = new CollitionPoint(
                World.isSpaceOpen(sprite.Position + new Vector2(10, 0), null, new Vector2(sprite.Size.X / 2 - 5, 1)),
                World.isSpaceOpen(sprite.Position + new Vector2(sprite.Size.X / 2, 0), null, new Vector2(sprite.Size.X / 2 - 5, 1))
                );
            Collitions.Down = new CollitionPoint(
                World.isSpaceOpen(sprite.Position + new Vector2(10, sprite.Size.Y - 1), null, new Vector2(sprite.Size.X / 2 - 5, 1)),
                World.isSpaceOpen(sprite.Position + new Vector2(sprite.Size.X / 2, sprite.Size.Y - 1), null, new Vector2(sprite.Size.X / 2 - 5, 1))
                );
            Collitions.Left = new CollitionPoint(
                World.isSpaceOpen(sprite.Position + new Vector2(0, 10), null, new Vector2(1, sprite.Size.Y / 2 - 5)),
                World.isSpaceOpen(sprite.Position + new Vector2(0, sprite.Size.Y / 2), null, new Vector2(1, sprite.Size.Y / 2 - 5))
                );
            Collitions.Right = new CollitionPoint(
                World.isSpaceOpen(sprite.Position + new Vector2(0, 10), null, new Vector2(sprite.Size.X - 1, sprite.Size.Y / 2 - 5)),
                World.isSpaceOpen(sprite.Position + new Vector2(sprite.Size.X - 1, sprite.Size.Y / 2), null, new Vector2(1, sprite.Size.Y / 2 - 5))
                );
        }

        public void UpdateCollitions(Sprite sprite, SpriteBatch s)
        {
            GeneratePoints(sprite);
        }
    }
}
