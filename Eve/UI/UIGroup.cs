using Eve.UI.Controls;
using Eve.UI.Effects;
using Eve.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.UI
{
    public class UIGroup
    {
        public Control Root { get; set; }
        public InputController InputController { get; set; }
        public List<Control> ModalControls { get; set; } = [];
        public EffectGroup EffectGroup { get; set; } = new();

        protected List<Control> PreviousPath = [];
        public void operator+=(Control c)
        {
            Root = c;
        }

        public void operator += (TimedEffect e)
        {
            EffectGroup.Add(e);
        }

        public void Update(float dt)
        {
            InputController.Update();
            EffectGroup.Update(dt);
        }

        public void Render(SpriteBatch sb)
        {
            var texture = Root.GetRenderTarget(sb);

            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.Begin();

            sb.Draw(texture, Root.AbsolutePosition, Color.White);

            sb.End();
        }

        public void SetModal(Control control) { ModalControls.Add(control); }
        public void RemoveModal(Control control) { ModalControls.Remove(control); }

        protected void InputHandling(MouseInputEvent @event)
        {
            // update modal controls first
            // even if the event gets consumed, we must update all the modals
            for (int i = 0; i < ModalControls.Count; i++) ModalControls[i].HandleInputBubbling(@event);
            if (@event.Consumed) return;

            var mPos = @event.MouseInfo.Position;
            List<Control> path = [];
            Control currentControl = Root!;

            // do not fire any events if the mouse is outside the root control
            if (!currentControl.Bounds.Contains(mPos)) { goto End_Input; };

            while (true)
            {
            Continue_While_Loop:
                path.Add(currentControl);
                if (currentControl.Children.Count == 0) break; // leaf found
                foreach (var child in currentControl.Children)
                {
                    if (child.Bounds.Contains(mPos)) { currentControl = child; goto Continue_While_Loop; }
                }
                break; // the click wasnt inside any of the children
            }

            if (path.Count == 0) goto End_Input;

            // tunneling phase (root -> leaf)
            foreach (var child in path)
            {
                child.HandleInputTunnelling(@event);
                if (@event.Consumed) goto End_Input;
            }

            // bubbling phase (leaf -> root)
            foreach (var child in path.Reverse<Control>())
            {
                child.HandleInputBubbling(@event);
                if (@event.Consumed) goto End_Input;
            }

        End_Input:
            // before propagating the event down the calculated path
            // invoke all the hover events based on the
            // path difference
            // (suboptimal implementation)
            var mouseLeft = PreviousPath.Where(c => !path.Contains(c));
            var mouseEntered = path.Where(c => !PreviousPath.Contains(c));

            foreach (var child in mouseLeft) child.OnMouseLeave.Invoke();
            foreach (var child in mouseEntered) child.OnMouseEnter.Invoke();

            PreviousPath = path;
            return;
        }

        public UIGroup()
        {
            InputController = new InputController();
            InputController.MouseEvent += InputHandling;
        }
    }
}
