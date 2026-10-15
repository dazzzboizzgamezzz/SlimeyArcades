using System.Collections;
using System.Collections.Generic;
using System.Net;
using Microsoft.Xna.Framework;

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
        public NewDebugGrid() : base(200, 50, 11, 11, Color.DarkMagenta, 2)
        {
            Pointer.Sprite.LayerData.LayerDepth = 3;
            Container.ObjectsToLoad.Add(Pointer);
            //PaintSlime = new Slime(Cells[0, 0], 0);
            //PaintSlime.Sprite.IsVisible = false;
            Container.ObjectsToLoad.Add(PaintSlime);
        }
        public override void Update()
        {
            if ((int)PaintProperty >= 0)
            {
                if (MouseControl.LeftHold(Transform))
                {
                    Cell MouseCell = GetMouseCell();
                    if (PaintProperty == CELLOBJECTS.NONE)
                    {
                        MouseCell.Properties.Clear();
                        ClearSlime(MouseCell);
                    }
                    else
                    {
                        //if (PaintProperty == CELLOBJECTS.WALL || PaintProperty == CELLOBJECTS.PIT)
                        //{
                        //    foreach(CELLOBJECTS Property in MouseCell.Properties)
                        //    {
                        //        if (Property != CELLOBJECTS.RCUTTER &&
                        //            Property != CELLOBJECTS.LCUTTER &&
                        //            Property != CELLOBJECTS.UCUTTER && 
                        //            Property != CELLOBJECTS.DCUTTER &&
                        //            Property != CELLOBJECTS.RTINT &&
                        //            Property != CELLOBJECTS.BTINT)
                        //        {
                        //            MouseCell.Properties.Remove(Property);
                        //        }
                        //    }
                        //}
                        if ((int)PaintProperty >= (int)CELLOBJECTS.GGOAL && (int)PaintProperty <= (int)CELLOBJECTS.PGOAL)
                        {
                            //if (MouseCell.Properties.Contains(CELLOBJECTS.GGOAL)) MouseCell.Properties.Remove(CELLOBJECTS.GGOAL);
                            //if (MouseCell.Properties.Contains(CELLOBJECTS.RGOAL)) MouseCell.Properties.Remove(CELLOBJECTS.RGOAL);
                            //if (MouseCell.Properties.Contains(CELLOBJECTS.BGOAL)) MouseCell.Properties.Remove(CELLOBJECTS.BGOAL);
                            //if (MouseCell.Properties.Contains(CELLOBJECTS.YGOAL)) MouseCell.Properties.Remove(CELLOBJECTS.YGOAL);
                            //if (MouseCell.Properties.Contains(CELLOBJECTS.PGOAL)) MouseCell.Properties.Remove(CELLOBJECTS.PGOAL);
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
                    if (MouseCell == null) return;
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
}
