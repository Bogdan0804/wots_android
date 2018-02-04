using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wots.UI;

namespace Wots.GamePlay.Intro
{
    public class GameIntro : UIComponent
    {
        public Story GameIntroStory = new Story();
        public SpeechDialog sp;
        

        public GameIntro()
        {
            GameIntroStory.Duration = 60;
            GameIntroStory.Name = "Intro1";
            GameIntroStory.Text = new string[3] { ("Hello There, $player_name$!\nWelcome to $world_name$!"), "I've heard that you want to fight?", "Do you want to fight?!" };
            GameIntroStory.Title = "Intro";

            this.sp = GameIntroStory.GetDialog();
            this.sp.IsQuestion = true;
            this.sp.Closed += a =>
            {
                
            };
        }

        public override void Update(GameTime gameTime)
        {
            sp.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sp.Draw(gameTime, spriteBatch);
        }

    }
}
