using Eve.UI.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.ControlModules.Input
{
    public class ClickInputModule : ControlInputModule
    {
        public event Action<MouseInfo> OnRightClick = new(_ => { });
        public event Action<MouseInfo> OnLeftClick = new(_ => { });
        public event Action<MouseInfo> OnMiddleClick = new(_ => { });

        public bool ConsumeEvent { get; set; } = true;

        protected bool RMBClickedInside = false;
        protected bool LMBClickedInside = false;
        protected bool MMBClickedInside = false;

        public override void HandleBubbling(InputEvent @event)
        {
            if (@event is MouseInputEvent mEvent)
            {
                if (mEvent.MouseInfo.RMBPressType == ButtonPressType.Pressed) 
                {
                    RMBClickedInside = true;
                    @event.Consumed = ConsumeEvent;
                }
                else if (mEvent.MouseInfo.RMBPressType == ButtonPressType.Released && RMBClickedInside)
                {
                    RMBClickedInside = false;
                    OnRightClick.Invoke(mEvent.MouseInfo);
                }

                if (mEvent.MouseInfo.LMBPressType == ButtonPressType.Pressed)
                {
                    LMBClickedInside = true;
                    @event.Consumed = ConsumeEvent;
                }
                else if (mEvent.MouseInfo.LMBPressType == ButtonPressType.Released && LMBClickedInside)
                {
                    LMBClickedInside = false;
                    OnLeftClick.Invoke(mEvent.MouseInfo);
                }

                if (mEvent.MouseInfo.MMBPressType == ButtonPressType.Pressed)
                {
                    MMBClickedInside = true;
                    @event.Consumed = ConsumeEvent;
                }
                else if (mEvent.MouseInfo.MMBPressType == ButtonPressType.Released && MMBClickedInside)
                {
                    MMBClickedInside = false;
                    OnMiddleClick.Invoke(mEvent.MouseInfo);
                }
            }
        }
    }
}
