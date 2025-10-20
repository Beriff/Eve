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
    public abstract class ControlModule : ICloneable 
    {
        public abstract object Clone();
    }

    public class ControlInputModule : ControlModule
    {
        public virtual void HandleTunnelling(InputEvent @event) { }
        public virtual void HandleBubbling(InputEvent @event) { }

        public override object Clone()
        {
            return new ControlInputModule();
        }
    }
}
