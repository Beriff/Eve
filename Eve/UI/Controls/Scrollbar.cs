using Eve.Model;
using Eve.UI.ControlModules.Input;
using Eve.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.UI.Controls
{
    public class VScrollbar : Control
    {
        public Observable<Panel> Background { get; set => field = GetLocalObservable(value.Value); }
        public Observable<Panel> Thumb { get; set => field = GetLocalObservable(value.Value); }

        public Observable<float> ThumbProgress { get; set => field = GetLocalObservable(value.Value); } = 0;
        public Observable<float> ThumbSize { get; set => field = GetLocalObservable(value.Value); } = 0.1f;

        public VScrollbar(UIGroup g, Panel? background = null, Panel? thumb = null)
        {
            Background = background?.Clone() as Panel ?? new Panel()
            { PanelColor = Color.Gray, Name = "ScrollbarBg" };
            Background.Value.Children.Clear();

            Thumb = thumb?.Clone() as Panel ?? new Panel()
            { PanelColor = Color.LightGray, Name = "ScrollbarThumb" };

            // Clear DragInputModule (if it exists)
            // since only one instance is allowed per control
            if (Thumb.Value.FindInputModule<DragInputModule>() != null)
                Thumb.Value.InputModules.Remove(Thumb.Value.FindInputModule<DragInputModule>()!);

            Thumb.Value.InputModules.Add(new DragInputModule(g, DragInputModule.Axis.Vertical));

            // set the hierarchy
            WithChildren(Background);
            Background.Value.WithChildren(Thumb);

            // if the thumb's position is updated externally (not via ThumbProgress)
            // update ThumbProgress
            Thumb.Value.Position.Updated += ("Scrollbar_Thumb", v =>
            {
                var abs_size = Background.Value.PixelSize.Y - Thumb.Value.PixelSize.Y;

                ThumbProgress.Value = v.Absolute.Y / abs_size;
                //Thumb.Value.RequestRedraw();
                Thumb.Value.Size.Value = new LayoutUnit(0, ThumbSize, Thumb.Value.PixelSize.X, 0);
            }
            );
            

            // make ThumbProgress and ThumbSize update the actual thumb
            ThumbProgress.Updated += ("Scrollbar_ThumbProgress", v =>
            {
                v = Math.Clamp(v, 0, 1);
                Thumb.Value.Position.QuietSet(LayoutUnit.FromRel(0, v * (1 - ThumbSize)));
                Thumb.Value.RequestRedraw();
            });
            
            ThumbSize.Updated += ("Scrollbar_ThumbSize", v =>
            {
                Thumb.Value.Size.Value = new LayoutUnit(1, v, 0, 0);
            });

            Size.Updated += ("Scrollbar_Size", v =>
            {
                Thumb.Value.Size.Value = new LayoutUnit(1, ThumbSize, 0, 0);
            });

            ThumbProgress.Value = 0;
            ThumbSize.Value = 0.1f;
        }

        public override RenderTarget2D GetRenderTarget(SpriteBatch sb) => Background.Value.GetRenderTarget(sb);
        public override void HandleInputBubbling(InputEvent @event) => Background.Value.HandleInputBubbling(@event);
        public override void HandleInputTunnelling(InputEvent @event) => Background.Value.HandleInputTunnelling(@event);

        public override object Clone()
        {
            var scrollbar = new VScrollbar(
                Thumb.Value.FindInputModule<DragInputModule>()!.ParentGroup,
                Background.Value, Thumb.Value
                );

            scrollbar.Name = Name;
            scrollbar.Position.Value = Position.Value;
            scrollbar.Size.Value = Size.Value;
            scrollbar.Origin.Value = Origin.Value;

            return scrollbar;
        }
    }

}
