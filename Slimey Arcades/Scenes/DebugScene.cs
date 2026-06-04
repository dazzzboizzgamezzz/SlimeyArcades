using Microsoft.Xna.Framework;
using Slimey_Arcades.Objects;
using Slimey_Arcades.Windows;
using Slimey_Arcades.Utilities;
using Slimey_Arcades.Managers;

namespace Slimey_Arcades.Scenes
{
    public class DebugScene :Scene
    {
        public bool LoadLevel { get; init; }
        public DebugScene() : base (Color.LightGray)
        {

        }
        public override void Load()
        {
            DebugGrid Grid = new DebugGrid(200, 100);
            if (LoadLevel)
            {
                Grid.LoadLevel(0);
            }
            else
            {
                Grid.LoadLevel(-1);
            }

            SubObjects.Add(Grid);

            DebugMenu Debug = new DebugMenu(100, 100);
            SubObjects.Add(Debug);

            Button PlayLevel = new Button(800, 100, 200, 50, Shapes.Square, Color.DarkGray, "Play Level");
            PlayLevel.Function = () =>
            {
                Grid.SaveLevel(0);
                LevelScene NewScene = new LevelScene();
                NewScene.DebugMode = true;
                NewScene.Level = 0;
                NewScene.Load();
                NextScene = NewScene;
            };
            SubObjects.Add(PlayLevel);

            SaveLoadWindow SaveLoadWindow = new SaveLoadWindow(new Vector2(800, 300));
            SaveLoadWindow.SetButtonFunctions(() => Grid.SaveLevel(SaveLoadWindow.SelectedLevel), () => Grid.LoadLevel(SaveLoadWindow.SelectedLevel));
            SubObjects.Add(SaveLoadWindow);

            //LoadObjects();
        }
    }
}
