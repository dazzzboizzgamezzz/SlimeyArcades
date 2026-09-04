using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Slimey_Arcades
{
    public class DebugMenu : Grid 
    {
        private Polygon Outline { get; set; }
        public int SelectedButton { get; set; } = -1;
        public int SelectedSlime { get; set; } = 0;
        public Color SelectedColor {  get; set; }
        private int CurrentSlime { get; set; } = 0;
        private int CurrentGoal { get; set; } = 0;
        private int CurrentCutter { get; set; } = 0;
        private Button SlimeButton { get; set; }
        private Button GoalButton { get; set; }
        private Button CutterButton { get; set; }
        private Button SwitchButton {  get; set; }
        private Button TintButton { get; set; }
        private Clicker SlimeClicker { get; set; }
        private Clicker GoalClicker { get; set; }
        private Clicker CutterClicker { get; set; }
        private Clicker SwitchClicker { get; set; }
        private Clicker TintClicker { get; set; }
        private Polygon CutterImage { get; set; }
        private Polygon SwitchImage { get; set; }
        public DebugMenu(int X, int Y) : base(X, Y, 1, 10, Color.DarkMagenta)
        {
            Outline = new Polygon(new Transform(X - 5, Y - 5, 60, 60), Color.Gold);
            Outline.Sprite.Texture = Shapes.MakeOutline(new Rectangle(X + 5, Y + 5, 50, 50));
            Container.ObjectsToLoad.Add(Outline);

            for (int i = 0; i < Rows; i++)
            {
                Cells[0, i].Sprite.Color = ColorManager.None;
                Button NewButton = new Button(Cells[0, i].Transform, BGColor: Color.Gray);
                NewButton.Sprite.LayerData.LayerIndex = 9;
                int ButtonValue = -1;
                int SelectedSlime = -1;
                Color PointerColor = ColorManager.None;
                Color ButtonColor = Color.Gray;
                switch (i)
                {
                    case 0: 
                        ButtonColor = ColorHelp.GreenSlime;
                        SelectedSlime = CurrentSlime;
                        SlimeButton = NewButton;
                        SlimeClicker = new Clicker(Cells[0, i].Transform);
                        break;
                    case 1:
                        NewButton.Text = "Goal";
                        NewButton.TextColor = ColorHelp.GreenSlime;
                        ButtonValue = (int)CELLOBJECTS.GGOAL;
                        PointerColor = ColorHelp.GreenSlime;
                        GoalButton = NewButton;
                        GoalClicker = new Clicker(Cells[0, i].Transform);
                        break;
                    case 2: 
                        ButtonColor = Color.Black;
                        ButtonValue = (int)CELLOBJECTS.WALL;
                        break;
                    case 3:
                        ButtonColor = ColorHelp.Ice;
                        ButtonValue = (int)CELLOBJECTS.ICE;
                        break;
                    case 4:
                        PointerColor = Color.Black;
                        NewButton.Sprite.Texture = Shapes.BackedCircle;
                        ButtonValue = (int)CELLOBJECTS.PIT;
                        break;
                    case 5:
                        Transform BT = NewButton.Transform;
                        CutterImage = new Polygon(new Transform(BT.X + 30, BT.Y + 10, 10, 30), Color.Black);
                        CutterImage.Sprite.Texture = Shapes.XCross;
                        NewButton.Container.ObjectsToLoad.Add(CutterImage);
                        ButtonValue = (int)CELLOBJECTS.RCUTTER;
                        PointerColor = Color.Black;
                        CutterButton = NewButton;
                        CutterClicker = new Clicker(Cells[0, i].Transform);
                        break;
                    case 6:
                        Polygon Barrel = new Polygon(new Transform(NewButton.Transform.X + 5, NewButton.Transform.Y + 5, 40, 40), ColorHelp.Barrel);
                        Cells[0, i].Container.ObjectsToLoad.Add(Barrel);
                        PointerColor = ColorHelp.Barrel;
                        ButtonValue = (int)CELLOBJECTS.BARREL;
                        break;
                    case 7:
                        PointerColor = ColorHelp.RedSwitch;
                        ButtonValue = (int)CELLOBJECTS.RSWITCH;
                        SwitchClicker = new Clicker(NewButton.Transform);
                        SwitchButton = NewButton;
                        SwitchImage = new Polygon(new Transform((int)NewButton.Transform.Center.X - 10, (int)NewButton.Transform.Center.Y - 10, 20, 20), ColorHelp.RedSwitch, "Circle");
                        Container.ObjectsToLoad.Add(SwitchImage);
                        break;
                    case 8:
                        ButtonColor = ColorHelp.RedSwitch;
                        ButtonValue = (int)CELLOBJECTS.RTINT;
                        TintClicker = new Clicker(NewButton.Transform);
                        TintButton = NewButton;
                        break;
                }
                NewButton.Function = () => 
                {
                    Outline.Transform.Pos = NewButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = ButtonValue;
                    this.SelectedSlime = SelectedSlime;
                    SelectedColor = ButtonColor;
                    if (PointerColor != ColorManager.None) SelectedColor = PointerColor;
                };
                NewButton.Sprite.Color = ButtonColor;
                NewButton.Highlightable = false;
                Container.ObjectsToLoad.Add(NewButton);
            }

            for (int i = 0; i < 5; i++)
            {
                Slimes[i].Container.Destroy = true;
                Slimes[i] = null;
            }
            SelectedColor = ColorManager.Colors["GSlime"];
        }
        public override void Update()
        {
            if (SlimeClicker.Click("Right"))
            {
                CurrentSlime = (CurrentSlime + 1) % 5;

                Color SlimeColor = ColorManager.None;
                switch (CurrentSlime)
                {
                    case 0: SlimeColor = ColorManager.Colors["GSlime"]; break;
                    case 1: SlimeColor = ColorManager.Colors["RSlime"]; break;
                    case 2: SlimeColor = ColorManager.Colors["BSlime"]; break;
                    case 3: SlimeColor = ColorManager.Colors["YSlime"]; break;
                    case 4: SlimeColor = ColorManager.Colors["PSlime"]; break;
                }
                SlimeButton.Sprite.Color = SlimeColor;

                SlimeButton.Function = () =>
                {
                    Outline.Transform.Pos = SlimeButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = -1;
                    SelectedSlime = CurrentSlime;
                    SelectedColor = SlimeColor;
                };
                SlimeButton.Function();
            }
            if (GoalClicker.Click("Right"))
            {
                CurrentGoal = ((CurrentGoal + 1) % 5);
                int EnumOffset = CurrentGoal + (int)CELLOBJECTS.GGOAL;

                Color GoalColor = ColorManager.None;
                switch (EnumOffset)
                {
                    case (int)CELLOBJECTS.GGOAL: GoalColor = ColorManager.Colors["GSlime"]; break;
                    case (int)CELLOBJECTS.RGOAL: GoalColor = ColorManager.Colors["RSlime"]; break;
                    case (int)CELLOBJECTS.BGOAL: GoalColor = ColorManager.Colors["BSlime"]; break;
                    case (int)CELLOBJECTS.YGOAL: GoalColor = ColorManager.Colors["YSlime"]; break;
                    case (int)CELLOBJECTS.PGOAL: GoalColor = ColorManager.Colors["PSlime"]; break;
                }
                GoalButton.TextColor = GoalColor;

                GoalButton.Function = () =>
                {
                    Outline.Transform.Pos = GoalButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = EnumOffset;
                    SelectedSlime = -1;
                    SelectedColor = GoalColor;
                };
                GoalButton.Function();
            }
            if (CutterClicker.Click("Right"))
            {
                CurrentCutter = ((CurrentCutter + 1) % 4);
                var EnumOffset = CurrentCutter + (int)CELLOBJECTS.RCUTTER;

                Transform BT = CutterButton.Transform;
                Transform NewTransform = null;
                switch (EnumOffset)
                {
                    case (int)CELLOBJECTS.RCUTTER: NewTransform = new Transform(BT.X + 30, BT.Y + 10, 10, 30); break;
                    case (int)CELLOBJECTS.LCUTTER: NewTransform = new Transform(BT.X + 10, BT.Y + 10, 10, 30); break;
                    case (int)CELLOBJECTS.UCUTTER: NewTransform = new Transform(BT.X + 10, BT.Y + 10, 30, 10); break;
                    case (int)CELLOBJECTS.DCUTTER: NewTransform = new Transform(BT.X + 10, BT.Y + 30, 30, 10); break;
                }
                CutterImage.Transform = NewTransform;

                CutterButton.Function = () =>
                {
                    Outline.Transform.Pos = CutterButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = EnumOffset;
                    SelectedSlime = -1;
                    SelectedColor = Color.Black;
                };
                CutterButton.Function();
            }
            if (SwitchClicker.Click("Right"))
            {
                SwitchButton.Function();
                Color NewColor = SelectedColor == ColorHelp.RedSwitch ? ColorHelp.BlueSwitch : ColorHelp.RedSwitch;
                int NewValue = NewColor == ColorHelp.RedSwitch ? (int)CELLOBJECTS.RSWITCH : (int)CELLOBJECTS.BSWITCH;
                SwitchImage.Sprite.Color = NewColor;
                SwitchButton.Function = () =>
                {
                    Outline.Transform.Pos = SwitchButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = NewValue;
                    SelectedSlime = -1;
                    SelectedColor = NewColor;
                };
                SwitchButton.Function();
            }
            if (TintClicker.Click("Right"))
            {
                Color NewColor = TintButton.Sprite.Color == ColorHelp.RedSwitch ? ColorHelp.BlueSwitch : ColorHelp.RedSwitch;
                int NewValue = NewColor == ColorHelp.RedSwitch ? (int)CELLOBJECTS.RTINT : (int)CELLOBJECTS.BTINT;
                TintButton.Sprite.Color = NewColor;
                TintButton.Function = () =>
                {
                    Outline.Transform.Pos = TintButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = NewValue;
                    SelectedSlime = -1;
                    SelectedColor = NewColor;
                };
                TintButton.Function();
            }
        }
    }
    public class DebugGrid : Grid
    {
        private Clicker Clicker {  get; set; }
        private Polygon Pointer {  get; set; }
        private DebugMenu DebugMenu { get; }
        private Vector2 MPos { get; set; }
        private int SelectedSlime { get => DebugMenu.SelectedSlime; }
        private Cell MouseCell { get; set; }
        private CELLOBJECTS SelectedProperty 
        {
            get 
            {
                CELLOBJECTS Property = (CELLOBJECTS)(-1);
                if (Enum.IsDefined(typeof(CELLOBJECTS), DebugMenu.SelectedButton)) Property = (CELLOBJECTS)DebugMenu.SelectedButton;
                return Property;
            }
        }
        public DebugGrid(int X, int Y, ref DebugMenu NewDebugMenu) : base(X, Y, 11, 11, Color.DarkMagenta, 2)
        {
            Slimes[0].Transform.Pos = new Vector2(10, 10);
            DebugMenu = NewDebugMenu;
            Clicker = new Clicker(Transform);
            Pointer = new Polygon(new Transform(0, 0, 10, 10), ColorManager.None, "Circle");
            Pointer.Sprite.LayerData.LayerDepth += 2;
            Container.ObjectsToLoad.Add(Pointer);
        }
        public override void Update()
        {
            MPos = Mouse.GetState().Position.ToVector2();
            MouseCell = GetMouseCell();
            ShowPointer();
            Paint();
        }
        private void Paint()
        {
            if (Clicker.Hold())
            {
                if (MouseCell != null)
                {
                    if (SelectedSlime >= 0)
                    {
                        ResetSlimes();
                        Slime Slime = Slimes[SelectedSlime];
                        //Slime.MoveToCell(MouseCell);
                        Slime.TargetCell = MouseCell;
                        Slime.MoveToTarget();
                    }
                    else if (SelectedProperty >= 0)
                    {
                        if (MouseCell.Properties.Add(SelectedProperty))
                        {
                            SetCutters();
                            SetRedBlue();
                            SetGoals();
                            MouseCell.ParseProperties();
                        }
                    }
                    else if (MouseCell.Col != 0 && MouseCell.Col != Cols - 1 && MouseCell.Row != 0 && MouseCell.Row != Rows - 1)
                    {
                        MouseCell.Properties.Clear();
                        MouseCell.ParseProperties();
                        ResetSlimes();
                    }
                }
            }
        }
        private void SetCutters()
        {
            Cell Neighbor = null;
            bool ParseProperties = false;
            if (SelectedProperty == CELLOBJECTS.UCUTTER)
            {
                Neighbor = GetCell(MouseCell.Col, MouseCell.Row - 1);
                if (Neighbor != null) ParseProperties = Neighbor.Properties.Add(CELLOBJECTS.DCUTTER);
            }
            if (SelectedProperty == CELLOBJECTS.DCUTTER)
            {
                Neighbor = GetCell(MouseCell.Col, MouseCell.Row + 1);
                if (Neighbor != null) ParseProperties = Neighbor.Properties.Add(CELLOBJECTS.UCUTTER);
            }
            if (SelectedProperty == CELLOBJECTS.RCUTTER)
            {
                Neighbor = GetCell(MouseCell.Col + 1, MouseCell.Row);
                if (Neighbor != null) ParseProperties = Neighbor.Properties.Add(CELLOBJECTS.LCUTTER);
            }
            if (SelectedProperty == CELLOBJECTS.LCUTTER)
            {
                Neighbor = GetCell(MouseCell.Col - 1, MouseCell.Row);
                if (Neighbor != null) ParseProperties = Neighbor.Properties.Add(CELLOBJECTS.RCUTTER);
            }
            if (ParseProperties && Neighbor != null) Neighbor.ParseProperties(); 
        }
        private void SetRedBlue()
        {
            if (MouseCell.Properties.Contains(CELLOBJECTS.RTINT))
            {
                if (SelectedProperty == CELLOBJECTS.BTINT)
                {
                    MouseCell.Properties.Remove(CELLOBJECTS.RTINT);
                    MouseCell.RedProperties.Clear();
                    MouseCell.RedProperties.Add(CELLOBJECTS.RTINT);
                    MouseCell.BlueProperties.Add(CELLOBJECTS.BTINT);
                }
                else MouseCell.RedProperties.Add(SelectedProperty);
            }
            if (MouseCell.Properties.Contains(CELLOBJECTS.BTINT))
            {
                if (SelectedProperty == CELLOBJECTS.RTINT)
                {
                    MouseCell.Properties.Remove(CELLOBJECTS.BTINT);
                    MouseCell.BlueProperties.Clear();
                    MouseCell.RedProperties.Add(CELLOBJECTS.RTINT);
                    MouseCell.BlueProperties.Add(CELLOBJECTS.BTINT);
                }
                else MouseCell.BlueProperties.Add(SelectedProperty);
            }
        }
        private void SetGoals()
        {
            if (SelectedProperty >= CELLOBJECTS.GGOAL && SelectedProperty <= CELLOBJECTS.PGOAL)
            {
                foreach (Cell Cell in Cells)
                {
                    if (Cell != MouseCell && Cell.Properties.Contains(SelectedProperty))
                    {
                        Cell.Properties.Remove(SelectedProperty);
                        break;
                    }
                }
            }
        }
        private void ShowPointer()
        {
            Color PointerColor = ColorManager.None;
            if (Transform.Rect.Contains(MPos))
            {
                Pointer.Transform.Pos = MPos - new Vector2(5, 5);
                PointerColor = DebugMenu.SelectedColor;
            }
            Pointer.Sprite.Color = PointerColor;
        }
        private void ResetSlimes()
        {
            foreach (Slime Slime in Slimes)
            {
                if (Slime.ColRowVec == MouseCell.ColRowVec)
                {
                    Slime.TargetCell = null;
                    Slime.MoveToTarget();
                    //Slime.MoveToCell(null);
                    break;
                }
            }
        }
    }
}
