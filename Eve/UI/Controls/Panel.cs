using Eve.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Controls
{
    public class Panel : Control
    {
        public Observable<Color> PanelColor { get => field; set => field = GetLocalObservable(value.Value); } = Color.White;
        protected override void DrawControl(SpriteBatch sb)
        {
            sb.Draw(
                ControlResources.UnitTexture, 
                new Rectangle(0, 0, (int)PixelSize.X, (int)PixelSize.Y), null, 
                PanelColor
                );
        }

        protected override void CloneLocalProperties(Control control)
        {
            (control as Panel)!.PanelColor = PanelColor;
        }

        public override object Clone()
        {
            var panel = new Panel();
            CloneBaseProperties(panel);
            CloneLocalProperties(panel);

            return panel;
        }
    }
}
