using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Slimey_Arcades
{
    public class Game1 : Game
    {
        //GraphicsDeviceManager Graphics;
        //SceneController SceneController;
        //SpriteBatch SpriteBatch;
        private GraphicsDeviceManager Graphics { get; init; }
        private SceneController SceneController { get; set; }
        SpriteBatch SpriteBatch { get; set; }
        private int BaseScreenHeight { get; init; } = (int)SETTINGS.SCREENHEIGHT;
        private int BaseScreenWidth { get; init; } = (int)SETTINGS.SCREENWIDTH;
        private float WindowScale { get; set; } = 1;
        private Vector2 ScreenOffset { get; set; } = Vector2.Zero;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            //Graphics.PreferredBackBufferHeight = (int)SETTINGS.SCREENHEIGHT;
            //Graphics.PreferredBackBufferWidth = (int)SETTINGS.SCREENWIDTH;
            Graphics.PreferredBackBufferHeight = BaseScreenHeight;
            Graphics.PreferredBackBufferWidth = BaseScreenWidth;
            //Window.AllowUserResizing = true;
            //Window.ClientSizeChanged += OnResize;
        }
        public void OnResize(Object sender, EventArgs e)
        {
            int ScreenWidth = Graphics.GraphicsDevice.Viewport.Width;
            int ScreenHeight = Graphics.GraphicsDevice.Viewport.Height;
            int ScreenWidthDiff = ScreenWidth - BaseScreenWidth;
            int ScreenHeightDiff = ScreenHeight - BaseScreenHeight;
            WindowScale = ScreenWidthDiff < ScreenHeightDiff ?
                ScreenWidth / BaseScreenWidth :
                ScreenHeight / BaseScreenHeight;
            ScreenOffset = new Vector2(ScreenWidthDiff / 2, ScreenHeightDiff / 2);
            Graphics.PreferredBackBufferWidth = (int)(BaseScreenWidth * WindowScale);
            Graphics.PreferredBackBufferHeight = (int)(BaseScreenHeight * WindowScale);
            Graphics.ApplyChanges();
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
            Graphics.GraphicsDevice.Clear(Color.Black);
            //SceneController.Draw(SpriteBatch);
            SceneController.Draw(SpriteBatch, WindowScale, ScreenOffset);
        }
    }
}