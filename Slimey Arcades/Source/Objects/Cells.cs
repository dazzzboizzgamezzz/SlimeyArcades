//using Slimey_Arcades.Managers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Slimey_Arcades
{
    public class Cell : Polygon, IContainer
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Vector2 ColRowVec { get => new Vector2(Col, Row); set { Col = (int)value.X; Row = (int) value.Y; } }
        public HashSet<CELLOBJECTS> Properties { get; set; } = new();
        public HashSet<CELLOBJECTS> RedProperties { get; set; } = new();
        public HashSet<CELLOBJECTS> BlueProperties { get; set; } = new();
        public Container Container { get; init; } = new();
        public Slime Barrel { get; set; } = null;
        public int CellGap { get; set; } = 0;
        public Cell(Transform NewTransform, Color BGColor, int NewCol = 0, int NewRow = 0) : base(NewTransform, BGColor, "Square")
        {
            Row = NewRow;
            Col = NewCol;
            ParseProperties();
        }
        public void ParseProperties()
        {
            Container.Clear();
            foreach (CELLOBJECTS Property in Properties)
            {
                Polygon NewObject = new Polygon(Transform, Color.Black);
                NewObject.Sprite.LayerData.LayerIndex = 9;
                switch (Property)
                {
                    case CELLOBJECTS.WALL: 
                        break;
                    case CELLOBJECTS.ICE: 
                        NewObject.Sprite.Color = ColorManager.Colors["Ice"]; 
                        break;
                    case CELLOBJECTS.PIT: 
                        NewObject.Sprite.Texture = Shapes.Circle; 
                        break;
                    case CELLOBJECTS.RCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X + Transform.Width - 7, Transform.Y, 16, Transform.Height);
                        break;
                    case CELLOBJECTS.LCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X - 7, Transform.Y, 16, Transform.Height);
                        break;
                    case CELLOBJECTS.UCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X, Transform.Y - 7, Transform.Width, 16);
                        break;
                    case CELLOBJECTS.DCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X, Transform.Y + Transform.Height - 7, Transform.Width, 16);
                        break;
                    case CELLOBJECTS.RSWITCH:
                        NewObject.Sprite.Texture = Shapes.Circle;
                        NewObject.Sprite.Color = ColorHelp.RedSwitch;
                        NewObject.Transform = new Transform((int)Transform.Center.X - 10, (int)Transform.Center.Y - 10, 20, 20);
                        break;
                    case CELLOBJECTS.BSWITCH:
                        NewObject.Sprite.Texture = Shapes.Circle;
                        NewObject.Sprite.Color = ColorHelp.BlueSwitch;
                        NewObject.Transform = new Transform((int)Transform.Center.X - 10, (int)Transform.Center.Y - 10, 20, 20);
                        break;
                    case CELLOBJECTS.RTINT:
                        //NewObject.Sprite.Color = new Color(255, 0, 0, 0.3f);
                        NewObject.Transform = new Transform(Transform.X - CellGap, Transform.Y - CellGap, Transform.Width + (CellGap * 2), Transform.Height + (CellGap *2));
                        NewObject.Sprite.Texture = Shapes.MakeOutline(NewObject.Transform.Rect, CellGap * 2);
                        NewObject.Sprite.Color = ColorHelp.RedSwitch;
                        NewObject.Sprite.LayerData.LayerIndex = 8;
                        break;
                    case CELLOBJECTS.BTINT:
                        //NewObject.Sprite.Color = new Color(0, 0, 255, 0.3f);
                        NewObject.Transform = new Transform(Transform.X - CellGap, Transform.Y - CellGap, Transform.Width + (CellGap * 2), Transform.Height + (CellGap * 2));
                        NewObject.Sprite.Texture = Shapes.MakeOutline(NewObject.Transform.Rect, CellGap * 2);
                        NewObject.Sprite.Color = ColorHelp.BlueSwitch;
                        NewObject.Sprite.LayerData.LayerIndex = 8;
                        break;
                    case CELLOBJECTS.GGOAL:
                        SetGoal("GGoal");
                        NewObject = null;
                        break;
                    case CELLOBJECTS.RGOAL:
                        SetGoal("RGoal");
                        NewObject = null;
                        break;
                    case CELLOBJECTS.BGOAL:
                        SetGoal("BGoal");
                        NewObject = null;
                        break;
                    case CELLOBJECTS.YGOAL:
                        SetGoal("YGoal");
                        NewObject = null;
                        break;
                    case CELLOBJECTS.PGOAL:
                        SetGoal("PGoal");
                        NewObject = null;
                        break;
                    case CELLOBJECTS.BARREL:
                        NewObject = null;
                        Slime NewBarrel = new Slime(this, 5);
                        NewBarrel.CanStick = false;
                        Barrel = NewBarrel;
                        Container.ObjectsToLoad.Add(Barrel);
                        break;
                }
                if (NewObject != null) Container.ObjectsToLoad.Add(NewObject);
            }
        }
        public bool HasCutter()
        {
            if (Properties.Contains(CELLOBJECTS.RCUTTER) || Properties.Contains(CELLOBJECTS.LCUTTER) ||
                Properties.Contains(CELLOBJECTS.DCUTTER) || Properties.Contains(CELLOBJECTS.UCUTTER))
            {
                return true; 
            }
            return false;
        }
        public List<CELLOBJECTS> GetCutters()
        {
            List<CELLOBJECTS> CutterType = new();

            if (Properties.Contains(CELLOBJECTS.RCUTTER)) CutterType.Add(CELLOBJECTS.RCUTTER);
            if (Properties.Contains(CELLOBJECTS.LCUTTER)) CutterType.Add(CELLOBJECTS.LCUTTER);
            if (Properties.Contains(CELLOBJECTS.DCUTTER)) CutterType.Add(CELLOBJECTS.DCUTTER);
            if (Properties.Contains(CELLOBJECTS.UCUTTER)) CutterType.Add(CELLOBJECTS.UCUTTER);

            return CutterType;
        }
        private void SetGoal(string GoalType)
        {
            Color TextColor = ColorManager.None;
            switch (GoalType.Substring(0, 1))
            {
                case "G": TextColor = ColorManager.Colors["GSlime"]; break;
                case "R": TextColor = ColorManager.Colors["RSlime"]; break;
                case "B": TextColor = ColorManager.Colors["BSlime"]; break;
                case "Y": TextColor = ColorManager.Colors["YSlime"]; break;
                case "P": TextColor = ColorManager.Colors["PSlime"]; break;
            }
            //Transform TextTransform = new Transform(Transform.Rect);
            Transform TextTransform = new Transform(Transform.X, Transform.Y, Transform.Width, Transform.Height);
            Text GoalText = new Text(TextTransform, "Goal");
            GoalText.Color = TextColor;
            GoalText.Pos = new Vector2(Transform.Center.X - (GoalText.TextWidth / 2) - 5, Transform.Center.Y - (GoalText.TextHeight / 2) - 2);
            Polygon GoalOutline = new Polygon(new Transform(Transform.X - 1, Transform.Y - 1, Transform.Width + 2, Transform.Height + 2), TextColor);
            GoalOutline.Sprite.Texture = Shapes.MakeOutline(GoalOutline.Transform.Rect, 6);
            Container.ObjectsToLoad.Add(GoalText);
            Container.ObjectsToLoad.Add(GoalOutline);
        }
    }
    public class Slime : Cell
    {
        public Cell TargetCell { get; set; }
        public int DistanceToCenter { get; set; }
        public int Type { get; init; }
        public bool CanStick { get; set; } = true;
        public bool WasCut { get; set; } = false;
        public Slime(Cell Cell, int NewSlimeType) : base(new Transform(Cell.Transform.X + 5, Cell.Transform.Y + 5, Cell.Transform.Width - 10, Cell.Transform.Height - 10), Cell.Sprite.Color, Cell.Col, Cell.Row)
        {
            Type = NewSlimeType;
            TargetCell = Cell;
            switch (Type)
            {
                case 0: Sprite.Color = ColorHelp.GreenSlime; break;
                case 1: Sprite.Color = ColorHelp.RedSlime; break;
                case 2: Sprite.Color = ColorHelp.BlueSlime; break;
                case 3: Sprite.Color = ColorHelp.YellowSlime; break;
                case 4: Sprite.Color = ColorHelp.PinkSlime; break;
                case 5: Sprite.Color = ColorHelp.Barrel; break;
            }
            Properties = null;
            RedProperties = null;
            BlueProperties = null;
        }
        public void MoveToTarget()
        {
            if (TargetCell == null)
            {
                Transform.Pos = new Vector2(-100, -100);
                Col = -1;
                Row = -1;
            }
            else
            {
                Transform.Pos = TargetCell.Transform.Pos + new Vector2(5, 5);
                Col = TargetCell.Col;
                Row = TargetCell.Row;
            }
        }
    }
    public enum CELLOBJECTS
    {
        NONE,
        WALL,
        ICE,
        PIT,
        RCUTTER,
        LCUTTER,
        UCUTTER,
        DCUTTER,
        RSWITCH,
        BSWITCH,
        BARREL,
        RTINT,
        BTINT,
        GGOAL,
        RGOAL,
        BGOAL,
        YGOAL,
        PGOAL,
    }
}
