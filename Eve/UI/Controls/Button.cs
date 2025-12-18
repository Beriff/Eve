using Eve.Model;
using Eve.UI.ControlModules.Input;
using System;
using System.Collections.Generic;
using System.Text;



namespace Eve.UI.Controls
{
    using ButtonHandles = (Panel Background, Label TextLabel);
    public class ButtonFactory : IControlAbstractFactory<ButtonHandles>
    {
        public Panel Backdrop;

        public static readonly CompositeControlBlueprint<ButtonHandles> SimpleButton = new ButtonFactory(new()).GetBlueprint();

        public CompositeControlBlueprint<ButtonHandles> GetBlueprint()
        {
            var composite_root = ((Panel)Backdrop.Clone()).WithChildren(
                new Label() { Size = LayoutUnit.Full, Name = "ButtonLabel", Text = "Button" }
            );

            // enable the button to capture the input and prevent it from
            // propagating to button's parents (tunnel handling)
            composite_root!.InputModules.Add(new ClickInputModule(tunnel: true));

            CompositeControlBlueprint<ButtonHandles> blueprint =
                new(composite_root, c => (c as Panel, c.Children[0] as Label)!);
            blueprint.Instantiated += root =>
            {
                // button tints on hover
                root.OnMouseEnter += ("sfg", () => {  });
                root.OnMouseLeave += ("sfghh", () => {  });
            };

            return blueprint;
        }

        public ButtonFactory(Panel panel)
        {
            Backdrop = panel;
        }
    }
}
