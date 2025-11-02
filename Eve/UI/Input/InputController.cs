using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Eve.UI.Input
{

    public class InputController
    {
        public event Action<KeyboardInputEvent> KeyEvent = new(_ => { });
        public event Action<MouseInputEvent> MouseEvent = new(_ => { });

        protected KeyboardState PreviousKeyboardState;
        protected MouseState PreviousMouseState;
        protected KeyboardState CurrentKeyboardState;
        protected MouseState CurrentMouseState;

        public void Update()
        {
            ButtonPressType dispatchPressType(bool previous, bool current)
            {
                return previous ?
                        current ? ButtonPressType.Held : ButtonPressType.Released :
                        current ? ButtonPressType.Pressed : ButtonPressType.Raised;
            }

            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();

            // generate a map of mouse buttons
            MouseInfo mouseInputInfo = new(
                CurrentMouseState.Position.ToVector2(),
                (CurrentMouseState.Position - PreviousMouseState.Position).ToVector2(),
                CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue,
                dispatchPressType(
                    PreviousMouseState.LeftButton == ButtonState.Pressed, 
                    CurrentMouseState.LeftButton == ButtonState.Pressed
                    ),
                dispatchPressType(
                    PreviousMouseState.RightButton == ButtonState.Pressed, 
                    CurrentMouseState.RightButton == ButtonState.Pressed
                    ),
                dispatchPressType(
                    PreviousMouseState.MiddleButton == ButtonState.Pressed, 
                    CurrentMouseState.MiddleButton == ButtonState.Pressed
                    )
            );

            // generate a map of keyboard keys
            List<KeyPressInfo> keyPresses = [];
            foreach (var key in Enum.GetValues<Keys>())
            {
                bool hasPrev = PreviousKeyboardState.GetPressedKeys().Contains(key);
                bool hasCurr = CurrentKeyboardState.GetPressedKeys().Contains(key);

                keyPresses.Add(new( key, dispatchPressType(hasPrev, hasCurr) ));
            }

            // we always fire the keyEvent, since even a raised key is still an event
            KeyEvent.Invoke(new([..keyPresses]));
            // same goes for mouse events
            MouseEvent.Invoke(new(mouseInputInfo));
            //Console.WriteLine(mouseInputInfo.LMBPressType);
        }
    }
}
