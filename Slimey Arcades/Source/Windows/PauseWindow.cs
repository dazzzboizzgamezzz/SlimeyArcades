using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Scenes;
using System;

//using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using System.Security.Cryptography.X509Certificates;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace Slimey_Arcades.Windows
namespace Slimey_Arcades
{
    public class PauseWindow : Window, INotifier
    {
        //public int SelectedLevel { get; set; } = 1;
        private Text LevelText { get; init; }
        //public string WindowState { get; set; }
        private string WindowState { get; set; } = "";
        private string WindowText { get => LevelText.Txt; set => LevelText.Txt = value; }
        //public Scene NextScene { get; set; } = null;
        private bool PressKey { get; set; } = false;
        private Vector2 StartPos { get; set; }
        //public string PauseState { get => LevelScene.PauseState; set => LevelScene.PauseState = value; } 
        private Button ContinueButton { get; set; }
        private int Level {  get; set; }
        public Notifier Notifier { get; init; } = new();
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
            //LevelSelectButton.Function = () => { NextScene = new LevelSelectScene(); PauseState = ""; };
            //LevelSelectButton.Function = () => { NextScene = new LevelSelectScene(); };
            LevelSelectButton.Function = () => ChangeLevel();
            SubObjects.Add(LevelSelectButton);

            if (Level <= 10)
            {
                ContinueButton = new Button((int)(Transform.X + 140), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Continue");
                //ContinueButton.Function = () => { Container.MoveAllChildren(this, new Vector2(-500, -500)); PauseState = ""; };
                ContinueButton.Function = () => { Container.MoveAllChildren(this, new Vector2(-500, -500)); };
                SubObjects.Add(ContinueButton);
            }

            Button RestartButton = new Button((int)(Transform.X + 260), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Restart");
            //RestartButton.Function = () => { NextScene = new LevelScene(CurrentLevel); PauseState = ""; };
            RestartButton.Function = () => ChangeLevel(Level);
            SubObjects.Add(RestartButton);

            Notifier.ProcessNotifications = ProcessNotifications;
        }
        private void ChangeLevel(int Level = -1)
        {
            Scene NextScene = Level < 0 ? new LevelSelectScene() : new LevelScene(Level); 
            Notification Notification = new Notification(NOTTYPES.CHANGELEVEL, NextScene);
            Notifier.SendNotification(Notification);
        }
        public override void Update()
        {
            List<Keys> PressedKeys = Keyboard.GetState().GetPressedKeys().ToList();

            if ((WindowState == "" || WindowState == "Paused") && PressedKeys.Contains(Keys.Space) && !PressKey)
            {
                Notification Notification = new Notification(NOTTYPES.PAUSED);
                Notifier.SendNotification(Notification);
                PressKey = true;
            }

            //switch (PauseState)
            //{
            //    case "":
            //        if (PressedKeys.Contains(Keys.Space) && !PressKey)
            //        {
            //            Container.MoveAllChildren(this, StartPos);
            //            PressKey = true;
            //            PauseState = "Paused";
            //        }
            //        break;
            //    case "Paused":
            //        WindowText = "Paused";
            //        if (PressedKeys.Contains(Keys.Space) && !PressKey)
            //        {
            //            Container.MoveAllChildren(this, new Vector2(-500, -500));
            //            PressKey = true;
            //            PauseState = "";
            //        }
            //        break;
            //    case "Winning":
            //        Container.MoveAllChildren(this, StartPos);
            //        WindowText = "You Win!";
            //        if (ContinueButton != null) ContinueButton.Function = () => { NextScene = new LevelScene(Level + 1); PauseState = ""; };
            //        break;
            //    case "Losing":
            //        WindowText = "You lose!";
            //        if (ContinueButton != null) ContinueButton.Container.Destroy = true;
            //        break;
            //}
            if (!PressedKeys.Contains(Keys.Space))
            {
                PressKey = false;
            }
        }
        private void ProcessNotifications(Notification Notification)
        {
            switch (Notification.Type)
            {
                case NOTTYPES.PAUSED:
                    if (WindowState == "Paused")
                    {
                        Container.MoveAllChildren(this, new Vector2(-500, -500));
                        WindowState = "";
                    }
                    else
                    {
                        Container.MoveAllChildren(this, StartPos);
                        WindowState = "Paused";
                    }
                    break;
                case NOTTYPES.WIN:
                    Container.MoveAllChildren(this, StartPos);
                    WindowState = "Win";
                    WindowText = "You Win!";
                    ContinueButton.Function = () => ChangeLevel(Level + 1);
                    break;
                case NOTTYPES.LOSE:
                    Container.MoveAllChildren(this, StartPos);
                    WindowState = "Lose";
                    WindowText = "You lose!";
                    ContinueButton.Container.Destroy = true;
                    break;
            }
        }
    }
}
