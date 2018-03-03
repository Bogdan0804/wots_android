#region Licence - LGPLv3
// ***********************************************************************
// Assembly         : MonoGameUI
// Author           : Thomas Christof
// Created          : 02-15-2016
//
// Last Modified By : Thomas Christof
// Last Modified On : 05-31-2016
// ***********************************************************************
// <copyright>
// Company: Indie-Dev
// Thomas Christof (c) 2015
// </copyright>
// <License>
// GNU LESSER GENERAL PUBLIC LICENSE
// </License>
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ***********************************************************************
#endregion Licence - LGPLv3
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SwordRush.Components
{
    public enum MouseListenerType
    {
        KeyDown,
        KeyPress,
        MousePress,
        MouseDown
    }

    public enum TouchListenerType
    {
        /// <summary>
        /// Finger moved on the screen.
        /// </summary>
        Move,
        /// <summary>
        /// The finger just touched the screen.
        /// </summary>
        Press,
        /// <summary>
        /// The finger left the touch panel.
        /// </summary>
        Release
    }

    public class InputManager : DrawableGameComponent
    {
        private Dictionary<Keys, List<Action>> keyDown = new Dictionary<Keys, List<Action>>();
        private Dictionary<Keys, List<Action>> keyPress = new Dictionary<Keys, List<Action>>();
        private Dictionary<Buttons, List<Action>> buttonDown = new Dictionary<Buttons, List<Action>>();
        private Dictionary<Buttons, List<Action>> buttonPress = new Dictionary<Buttons, List<Action>>();
        private Dictionary<MouseListenerType, List<Action>> mouseDown = new Dictionary<MouseListenerType, List<Action>>();
        private Dictionary<MouseListenerType, List<Action>> mousePress = new Dictionary<MouseListenerType, List<Action>>();
        private Dictionary<TouchListenerType, List<Action<int, float, Vector2>>> touchData = new Dictionary<TouchListenerType, List<Action<int, float, Vector2>>>();
        private Action<Keys[], Keys[]> blockCallback;
        private SpriteBatch spriteBatch;

        public InputManager(Game game)
            : base(game)
        {
            UpdateOrder = DrawOrder = 20;
            Singleton = this;

            touchData.Add(TouchListenerType.Move, new List<Action<int, float, Vector2>>());
            touchData.Add(TouchListenerType.Press, new List<Action<int, float, Vector2>>());
            touchData.Add(TouchListenerType.Release, new List<Action<int, float, Vector2>>());
        }

        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public static InputManager Singleton { get; private set; }

        public KeyboardState KeyboardState { get; private set; }

        public KeyboardState LastKeyboardState { get; private set; }

        public MouseState MouseState { get; private set; }

        public MouseState LastMouseState { get; private set; }

        public GamePadState GamePadState { get; private set; }

        public GamePadState LastGamePadState { get; private set; }

        public TouchCollection TouchPanelState { get; private set; }

        public TouchCollection LastTouchPanelState { get; private set; }

        public bool IsGamePadConnected { get { return GamePadState.IsConnected; } }

        public Point MousePositionPoint { get { return MouseState.Position; } }

        public Vector2 MousePositionVector { get { return new Vector2(MouseState.X, MouseState.Y); } }

        public bool IsLeftMouseUp { get { return MouseState.LeftButton == ButtonState.Released; } }

        public bool IsRightMouseUp { get { return MouseState.RightButton == ButtonState.Released; } }

        public bool IsLeftMouseDown { get { return MouseState.LeftButton == ButtonState.Pressed; } }

        public bool IsRightMouseDown { get { return MouseState.RightButton == ButtonState.Pressed; } }

        public bool LeftClickPerformed { get { return MouseState.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed; } }

        public bool RightClickPerformed { get { return MouseState.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed; } }

        public bool IsTouchDataAvailable { get { return TouchPanelState.Count > 0; } }

        public bool IsGestureAvailable { get { return TouchPanel.IsGestureAvailable; } }

        public bool IsKeyDown(Keys key) { return KeyboardState.IsKeyDown(key); }

        public bool IsKeyUp(Keys key) { return KeyboardState.IsKeyUp(key); }

        public bool IsButtonDown(Buttons button) { return GamePadState.IsButtonUp(button); }

        public bool IsButtonUp(Buttons button) { return GamePadState.IsButtonDown(button); }

        public bool IsGamePadIndexConnected(PlayerIndex index) { return GamePad.GetState(index).IsConnected; }

        public bool KeyPressPerformed(Keys key) { return KeyboardState.IsKeyUp(key) && LastKeyboardState.IsKeyDown(key); }

        public Keys[] KeyPressPerformedKeys { get { return LastKeyboardState.GetPressedKeys().Where(l => !KeyboardState.GetPressedKeys().Contains(l)).ToArray(); } }

        public bool MouseIntersects(Rectangle rectangle)
        {
            return MousePositionPoint.X >= rectangle.X &&
                MousePositionPoint.X <= rectangle.X + rectangle.Width &&
                MousePositionPoint.Y >= rectangle.Y &&
                MousePositionPoint.Y <= rectangle.Y + rectangle.Height;
        }

        public bool TouchIntersects(Rectangle rectangle, int accuracy = 1)
        {
            return TouchPanelState.Where(s => s.State == TouchLocationState.Pressed ||
                s.State == TouchLocationState.Moved).ToList().Any(s => rectangle.Intersects(new Rectangle((int)s.Position.X - accuracy / 2, (int)s.Position.Y - accuracy / 2, accuracy, accuracy)));
        }

        public void BlockAndDelegateInput(Action<Keys[], Keys[]> callback)
        {
            if (blockCallback != null)
                throw new InvalidOperationException("Someone already listening to all keys");
            blockCallback = callback;
        }

        public void UnBlockInput()
        {
            blockCallback = null;
        }

        public void StopVibration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }

        /// <summary>
        /// Starts the vribration.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="time">The time in [ms].</param>
        public async void StartVibration(float left, float right, int time)
        {
            GamePad.SetVibration(PlayerIndex.One, left, right);
            await Task.Delay(time);
            StopVibration();
        }

        public void AddKeyListener(Keys key, Action callback, MouseListenerType type = MouseListenerType.KeyPress)
        {
            switch (type)
            {
                case MouseListenerType.KeyPress:
                    if (!keyPress.ContainsKey(key))
                        keyPress.Add(key, new List<Action>());
                    keyPress[key].Add(callback);
                    break;
                case MouseListenerType.KeyDown:
                    if (!keyDown.ContainsKey(key))
                        keyDown.Add(key, new List<Action>());
                    keyDown[key].Add(callback);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void RemoveKeyListener(Keys key, Action callback, MouseListenerType type = MouseListenerType.KeyPress)
        {
            switch (type)
            {
                case MouseListenerType.KeyPress:
                    if (keyPress.ContainsKey(key))
                        keyPress[key].Remove(callback);
                    if (keyPress.Count == 0)
                        keyPress.Remove(key);
                    break;
                case MouseListenerType.KeyDown:
                    if (keyDown.ContainsKey(key))
                        keyDown[key].Remove(callback);
                    if (keyDown.Count == 0)
                        keyDown.Remove(key);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void AddGamepadListener(Buttons button, Action callback, MouseListenerType type = MouseListenerType.KeyPress)
        {
            switch (type)
            {
                case MouseListenerType.KeyPress:
                    if (!buttonPress.ContainsKey(button))
                        buttonPress.Add(button, new List<Action>());
                    buttonPress[button].Add(callback);
                    break;
                case MouseListenerType.KeyDown:
                    if (!buttonDown.ContainsKey(button))
                        buttonDown.Add(button, new List<Action>());
                    buttonDown[button].Add(callback);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void RemoveGamepadListener(Buttons button, Action callback, MouseListenerType type = MouseListenerType.KeyPress)
        {
            switch (type)
            {
                case MouseListenerType.KeyPress:
                    if (buttonPress.ContainsKey(button))
                        buttonPress[button].Remove(callback);
                    if (buttonPress.Count == 0)
                        buttonPress.Remove(button);
                    break;
                case MouseListenerType.KeyDown:
                    if (buttonDown.ContainsKey(button))
                        buttonDown[button].Remove(callback);
                    if (buttonDown.Count == 0)
                        buttonDown.Remove(button);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void AddMouseListener(Action callback, MouseListenerType type = MouseListenerType.MousePress)
        {
            switch (type)
            {
                case MouseListenerType.MouseDown:
                    if (!mouseDown.ContainsKey(type))
                        mouseDown.Add(type, new List<Action>());
                    mouseDown[type].Add(callback);
                    break;
                case MouseListenerType.MousePress:
                    if (!mousePress.ContainsKey(type))
                        mousePress.Add(type, new List<Action>());
                    mousePress[type].Add(callback);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void RemoveMouseListener(Action callback, MouseListenerType type = MouseListenerType.MousePress)
        {
            switch (type)
            {
                case MouseListenerType.MouseDown:
                    if (mouseDown.ContainsKey(type))
                        mouseDown[type].Remove(callback);
                    if (mouseDown[type].Count == 0)
                        mouseDown.Remove(type);
                    break;
                case MouseListenerType.MousePress:
                    if (mousePress.ContainsKey(type))
                        mousePress[type].Remove(callback);
                    if (mousePress[type].Count == 0)
                        mousePress.Remove(type);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Adds a touch listener.
        /// </summary>
        /// <param name="action">The action which will be invoked if a touch has been performed. int = finger ID, float = preasure strengh, Vector2 = position</param>
        /// <param name="type">The type of the interaction.</param>
        public void AddTouchListener(Action<int, float, Vector2> action, TouchListenerType type = TouchListenerType.Press)
        {
            touchData[type].Add(action);
        }

        public void RemoveTouchListener(Action<int, float, Vector2> action, TouchListenerType type = TouchListenerType.Press)
        {
            if (touchData[type].Contains(action))
                touchData[type].Remove(action);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Remember last states and apply new ones.
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();
            LastGamePadState = GamePadState;
            GamePadState = GamePad.GetState(PlayerIndex.One);
            LastTouchPanelState = TouchPanelState;
            TouchPanelState = TouchPanel.GetState();

            foreach (Buttons currentButton in buttonDown.Keys)
                if (GamePadState.IsButtonDown(currentButton))
                    buttonDown[currentButton].ForEach(b => b.Invoke());

            foreach (Buttons currentButton in buttonPress.Keys)
                if (GamePadState.IsButtonUp(currentButton) &&
                    LastGamePadState.IsButtonDown(currentButton))
                    buttonPress[currentButton].ForEach(b => b.Invoke());

            if (IsLeftMouseDown && mouseDown.Count > 0)
                mouseDown[MouseListenerType.MouseDown].ForEach(m => m.Invoke());
            else if (LastMouseState.LeftButton == ButtonState.Pressed &&
                MouseState.LeftButton == ButtonState.Released && mousePress.Count > 0)
                mousePress[MouseListenerType.MousePress].ForEach(m => m.Invoke());

            foreach (TouchLocation currentTouch in TouchPanelState)
            {
                if (currentTouch.State == TouchLocationState.Moved &&
                    touchData[TouchListenerType.Move].Count > 0)
                    touchData[TouchListenerType.Move].ForEach(d => d.Invoke(currentTouch.Id, currentTouch.Pressure, currentTouch.Position));
                else if (currentTouch.State == TouchLocationState.Pressed &&
                    touchData[TouchListenerType.Press].Count > 0)
                    touchData[TouchListenerType.Press].ForEach(d => d.Invoke(currentTouch.Id, currentTouch.Pressure, currentTouch.Position));
                else if (currentTouch.State == TouchLocationState.Released &&
                    touchData[TouchListenerType.Release].Count > 0)
                    touchData[TouchListenerType.Release].ForEach(d => d.Invoke(currentTouch.Id, currentTouch.Pressure, currentTouch.Position));
            }

            if (blockCallback != null)
            {
                if (KeyPressPerformedKeys.Length > 0 || LastKeyboardState.GetPressedKeys().Length > 0)
                    blockCallback(KeyPressPerformedKeys, LastKeyboardState.GetPressedKeys());
                return;
            }

            foreach (Keys currentKey in KeyboardState.GetPressedKeys())
            {
                if (keyDown.ContainsKey(currentKey))
                    keyDown[currentKey].ForEach(c => c.Invoke());
            }

            foreach (Keys lastKey in LastKeyboardState.GetPressedKeys())
            {
                if (!KeyboardState.GetPressedKeys().Contains(lastKey))
                    if (keyPress.ContainsKey(lastKey))
                        keyPress[lastKey].ForEach(c => c.Invoke());
            }
        }
    }
}