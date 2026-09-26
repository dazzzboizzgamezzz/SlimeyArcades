using Microsoft.Xna.Framework;
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

            Button TestButton = new Button(new Transform(500, 300, 50, 50), BGColor: Color.Red);
            TestButton.Function = () => 
            {
                Notification Notification = new Notification(NOTTYPES.SCREENCHANGE, "Left");
                Notifier.SendNotification(Notification);
            };
            Container.ObjectsToLoad.Add(TestButton);

            Button TestButton2 = new Button(new Transform(1500, 300, 50, 50), BGColor: Color.Red);
            TestButton2.Function = () => 
            {
                Notification Notification = new Notification(NOTTYPES.SCREENCHANGE, "Right");
                Notifier.SendNotification(Notification);
            };
            Container.ObjectsToLoad.Add(TestButton2);

            //Polygon TestPolygon = new Polygon(new Transform(300, 1000, 100, 100), Color.Red);
            //Container.ObjectsToLoad.Add(TestPolygon);
        }
    }
}
