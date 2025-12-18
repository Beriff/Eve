using Eve.Model;
using Eve.UI;
using Eve.UI.ControlModules.Input;
using Eve.UI.Controls;
using Eve.UI.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Eve
{
    public class EveProgram : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public UIGroup Group;

        public EveProgram()
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

             var (button, (bg, _)) = ButtonFactory.SimpleButton.GetHookedInstance();
            button.Size.Value = LayoutUnit.FromAbs(100, 50);
            bg.PanelColor = Color.Gray;

            Group = new();

            Group += button;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Group.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Group.Render(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
