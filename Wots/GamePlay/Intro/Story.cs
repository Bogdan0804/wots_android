using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wots.UI;

namespace Wots.GamePlay.Intro
{
    public class Story
    {
        private StoryItem _story_item = new StoryItem();

        public int Duration
        {
            get
            {
                return _story_item.Duration;
            }
            set
            {
                _story_item.Duration = value;
            }
        }
        public string Title
        {
            get
            {
                return _story_item.Title;
            }
            set
            {
                _story_item.Title = value;
            }
        }
        public string Name
        {
            get
            {
                return _story_item.Name;
            }
            set
            {
                _story_item.Name = value;
            }
        }
        public string[] Text
        {
            get
            {
                return _story_item.Text;
            }
            set
            {
                _story_item.Text = value;
            }
        }

        public SpeechDialog GetDialog()
        {
            Bag<SpeechFrame> s = new Bag<SpeechFrame>();
            for (int i = 0; i < Text.Length; i++)
            {
                s.Add(new SpeechFrame(this.Text[i]));
            }

            return new SpeechDialog { Font = "12", Index = 0, isVisible = true, Subject = Name, SpeechFrames = s };
        }
    }
}
