using Eve.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Eve.UI
{
    public static class ControlResources
    {
        public static Texture2D UnitTexture;
        static ControlResources()
        {
            // generate unit texture (a single white pixel)
            UnitTexture = new(Control.MainGraphicsDevice, 1, 1);
            UnitTexture.SetData([Color.White]);
        }
    }

    public class Control
    {
        public static Viewport RootViewport;
        public static Stack<RenderTarget2D> RenderTargets = [];
        public static GraphicsDevice MainGraphicsDevice;
        public static RenderTarget2D CurrentRenderTarget => RenderTargets.Peek();

        // Hierarchy
        public Control? Parent;
        public List<Control> Children = [];

        // Positioning
        public LayoutUnit Position { get; set; } = LayoutUnit.Zero;
        public LayoutUnit Size { get; set; } = LayoutUnit.Full;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Rectangle Bounds
        {
            get
            {
                var size = PixelSize;
                var pos = PixelPosition;
                return new(new((int)pos.X, (int)pos.Y), new((int)size.X, (int)size.Y));
            }
        }

        protected Vector2 GetParentSize() => Parent?.PixelSize ?? RootViewport.Bounds.Size.ToVector2();
        protected Vector2 GetRelativePosition(Control? relTo)
        {
            return PixelPosition - (relTo?.PixelPosition ?? Vector2.Zero);
        }
        protected Vector2 ParentRelativePosition { get => GetRelativePosition(Parent); }

        public Vector2 PixelSize => Size.Normalize(GetParentSize());
        public Vector2 PixelPosition => Position.Normalize(GetParentSize()) - PixelSize * Origin;

        // Rendering
        protected RenderTarget2D LocalRenderTarget;
        protected bool NeedsRedraw = true;

        protected virtual void DrawControl(SpriteBatch sb) { }
        protected virtual void DrawControlTop(SpriteBatch sb) { }
        public RenderTarget2D GetRenderTarget(SpriteBatch sb)
        {
            if(NeedsRedraw)
            {
                // generate new texture
                LocalRenderTarget = new(sb.GraphicsDevice, (int)PixelSize.X, (int)PixelSize.Y);

                sb.GraphicsDevice.SetRenderTarget(LocalRenderTarget);
                sb.GraphicsDevice.Clear(Color.Black);

                sb.Begin();
                DrawControl(sb);
                foreach (var child in Children)
                {
                    sb.Draw(child.GetRenderTarget(sb), child.ParentRelativePosition, Color.White);
                }
                DrawControlTop(sb);
                sb.End();

                NeedsRedraw = false;
                sb.GraphicsDevice.SetRenderTarget(null);
                return LocalRenderTarget;

            } else { return LocalRenderTarget; }
        }

        public void RequestRedraw()
        {
            NeedsRedraw = true;
            Parent?.RequestRedraw();
        }

        /// <summary>
        /// Returns an observable instance that requests current control's redraw when updated
        /// </summary>
        public Observable<T> GetLocalObservable<T>(T value)
        {
            var observable = new Observable<T>(value);
            observable.Updated += _ => RequestRedraw();
            return observable;
        }

        public static void Initialize(GraphicsDevice gdev)
        {
            MainGraphicsDevice = gdev;
            RootViewport = gdev.Viewport;
            gdev.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }
    }
}
