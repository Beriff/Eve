using Eve.UI.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI
{
    /// <summary>
    /// Modular unit attachable to any control. Extends default capabilities
    /// (ex. handling click logic)
    /// </summary>
    public abstract class ControlModule() : ICloneable 
    {
        public abstract object Clone();
    }

    public class ControlInputModule : ControlModule
    {
        protected Action<Control, InputEvent> TunnellingHandler = (_,_) => { };
        protected Action<Control, InputEvent> BubblingHandler = (_,_) => { };

        public virtual void HandleTunnelling(Control self, InputEvent @event) { TunnellingHandler(self, @event); }
        public virtual void HandleBubbling(Control self, InputEvent @event) { BubblingHandler(self, @event); }

        public ControlInputModule
            (Action<Control, InputEvent>? tunnelHandler = null, Action<Control, InputEvent>? bubbleHandler = null) 
        {
            TunnellingHandler = tunnelHandler ?? new((_,_) => { });
            BubblingHandler = bubbleHandler ?? new((_, _) => { });
        }

        public ControlInputModule() { }

        public override object Clone()
        {
            return new ControlInputModule()
            {
                TunnellingHandler = TunnellingHandler,
                BubblingHandler = BubblingHandler
            };
        }
    }
}
