using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Slimey_Arcades
{
    public class SaveLoadWindow : Window, INotifier
    {
        public Notifier Notifier { get; init; } = new();
        public int SelectedLevel { get; set; } = 1;
        private Text LevelText { get; init; }
        //private Button SaveButton { get; init; }
        //private Button LoadButton {  get; init; }
        public SaveLoadWindow(Vector2 Position) : base(new Transform((int)Position.X, (int)Position.Y, 250, 200), Color.DarkGray)
        {
            Text SelectText = new Text(new Transform((int)(Position.X + 40), (int)(Position.Y + 50), 150, 30), "Select Level:", NewScale: 0.2f);
            SelectText.Color = Color.White;
            SubObjects.Add(SelectText);

            Polygon NumberOutline = new Polygon(new Transform((int)(Position.X + 80), (int)(Position.Y + 100), 50, 50), Color.LightGray);
            NumberOutline.Sprite.LayerData.LayerIndex = 9;
            SubObjects.Add(NumberOutline);

            LevelText = new Text(new Transform((int)(Position.X + 90), (int)(Position.Y + 105), 40, 40), SelectedLevel.ToString(), NewScale: 0.28f);
            SubObjects.Add(LevelText);

            Button UpButton = new Button((int)(Position.X + 130), (int)(Position.Y + 100), 25, 25, Shapes.Square, Color.LightGray, "");
            UpButton.Sprite.LayerData.LayerIndex = 8;
            UpButton.Function = () =>
            {
                SelectedLevel = Math.Min(99, SelectedLevel + 1);
                LevelText.Txt = SelectedLevel.ToString();
            };
            SubObjects.Add(UpButton);

            Button DownButton = new Button((int)(Position.X + 130), (int)(Position.Y + 125), 25, 25, Shapes.Square, Color.LightGray, "");
            DownButton.Sprite.LayerData.LayerIndex = 8;
            DownButton.Function = () =>
            {
                SelectedLevel = Math.Max(1, SelectedLevel - 1);
                LevelText.Txt = SelectedLevel.ToString();
            };
            SubObjects.Add(DownButton);

            Polygon UpTriangle = new Polygon(new Transform((int)(Position.X + 135), (int)(Position.Y + 110), 15, 10), Color.Black, "ETriangle");
            SubObjects.Add(UpTriangle);

            Polygon DownTriangle = new Polygon(new Transform((int)(Position.X + 135), (int)(Position.Y + 130), 15, 10), Color.Black, "ETriangle");
            DownTriangle.Sprite.Effect = SpriteEffects.FlipVertically;
            SubObjects.Add(DownTriangle);

            //SaveButton = new Button((int)(Position.X + 20), (int)(Position.Y + 165), 90, 25, Shapes.Square, Color.LightGray, "Save Level");
            Button SaveButton = new Button((int)(Position.X + 20), (int)(Position.Y + 165), 90, 25, Shapes.Square, Color.LightGray, "Save Level");
            SaveButton.Function = () =>
            {
                Notifier.SendNotification(new Notification(NOTTYPES.SAVELEVEL, SelectedLevel));
            };
            SubObjects.Add(SaveButton);

            //LoadButton = new Button((int)(Position.X + 140), (int)(Position.Y + 165), 90, 25, Shapes.Square, Color.LightGray, "Load Level");
            Button LoadButton = new Button((int)(Position.X + 140), (int)(Position.Y + 165), 90, 25, Shapes.Square, Color.LightGray, "Load Level");
            LoadButton.Function = () =>
            {
                Notifier.SendNotification(new Notification(NOTTYPES.LOADLEVEL, SelectedLevel));
            };
            SubObjects.Add(LoadButton);
        }

        //public void SetButtonFunctions(Action SaveButtonAction, Action LoadButtonAction)
        //{
        //    SaveButton.Function = SaveButtonAction;
        //    LoadButton.Function = LoadButtonAction;
        //}
    }
}
