using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slimey_Arcades
{
    public class SceneController
    {
        private Scene ActiveScene { get; set; }
        public SceneController(Scene StartScene) 
        {  
            StartScene.Load();
            ActiveScene = StartScene;
        }
        public void Draw(SpriteBatch SpriteBatch, float WindowScale, Vector2 ScreenOffset)
        {
            ActiveScene.Draw(SpriteBatch, WindowScale, ScreenOffset);
        }
        public void Update()
        {
            if (ActiveScene.NextScene != null)
            {
                Scene PreviousScene = ActiveScene;
                ActiveScene = ActiveScene.NextScene;
                ActiveScene.Load();
                ActiveScene.Update();
                PreviousScene = null;
            }
            ActiveScene.Update();
        }
    }
}
