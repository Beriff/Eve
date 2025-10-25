using Eve.Model;
using Eve.UI.ControlModules.Input;
using Eve.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
            Background = background ?? new Panel()
            { PanelColor = Color.Gray, Name = "ScrollbarBg" };

            Thumb = thumb ?? new Panel()
            { PanelColor = Color.LightGray, Name = "ScrollbarThumb" };

            Thumb.Value.InputModules.Add(new DragInputModule(g, DragInputModule.Axis.Vertical));

            // set the hierarchy
            WithChildren(Background);
            Background.Value.WithChildren(Thumb);

            // if the thumb's position is updated externally (not via ThumbProgress)
            // update ThumbProgress
            Thumb.Value.Position.Updated += v =>
            {
                var abs_size = Background.Value.PixelSize.Y - Thumb.Value.PixelSize.Y;

                ThumbProgress.Value = v.Absolute.Y / abs_size;
                //Thumb.Value.RequestRedraw();
                Thumb.Value.Size.Value = new LayoutUnit(0, ThumbSize, Thumb.Value.PixelSize.X, 0);
            };
            

            // make ThumbProgress and ThumbSize update the actual thumb
            ThumbProgress.Updated += v =>
            {
                v = Math.Clamp(v, 0, 1);
                Thumb.Value.Position.QuietSet(LayoutUnit.FromRel(0, v * (1 - ThumbSize)));
                Thumb.Value.RequestRedraw();
            };
            
            ThumbSize.Updated += v =>
            {
                Thumb.Value.Size.Value = new LayoutUnit(1, v, 0, 0);
            };

            Size.Updated += v =>
            {
                Thumb.Value.Size.Value = new LayoutUnit(1, ThumbSize, 0, 0);
            };

            ThumbProgress.Value = 0;
            ThumbSize.Value = 0.1f;
        }

        public override RenderTarget2D GetRenderTarget(SpriteBatch sb) => Background.Value.GetRenderTarget(sb);
        public override void HandleInputBubbling(InputEvent @event) => Background.Value.HandleInputBubbling(@event);
        public override void HandleInputTunnelling(InputEvent @event) => Background.Value.HandleInputTunnelling(@event);
    }

    public class HScrollbar : Control
    {
        public Observable<Panel> Background { get; set => field = GetLocalObservable(value.Value); }
        public Observable<Panel> Thumb { get; set => field = GetLocalObservable(value.Value); }

        public Observable<float> ThumbProgress { get; set => field = GetLocalObservable(value.Value); } = 0;
        public Observable<float> ThumbSize { get; set => field = GetLocalObservable(value.Value); } = 0.1f;

        public HScrollbar(UIGroup g, Panel? background = null, Panel? thumb = null)
        {
            Background = background ?? new Panel()
            { PanelColor = Color.Gray, Name = "ScrollbarBg" };

            Thumb = thumb ?? new Panel()
            { PanelColor = Color.LightGray, Name = "ScrollbarThumb" };

            Thumb.Value.InputModules.Add(new DragInputModule(g, DragInputModule.Axis.Horizontal));

            // set the hierarchy
            WithChildren(Background);
            Background.Value.WithChildren(Thumb);

            // if the thumb's position is updated externally (not via ThumbProgress)
            // update ThumbProgress
            Thumb.Value.Position.Updated += v =>
            {
                var abs_size = Background.Value.PixelSize.X - Thumb.Value.PixelSize.X;

                ThumbProgress.Value = v.Absolute.X / abs_size;
                //Thumb.Value.RequestRedraw();
                Thumb.Value.Size.Value = new LayoutUnit(ThumbSize, 0, 0, Thumb.Value.PixelSize.Y);
            };


            // make ThumbProgress and ThumbSize update the actual thumb
            ThumbProgress.Updated += v =>
            {
                v = Math.Clamp(v, 0, 1);
                Thumb.Value.Position.QuietSet(LayoutUnit.FromRel(v * (1 - ThumbSize), 0));
                Thumb.Value.RequestRedraw();
            };

            ThumbSize.Updated += v =>
            {
                Thumb.Value.Size.Value = new LayoutUnit(v, 1, 0, 0);
            };

            Size.Updated += v =>
            {
                Thumb.Value.Size.Value = new LayoutUnit(ThumbSize, 1, 0, 0);
            };

            ThumbProgress.Value = 0;
            ThumbSize.Value = 0.1f;
        }

        public override RenderTarget2D GetRenderTarget(SpriteBatch sb) => Background.Value.GetRenderTarget(sb);
        public override void HandleInputBubbling(InputEvent @event) => Background.Value.HandleInputBubbling(@event);
        public override void HandleInputTunnelling(InputEvent @event) => Background.Value.HandleInputTunnelling(@event);
    }
}
