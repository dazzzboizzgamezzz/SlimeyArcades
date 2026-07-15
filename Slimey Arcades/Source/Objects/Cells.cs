using Slimey_Arcades.Managers;
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
        public Container Container { get; init; } = new();
        public Cell(Transform NewTransform, Color BGColor, int NewCol = 0, int NewRow = 0) : base(NewTransform, BGColor, "Square")
        {
            Row = NewRow;
            Col = NewCol;
            //Index = Newindex;
            ParseProperties();
        }
        public void ParseProperties()
        {
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
                    case CELLOBJECTS.SWITCH:
                        NewObject.Sprite.Texture = Shapes.Circle;
                        NewObject.Sprite.Color = Color.Red;
                        NewObject.Transform = new Transform((int)Transform.Center.X - 10, (int)Transform.Center.Y - 10, 20, 20);
                        break;
                    case CELLOBJECTS.RTINT:
                        NewObject.Sprite.Color = new Color(255, 0, 0, 0.5f);
                        NewObject.Sprite.LayerData.LayerIndex = 8;
                        break;
                    case CELLOBJECTS.BTINT:
                        NewObject.Sprite.Color = new Color(0, 0, 255, 0.5f);
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
                }
                if (NewObject != null) Container.ObjectsToLoad.Add(NewObject);
            }
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
            Transform TextTransform = new Transform(Transform.Rect);
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
        public Slime(Cell Cell, string NewType, int NewSlimeType) : base(new Transform(Cell.Transform.X + 5, Cell.Transform.Y + 5, Cell.Transform.Width - 10, Cell.Transform.Height - 10), Cell.Sprite.Color, Cell.Col, Cell.Row)
        {
            Type = NewSlimeType;
            switch (Type)
            {
                case 0: Sprite.Color = ColorManager.Colors["GSlime"]; break;
                case 1: Sprite.Color = ColorManager.Colors["RSlime"]; break;
                case 2: Sprite.Color = ColorManager.Colors["BSlime"]; break;
                case 3: Sprite.Color = ColorManager.Colors["YSlime"]; break;
                case 4: Sprite.Color = ColorManager.Colors["PSlime"]; break;
            }
        }
        public void MoveToCell(Cell Cell)
        {
            if (Cell == null)
            {
                Transform.Pos = new Vector2(-100, -100);
                Col = -1;
                Row = -1;
            }
            else
            {
                Transform.Pos = Cell.Transform.Pos + new Vector2(5, 5);
                Col = Cell.Col;
                Row = Cell.Row;
            }
            TargetCell = Cell;
        }
    }
    public enum CELLOBJECTS
    {
        WALL,
        ICE,
        PIT,
        RCUTTER,
        LCUTTER,
        UCUTTER,
        DCUTTER,
        SWITCH,
        RTINT,
        BTINT,
        GGOAL,
        RGOAL,
        BGOAL,
        YGOAL,
        PGOAL,
    }
}
