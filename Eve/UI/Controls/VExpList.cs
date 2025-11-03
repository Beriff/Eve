using Eve.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.UI.Controls
{
    public class VExpList : Control
    {
        public Observable<Cardinal> GrowthDirection = Cardinal.Down;
        public Observable<float> Padding = 2;

        public VExpList()
        {
            // i've worked 7 years at blizzard
            GrowthDirection.Updated += ("VExpList_ChildrenUpdated", _ => { RepositionChildren(); RecalculateSize(); });
            Padding.Updated += ("VExpList_ChildrenUpdated", _ => { RepositionChildren(); RecalculateSize(); });
            Children.Updated += ("VExpList_ChildrenUpdated", () => { RepositionChildren(); RecalculateSize(); });
        }

        protected void RecalculateSize()
        {
            float border;

            if(GrowthDirection.Value == Cardinal.Down) 
            {
                border = Children.Max(c => c.AbsolutePosition.Y + c.PixelSize.Y)!; 
            } else if (GrowthDirection.Value == Cardinal.Up)
            {
                border = Children.Min(c => c.AbsolutePosition.Y)!;
            } else { throw new UIException("VExpList only supports Up/Down growth directions"); }

            float newSize = MathF.Abs(AbsolutePosition.Y - border);
            Size.Value = Size.Value with { Absolute = new(Size.Value.Absolute.X, newSize) };
        }

        protected void RepositionChildren()
        {
            float accumulated_offset = 0f;
            float sign = GrowthDirection.Value == Cardinal.Down ? 1f : -1f;
            foreach (var child in Children)
            {
                child.Position.Value = LayoutUnit.FromAbs(child.Position.Value.Absolute.X, accumulated_offset * sign);
                accumulated_offset += child.PixelSize.Y + Padding.Value;
            }
        }

        protected override void CloneLocalProperties(Control control)
        {
            var explist = (control as VExpList)!;
            explist.Padding.Value = Padding.Value;
            explist.GrowthDirection.Value = GrowthDirection.Value;
        }

        public override object Clone()
        {
            var list = new VExpList();
            CloneBaseProperties(list);
            CloneLocalProperties(list);

            return list;
        }
    }
}
