using Microsoft.Xna.Framework;
using Slimey_Arcades.Scenes;

namespace Slimey_Arcades
{
    public class LevelScene : Scene
    {
        public int Level { get; set; }
        private PauseWindow PauseWindow { get; set; }
        private Button MenuButton { get; set; }
        private LevelGrid LevelGrid { get; set; } = null;
        public bool Debug { get; init; } = false;
        public LevelScene(int NewLevel) : base (Color.CornflowerBlue)
        {
            Level = NewLevel;
        }
        public override void Load()
        {
            Container.LoadObjects(this);
            PauseWindow = new PauseWindow(new Vector2(400, 300), Level);
            Container.ObjectsToLoad.Add(PauseWindow);
            Container.LoadObjects(this, 3);

            LevelGrid = new LevelGrid(200, 100, Level);
            Container.ObjectsToLoad.Add(LevelGrid);

            if (!Debug)
            {
                MenuButton = new Button(new Transform(800, 300, 200, 50), Shapes.Square, Color.Gray, "Return to level select");
                MenuButton.Function = () =>
                {
                    LevelSelectScene NewScene = new LevelSelectScene();
                    NextScene = NewScene;
                };
            }
            else
            {
                MenuButton = new Button(new Transform(800, 300, 200, 50), Shapes.Square, Color.Gray, "Return to debug");
                MenuButton.Function = () =>
                {
                    DebugScene NewScene = new DebugScene() { LoadLevel = true };
                    NextScene = NewScene;
                };
            }
            Container.ObjectsToLoad.Add(MenuButton);
        }
    }
}
