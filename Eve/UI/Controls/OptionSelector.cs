using Eve.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Controls
{
    
    using ButtonHandles = (Panel Background, Label TextLabel);
    using OptionSelectorHandles = (Panel Background, Label TextLabel);

    /// <summary>
    /// A control that is able to show multi-leveled buttons, ordered vertically
    /// (what is traditionally called a combobox or menustrip), forwarding button presses
    /// to a single event, providing a string parameter which identifies the button pressed
    /// </summary>
    public class OptionSelectorFactory : IControlAbstractFactory<OptionSelectorHandles>
    {
        public CompositeControlBlueprint<ButtonHandles> ButtonBlueprint;
        public TreeNode<string> Options;

        //protected 

        public CompositeControlBlueprint<OptionSelectorHandles> GetBlueprint()
        {
            var (root, handles) = ButtonBlueprint.GetHookedInstance();
            throw new NotImplementedException();
        }
    }
}
