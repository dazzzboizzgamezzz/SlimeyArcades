using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Slimey_Arcades
{
    //class NewDebugScene : Scene
    class DebugScene : Scene
    {
        public DebugGrid Grid { get; set; }
        private int Rows { get; set; } = 11;
        private int Cols { get; set; } = 11;
        private Button ColDownButton { get; set; }
        private Button ColUpButton { get; set; }
        private Button RowUpButton { get; set; }
        private Button RowDownButton { get; set; }
        private SaveLoadWindow SaveLoadWindow { get; set; }
        private ScreenMover ScreenMover { get; set; }
        //public NewDebugScene() : base(Color.CornflowerBlue)
        public DebugScene() : base(Color.CornflowerBlue)
        {
            
        }
        public override void Load()
        {
            Grid = new DebugGrid(Cols, Rows);
            Container.ObjectsToLoad.Add(Grid);

            DebugSelect Test = new DebugSelect();
            Container.ObjectsToLoad.Add(Test);

            Transform ColButtonTransform = new Transform(Grid.Transform.X + Grid.Transform.Width + 10, (int)Grid.Transform.Center.Y - 10, 10, 20);
            ColDownButton = new Button(ColButtonTransform, Shapes.Square, Color.LightGray, "<", false);
            ColDownButton.Function = () => ChangeGrid(-1, 0);
            ColUpButton = new Button(new Transform(ColButtonTransform.X + 15, ColButtonTransform.Y, ColButtonTransform.Width, ColButtonTransform.Height), Shapes.Square, Color.LightGray, ">", false);
            ColUpButton.Function = () => ChangeGrid(1, 0);
            Container.ObjectsToLoad.Add(ColDownButton);
            Container.ObjectsToLoad.Add(ColUpButton);

            Transform RowButtonTransform = new Transform((int)Grid.Transform.Center.X - 5, Grid.Transform.Y + Grid.Transform.Height + 10, 20, 10);
            RowDownButton = new Button(RowButtonTransform, Shapes.Square, Color.LightGray, "^");
            RowDownButton.Function = () => ChangeGrid(0, -1);
            RowUpButton = new Button(new Transform(RowButtonTransform.X, RowButtonTransform.Y + 15, RowButtonTransform.Width, RowButtonTransform.Height), Shapes.Square, Color.LightGray, "V");
            RowUpButton.Function = () => ChangeGrid(0, 1);
            Container.ObjectsToLoad.Add(RowDownButton);
            Container.ObjectsToLoad.Add(RowUpButton);

            ScreenMover = new ScreenMover(new Rectangle(0, 0, 1800, 1300));
            Container.ObjectsToLoad.Add(ScreenMover);

            SaveLoadWindow = new SaveLoadWindow(new Vector2(175, 150));
            Container.ObjectsToLoad.Add(SaveLoadWindow);

            Button ReturnToTitle = new Button(175, 375, 200, 50, Shapes.Square, Color.DarkGray, "Return To Title");
            ReturnToTitle.Function = () => { NextScene = new TitleScene(); };
            Container.ObjectsToLoad.Add(ReturnToTitle);

            Button PlayLevel = new Button(175, 450, 200, 50, Shapes.Square, Color.DarkGray, "Play Level");
            PlayLevel.Function = () =>
            {
                Grid.SaveLevel(0);
                NextScene = new LevelScene(0) { Debug = true };
            };
            Container.ObjectsToLoad.Add(PlayLevel);
        }
        public override void ProcessNotifications(Notification Notification)
        {
            base.ProcessNotifications(Notification);
            if (Notification.Type == NOTTYPES.SAVELEVEL)
            {
                Grid.SaveLevel((int)Notification.Data);
            }
            if (Notification.Type == NOTTYPES.LOADLEVEL)
            {
                int NewLevel = (int)Notification.Data;
                Grid TempGrid = new Grid(0, 0, NewLevel: NewLevel);
                int ColDiff = TempGrid.Cols - Grid.Cols;
                int RowDiff = TempGrid.Rows - Grid.Rows;
                ChangeGrid(ColDiff, RowDiff);
                Grid.ResetGrid();
                Grid.LoadLevel(NewLevel);
            }
        }
        private void ChangeGrid(int ChangeCol, int ChangeRow)
        {
            Grid.SetBorders(false);
            Grid.SaveLevel(0);
            Rows += ChangeRow;
            Cols += ChangeCol;
            UpdateButtons(ChangeCol, ChangeRow);
            Grid.Container.Destroy = true;
            Grid = new DebugGrid(Cols, Rows);
            Grid.LoadLevel(0);
            Container.ObjectsToLoad.Add(Grid);
        }
        private void UpdateButtons(int ChangeCol, int ChangeRow)
        {
            Vector2 ChangePosition = new Vector2((ChangeCol * Grid.CellSize) + (ChangeCol * 3), (ChangeRow * Grid.CellSize) + (ChangeRow * 3));
            Vector2 NewMousePos = Mouse.GetState().Position.ToVector2() + ChangePosition;
            Mouse.SetPosition((int)NewMousePos.X, (int)NewMousePos.Y);

            int MaxRows = 20;
            int MaxCols = 20;
            int MinRows = 5;
            int MinCols = 5;

            ColDownButton.Container.MoveAllChildren(ColDownButton, ColDownButton.Transform.Pos + ChangePosition);
            ColUpButton.Container.MoveAllChildren(ColUpButton, ColUpButton.Transform.Pos + ChangePosition);
            if (Cols <= MinCols + 2) ColDownButton.IsActive = false;
            else ColDownButton.IsActive = true;
            if (Cols >= MaxCols + 2) ColUpButton.IsActive = false;
            else ColUpButton.IsActive = true;

            RowDownButton.Container.MoveAllChildren(RowDownButton, RowDownButton.Transform.Pos + ChangePosition);
            RowUpButton.Container.MoveAllChildren(RowUpButton, RowUpButton.Transform.Pos + ChangePosition);
            if (Rows <= MinRows + 2) RowDownButton.IsActive = false;
            else RowDownButton.IsActive = true;
            if (Rows >= MaxRows + 2) RowUpButton.IsActive = false;
            else RowUpButton.IsActive = true;
        }
    }
}
