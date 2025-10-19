using Eve.Model;
using Eve.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Eve.UI
{
    /// <summary>
    /// Base UI Control class, implementing positioning and hierarchical logic.
    /// Rendering implementation is left to the inheriting classes
    /// </summary>
    public class Control
    {
        // Root viewport used for relative positioning when there are no parent controls
        // (positionment relative to the window size)
        public static Viewport RootViewport;
        public static GraphicsDevice MainGraphicsDevice;

        // Hierarchy
        public Control? Parent;
        public List<Control> Children = [];
        public string Name;

        public T WithChildren<T>(params Control[] children) where T : Control
        {
            foreach (var c in children) { c.Parent = this; }
            Children = [..children];
            return (T)this;
        }

        public Control WithChildren(params Control[] children) => WithChildren<Control>(children);

        // Positioning
        public LayoutUnit Position { get; set; } = LayoutUnit.Zero;
        public LayoutUnit Size { get; set; } = LayoutUnit.Full;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Rectangle Bounds
        {
            get
            {
                var size = PixelSize;
                var pos = AbsolutePosition;
                return new(new((int)pos.X, (int)pos.Y), new((int)size.X, (int)size.Y));
            }
        }

        protected Vector2 GetParentSize() => Parent?.PixelSize ?? RootViewport.Bounds.Size.ToVector2();

        public Vector2 PixelSize => Size.Normalize(GetParentSize());
        // PixelPosition is relative to the parent's PixelPosition
        public Vector2 PixelPosition => Position.Normalize(GetParentSize()) - PixelSize * Origin;
        // AbsolutePosition is relative to the RootViewport
        public Vector2 AbsolutePosition => (Parent?.AbsolutePosition ?? Vector2.Zero) + PixelPosition;

        // Rendering
        protected RenderTarget2D LocalRenderTarget;
        protected bool NeedsRedraw = true;

        protected virtual void DrawControl(SpriteBatch sb) { }
        protected virtual void DrawControlTop(SpriteBatch sb) { }
        public RenderTarget2D GetRenderTarget(SpriteBatch sb)
        {
            if(NeedsRedraw)
            {
                // create own texture container
                LocalRenderTarget = new(sb.GraphicsDevice, (int)PixelSize.X, (int)PixelSize.Y);
                // get (or generate) children textures
                var childTextures = Children.Select(x => x.GetRenderTarget(sb)).ToList();

                sb.GraphicsDevice.SetRenderTarget(LocalRenderTarget);
                sb.GraphicsDevice.Clear(Color.Transparent);

                sb.Begin();
                DrawControl(sb);
                sb.End();

                sb.Begin();
                for(int i = 0; i < childTextures.Count; i++)
                {
                    sb.Draw(childTextures[i], Children[i].PixelPosition, Color.White);
                }
                sb.End();

                sb.Begin();
                DrawControlTop(sb);
                sb.End();

                NeedsRedraw = false;
                return LocalRenderTarget;

            } else { return LocalRenderTarget; }
        }

        /// <summary>
        /// Sets <see cref="NeedsRedraw">NeedsRedraw</see> flag to <c>true</c>
        /// and propagates it upstream
        /// </summary>
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

        // Update logic
        public List<ControlInputModule> InputModules { get; set; } = [];
        public virtual void HandleInputTunneling(InputEvent @event) 
        {
            foreach (var module in InputModules) module.HandleTunneling(@event);
        }
        public virtual void HandleInputBubbling(InputEvent @event)
        {
            foreach (var module in InputModules) module.HandleBubbling(@event);
        }

        public static void Initialize(GraphicsDevice gdev)
        {
            MainGraphicsDevice = gdev;
            RootViewport = gdev.Viewport;
            gdev.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }
    }
}
