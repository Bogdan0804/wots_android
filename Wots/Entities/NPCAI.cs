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
using Wots.GamePlay;
using Wots.UI;

namespace Wots.Entities
{
    public class NPCAI : AI
    {
        public NPCAI(Vector2 position) : base(new Vector2(96, 192), position)
        {
            this.Sprite.Animations.Add("left", new Animation(
                new Frame(AssetManager.GetTexture("left_1"))
                ));
            this.Sprite.Animations.Add("right", new Animation(
                new Frame(AssetManager.GetTexture("right_1"))
                ));

            this.Sprite.CurrentAnimation = "right";
        }

        public override void Damage(int damage, Vector2 gestureDelta)
        {

        }
        string name = "Joe";
        public override void Update(GameTime gameTime)
        {
            if (this.Sprite.Position.X > GameScreen.Player.PlayerSprite.Position.X)
                this.Sprite.CurrentAnimation = "left";
            else if (this.Sprite.Position.X < GameScreen.Player.PlayerSprite.Position.X)
                this.Sprite.CurrentAnimation = "right";

            
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gestures)
        {
            foreach (var touch in touches)
            {
                //if (this.Sprite.GetRectangle().Contains(GameScreen.Camera.ScreenToWorld(touch.Position)))
            }
        }
        

        public override void Draw(SpriteBatch sp)
        {
            sp.DrawString(AssetManager.GetFont("24"), name, this.Sprite.Position - new Vector2(0, AssetManager.GetFont("12").MeasureString("I").Y + 10), Color.White);
        }
    }
}