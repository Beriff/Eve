using Eve.Model;
using Eve.UI.ControlModules.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.UI.Controls
{
    using ScrollPanelHooks = (Panel Background, VScrollbar scrollbar);
    public class ScrollPanelFactory : IControlAbstractFactory<ScrollPanelHooks>
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

                if (scrollArea.Children.Count == 0)
                    return;

                // Find content bounds
                Control topmost = scrollArea.Children.AsReadonly().MinBy(c => c.AbsolutePosition.Y)!;
                Control bottom = scrollArea.Children.AsReadonly().MaxBy(c => c.AbsolutePosition.Y + c.PixelSize.Y)!;

                float contentTop = topmost.AbsolutePosition.Y;
                float contentBottom = bottom.AbsolutePosition.Y + bottom.PixelSize.Y;
                float contentHeight = contentBottom - contentTop;
                float viewportHeight = areaViewport.Height;

                // avoid divide-by-zero when content fits fully
                if (contentHeight <= viewportHeight)
                {
                    vscroll.ThumbSize.Value = 1f;
                    vscroll.ThumbProgress.Value = 0f;
                    return;
                }

                // Set scrollbar thumb size relative to visible portion
                vscroll.ThumbSize.Value = viewportHeight / contentHeight;

                // Re-hook the scroll event
                vscroll.ThumbProgress.Updated -= "ScrollPanel_ThumbProgress";
                vscroll.ThumbProgress.Updated += ("ScrollPanel_ThumbProgress", _ =>
                {
                    // how much we can scroll
                    float scrollableHeight = contentHeight - viewportHeight;
                    // compute scroll offset
                    float offset = scrollableHeight * vscroll.ThumbProgress.Value;

                    // apply offset to children
                    foreach (var child in scrollArea.Children.AsReadonly())
                    {
                        child.Position.Value = child.Position.Value with
                        {
                            Absolute = new(0, -(offset - contentTop))
                        };
                    }
                });

            };
        }

        public CompositeControlBlueprint<ScrollPanelHooks> GetBlueprint()
        {
            // ScrollFrame structure:
            /*
                background -> scroll_area -> [user controls]
                           -> vscroll
                           -> hscroll
             */

            var compositeRoot = (Background.Clone() as Panel)!;
            var scrollArea = new Panel() 
            { PanelColor = Color.Transparent, Size = new LayoutUnit(1,1,-8,0), Name = "ScrollPanelArea" };

            var vscroll = (ScrollbarVertical.Clone() as VScrollbar)!;

            vscroll.Origin = new Vector2(1, 0);
            vscroll.Position = LayoutUnit.FromRel(1, 0);
            vscroll.Size = new LayoutUnit(0, 1, 8, 0);
            vscroll.Name = "ScrollPanelVScroll";

            compositeRoot.WithChildren(
                scrollArea, vscroll
            );

            var blueprint = new CompositeControlBlueprint<ScrollPanelHooks>
                (compositeRoot, c => (c.Children[0] as Panel, c.Children[1] as VScrollbar)!);

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
