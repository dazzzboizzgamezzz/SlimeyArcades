using Microsoft.Xna.Framework;
using Slimey_Arcades.Managers;
using Slimey_Arcades.Scenes;
using Slimey_Arcades.Utilities;
using Slimey_Arcades.Objects;

namespace Slimey_Arcades
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SceneController SceneController;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Graphics.PreferredBackBufferHeight = (int)SETTINGS.SCREENHEIGHT;
            Graphics.PreferredBackBufferWidth = (int)SETTINGS.SCREENWIDTH;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Shapes.MakeShapes(GraphicsDevice);
            FontManager.Initialize(Content);
            TextureManager.Initialize(Content);
            SceneController = new SceneController(GraphicsDevice, new LevelSelectScene());
        }
        protected override void Update(GameTime gameTime)
        {
            SceneController.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneController.Draw();

            base.Draw(gameTime);
        }
    }
}