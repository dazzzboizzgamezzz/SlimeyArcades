using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Slimey_Arcades.Windows
{
    public class PauseWindow : Window
    {
        public int SelectedLevel { get; set; } = 1;
        private Text LevelText { get; init; }
        //private Button LevelSelectButton { get; init; }
        //private Button ContinueButton { get; init; }
        //private Button RestartButton {  get; init; }
        public string WindowState { get; set; }
        private string WindowText { get => LevelText.Txt; set => LevelText.Txt = value; }
        public Scene NextScene { get; set; } = null;
        private bool PressKey { get; set; } = false;
        private Vector2 StartPos { get; set; }
        public string PauseState { get => LevelScene.PauseState; set => LevelScene.PauseState = value; } 
        private Button ContinueButton { get; set; }
        private int Level {  get; set; }
        public PauseWindow(Vector2 Pos, int CurrentLevel) : base(new Transform((int)Pos.X, (int)Pos.Y, 400, 200), Color.DarkGray)
        {
            //Header.Container.Destroy = true;

            StartPos = Pos;
            Level = CurrentLevel;
            Container.MoveAllChildren(this, new Vector2(-500, -500));

            LevelText = new Text(new Transform((int)(Transform.X + 120), (int)(Transform.Y + 50), 150, 30), "", NewScale: 0.3f);
            LevelText.Color = Color.White;
            SubObjects.Add(LevelText);

            WindowText = "Paused";

            Button LevelSelectButton = new Button((int)(Transform.X + 20), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Level Select");
            LevelSelectButton.Function = () => { NextScene = new LevelSelectScene(); PauseState = ""; };
            SubObjects.Add(LevelSelectButton);

            if (Level <= 10)
            {
                ContinueButton = new Button((int)(Transform.X + 140), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Continue");
                ContinueButton.Function = () => { Container.MoveAllChildren(this, new Vector2(-500, -500)); PauseState = ""; };
                SubObjects.Add(ContinueButton);
            }

            Button RestartButton = new Button((int)(Transform.X + 260), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Restart");
            RestartButton.Function = () => { NextScene = new LevelScene(CurrentLevel); PauseState = ""; };
            SubObjects.Add(RestartButton);
        }
        //public void UpdateState(string WindowState)
        //{
        //    switch (WindowState)
        //    {
        //        case "Paused": 
        //            break;
        //        case "Win":
        //            WindowText = "You Win!";
        //            ContinueButton.Text = "Next Level";
        //            break;
        //        case "Lose": 
        //            break;
        //    }
        //}
        //public void SetButtonFunctions(Action SaveButtonAction, Action LoadButtonAction, Action ContinueButtonAction)
        //{
        //    LevelSelectButton.Function = SaveButtonAction;
        //    RestartButton.Function = LoadButtonAction;
        //    ContinueButton.Function = ContinueButtonAction;
        //}
        public override void Update()
        {
            List<Keys> PressedKeys = Keyboard.GetState().GetPressedKeys().ToList();
            
            switch (PauseState)
            {
                case "":
                    if (PressedKeys.Contains(Keys.Space) && !PressKey)
                    {
                        Container.MoveAllChildren(this, StartPos);
                        PressKey = true;
                        PauseState = "Paused";
                    }
                    break;
                case "Paused":
                    WindowText = "Paused";
                    if (PressedKeys.Contains(Keys.Space) && !PressKey)
                    {
                        Container.MoveAllChildren(this, new Vector2(-500, -500));
                        PressKey = true;
                        PauseState = "";
                    }
                    break;
                case "Winning":
                    Container.MoveAllChildren(this, StartPos);
                    WindowText = "You Win!";
                    if (ContinueButton != null) ContinueButton.Function = () => { NextScene = new LevelScene(Level + 1); PauseState = ""; };
                    break;
                case "Losing":
                    WindowText = "You lose!";
                    if (ContinueButton != null) ContinueButton.Container.Destroy = true;
                    break;

            }
            if (!PressedKeys.Contains(Keys.Space))
            {
                PressKey = false;
            }

            //base.Update();
        }
    }
}
