using Microsoft.Xna.Framework;
using Slimey_Arcades.Objects;
using Slimey_Arcades.Windows;
using Slimey_Arcades.Utilities;
using System;

namespace Slimey_Arcades.Scenes
{
    public class LevelScene :Scene
    {
        public int Level { get; set; }
        public static Window WinWindow { get; set; }
        Button MenuButton { get; set; }
        public LevelScene() : base (Color.CornflowerBlue)
        {

        }
        public override void Load()
        {
            LevelGrid Grid = new LevelGrid(200, 100, Level);
            Container.ObjectsToLoad.Add(Grid);

            MenuButton = new Button(new Transform(800, 300, 200, 50), Shapes.Square, Color.Gray, "Return to level select");
            MenuButton.Sprite.Color = Color.Gray;
            MenuButton.Function = () =>
            {
                LevelSelectScene NewScene = new LevelSelectScene();
                NewScene.Load();
                NextScene = NewScene;
            }; 
            Container.ObjectsToLoad.Add(MenuButton);
        }
        public void EnterDebug()
        {
            MenuButton.Text = "Return to debug";
            //MenuButton.Transform = new Transform(800, 200, 200, 50);
            MenuButton.Function = () =>
            {
                DebugScene NewScene = new DebugScene() { LoadLevel = true };
                NewScene.Load();
                NextScene = NewScene;
            };
        }
    }
}
