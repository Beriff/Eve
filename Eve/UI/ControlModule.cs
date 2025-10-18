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
    public class ControlModule { }

    public class ControlInputModule : ControlModule
    {
        public virtual void HandleTunneling(InputEvent @event) { }
        public virtual void HandleBubbling(InputEvent @event) { }
    }
}
