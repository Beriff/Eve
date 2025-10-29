using Eve.Model;
using Eve.UI.ControlModules.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Controls
{
    public class ScrollPanelFactory : IControlAbstractFactory
    {
        public Panel Background;
        public VScrollbar ScrollbarVertical;
        public HScrollbar ScrollbarHorizontal;

        public ScrollPanelFactory(UIGroup g, Panel? bg = null, VScrollbar? vs = null, HScrollbar? hs = null) 
        {
            Background = bg ?? new Panel() { PanelColor = Color.DarkGray };
            ScrollbarVertical = vs ?? new VScrollbar(g);
            ScrollbarHorizontal = hs ?? new HScrollbar(g);
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
            { PanelColor = Color.Yellow, Size = new LayoutUnit(1,1,-8,0) };

            var vscroll = ScrollbarVertical.Clone() as VScrollbar;

            vscroll.Origin = new Vector2(1, 0);
            vscroll.Position = LayoutUnit.FromRel(1, 0);
            vscroll.Size = new LayoutUnit(0, 1, 8, 0);
            vscroll.Name = "ScrollPanelVScroll";

            compositeRoot.WithChildren(
                scrollArea, vscroll
            );

            return new(compositeRoot);
        }
    }
}
