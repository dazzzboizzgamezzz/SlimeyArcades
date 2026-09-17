using Microsoft.Xna.Framework;
using Slimey_Arcades.Managers;
//using Slimey_Arcades.Scenes;
//using Slimey_Arcades.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace Slimey_Arcades
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SceneController SceneController;
        SpriteBatch SpriteBatch;

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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Shapes.MakeShapes(GraphicsDevice);
            FontManager.Initialize(Content);
            TextureManager.Initialize(Content);
            SceneController = new SceneController(new TitleScene());
        }
        protected override void Update(GameTime gameTime)
        {
            SceneController.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneController.Draw(SpriteBatch);
        }
    }
}