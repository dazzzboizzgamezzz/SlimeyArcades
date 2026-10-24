using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Slimey_Arcades
{
    class DebugSelect: IDraw, IUpdate, IContainer, INotifier
    {
        public Container Container { get; init; } = new();
        public Sprite Sprite { get; set; }
        public Transform Transform { get; set; }
        public Notifier Notifier { get; init; } = new();
        private Polygon Outline { get; init; }
        public DebugSelect()
        {
            Sprite = new Sprite(Shapes.Square, Color.Gray);
            Transform = new Transform(0, 0, 0, 0);
            Outline = new Polygon(new Transform(75, 45, 60, 60), Color.Gold);
            Outline.Sprite.Texture = Shapes.MakeOutline(Outline.Transform.Rect);
            Outline.Sprite.LayerData.LayerDepth = 2;
            Container.ObjectsToLoad.Add(Outline);

            for (int i = 0; i < 10; i++)
            {
                int X = 80;
                int Y = (i * 60) + 50;
                Transform ButtonTransform = new Transform(X, Y, 50, 50);
                List<Button> ButtonList = new();
                Button Button = null;
                Notification Notification = new Notification(NOTTYPES.PAINT);
                CELLOBJECTS Property = CELLOBJECTS.NONE;
                switch (i)
                {
                    case 0:
                        for (int j = 0; j < 5; j++)
                        {
                            //Color SlimeColor = ColorHelp.GreenSlime;
                            //switch (j)
                            //{
                            //case 0: SlimeColor = ColorHelp.GreenSlime; break;
                            //case 1: SlimeColor = ColorHelp.RedSlime; break;
                            //case 2: SlimeColor = ColorHelp.BlueSlime; break;
                            //case 3: SlimeColor = ColorHelp.YellowSlime; break;
                            //case 4: SlimeColor = ColorHelp.PinkSlime; break;
                            //}
                            //Button = new Button(ButtonTransform, BGColor: SlimeColor);
                            Slime Slime = new Slime(new Cell(new Transform(0, 0, 0, 0), Color.White), j);
                            //Notification.Data = Slime;
                            Button = new Button(ButtonTransform, BGColor: Slime.Sprite.Color);
                            Button.Function = () => 
                            { 
                                Notifier.SendNotification(new Notification(NOTTYPES.PAINT, Slime)); 
                                MoveOutline(); 
                            };
                            ButtonList.Add(Button);
                        }
                        break;
                    case 1:
                        for (int j = 0; j < 5; j++)
                        {
                            //Color SlimeColor = ColorHelp.GreenSlime;
                            //switch (j)
                            //{
                            //    case 0: SlimeColor = ColorHelp.GreenSlime; Property = CELLOBJECTS.GGOAL; break;
                            //    case 1: SlimeColor = ColorHelp.RedSlime; Property = CELLOBJECTS.RGOAL; break;
                            //    case 2: SlimeColor = ColorHelp.BlueSlime; Property = CELLOBJECTS.BGOAL; break;
                            //    case 3: SlimeColor = ColorHelp.YellowSlime; Property = CELLOBJECTS.YGOAL; break;
                            //    case 4: SlimeColor = ColorHelp.PinkSlime; Property = CELLOBJECTS.PGOAL; break;
                            //}
                            ////Notification.Data = Property;
                            //Button = new Button(ButtonTransform, Shapes.MakeOutline(ButtonTransform.Rect), SlimeColor, "Goal");
                            //Button.Function = () => 
                            //{ 
                            //    Notifier.SendNotification(new Notification(NOTTYPES.PAINT, Property)); 
                            //    MoveOutline(); 
                            //};
                            switch (j)
                            {
                                case 0: 
                                    Button = new Button(ButtonTransform, Shapes.MakeOutline(ButtonTransform.Rect), ColorHelp.GreenSlime, "Goal");
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.GGOAL)); MoveOutline(); };
                                    break;
                                case 1:
                                    Button = new Button(ButtonTransform, Shapes.MakeOutline(ButtonTransform.Rect), ColorHelp.RedSlime, "Goal");
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.RGOAL)); MoveOutline(); };
                                    break;
                                case 2:
                                    Button = new Button(ButtonTransform, Shapes.MakeOutline(ButtonTransform.Rect), ColorHelp.BlueSlime, "Goal");
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.BGOAL)); MoveOutline(); };
                                    break;
                                case 3:
                                    Button = new Button(ButtonTransform, Shapes.MakeOutline(ButtonTransform.Rect), ColorHelp.YellowSlime, "Goal");
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.YGOAL)); MoveOutline(); };
                                    break;
                                case 4:
                                    Button = new Button(ButtonTransform, Shapes.MakeOutline(ButtonTransform.Rect), ColorHelp.PinkSlime, "Goal");
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.PGOAL)); MoveOutline(); };
                                    break;
                            }
                            ButtonList.Add(Button);
                        }
                        break;
                    case 2:
                        Button = new Button(ButtonTransform, BGColor: ColorHelp.Wall);
                        Property = CELLOBJECTS.WALL;
                        Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(Notification); MoveOutline(); };
                        ButtonList.Add(Button);
                        break;
                    case 3:
                        Button = new Button(ButtonTransform, BGColor: ColorHelp.Ice);
                        Property = CELLOBJECTS.ICE;
                        Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(Notification); MoveOutline(); };
                        ButtonList.Add(Button);
                        break;
                    case 4:
                        Button = new Button(ButtonTransform, Shapes.BackedCircle, Color.White);
                        Property = CELLOBJECTS.PIT;
                        Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(Notification); MoveOutline(); };
                        ButtonList.Add(Button);
                        break;
                    case 5:
                        //Polygon Cross = null;
                        //Transform CrossTransform = null;
                        //for (int j = 0; j < 4; j++)
                        //{
                        //    switch (j)
                        //    {
                        //        case 0: CrossTransform = new Transform((int)ButtonTransform.Center.X, (int)ButtonTransform.Center.Y - 10, 10, 25); Property = CELLOBJECTS.LCUTTER; break;
                        //        case 1: CrossTransform = new Transform((int)ButtonTransform.Center.X - 10, (int)ButtonTransform.Center.Y - 10, 10, 25); Property = CELLOBJECTS.RCUTTER; break;
                        //        case 2: CrossTransform = new Transform((int)ButtonTransform.Center.X - 10, (int)ButtonTransform.Center.Y - 10, 25, 10); Property = CELLOBJECTS.UCUTTER; break;
                        //        case 3: CrossTransform = new Transform((int)ButtonTransform.Center.X - 10, (int)ButtonTransform.Center.Y, 25, 10); Property = CELLOBJECTS.DCUTTER; break;
                        //    }
                        //    Cross = new Polygon(CrossTransform, Color.Black);
                        //    Cross.Sprite.Texture = Shapes.XCross;
                        //    Button = new Button(ButtonTransform, BGColor: Color.Gray);
                        //    //Notification.Data = Property;
                        //    Button.Function = () => 
                        //    { 
                        //        Notifier.SendNotification(new Notification(NOTTYPES.PAINT, Property)); 
                        //        MoveOutline(); 
                        //    };
                        //    Button.Container.ObjectsToLoad.Add(Cross);
                        //    ButtonList.Add(Button);
                        //}
                        for (int j = 0; j < 4; j++)
                        {
                            Button = new Button(ButtonTransform, BGColor: Color.Gray);
                            Polygon Cross = new Polygon(Transform.None, Color.Black);
                            Cross.Sprite.Texture = Shapes.XCross;
                            switch (j)
                            {
                                case 0:
                                    Cross.Transform = new Transform((int)ButtonTransform.Center.X + 5, (int)ButtonTransform.Center.Y - 15, 10, 25);
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.RCUTTER)); MoveOutline(); };
                                    break;
                                case 1:
                                    Cross.Transform = new Transform((int)ButtonTransform.Center.X - 15, (int)ButtonTransform.Center.Y + 5, 25, 10);
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.DCUTTER)); MoveOutline(); };
                                    break;
                                case 2:
                                    Cross.Transform = new Transform((int)ButtonTransform.Center.X - 15, (int)ButtonTransform.Center.Y - 15, 10, 25);
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.LCUTTER)); MoveOutline(); };
                                    break;
                                case 3:
                                    Cross.Transform = new Transform((int)ButtonTransform.Center.X - 15, (int)ButtonTransform.Center.Y - 15, 25, 10);
                                    Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.UCUTTER)); MoveOutline(); };
                                    break;

                            }
                            Button.Container.ObjectsToLoad.Add(Cross);
                            ButtonList.Add(Button);
                        }
                        break;
                    case 6:
                        Button = new Button(ButtonTransform, BGColor: ColorHelp.Barrel);
                        Property = CELLOBJECTS.BARREL;
                        Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(Notification); MoveOutline(); };
                        ButtonList.Add(Button);
                        break;
                    case 7:
                        Button = new Button(ButtonTransform, BGColor: Color.Gray);
                        Polygon Circle = new Polygon(new Transform((int)ButtonTransform.Center.X - 8, (int)ButtonTransform.Center.Y - 8, 15, 15), ColorHelp.RedSwitch, "Circle");
                        Button.Container.ObjectsToLoad.Add(Circle);
                        //Property = CELLOBJECTS.RSWITCH;
                        //Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.RSWITCH)); MoveOutline(); };
                        ButtonList.Add(Button);
                        Button = new Button(ButtonTransform, BGColor: Color.Gray);
                        Circle = new Polygon(new Transform((int)ButtonTransform.Center.X - 8, (int)ButtonTransform.Center.Y - 8, 15, 15), ColorHelp.BlueSwitch, "Circle");
                        Button.Container.ObjectsToLoad.Add(Circle);
                        //Property = CELLOBJECTS.BSWITCH;
                        //Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.BSWITCH)); MoveOutline(); };
                        ButtonList.Add(Button);
                        break;
                    case 8:
                        Button = new Button(ButtonTransform, BGColor: ColorHelp.RedSwitch);
                        //Property = CELLOBJECTS.RTINT;
                        //Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.RTINT)); MoveOutline(); };
                        ButtonList.Add(Button);
                        Button = new Button(ButtonTransform, BGColor: ColorHelp.BlueSwitch);
                        //Property = CELLOBJECTS.BTINT;
                        //Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.PAINT, CELLOBJECTS.BTINT)); MoveOutline(); };
                        ButtonList.Add(Button);
                        break;
                    case 9:
                        Button = new Button(ButtonTransform, BGColor: Color.Gray);
                        Notification.Data = Property;
                        Button.Function = () => { Notifier.SendNotification(Notification); MoveOutline(); };
                        ButtonList.Add(Button);
                    break;
                }
                LoopingButton LoopingButton = new LoopingButton(ButtonList, Color.LightGray);
                Container.ObjectsToLoad.Add(LoopingButton);
            }
        }
        public void Update()
        {

        }
        private void MoveOutline()
        {
            int MouseOffset = (int)MouseControl.MousePosition.Y - 50;
            int Y = (MouseOffset - (MouseOffset % 60)) + 45;
            Vector2 NewPos = new Vector2(75, Y);
            Outline.Transform.Pos = NewPos;
        }
    }
    class NewDebugGrid : Grid
    {
        private CELLOBJECTS PaintProperty { get; set; } = (CELLOBJECTS)(-1);
        private int PaintSlime { get; set; } = 0;
        private Pointer Pointer { get; init; } = new();
        //public NewDebugGrid() : base(200, 50, 11, 11, Color.DarkMagenta, 2)
        public NewDebugGrid(int NewCols, int NewRows) : base(450, 50, NewCols, NewRows, Color.DarkMagenta, 2)
        {
            Pointer.Sprite.LayerData.LayerDepth = 3;
            Container.ObjectsToLoad.Add(Pointer);
            Container.ObjectsToLoad.Add(PaintSlime);
            SetBorders(true);
        }
        public override void Update()
        {
            if ((int)PaintProperty >= 0)
            {
                if (MouseControl.LeftHold(Transform))
                {
                    Cell MouseCell = GetMouseCell();
                    if (MouseCell == null || MouseCell.Row == 0 || MouseCell.Row == Rows - 1 || MouseCell.Col == 0 || MouseCell.Col == Cols - 1) return;
                    if (PaintProperty == CELLOBJECTS.NONE)
                    {
                        MouseCell.Properties.Clear();
                        ClearSlime(MouseCell);
                    }
                    else
                    {
                        if ((int)PaintProperty >= (int)CELLOBJECTS.GGOAL && (int)PaintProperty <= (int)CELLOBJECTS.PGOAL)
                        {
                            foreach (Cell Cell in Cells)
                            {
                                if (Cell.Properties.Contains(PaintProperty))
                                {
                                    Cell.Properties.Remove(PaintProperty);
                                    Cell.ParseProperties();
                                    break;
                                }
                            }
                        }
                        SetCutters();
                        MouseCell.Properties.Add(PaintProperty);
                    }
                    MouseCell.ParseProperties();
                }
            }
            else
            {
                if (MouseControl.LeftClick(Transform))
                {
                    Cell MouseCell = GetMouseCell();
                    if (MouseCell == null || MouseCell.Row == 0 || MouseCell.Row == Rows - 1 || MouseCell.Col == 0 || MouseCell.Col == Cols - 1) return;
                    Slime CellSlime = GetCellSlime(MouseCell);
                    ClearSlime(MouseCell);
                    Slimes[PaintSlime].TargetCell = GetMouseCell();
                    Slimes[PaintSlime].MoveToTarget();

                    if (MouseCell.Properties.Contains(CELLOBJECTS.WALL)) MouseCell.Properties.Remove(CELLOBJECTS.WALL);
                    if (MouseCell.Properties.Contains(CELLOBJECTS.PIT)) MouseCell.Properties.Remove(CELLOBJECTS.PIT);
                    if (MouseCell.Properties.Contains(CELLOBJECTS.BSWITCH)) MouseCell.Properties.Remove(CELLOBJECTS.BSWITCH);
                    if (MouseCell.Properties.Contains(CELLOBJECTS.RSWITCH)) MouseCell.Properties.Remove(CELLOBJECTS.RSWITCH);
                    if (MouseCell.Properties.Contains(CELLOBJECTS.BARREL)) MouseCell.Properties.Remove(CELLOBJECTS.BARREL);
                    MouseCell.ParseProperties();
                }
            }
        }
        protected override void ProcessNotifications(Notification Notification)
        {
            if (Notification.Type == NOTTYPES.PAINT)
            {
                if (Notification.Data.GetType() == typeof(CELLOBJECTS))
                {
                    PaintProperty = (CELLOBJECTS)Notification.Data;
                    switch (Notification.Data)
                    {
                        case CELLOBJECTS.GGOAL: Pointer.Sprite.Color = ColorHelp.GreenSlime; break;
                        case CELLOBJECTS.RGOAL: Pointer.Sprite.Color = ColorHelp.RedSlime; break;
                        case CELLOBJECTS.BGOAL: Pointer.Sprite.Color = ColorHelp.BlueSlime; break;
                        case CELLOBJECTS.YGOAL: Pointer.Sprite.Color = ColorHelp.YellowSlime; break;
                        case CELLOBJECTS.PGOAL: Pointer.Sprite.Color = ColorHelp.PinkSlime; break;
                        case CELLOBJECTS.WALL: Pointer.Sprite.Color = ColorHelp.Wall; break;
                        case CELLOBJECTS.PIT: Pointer.Sprite.Color = ColorHelp.Wall; break;
                        case CELLOBJECTS.ICE: Pointer.Sprite.Color = ColorHelp.Ice; break;
                        case CELLOBJECTS.BARREL: Pointer.Sprite.Color = ColorHelp.Barrel; break;
                        case CELLOBJECTS.RCUTTER: Pointer.Sprite.Color = ColorHelp.Wall; break;
                        case CELLOBJECTS.LCUTTER: Pointer.Sprite.Color = ColorHelp.Wall; break;
                        case CELLOBJECTS.DCUTTER: Pointer.Sprite.Color = ColorHelp.Wall; break;
                        case CELLOBJECTS.UCUTTER: Pointer.Sprite.Color = ColorHelp.Wall; break;
                        case CELLOBJECTS.RSWITCH: Pointer.Sprite.Color = ColorHelp.RedSwitch; break;
                        case CELLOBJECTS.BSWITCH: Pointer.Sprite.Color = ColorHelp.BlueSwitch; break;
                        case CELLOBJECTS.RTINT: Pointer.Sprite.Color = ColorHelp.RedSwitch; break;
                        case CELLOBJECTS.BTINT: Pointer.Sprite.Color = ColorHelp.BlueSwitch; break;
                    }
                }
                else if (Notification.Data.GetType() == typeof(Slime))
                {
                    PaintProperty = (CELLOBJECTS)(-1);
                    Slime Slime = (Slime)Notification.Data;
                    PaintSlime = Slime.Type;
                    //PaintSlime = new Slime(Cells[0, 0], Slime.Type);
                    Pointer.Sprite.Color = Slime.Sprite.Color;
                }
            }
        }
        private void ClearSlime(Cell Cell)
        {
            Slime CellSlime = GetCellSlime(Cell);
            if (CellSlime != null)
            {
                CellSlime.TargetCell = null;
                CellSlime.MoveToTarget();
            }
        }
        private void SetCutters()
        {
            Cell MouseCell = GetMouseCell();
            Cell AdjacentCell = null;
            switch (PaintProperty)
            {
                case CELLOBJECTS.RCUTTER:
                    AdjacentCell = GetCell(MouseCell.ColRowVec + Vector2.UnitX);
                    if (AdjacentCell != null)
                    {
                        AdjacentCell.Properties.Add(CELLOBJECTS.LCUTTER);
                    }
                    break;
                case CELLOBJECTS.LCUTTER:
                    AdjacentCell = GetCell(MouseCell.ColRowVec - Vector2.UnitX);
                    if (AdjacentCell != null)
                    {
                        AdjacentCell.Properties.Add(CELLOBJECTS.RCUTTER);
                    }
                    break;
                case CELLOBJECTS.DCUTTER:
                    AdjacentCell = GetCell(MouseCell.ColRowVec + Vector2.UnitY);
                    if (AdjacentCell != null)
                    {
                        AdjacentCell.Properties.Add(CELLOBJECTS.UCUTTER);
                    }
                    break;
                case CELLOBJECTS.UCUTTER:
                    AdjacentCell = GetCell(MouseCell.ColRowVec - Vector2.UnitY);
                    if (AdjacentCell != null)
                    {
                        AdjacentCell.Properties.Add(CELLOBJECTS.DCUTTER);
                    }
                    break;
            }
            if (AdjacentCell != null) AdjacentCell.ParseProperties();
        }
        public void SetBorders(bool SetBorders)
        {
            for (int i = 0; i < Rows; i++)
            {
                if (SetBorders)
                {
                    Cells[0, i].Properties.Add(CELLOBJECTS.WALL);
                    Cells[Cols - 1, i].Properties.Add(CELLOBJECTS.WALL);
                }
                else
                {
                    Cells[0, i].Properties.Remove(CELLOBJECTS.WALL);
                    Cells[Cols - 1, i].Properties.Remove(CELLOBJECTS.WALL);
                }
                Cells[0, i].ParseProperties();
                Cells[Cols - 1, i].ParseProperties();
            }
            for (int i = 0; i < Cols; i++)
            {
                if (SetBorders)
                {
                    Cells[i, 0].Properties.Add(CELLOBJECTS.WALL);
                    Cells[i, Rows - 1].Properties.Add(CELLOBJECTS.WALL);
                }
                else
                {
                    Cells[i, 0].Properties.Remove(CELLOBJECTS.WALL);
                    Cells[i, Rows - 1].Properties.Remove(CELLOBJECTS.WALL);
                }
                Cells[i, 0].ParseProperties();
                Cells[i, Rows - 1].ParseProperties();
            }
        }
        public void ResetGrid()
        {
            foreach (Cell Cell in Cells)
            {
                Cell.Properties.Clear();
                Cell.ParseProperties();
            }
            SetBorders(true);
        }
    }
    class Pointer: Polygon, IUpdate
    {
        public Pointer() : base(new Transform(0, 0, 10, 10), ColorHelp.GreenSlime, "Circle")
        {

        }
        public void Update()
        {
            Transform.Pos = MouseControl.MousePosition;
        }
    }
    
    //class ScreenScroll: IUpdate, INotifier, IDraw
    //{
    //    public Notifier Notifier { get; init; } = new();
    //    public Transform Transform { get; set; }
    //    public Sprite Sprite { get; set; }
    //    private Color HighlightColor { get; init; } = new Color(0, 0, 0, 0.5f);
    //    public bool IsActive { get; set; } = false;
    //    private Vector3 Direction { get; init; }
    //    public ScreenScroll(Transform NewTransform, Vector2 NewDirection)
    //    {
    //        Transform = NewTransform;
    //        Sprite = new Sprite(Shapes.Square, ColorHelp.None);
    //        Direction = new Vector3(NewDirection.X, NewDirection.Y, 0);
    //    }
    //    public void Update() 
    //    {
    //        if (IsActive && MouseControl.Hovering(Transform))
    //        {
    //            Notifier.SendNotification(new Notification(NOTTYPES.SCREENCHANGE, Direction));
    //            //Transform = new Transform(Transform.X + (int)Direction.X, Transform.Y - (int)Direction.Y, Transform.Width, Transform.Height);
    //            Sprite.Color = HighlightColor;
    //        }
    //        else
    //        {
    //            //Sprite.Color = ColorHelp.None;
    //        }
    //    }
    //}
    class ScreenMover : IContainer, IDraw, IUpdate, INotifier
    {
        public Sprite Sprite { get; set; }
        public Transform Transform { get; set; }
        public Container Container { get; init; } = new();
        public Notifier Notifier { get; init; } = new();
        public Rectangle OuterScreen { get; init; }
        private Rectangle InnerScreen { get => Transform.Rect; }
        private Polygon LeftScroll { get; init; }
        private Polygon RightScroll { get; init; }
        private Polygon UpScroll { get; init; }
        private Polygon DownScroll { get; init; }
        private Vector2 OffSet { get; set; } = Vector2.Zero;
        public ScreenMover(Rectangle NewOuterScreen)
        {
            Sprite = new Sprite(Shapes.Square, ColorHelp.None);
            Transform = new Transform(0, 0, (int)SETTINGS.SCREENWIDTH, (int)SETTINGS.SCREENHEIGHT);
            OuterScreen = NewOuterScreen;

            LeftScroll = new Polygon(new Transform(0, -50, 100, (int)SETTINGS.SCREENHEIGHT + 100), ColorHelp.None);
            Container.ObjectsToLoad.Add(LeftScroll);
            RightScroll = new Polygon(new Transform((int)SETTINGS.SCREENWIDTH - 100, -50, 100, (int)SETTINGS.SCREENHEIGHT + 100), ColorHelp.None);
            Container.ObjectsToLoad.Add(RightScroll);
            UpScroll = new Polygon(new Transform(-50, 0, (int)SETTINGS.SCREENWIDTH + 100, 100), ColorHelp.None);
            Container.ObjectsToLoad.Add(UpScroll);
            DownScroll = new Polygon(new Transform(-50, (int)SETTINGS.SCREENHEIGHT - 100, (int)SETTINGS.SCREENWIDTH + 100, 100), ColorHelp.None);
            Container.ObjectsToLoad.Add(DownScroll);
        }
        public void Update()
        {
            if (InnerScreen.X + InnerScreen.Width < OuterScreen.Width
            && (RightScroll.Transform.Rect.Contains(MouseControl.MousePosition)))
            {
                RightScroll.Sprite.Color = new Color(0, 0, 0, 0.5f);
                Notifier.SendNotification(new Notification(NOTTYPES.SCREENCHANGE, new Vector3(-5, 0, 0)));
                OffSet += new Vector2(5, 0);
                Container.MoveAllChildren(this, OffSet);
            }
            else
            {
                RightScroll.Sprite.Color = Color.Lerp(RightScroll.Sprite.Color, ColorHelp.None, 0.1f);
            }

            if (InnerScreen.X > OuterScreen.X
            && (LeftScroll.Transform.Rect.Contains(MouseControl.MousePosition)))
            {
                LeftScroll.Sprite.Color = new Color(0, 0, 0, 0.5f);
                Notifier.SendNotification(new Notification(NOTTYPES.SCREENCHANGE, new Vector3(5, 0, 0)));
                OffSet += new Vector2(-5, 0);
                Container.MoveAllChildren(this, OffSet);
            }
            else
            {
                LeftScroll.Sprite.Color = Color.Lerp(LeftScroll.Sprite.Color, ColorHelp.None, 0.1f);
            }

            if (InnerScreen.Y + InnerScreen.Height < OuterScreen.Height
            && (DownScroll.Transform.Rect.Contains(MouseControl.MousePosition)))
            {
                DownScroll.Sprite.Color = new Color(0, 0, 0, 0.5f);
                Notifier.SendNotification(new Notification(NOTTYPES.SCREENCHANGE, new Vector3(0, -5, 0)));
                OffSet += new Vector2(0, 5);
                Container.MoveAllChildren(this, OffSet);
            }
            else
            {
                DownScroll.Sprite.Color = Color.Lerp(DownScroll.Sprite.Color, ColorHelp.None, 0.1f);
            }

            if (InnerScreen.Y > OuterScreen.Y
            && (UpScroll.Transform.Rect.Contains(MouseControl.MousePosition)))
            {
                UpScroll.Sprite.Color = new Color(0, 0, 0, 0.5f);
                Notifier.SendNotification(new Notification(NOTTYPES.SCREENCHANGE, new Vector3(0, 5, 0)));
                OffSet += new Vector2(0, -5);
                Container.MoveAllChildren(this, OffSet);
            }
            else
            {
                UpScroll.Sprite.Color = Color.Lerp(UpScroll.Sprite.Color, ColorHelp.None, 0.1f);
            }
        }
    }
}
