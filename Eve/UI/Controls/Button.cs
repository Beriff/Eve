using Eve.Model;
using Eve.UI.ControlModules.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Controls
{
    public class ButtonFactory : IControlAbstractFactory
    {
        public Panel Backdrop;

        public static readonly CompositeControlBlueprint SimpleButton = new ButtonFactory(new()).GetBlueprint();

        public CompositeControlBlueprint GetBlueprint()
        {
            var composite_root = ((Panel)Backdrop.Clone()).WithChildren(
                new Label() { Size = LayoutUnit.Full, Name = "ButtonLabel", Text = "Button" }
            );

            // enable the button to capture the input and prevent it from
            // propagating to button's parents (tunnel handling)
            composite_root!.InputModules.Add(new ClickInputModule(tunnel: true));


            return new(composite_root);
        }

        public ButtonFactory(Panel panel)
        {
            Backdrop = panel;
        }
    }
}
