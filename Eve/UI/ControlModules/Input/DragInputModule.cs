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

        // optionally restricts dragging to a single axis (null = no restriction)
        public Axis? AxisRestriction { get; set; } = axisRestriction;

        public override void HandleBubbling(Control self, InputEvent @event)
        {
            if (@event is MouseInputEvent mEvent)
            {
                if (mEvent.MouseInfo.LMBPressType == ButtonPressType.Pressed)
                {
                    Grabbed = true;
                    // make modal to intercept LMB raise from anywhere
                    ParentGroup.SetModal(self);
                    @event.Consumed = true;
                }
                else if (mEvent.MouseInfo.LMBPressType == ButtonPressType.Released)
                {
                    Grabbed = false;
                    ParentGroup.RemoveModal(self);
                    @event.Consumed = true;
                }

                // handle dragging
                else
                {
                    if (!Grabbed) return;
                    var delta = mEvent.MouseInfo.Delta;
                    if (AxisRestriction == Axis.Vertical) { delta.X = 0; }
                    else if (AxisRestriction == Axis.Horizontal) { delta.Y = 0; }

                    self.Position.Value = self.Position.Value with { Absolute = self.Position.Value.Absolute + delta };
                }
            }
        }

        public override object Clone()
        {
            return new DragInputModule(ParentGroup, AxisRestriction);
        }
    }
}
