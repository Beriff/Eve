using Eve.UI.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Eve.UI.ControlModules.Input.DragInputModule;

namespace Eve.UI.ControlModules.Input
{
    public class DragInputModule(UIGroup group, Axis? axisRestriction = null) : ControlInputModule
    {
        public enum Axis { Vertical, Horizontal }

        public bool Grabbed { get; set; }
        public bool ConsumeEvent { get; set; } = true;

        protected UIGroup ParentGroup = group;
        protected Vector2 GrabOffset;

        // optionally restricts dragging to a single axis (null = no restriction)
        public Axis? AxisRestriction { get; set; } = axisRestriction;

        public override void HandleBubbling(Control self, InputEvent @event)
        {
            if (@event is MouseInputEvent mEvent)
            {
                if (mEvent.MouseInfo.LMBPressType == ButtonPressType.Pressed)
                {
                    Grabbed = true;
                    ParentGroup.SetModal(self);

                    // Store offset between mouse and control top-left
                    GrabOffset = mEvent.MouseInfo.Position - self.AbsolutePosition;

                    @event.Consumed = true;
                }
                else if (mEvent.MouseInfo.LMBPressType == ButtonPressType.Released)
                {
                    Grabbed = false;
                    ParentGroup.RemoveModal(self);
                    @event.Consumed = true;
                }
                else if (Grabbed)
                {
                    var mousePos = mEvent.MouseInfo.Position;

                    // Compute new position so that the offset is preserved
                    var newPos = mousePos - GrabOffset;

                    if (AxisRestriction == Axis.Vertical) newPos.X = self.Position.Value.Absolute.X;
                    else if (AxisRestriction == Axis.Horizontal) newPos.Y = self.Position.Value.Absolute.Y;

                    self.Position.Value = self.Position.Value with { Absolute = newPos };
                    @event.Consumed = true;
                }
            }
        }

        public override object Clone()
        {
            return new DragInputModule(ParentGroup, AxisRestriction);
        }
    }
}
