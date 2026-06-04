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
        public bool DebugMode { get; set; } = false;
        public static Window WinWindow { get; set; }
        public LevelScene() : base (Color.CornflowerBlue)
        {
        }

        public override void Load()
        {
            LevelGrid Grid = new LevelGrid(200, 100, Level);

            SubObjects.Add(Grid);

            Action MenuButtonAction = null;
            string MenuButtonText = null;   
            Transform MenuButtonTransform = null;   
            if (DebugMode)
            {
                MenuButtonText = "Return to debug";
                MenuButtonTransform = new Transform(800, 200, 200, 50);
                MenuButtonAction = () =>
                {
                    DebugScene NewScene = new DebugScene() { LoadLevel = true };
                    NewScene.Load();
                    NextScene = NewScene;
                };
            }
            else
            {
                MenuButtonText = "Return to level select";
                MenuButtonTransform = new Transform(800, 300, 200, 50);
                MenuButtonAction = () =>
                {
                    LevelSelectScene NewScene = new LevelSelectScene();
                    NewScene.Load();
                    NextScene = NewScene;
                };
            }
            Button MenuButton = new Button(MenuButtonTransform, Shapes.Square, Color.Gray, MenuButtonText);
            MenuButton.Sprite.Color = Color.Gray;
            MenuButton.Function = MenuButtonAction;
            SubObjects.Add(MenuButton);

            //LoadObjects();
        }
    }
}
