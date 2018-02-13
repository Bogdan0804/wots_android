using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Wots.UI
{
    public enum DialogResult
    {
        Yes, No
    }

    public class SpeechDialog : UIComponent
    {
        public static Texture2D Arrow_Texture;
        public bool IsQuestion = false;
        public Bag<SpeechFrame> SpeechFrames = new Bag<SpeechFrame>();
        public DialogResult Result = DialogResult.No;
        public int Index = 0;

        public bool isVisible = false;

        private string _fontString = "";
        public string Font
        {
            get
            {
                return _fontString;
            }
            set
            {
                _fontString = value;
            }
        }

        private string _subject = "";
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public delegate void _clicked(object sender);
        public event _clicked Closed;

        private Vector2 _dialog_dimentions = Vector2.Zero;
        private Vector2 _position = Vector2.Zero;
        private Button nextBtn;

        public SpeechDialog()
        {
            _dialog_dimentions = new Vector2(500, 250);
            _position = new Vector2(GameManager.Game.ScreenSize.X / 2 - this._dialog_dimentions.X / 2, (GameManager.Game.ScreenSize.Y - this._dialog_dimentions.Y) - 70);

            if (Arrow_Texture == null)
                Arrow_Texture = AssetManager.LoadImage("art/ui/arrow_selected");
            nextBtn = new Button(Arrow_Texture, _position + _dialog_dimentions - new Vector2(Arrow_Texture.Width + 35, Arrow_Texture.Height + 35), new Vector2(Arrow_Texture.Width + 35, Arrow_Texture.Height + 35));
            nextBtn.Pressed += (e) => {
                if (Index < SpeechFrames.Count)
                    Index++;
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            if (isVisible)
            {
                uiSpriteBatch.Draw(AssetManager.GetTexture("dialog"), new Rectangle(_position.ToPoint(), _dialog_dimentions.ToPoint()), Color.White);
                string text = _subject + ": " + SpeechFrames[Index].Text;
                if (AssetManager.GetFont(_fontString).MeasureString(text).X + 80 >= this._dialog_dimentions.X)
                {
                    string s = "";
                    for (int i = 0; i < text.ToCharArray().Length; i++)
                    {
                        s += text.ToCharArray()[i];

                        if (AssetManager.GetFont(_fontString).MeasureString(s.Split('\n').LastOrDefault()).X + 50 >= this._dialog_dimentions.X)
                        {
                            if (s.ToCharArray().LastOrDefault() == ' ') s += "\n";
                            else s += "\n-";
                        }
                    }

                    text = s;
                }
                uiSpriteBatch.DrawString(AssetManager.GetFont(_fontString), text, _position + new Vector2(40), Color.White);
                if (IsQuestion && Index == SpeechFrames.Count - 1)
                {
                    // qiestion
                }
                else
                {
                    // normal
                    nextBtn.Draw(gameTime, uiSpriteBatch);
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            nextBtn.Update(gameTime);
        }

        public override void UpdateGestures(TouchCollection touches, GestureSample gesture)
        {
        }
    }


    public class SpeechFrame
    {
        public string Text { get; set; }

        public SpeechFrame(string text)
        {
            this.Text = text;
        }
    }
}
