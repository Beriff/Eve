using Eve.Model;
using Eve.UI.ControlModules.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.UI.Controls
{
    public class ScrollPanelFactory : IControlAbstractFactory
    {
        public Panel Background;
        public VScrollbar ScrollbarVertical;

        public ScrollPanelFactory(UIGroup g, Panel? bg = null, VScrollbar? vs = null) 
        {
            Background = bg ?? new Panel() { PanelColor = Color.DarkGray };
            ScrollbarVertical = vs ?? new VScrollbar(g);
        }

        protected Action GetInnerBoundsRecalculation(Control scrollPanelEntry)
        {
            // invoked by scroll_area.Children.Updated
            return () =>
            {
                Control scrollArea = scrollPanelEntry.Children[0];
                VScrollbar vscroll = (scrollPanelEntry.Children[1] as VScrollbar)!;
                Rectangle areaViewport = scrollArea.Bounds;

                if (scrollArea.Children.Count == 0) return;

                float topmost = scrollArea.Children.AsReadonly().Min(c => c.AbsolutePosition.Y);
                float bottom = scrollArea.Children.AsReadonly().Max(c => c.AbsolutePosition.Y);
                float scrollable_vert_area = bottom - topmost - areaViewport.Height;
                float thumbProg = areaViewport.X / scrollable_vert_area;

                vscroll.ThumbProgress.Updated -= "ScrollPanel_ThumbProgress";
                vscroll.ThumbProgress.Updated += ("ScrollPanel_ThumbProgress", _ =>
                {
                    float offset = (bottom - topmost) * vscroll.ThumbProgress.Value;
                    // offset each child of scroll_area by a calculated amount
                    foreach (var child in scrollArea.Children.AsReadonly())
                    {
                        child.Position.Value = child.Position.Value with { Absolute = new(0, offset) };
                    }
                });
            };
        }

        public CompositeControlBlueprint GetBlueprint()
        {
            // ScrollFrame structure:
            /*
                background -> scroll_area -> [user controls]
                           -> vscroll
                           -> hscroll
             */

            var compositeRoot = Background.Clone() as Panel;
            var scrollArea = new Panel() 
            { PanelColor = Color.Transparent, Size = new LayoutUnit(1,1,-8,0) };

            var vscroll = ScrollbarVertical.Clone() as VScrollbar;

            vscroll.Origin = new Vector2(1, 0);
            vscroll.Position = LayoutUnit.FromRel(1, 0);
            vscroll.Size = new LayoutUnit(0, 1, 8, 0);
            vscroll.Name = "ScrollPanelVScroll";

            compositeRoot.WithChildren(
                scrollArea, vscroll
            );

            var blueprint = new CompositeControlBlueprint(compositeRoot);
            blueprint.Instantiated += (root) =>
            {
                var e = GetInnerBoundsRecalculation(root);

                root.Children[0].Children.Updated += ("ScrollPanel_BoundRecalc", e); 
                root.Size.Updated += ("ScrollPanel_BoundRecalc", _ => e()); // recalculate dragging logic when the scrollPanel size changes too
            };

            return blueprint;
        }
    }
}
