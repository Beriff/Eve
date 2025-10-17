using Eve.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI
{
    public class UIGroup
    {
        public Control Root { get; set; }

        public void operator+=(Control c)
        {
            Root = c;
        }

        public void Render(SpriteBatch sb)
        {
            var texture = Root.GetRenderTarget(sb);

            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.Begin();

            sb.Draw(texture, Vector2.Zero, Color.White);

            sb.End();
        }
    }
}
