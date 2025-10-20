using Eve.Model;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.UI.Controls
{
    public enum TextAlignment
    {
        Start, Middle, End
    }
    public class Label : Control
    {
        public Observable<string> Text { get; set => field = GetLocalObservable(value.Value); } = "";
        public Observable<Color> TextColor { get; set => field = GetLocalObservable(value.Value); } = Color.Black;
        public Observable<int> FontSize { get; set => field = GetLocalObservable(value.Value); } = 18;

        public Observable<TextAlignment> HorizontalAlignment
            { get; set => field = GetLocalObservable(value.Value); } = TextAlignment.Middle;

        public Observable<TextAlignment> VerticalAlignment
            { get; set => field = GetLocalObservable(value.Value); } = TextAlignment.Middle;

        protected override void DrawControl(SpriteBatch sb)
        {
            Vector2 string_measure = Theme.GetFont(FontSize).MeasureString(Text);
            Vector2 pos = HorizontalAlignment.Value switch
            {
                TextAlignment.Start => Vector2.Zero,
                TextAlignment.Middle => PixelSize / 2 - (string_measure / 2),
                TextAlignment.End => PixelSize - string_measure,
                _ => Vector2.Zero
            };
            pos = pos.ToPoint().ToVector2(); // convert values to int
            sb.DrawString(Theme.GetFont(FontSize), Text, pos, TextColor);
        }

        protected override void CloneLocalProperties(Control control)
        {
            Label label = (control as Label)!;
            label.Text = Text;
            label.TextColor = TextColor;
            label.FontSize = FontSize;
            label.HorizontalAlignment = HorizontalAlignment;
            label.VerticalAlignment = VerticalAlignment;
        }

        public override object Clone()
        {
            var label = new Label();
            CloneBaseProperties(label);
            CloneLocalProperties(label);

            return label;
        }
    }
}
