using Eve.UI.Controls;
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

        public void operator+=(Control c)
        {
            Root = c;
        }

        public void Update()
        {
            InputController.Update();
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

        public UIGroup()
        {
            InputController = new InputController();
            InputController.MouseEvent += (@event) =>
            {
                // update modal controls first
                // even if the event gets consumed, we must update all the modals
                for(int i = 0; i < ModalControls.Count; i++) ModalControls[i].HandleInputBubbling(@event);
                if (@event.Consumed) return;

                var mPos = @event.MouseInfo.Position;
                List<Control> path = [];
                Control currentControl = Root!;

                // do not fire any events if the mouse is outside the root control
                if (!currentControl.Bounds.Contains(mPos)) return;

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
                if (path.Count == 0) return;
                
                // tunneling phase (root -> leaf)
                foreach(var child in path)
                {
                    child.HandleInputTunnelling(@event);
                    if (@event.Consumed) return;
                }

                // bubbling phase (leaf -> root)
                foreach(var child in path.Reverse<Control>())
                {
                    child.HandleInputBubbling(@event);
                    if (@event.Consumed) return;
                }
            };
        }
    }
}
