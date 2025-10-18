using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Input
{
    public enum ButtonPressType
    {
        Raised,     // released for >1 consecutive frames
        Held,       // held for >1 consecutive frames
        Pressed,    // been held for 1 frame
        Released    // been released for 1 frame
    }
    public readonly record struct KeyPressInfo(Keys key, ButtonPressType pType)
    {
        public readonly Keys Key = key;
        public readonly ButtonPressType KeyPressType = pType;
    }
    public readonly record struct MouseInfo
        (Vector2 pos, Vector2 delta, ButtonPressType lmb, ButtonPressType rmb, ButtonPressType mmb)
    {
        public readonly Vector2 Position = pos;
        public readonly Vector2 Delta = delta;
        public readonly ButtonPressType LMBPressType = lmb;
        public readonly ButtonPressType RMBPressType = rmb;
        public readonly ButtonPressType MMBPressType = mmb;
    }

    public class InputEvent
    {
        public bool Consumed { get; set; } = false;
    }

    public class KeyboardInputEvent(KeyPressInfo[] info) : InputEvent
    {
        public KeyPressInfo[] KeyPressInfo { get; private set; } = info;
    }

    public class MouseInputEvent(MouseInfo info) : InputEvent
    {
        public MouseInfo MouseInfo { get; private set; } = info;
    }
}
