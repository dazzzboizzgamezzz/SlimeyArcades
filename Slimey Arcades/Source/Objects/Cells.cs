using Slimey_Arcades.Managers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace Slimey_Arcades.Objects
{
    public class Cell : Polygon, IContainer
    {
        //public int LoadedCount { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public Vector2 ColRowVec { get => new Vector2(Col, Row); set { Col = (int)value.X; Row = (int) value.Y; } }
        //public int Index { get; set; }
        //public List<string> Properties { get; set; } = new();
        public HashSet<CellObjects> Properties { get; set; } = new();
        public Container Container { get; init; } = new();
        public Cell(Transform NewTransform, Color BGColor, int NewCol = 0, int NewRow = 0) : base(NewTransform, BGColor, "Square")
        {
            Row = NewRow;
            Col = NewCol;
            //Index = Newindex;
            NewParseProperties();
        }
        //public void ParseProperties()
        //{
        //    for (int i = 0; i < Properties.Count; i++)
        //    {
        //        string Type = Properties[i];
        //        Polygon Cutter = null;
        //        Polygon Hole = null;
        //        Polygon Switch = null;
        //        Polygon RedBlueMarker = null;
        //        switch (Type)
        //        {
        //            case "Wall":
        //                Sprite.Color = ColorManager.Colors["Wall"];
        //                break;
        //            case "Ice":
        //                Sprite.Color = ColorManager.Colors["Ice"];
        //                break;
        //            case "Pit":
        //                Hole = new Polygon(Transform, ColorManager.Colors["Pit"]);
        //                Hole.Sprite.Texture = Shapes.BackedCircle;
        //                Container.ObjectsToLoad.Add(Hole);
        //                break;
        //            case "Switch":
        //                Switch = new Polygon(new Transform(Transform.X + 15, Transform.Y + 15, 20, 20), Color.Red, "Circle");
        //                Container.ObjectsToLoad.Add(Switch);
        //                break;
        //            case "LCutter":
        //                Cutter = new Polygon(new Transform(Transform.X + 45, Transform.Y, 12, 50), Color.Black);
        //                Cutter.Sprite.Texture = Shapes.XCross;
        //                Container.ObjectsToLoad.Add(Cutter);
        //                break;
        //            case "RCutter":
        //                Cutter = new Polygon(new Transform(Transform.X - 7, Transform.Y, 12, 50), Color.Black);
        //                Cutter.Sprite.Texture = Shapes.XCross;
        //                Container.ObjectsToLoad.Add(Cutter);
        //                break;
        //            case "UCutter":
        //                Cutter = new Polygon(new Transform(Transform.X, Transform.Y - 7, 50, 12), Color.Black);
        //                Cutter.Sprite.Texture = Shapes.XCross;
        //                Container.ObjectsToLoad.Add(Cutter);
        //                break;
        //            case "DCutter":
        //                Cutter = new Polygon(new Transform(Transform.X, Transform.Y + 45, 50, 12), Color.Black);
        //                Cutter.Sprite.Texture = Shapes.XCross;
        //                Container.ObjectsToLoad.Add(Cutter);
        //                break;
        //            case "Red":
        //                RedBlueMarker = new Polygon(Transform, new Color(255, 0, 0, 0.5f));
        //                Container.ObjectsToLoad.Add(RedBlueMarker);
        //                break;
        //            case "Blue":
        //                RedBlueMarker = new Polygon(Transform, new Color(0, 0, 255, 0.5f));
        //                Container.ObjectsToLoad.Add(RedBlueMarker);
        //                break;
        //        }
        //        if (Type.Contains("Goal"))
        //        {
        //            Color TextColor = ColorManager.None;
        //            //string test = Type.Substring(0, 1);
        //            switch (Type.Substring(0, 1))
        //            {
        //                case "G": TextColor = ColorManager.Colors["GSlime"]; break;
        //                case "R": TextColor = ColorManager.Colors["RSlime"]; break;
        //                case "B": TextColor = ColorManager.Colors["BSlime"]; break;
        //                case "Y": TextColor = ColorManager.Colors["YSlime"]; break;
        //                case "P": TextColor = ColorManager.Colors["PSlime"]; break;
        //            }
        //            Transform TextTransform = new Transform(Transform.Rect);
        //            Text GoalText = new Text(TextTransform, "Goal");
        //            GoalText.Color = TextColor;
        //            GoalText.Pos = new Vector2(Transform.Center.X - (GoalText.TextWidth / 2), Transform.Center.Y - (GoalText.TextHeight / 2));
        //            Polygon GoalOutline = new Polygon(new Transform(Transform.X - 4, Transform.Y - 4, Transform.Width + 8, Transform.Height + 8), TextColor);
        //            GoalOutline.Sprite.Texture = Shapes.MakeOutline(GoalOutline.Transform.Rect, 6);
        //            Container.ObjectsToLoad.Add(GoalText);
        //            Container.ObjectsToLoad.Add(GoalOutline);
        //        }
        //    }
        //}
        public void NewParseProperties()
        {
            foreach (CellObjects Property in Properties)
            {
                Polygon NewObject = new Polygon(Transform, Color.Black);
                NewObject.Sprite.LayerData.LayerIndex = 9;
                switch (Property)
                {
                    case CellObjects.WALL: 
                        break;
                    case CellObjects.ICE: 
                        NewObject.Sprite.Color = ColorManager.Colors["Ice"]; 
                        break;
                    case CellObjects.PIT: 
                        NewObject.Sprite.Texture = Shapes.Circle; 
                        break;
                    case CellObjects.RCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X + Transform.Width - 5, Transform.Y, 10, Transform.Height);
                        break;
                    case CellObjects.LCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X - 10, Transform.Y, 10, Transform.Height);
                        break;
                    case CellObjects.UCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X, Transform.Y - 10, Transform.Width, 10);
                        break;
                    case CellObjects.DCUTTER:
                        NewObject.Sprite.Texture = Shapes.XCross;
                        NewObject.Transform = new Transform(Transform.X, Transform.Y + Transform.Height - 5, Transform.Width, 10);
                        break;
                    case CellObjects.SWITCH:
                        NewObject.Sprite.Texture = Shapes.Circle;
                        NewObject.Sprite.Color = Color.Red;
                        NewObject.Transform = new Transform((int)Transform.Center.X - 10, (int)Transform.Center.Y - 10, 20, 20);
                        break;
                    case CellObjects.RTINT:
                        NewObject.Sprite.Color = new Color(255, 0, 0, 0.5f);
                        NewObject.Sprite.LayerData.LayerIndex = 8;
                        break;
                    case CellObjects.BTINT:
                        NewObject.Sprite.Color = new Color(0, 0, 255, 0.5f);
                        NewObject.Sprite.LayerData.LayerIndex = 8;
                        break;
                    case CellObjects.GGOAL:
                        SetGoal("GGoal");
                        NewObject = null;
                        break;
                    case CellObjects.RGOAL:
                        SetGoal("RGoal");
                        NewObject = null;
                        break;
                    case CellObjects.BGOAL:
                        SetGoal("BGoal");
                        NewObject = null;
                        break;
                    case CellObjects.YGOAL:
                        SetGoal("YGoal");
                        NewObject = null;
                        break;
                    case CellObjects.PGOAL:
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
        public int Distance { get; set; }
        //public string Type { get => Properties[0]; }
        public int Type { get; init; }
        public Slime(Cell Cell, string NewType, int NewSlimeType) : base(new Transform(Cell.Transform.X + 5, Cell.Transform.Y + 5, Cell.Transform.Width - 10, Cell.Transform.Height - 10), Cell.Sprite.Color, Cell.Col, Cell.Row)
        {
            //Properties.Add(NewType);
            //Sprite.Color = ColorManager.Colors[NewType];
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
    public enum CellObjects
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
    //public class CellObjects
    //{
    //    public Dictionary<string, Polygon> Objects { get; set; } = new();
    //    public CellObjects(Transform CellTransform)
    //    {
    //        Polygon Wall = new Polygon(CellTransform, ColorManager.Colors["Wall"]);
    //        //Wall.Sprite.Layer = 0.000008f;
    //        Objects.Add("Wall", Wall);

    //        Polygon Ice = new Polygon(CellTransform, ColorManager.Colors["Ice"]);
    //        //Ice.Sprite.Layer = 0.000005f;
    //        Objects.Add("Ice", Ice);

    //        Polygon Pit = new Polygon(CellTransform, Color.White);
    //        Pit.Sprite.Texture = Shapes.BackedCircle;
    //        //Pit.Sprite.Layer = 0.000004f;
    //        Objects.Add("Pit", Pit);

    //        Transform RCutterTransform = new Transform(CellTransform.X + 45, CellTransform.Y, 12, 50);
    //        Transform LCutterTransform = new Transform(CellTransform.X - 7, CellTransform.Y, 12, 50);
    //        Transform UCutterTransform = new Transform(CellTransform.X, CellTransform.Y - 7, 50, 12);
    //        Transform DCutterTransform = new Transform(CellTransform.X, CellTransform.Y + 45, 50, 12);
    //        Polygon UCutter = new Polygon(UCutterTransform, Color.White);
    //        Pit.Sprite.Texture = Shapes.XCross;
    //        //Pit.Sprite.Layer = 0.000007f;
    //        //Objects.Add("UCutter", UCutter);

    //        Polygon DCutter = new Polygon(DCutterTransform, Color.White);
    //        Pit.Sprite.Texture = Shapes.XCross;
    //        //Pit.Sprite.Layer = 0.000007f;
    //        //Objects.Add("DCutter", DCutter);

    //        Polygon LCutter = new Polygon(LCutterTransform, Color.White);
    //        Pit.Sprite.Texture = Shapes.XCross;
    //        //Pit.Sprite.Layer = 0.000007f;
    //        //Objects.Add("LCutter", LCutter);

    //        Polygon RCutter = new Polygon(RCutterTransform, Color.White);
    //        Pit.Sprite.Texture = Shapes.XCross;
    //        //Pit.Sprite.Layer = 0.000007f;
    //        //Objects.Add("RCutter", RCutter);

    //        Transform SwitchTransform = new Transform((int)CellTransform.Center.X - 5, (int)CellTransform.Center.Y - 5, 10, 10);
    //        Polygon RedSwitch = new Polygon(SwitchTransform, Color.Red, "Circle");
    //        Polygon BlueSwitch = new Polygon(SwitchTransform, Color.Blue, "Circle");
    //        //RedSwitch.Sprite.Layer = 0.000006f;
    //        //BlueSwitch.Sprite.Layer = 0.000006f;
    //        Objects.Add("RedSwitch", RedSwitch);
    //        Objects.Add("BlueSwitch", BlueSwitch);

    //        Polygon RedTint = new Polygon(CellTransform, new Color(255, 0, 0, 0.5f));
    //        Polygon BlueTint = new Polygon(CellTransform, new Color(0, 0, 255, 0.5f));
    //        //RedTint.Sprite.Layer = 0.000009f;
    //        //BlueTint.Sprite.Layer = 0.000009f;
    //        Objects.Add("RedTint", RedTint);
    //        Objects.Add("BueTint", BlueTint);
    //    }
    //}

}
