using Microsoft.Xna.Framework;
//using Slimey_Arcades.Objects;

namespace Slimey_Arcades.Scenes
{
    public class LevelSelectScene :Scene
    {
        public LevelSelectScene() : base (Color.CornflowerBlue)
        {

        }
        public override void Load()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    int Col = i + 1;
                    int Row = j + 1;
                    int X = 175 * Col;
                    int Y = 100 * Row;
                    int Width = 125;
                    int Height = 50;
                    Transform ButtonTransform = new Transform(X, Y, Width, Height);
                    int CurrentLevel = Col + (j * 5);
                    string LevelName = "Level" + CurrentLevel.ToString();
                    Button NewButton = new Button(ButtonTransform, Shapes.Square, Color.Gray, LevelName, true);
                    NewButton.Function = () => SetLevel(CurrentLevel);
                    Container.ObjectsToLoad.Add(NewButton);
                }
            }

            Button TitleButton = new Button(new Transform(600, 600, 125, 50), Shapes.Square, Color.Gray, "Return to Title", true);
            TitleButton.Function = () => { NextScene = new TitleScene(); };
            Container.ObjectsToLoad.Add(TitleButton);
        }
        private void SetLevel(int Level)
        {
            LevelScene NewLevel = new(Level);
            NextScene = NewLevel;
        }
    }
}
