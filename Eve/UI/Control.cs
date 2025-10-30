using Eve.Model;
using Eve.UI.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eve.UI
{
    /// <summary>
    /// Base UI Control class, implementing positioning and hierarchical logic.
    /// Rendering implementation is left to the inheriting classes
    /// </summary>
    public class Control : ICloneable
    {
        // Root viewport used for relative positioning when there are no parent controls
        // (positionment relative to the window size)
        public static Viewport RootViewport;
        public static GraphicsDevice MainGraphicsDevice;

        // Hierarchy
        public Control? Parent;
        public ObservableList<Control> Children = [];
        public string Name = "Control";

        public T WithChildren<T>(params Control[] children) where T : Control
        {
            foreach (var c in children) { c.Parent = this; }
            Children.AddRange(children);
            return (T)this;
        }

        public Control WithChildren(params Control[] children) => WithChildren<Control>(children);

        // Positioning
        public Observable<LayoutUnit> Position { get; set => field = GetLocalObservable(value.Value); }
        public Observable<LayoutUnit> Size { get; set => field = GetLocalObservable(value.Value); }
        public Observable<Vector2> Origin { get; set => field = GetLocalObservable(value.Value); }
        public Rectangle Bounds
        {
            get
            {
                var size = PixelSize;
                var pos = AbsolutePosition;
                return new(new((int)pos.X, (int)pos.Y), new((int)size.X, (int)size.Y));
            }
        }

        public Control WithPlacement(LayoutUnit position, LayoutUnit size)
        {
            Position = position;
            Size = size;
            return this;
        }

        protected Vector2 GetParentSize() => Parent?.PixelSize ?? RootViewport.Bounds.Size.ToVector2();

        public Vector2 PixelSize => Size.Value.Normalize(GetParentSize());
        // PixelPosition is relative to the parent's PixelPosition
        public Vector2 PixelPosition => Position.Value.Normalize(GetParentSize()) - PixelSize * Origin;
        // AbsolutePosition is relative to the RootViewport
        public Vector2 AbsolutePosition => (Parent?.AbsolutePosition ?? Vector2.Zero) + PixelPosition;

        // Rendering
        protected RenderTarget2D LocalRenderTarget;
        protected bool NeedsRedraw = true;

        protected virtual void DrawControl(SpriteBatch sb) { }
        protected virtual void DrawControlTop(SpriteBatch sb) { }
        public virtual RenderTarget2D GetRenderTarget(SpriteBatch sb)
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
            observable.Updated += ("UI_Redraw", _ => RequestRedraw());
            return observable;
        }

        // Update logic
        public List<ControlInputModule> InputModules { get; set; } = [];
        public T? FindInputModule<T>() where T : ControlInputModule
            => (T?)InputModules.Find(m => m is T);
        public virtual void HandleInputTunnelling(InputEvent @event) 
        {
            foreach (var module in InputModules) module.HandleTunnelling(this, @event);
        }
        public virtual void HandleInputBubbling(InputEvent @event)
        {
            foreach (var module in InputModules) module.HandleBubbling(this, @event);
        }

        public static void Initialize(GraphicsDevice gdev)
        {
            MainGraphicsDevice = gdev;
            RootViewport = gdev.Viewport;
            gdev.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }

        protected virtual void CloneBaseProperties(Control control, bool copyInputModules = true)
        {
            control.Name = Name;
            control.Position.Value = Position.Value;
            control.Size.Value = Size.Value;
            control.Origin.Value = Origin.Value;
            if(copyInputModules)
                control.InputModules = InputModules.Select(module => module.Clone() as ControlInputModule).ToList()!;

            control.WithChildren<Control>(
                Children.Select(child => child.Clone() as Control).ToArray()!
            );
        }
        protected virtual void CloneLocalProperties(Control control) { }

        /// <summary>
        /// Cloning a control copies all of its children and parents them properly,
        /// however it does *not* clone the parent.
        /// </summary>
        public virtual object Clone()
        {
            var control = new Control();
            CloneBaseProperties(control);
            CloneLocalProperties(control);

            return control;
        }

        public Control()
        {
            Name = "";
            Position = GetLocalObservable(LayoutUnit.Zero);
            Size = GetLocalObservable(LayoutUnit.Full);
            Origin = GetLocalObservable(Vector2.Zero);
        }

        public override string ToString() => Name;
    }
}
