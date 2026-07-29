using Microsoft.Xna.Framework.Graphics;

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
        public void Draw(SpriteBatch SpriteBatch)
        {
            //back is 1, front is 0
            SpriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

            ActiveScene.Draw(SpriteBatch);

            SpriteBatch.End();
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
