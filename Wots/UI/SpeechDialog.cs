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
        private Rectangle next_button_bounds;
        private TouchCollection old_mouse_state;

        public SpeechDialog()
        {
            _dialog_dimentions = new Vector2(400, 200);
            _position = new Vector2(GameManager.Game.ScreenSize.X / 2 - this._dialog_dimentions.X / 2, (GameManager.Game.ScreenSize.Y - this._dialog_dimentions.Y) - 70);

            if (Arrow_Texture == null)
                Arrow_Texture = AssetManager.LoadImage("art/ui/arrow_selected");

            next_button_bounds = new Rectangle((_position + _dialog_dimentions - new Vector2(Arrow_Texture.Width + 20, Arrow_Texture.Height + 20)).ToPoint(), new Point(Arrow_Texture.Width, Arrow_Texture.Height));
        }

        public override void Draw(GameTime gameTime, SpriteBatch uiSpriteBatch)
        {
            if (isVisible)
            {
                uiSpriteBatch.Draw(AssetManager.GetTexture("dialog"), new Rectangle(_position.ToPoint(), _dialog_dimentions.ToPoint()), Color.White);
                string text = _subject + ": " + SpeechFrames[Index].Text;
                if (AssetManager.GetFont(_fontString).MeasureString(text).X + 50 >= this._dialog_dimentions.X)
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
                uiSpriteBatch.DrawString(AssetManager.GetFont(_fontString), text, _position + new Vector2(25), Color.White);
                if (IsQuestion && Index == SpeechFrames.Count - 1)
                {
                    uiSpriteBatch.Draw(Arrow_Texture, new Rectangle((next_button_bounds.X - next_button_bounds.Width) - 10, next_button_bounds.Y, next_button_bounds.Width, next_button_bounds.Height), Color.Green);
                    uiSpriteBatch.Draw(Arrow_Texture, next_button_bounds, Color.Red);
                }
                else
                {
                    uiSpriteBatch.Draw(Arrow_Texture, next_button_bounds, Color.White);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            var touchCol = TouchPanel.GetState();

            try
            {
                for (int i = 0; i < touchCol.Count; i++)
                {
                    var touch = touchCol[i];

                    if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
                        continue;

                    if (this.next_button_bounds.Contains(touch.Position) && Index < SpeechFrames.Count - 1)
                    {
                        if (Index < this.SpeechFrames.Count)
                            Index++;
                    }
                    else
                    {
                        Rectangle rect_yes = new Rectangle((next_button_bounds.X - next_button_bounds.Width) - 10, next_button_bounds.Y, next_button_bounds.Width, next_button_bounds.Height);
                        if (this.next_button_bounds.Contains(touch.Position))
                        {
                            this.Result = DialogResult.No;
                            isVisible = false;

                            Closed?.Invoke(this);
                        }
                        else if (rect_yes.Contains(touch.Position))
                        {
                            this.Result = DialogResult.Yes;
                            isVisible = false;

                            Closed?.Invoke(this);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            old_mouse_state = touchCol;
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
