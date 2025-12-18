using Eve.Model;
using Eve.UI.ControlModules.Input;
using Eve.UI.Effects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;



namespace Eve.UI.Controls
{
    using ButtonHandles = (Panel Background, Label TextLabel);
    public class ButtonFactory : IControlAbstractFactory<ButtonHandles>
    {
        public Panel Backdrop;
        protected EffectGroup EffectGroup;

        public static CompositeControlBlueprint<ButtonHandles> SimpleButtonFactory(EffectGroup eg, Panel? p = null) => new ButtonFactory(eg, p ?? new()).GetBlueprint();

        protected TimedEffect ButtonMEnterEffect(Panel button_root)
        {
            var initialColor = button_root.PanelColor.Value;
            var lightenedColor = new Color(initialColor.ToVector3() * 1.3f);

            return new((effect) =>
            {
                button_root.PanelColor.Value = Tween.Lerp(initialColor, lightenedColor, effect.Progress);
            }, lifespan: .1f, name: $"{GetHashCode()}_btnOnHover", behavior: EffectEnqueuing.Replace);
        }

        protected TimedEffect ButtonMLeaveEffect(Panel button_root)
        {
            var initialColor = button_root.PanelColor.Value;
            var lightenedColor = new Color(initialColor.ToVector4() / 1.3f);

            return new((effect) =>
            {
                button_root.PanelColor.Value = Tween.Lerp(initialColor, lightenedColor, effect.Progress);
            }, lifespan: .1f, name: $"{GetHashCode()}_btnOnHover", behavior: EffectEnqueuing.Replace);
        }

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
                // create button tint effects and hook them to the hover events
                root.OnMouseEnter += ("buttonOnMouseEnter", () => {
                    EffectGroup.Add(ButtonMEnterEffect(root as Panel)); 
                });
                root.OnMouseLeave += ("buttonOnMouseLeave", () => { 
                    EffectGroup.Add(ButtonMLeaveEffect(root as Panel)); 
                });
            };

            return blueprint;
        }

        public ButtonFactory(EffectGroup eg, Panel panel)
        {
            Backdrop = panel;
            EffectGroup = eg;
        }
    }
}
