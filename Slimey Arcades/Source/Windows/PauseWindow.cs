using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Scenes;
using System;

using System.Collections.Generic;
using System.Linq;

namespace Slimey_Arcades
{
    public class PauseWindow : Window, INotifier
    {
        private Text LevelText { get; init; }
        private string WindowState { get; set; } = "";
        private string WindowText { get => LevelText.Txt; set => LevelText.Txt = value; }
        private bool PressKey { get; set; } = false;
        private Vector2 StartPos { get; set; }
        private Button ContinueButton { get; set; }
        private int Level {  get; set; }
        public Notifier Notifier { get; init; } = new();
        public PauseWindow(Vector2 Pos, int CurrentLevel) : base(new Transform((int)Pos.X, (int)Pos.Y, 400, 200), Color.DarkGray)
        {

            StartPos = Pos;
            Level = CurrentLevel;
            Container.MoveAllChildren(this, new Vector2(-500, -500));

            LevelText = new Text(new Transform((int)(Transform.X + 120), (int)(Transform.Y + 50), 150, 30), "", NewScale: 0.3f);
            LevelText.Color = Color.White;
            SubObjects.Add(LevelText);

            WindowText = "Paused";

            Button LevelSelectButton = new Button((int)(Transform.X + 20), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Level Select");
            LevelSelectButton.Function = () => ChangeLevel();
            SubObjects.Add(LevelSelectButton);

            if (Level <= 10)
            {
                ContinueButton = new Button((int)(Transform.X + 140), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Continue");
                ContinueButton.Function = () => { Container.MoveAllChildren(this, new Vector2(-500, -500)); };
                SubObjects.Add(ContinueButton);
            }

            Button RestartButton = new Button((int)(Transform.X + 260), (int)(Transform.Y + 130), 90, 25, Shapes.Square, Color.LightGray, "Restart");
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
