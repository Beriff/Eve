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

            Group += new VScrollbar(Group, thumb: new TilePanel() { BorderRadius = 5 })
            { Position = LayoutUnit.FromAbs(10), Size = LayoutUnit.FromAbs(8, 150) };

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
