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
using Wots.GamePlay;

namespace Wots.Entities
{
    public class SlimeAI : AI
    {
        public SlimeAI() 
            : base(new Vector2(0), new Vector2(0))
        {
            Sprite.Animations.Add("move", new Animation(
                new Frame(AssetManager.GetTexture("slime0")),
                new Frame(AssetManager.GetTexture("slime1"))
                ));

            Sprite.CurrentAnimation = "move";
        }
        public override void Update(GameTime gameTime)
        {
            //if (this.Sprite.Position.X > GameScreen.Player.PlayerSprite.Position.X)
            //    this.Sprite.Position.X++;
            //else if (this.Sprite.Position.X < GameScreen.Player.PlayerSprite.Position.X)
            //    this.Sprite.Position.X--;
           // this.Sprite.Position = GameScreen.Player.PlayerSprite.Position;
            Sprite.Update(gameTime);
        }
    }
}