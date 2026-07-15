using Microsoft.Xna.Framework.Graphics;

namespace Slimey_Arcades
{
    public class SceneController
    {
        //private SpriteBatch SpriteBatch {  get; set; }
        //private GraphicsDevice GraphicsDevice { get; set; }
        private Scene ActiveScene { get; set; }
        //public SceneController(GraphicsDevice NewGraphicsDevice, Scene StartScene) 
        public SceneController(Scene StartScene) 
        {  
            //GraphicsDevice = NewGraphicsDevice;
            //SpriteBatch = new SpriteBatch(GraphicsDevice);
            StartScene.Load();
            //StartScene.AssignAllLayers(StartScene);
            ActiveScene = StartScene;
        }
        public void Draw(SpriteBatch SpriteBatch)
        {
            //back is 1, front is 0
            SpriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

            //IDraw DrawScene = (IDraw)ActiveScene;
            //SpriteBatch.Draw(DrawScene.Texture,
            //                 DrawScene.Rect,
            //                 null,
            //                 DrawScene.Color,
            //                 DrawScene.Rotation,
            //                 DrawScene.Origin,
            //                 DrawScene.Effect,
            //                 1); //Draw Scene

            //RenderObjects();

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
                //ActiveScene.AssignAllLayers(ActiveScene);
                //ActiveScene.PostLoad();
                //PreviousScene.NextScene = null;
                PreviousScene = null;
            }
            //IContainer CurrentScene = ActiveScene;
            //for (int i = 0; i < CurrentScene.Updates.Count; i++)
            //{
            //    IUpdate Update = CurrentScene.Updates[i];
            //    Update.Update();
            //}
            //for (int i = 0; i < CurrentScene.Containers.Count; i++)
            //{
            //    IContainer Container = CurrentScene.Containers[i];
            //    if (Container.Destroy) 
            //    { 
            //        CurrentScene.Container.DestroyObjects(Container); i--; 
            //    }
            //    if (Container.ObjectsToLoad.Count > 0) 
            //    ActiveScene.AssignAllLayers(Container);
            //}
            ActiveScene.Update();
        }
        //private void RenderObjects()
        //{
        //    IContainer CurrentScene = ActiveScene;
        //    foreach (IDraw Object in CurrentScene.Draws)
        //    {
        //        SpriteBatch.Draw(Object.Texture,
        //                 Object.Rect,
        //                 null,
        //                 Object.Color,
        //                 Object.Rotation,
        //                 Object.Origin,
        //                 Object.Effect,
        //                 Object.Layer); //Draw Objects
        //    }
        //    foreach (IText Object in CurrentScene.Texts)
        //    //foreach (Text Object in CurrentScene.Texts) 
        //    {
        //        //if (Object.Txt != "")
        //        if (Object.Text != "")
        //        {
        //            SpriteBatch.DrawString(Object.Font,
        //               //Object.Txt,
        //               Object.Text,
        //               Object.Pos,
        //               Object.Color,
        //               Object.Rotation,
        //               Object.Origin,
        //               Object.Scale,
        //               Object.Effect,
        //               Object.Layer); //Draw Text
        //        }
        //    }
        //}
    }
}
