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

            var dummy1 = new Panel() 
            { Position = LayoutUnit.FromRel(0), Size = LayoutUnit.FromAbs(30), PanelColor = Color.Red };
            var dummy2 = (dummy1.Clone() as Panel)!;
            dummy2.Position = LayoutUnit.FromRel(0, 2);
            dummy2.PanelColor = Color.Green;
            var dummy3 = (dummy1.Clone() as Panel)!;
            dummy3.Position = LayoutUnit.FromRel(0, 3);
            dummy3.PanelColor = Color.Green;

            Group = new();

            var (scrollPanel, (area, _)) = new ScrollPanelFactory(Group).GetBlueprint().GetHookedInstance();

            

            scrollPanel.Size = LayoutUnit.FromAbs(200, 100);
            scrollPanel.Position = LayoutUnit.FromAbs(20);

            (scrollPanel as Panel).PanelColor = Color.Yellow;
            Console.WriteLine(area.Bounds);

            area.WithChildren(dummy1,dummy2,dummy3);
            

            Group += scrollPanel;


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
