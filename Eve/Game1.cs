using Eve.Model;
using Eve.UI;
using Eve.UI.ControlModules.Input;
using Eve.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Eve
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public UIGroup Group;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Control.Initialize(GraphicsDevice);
            Theme.Initialize(Content);

            Group = new();

            Group += new HScrollbar(Group, thumb: new TilePanel() { BorderRadius = 5 })
            { Position = LayoutUnit.FromAbs(20), Size = LayoutUnit.FromAbs(150, 8) };

            /*Group += new Panel()
            { PanelColor = Color.Black, Position = LayoutUnit.FromAbs(20), Size = LayoutUnit.FromAbs(8, 150) }.WithChildren(
                new Panel() { Size = new LayoutUnit(1,0, 0, 5), InputModules = [new DragInputModule(Group, DragInputModule.Axis.Vertical)] }
            );*/

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Group.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Group.Render(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
