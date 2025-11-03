using Eve.Model;
using Eve.UI.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.ControlModules.Input
{
    public class ClickInputModule : ControlInputModule
    {

        public NamedEvent<MouseInfo> OnRightClick = new();
        public NamedEvent<MouseInfo> OnLeftClick = new();
        public NamedEvent<MouseInfo> OnMiddleClick = new();

        public bool ConsumeEvent { get; set; } = true;

        protected bool RMBClickedInside = false;
        protected bool LMBClickedInside = false;
        protected bool MMBClickedInside = false;

        protected bool TunnellingMode;

        public ClickInputModule
            (bool tunnel = false, Action<MouseInfo>? rmbHook = null, Action<MouseInfo>? lmbHook = null, Action<MouseInfo>? mmbHook = null) 
            : base(null,null)
        {
            TunnellingMode = tunnel;
            if (rmbHook != null) OnRightClick += ("DefaultHandler", rmbHook);
            if (lmbHook != null) OnLeftClick += ("DefaultHandler", lmbHook);
            if (mmbHook != null) OnMiddleClick += ("DefaultHandler", mmbHook);
        }

        public override void HandleTunnelling(Control self, InputEvent @event)
        {
            base.HandleTunnelling(self, @event);

            if (!TunnellingMode) return;

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

        public override void HandleBubbling(Control self, InputEvent @event)
        {
            base.HandleBubbling(self, @event);

            if (TunnellingMode) return;

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

        public override object Clone()
        {
            return new ClickInputModule(TunnellingMode);
        }
    }
}
