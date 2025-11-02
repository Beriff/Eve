using Eve.Model;
using Eve.UI.ControlModules.Input;
using Eve.UI.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.UI.Controls
{
    using ScrollPanelHooks = (Panel Background, VScrollbar scrollbar);

    /// <summary>
    /// Creates a new composite control blueprint, representing a 
    /// scroll area and a scrollbar that dynamically repositions the children.
    /// </summary>
    /// <remarks>
    /// The scrolling functions properly only if the first children is positioning
    /// at (x,y) = 0 relative to the scroll area
    /// </remarks>
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

                if (contentHeight <= viewportHeight)
                {
                    vscroll.ThumbSize.Value = 1f;
                    vscroll.ThumbProgress.Value = 0f;
                    return;
                }

                // Thumb size = visible area ratio
                vscroll.ThumbSize.Value = viewportHeight / contentHeight;

                // Cache initial positions
                var originalPositions = scrollArea.Children.AsReadonly()
                    .ToDictionary(c => c, c => c.Position.Value.Absolute.Y);

                // Detach any previous handler
                vscroll.ThumbProgress.Updated -= "ScrollPanel_ThumbProgress";

                // Attach fresh handler
                vscroll.ThumbProgress.Updated += ("ScrollPanel_ThumbProgress", _ =>
                {
                    float scrollableHeight = contentHeight - viewportHeight;
                    float offset = scrollableHeight * vscroll.ThumbProgress.Value;

                    foreach (var child in scrollArea.Children.AsReadonly())
                    {
                        float originalY = originalPositions[child];
                        child.Position.Value = child.Position.Value with
                        {
                            Absolute = new(child.Position.Value.Absolute.X, originalY - offset)
                        };
                    }
                }
                );


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
            compositeRoot.Name = "ScrollPanelRoot";
            var scrollArea = new Panel() 
            { PanelColor = Color.Transparent, Size = new LayoutUnit(1,1,-8,0), Name = "ScrollPanelArea" };

            var vscroll = (ScrollbarVertical.Clone() as VScrollbar)!;

            vscroll.Origin = new Vector2(1, 0);
            vscroll.Position = LayoutUnit.FromRel(1, 0);
            vscroll.Size = new LayoutUnit(0, 1, 8, 0);
            vscroll.Name = "ScrollPanelVScroll";

            // attach a conttrolInputModule that redirects mouse scroll (at scroll_area) to scrollbar
            scrollArea.InputModules.Add(
                new ControlInputModule(tunnelHandler: (self, @event) =>
                {
                    if(@event is MouseInputEvent mEvent)
                    {
                        if(mEvent.MouseInfo.ScrollDelta != 0) 
                        {
                            (self.Parent.Children[1] as VScrollbar).ThumbProgress.Value 
                                -= MathF.Sign(mEvent.MouseInfo.ScrollDelta) / 25f;
                        }
                    }
                })
            );

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
