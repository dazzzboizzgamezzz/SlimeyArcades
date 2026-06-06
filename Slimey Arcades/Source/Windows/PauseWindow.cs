using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Slimey_Arcades.Windows
{
    public class PauseWindow : Window
    {
        public int SelectedLevel { get; set; } = 1;
        private Text LevelText { get; init; }
        private Button LevelSelectButton { get; init; }
        private Button ContinueButton { get; init; }
        private Button RestartButton {  get; init; }
        public string WindowState { get; set; }
        private string WindowText { get; set; }
        public PauseWindow(Vector2 Pos) : base(new Transform((int)Pos.X, (int)Pos.Y, 400, 200), Color.DarkGray)
        {
            Header = null;
            
            WindowText = "Paused";

            Transform.Pos = Pos;

            Text SelectText = new Text(new Transform((int)(Transform.X + 120), (int)(Transform.Y + 50), 150, 30), WindowText, 0.3f);
            SelectText.Color = Color.White;
            SubObjects.Add(SelectText);

            LevelSelectButton = new Button((int)(Transform.X + 20), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Level Select");
            SubObjects.Add(LevelSelectButton);

            ContinueButton = new Button((int)(Transform.X + 140), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Continue");
            SubObjects.Add(ContinueButton);
            
            RestartButton = new Button((int)(Transform.X + 260), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Restart");
            SubObjects.Add(RestartButton);
        }
        public void UpdateState(string WindowState)
        {
            switch (WindowState)
            {
                case "Paused": 
                    break;
                case "Win":
                    WindowText = "You Win!";
                    ContinueButton.Text = "Next Level";
                    break;
                case "Lose": 
                    break;
            }
        }
        public void SetButtonFunctions(Action SaveButtonAction, Action LoadButtonAction, Action ContinueButtonAction)
        {
            LevelSelectButton.Function = SaveButtonAction;
            RestartButton.Function = LoadButtonAction;
            ContinueButton.Function = ContinueButtonAction;
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
