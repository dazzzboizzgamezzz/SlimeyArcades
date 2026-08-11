using Microsoft.Xna.Framework;
//using Slimey_Arcades.Managers;
//using Slimey_Arcades.Objects;
using Slimey_Arcades.Scenes;

namespace Slimey_Arcades
{
    public class TitleScene :Scene
    {
        public TitleScene() : base (Color.CornflowerBlue)
        {

        }
        public override void Load()
        {
            Text Title = new Text(new Transform(450, 100, 300, 100), "Slimey Arcades", NewScale: 0.5f);
            Container.ObjectsToLoad.Add(Title);

            Button LevelSelect = new Button(new Transform(100, 400, 200, 50), Shapes.Square, Color.Gray, "Level Select", true);
            LevelSelect.Function = () => { NextScene = new LevelSelectScene(); };
            Container.ObjectsToLoad.Add(LevelSelect);

            Button Debug = new Button(new Transform(400, 400, 200, 50), Shapes.Square, Color.Gray, "Debug", true);
            Debug.Function = () => { NextScene = new DebugScene(); };
            Container.ObjectsToLoad.Add(Debug);

            Button Options = new Button(new Transform(700, 400, 200, 50), Shapes.Square, Color.Gray, "Options", true);
            Options.Function = () => { };
            Container.ObjectsToLoad.Add(Options);
        }
    }
}
