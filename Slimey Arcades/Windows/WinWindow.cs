using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slimey_Arcades.Utilities;
using Slimey_Arcades.Objects;

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
                case "Win": break;
                case "Lose": break;
            }
        }
        public void SetButtonFunctions(Action SaveButtonAction, Action LoadButtonAction, Action ContinueButtonAction)
        {
            LevelSelectButton.Function = SaveButtonAction;
            RestartButton.Function = LoadButtonAction;
            ContinueButton.Function = ContinueButtonAction;
        }
        public bool test { get; set; } = false;
        public override void Update()
        {
            if (!test)
            {
                //Polygon Polygon = new Polygon(new Transform(0, 0, 100, 100), Color.White);
                //SubObjects.Add(Polygon);
                //test = true;
            }
            base.Update();
        }
    }
}
