using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Managers;
using Slimey_Arcades.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Slimey_Arcades.Objects
{
    public class DebugMenu : Grid 
    {
        private Polygon Outline { get; set; }
        //public static string SelectedButton { get; set; }
        public int SelectedButton { get; set; } = -1;
        public int SelectedSlime { get; set; } = 0;
        //public int SelectedGoal { get; set; } = -1;
        public Color SelectedColor {  get; set; }
        private int CurrentSlime { get; set; } = 0;
        private int CurrentGoal { get; set; } = (int)CellObjects.GGOAL;
        private Button SlimeButton { get; set; }
        private Button GoalButton { get; set; }
        //public int SelectedSlime { get; set; } = 0;
        //public int SelectedGoal { get; set; } = 0;
        private Clicker SlimeClicker { get; set; }
        private Clicker GoalClicker { get; set; }
        public DebugMenu(int X, int Y) : base(X, Y, 1, 10, Color.DarkMagenta)
        {
            //SelectedButton = "GSlime";
            //NewSelectedButton = -1;
            Outline = new Polygon(new Transform(X - 5, Y - 5, 60, 60), Color.Gold);
            Outline.Sprite.Texture = Shapes.MakeOutline(new Rectangle(X + 5, Y + 5, 50, 50));
            Container.ObjectsToLoad.Add(Outline);

            for (int i = 0; i < Rows; i++)
            {
                Cells[0, i].Sprite.Color = ColorManager.None;
                Button NewButton = new Button(Cells[0, i].Transform, BGColor: Color.Gray);
                //string ButtonName = "";
                int ButtonValue = -1;
                int SelectedSlime = -1;
                Color TestingColor = ColorManager.None;
                Color ButtonColor = Color.Gray;
                switch (i)
                {
                    case 0: 
                        //ButtonName = "GSlime"; 
                        ButtonColor = ColorManager.Colors["GSlime"];
                        SlimeButton = NewButton;
                        SelectedSlime = CurrentSlime;
                        break;
                    case 1:
                        //ButtonName = "GGoal";
                        NewButton.Text = "Goal";
                        NewButton.TextColor = ColorManager.Colors["GSlime"];
                        GoalButton = NewButton;
                        ButtonValue = CurrentGoal;
                        TestingColor = ColorManager.Colors["GSlime"];
                        break;
                    case 2: 
                        //ButtonName = "Wall"; 
                        ButtonColor = Color.Black;
                        ButtonValue = (int)CellObjects.WALL;
                        break;
                    case 3: 
                        //ButtonName = "Ice"; 
                        ButtonColor = ColorManager.Colors["Ice"];
                        ButtonValue = (int)CellObjects.ICE;
                        break;
                    case 4:
                        //ButtonName = "Pit";
                        ButtonColor = ColorManager.Colors["Pit"];
                        NewButton.Sprite.Texture = Shapes.BackedCircle;
                        ButtonValue = (int)CellObjects.PIT;
                        break;
                    case 5:
                        //ButtonName = "Cutter";
                        break;
                    case 6:
                        //ButtonName = "Barrel";
                        ButtonColor = ColorManager.Colors["Barrel"];
                        break;
                    case 7: 
                        //ButtonName = "Switch";
                        break;
                    case 8: 
                        //ButtonName = "RedBlue";
                        break;
                }
                NewButton.Function = () => 
                {
                    Outline.Transform.Pos = NewButton.Transform.Pos - new Vector2(5, 5);
                    //SelectedButton = ButtonName;
                    SelectedButton = ButtonValue;
                    this.SelectedSlime = SelectedSlime;
                    SelectedColor = ButtonColor;
                    if (TestingColor != ColorManager.None) SelectedColor = TestingColor;
                };
                NewButton.Sprite.Color = ButtonColor;
                NewButton.Highlightable = false;
                Container.ObjectsToLoad.Add(NewButton);
            }
            SlimeClicker = new Clicker(Cells[0, 0].Transform);
            GoalClicker = new Clicker(Cells[0, 1].Transform);

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
                //int NextSlime = (SelectedSlime + 1) % 5;
                CurrentSlime = (CurrentSlime + 1) % 5;
                //string SlimeName = "";
                Color SlimeColor = ColorManager.None;
                switch (CurrentSlime)
                {
                    //case 0: SlimeName = "GSlime"; break;
                    //case 1: SlimeName = "RSlime"; break;
                    //case 2: SlimeName = "BSlime"; break;
                    //case 3: SlimeName = "YSlime"; break;
                    //case 4: SlimeName = "PSlime"; break;
                    case 0: SlimeColor = ColorManager.Colors["GSlime"]; break;
                    case 1: SlimeColor = ColorManager.Colors["RSlime"]; break;
                    case 2: SlimeColor = ColorManager.Colors["BSlime"]; break;
                    case 3: SlimeColor = ColorManager.Colors["YSlime"]; break;
                    case 4: SlimeColor = ColorManager.Colors["PSlime"]; break;
                }
                //if (SelectedButton.Contains("Slime")) SelectedButton = SlimeName;
                //SlimeButton.Sprite.Color = ColorManager.Colors[SlimeName];
                SlimeButton.Sprite.Color = SlimeColor;
                SlimeButton.Function = () =>
                {
                    Outline.Transform.Pos = SlimeButton.Transform.Pos - new Vector2(5, 5);
                    //SelectedButton = SlimeName;
                    SelectedButton = -1;
                    SelectedSlime = CurrentSlime;
                    SelectedColor = SlimeColor;
                };
                //SelectedSlime = NextSlime;

                //CurrentSlime = NextSlime;
                //NewSelectedSlime = NextSlime;
                SlimeButton.Function();
            }
            if (GoalClicker.Click("Right"))
            {
                //int NextGoal = (SelectedGoal + 1) % 5;
                CurrentGoal = ((CurrentGoal + 1) % 5) + (int)CellObjects.GGOAL;
                //string GoalName = "";
                //string SlimeName = "";
                Color GoalColor = ColorManager.None;
                switch (CurrentGoal)
                {
                    //case (int)CellObjects.GGOAL: GoalName = "GGoal"; break;
                    //case (int)CellObjects.RGOAL: GoalName = "RGoal"; break;
                    //case (int)CellObjects.BGOAL: GoalName = "BGoal"; break;
                    //case (int)CellObjects.YGOAL: GoalName = "YGoal"; break;
                    //case (int)CellObjects.PGOAL: GoalName = "PGoal"; break;
                    case (int)CellObjects.GGOAL: GoalColor = ColorManager.Colors["GSlime"]; break;
                    case (int)CellObjects.RGOAL: GoalColor = ColorManager.Colors["RSlime"]; break;
                    case (int)CellObjects.BGOAL: GoalColor = ColorManager.Colors["BSlime"]; break;
                    case (int)CellObjects.YGOAL: GoalColor = ColorManager.Colors["YSlime"]; break;
                    case (int)CellObjects.PGOAL: GoalColor = ColorManager.Colors["PSlime"]; break;
                }
                //if (SelectedButton.Contains("Goal")) SelectedButton = GoalName;
                //Color GoalColor = ColorManager.Colors[GoalName.Substring(0, 1) + "Slime"];
                GoalButton.TextColor = GoalColor;
                //GoalButton.Sprite.Color = ColorManager.Colors[SlimeName];
                GoalButton.Function = () =>
                {
                    Outline.Transform.Pos = GoalButton.Transform.Pos - new Vector2(5, 5);
                    //SelectedButton = GoalName;
                    SelectedButton = CurrentGoal;
                    SelectedSlime = -1;
                    SelectedColor = GoalColor;
                };
                //SelectedGoal = NextGoal;

                //CurrentGoal = NextGoal;
                GoalButton.Function();
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
        private CellObjects SelectedProperty 
        {
            get 
            {
                CellObjects Property = (CellObjects)(-1);
                if (Enum.IsDefined(typeof(CellObjects), DebugMenu.SelectedButton)) Property = (CellObjects)DebugMenu.SelectedButton;
                return Property;
            }
        }
        public DebugGrid(int X, int Y, ref DebugMenu NewDebugMenu) : base(X, Y, 11, 11, Color.DarkMagenta, 2)
        {
            Slimes[0].Transform.Pos = new Vector2(10, 10);
            //Sprite.LayerData.LayerIndex = 9;
            DebugMenu = NewDebugMenu;
            //ResetGrid();
            Clicker = new Clicker(Transform);
            Pointer = new Polygon(new Transform(0, 0, 10, 10), ColorManager.None, "Circle");
            Pointer.Sprite.LayerData.LayerDepth += 2;
            Container.ObjectsToLoad.Add(Pointer);
            //Container.IncreaseLayerDepth(Pointer, 2);
            //foreach (Cell Cell in Cells)
            //{
            //    if (Cell.Col == 0 || Cell.Row == 0 || Cell.Col == 10 || Cell.Row == 10)
            //    {
            //        Cell.Sprite.Color = Color.Black;
            //        Cell.Properties.Add("Wall");
            //    }
            //    //Cell.Sprite.LayerData.LayerIndex = 8;
            //}
            //for (int i = 0; i < 5; i++)
            //{
                //Slime Slime = null;
                //switch (i)
                //{
                //    case 0: Slime = new Slime(Cells[0, 0], "GSlime", 0); break;
                //    case 1: Slime = new Slime(Cells[0, 0], "RSlime", 1); break;
                //    case 2: Slime = new Slime(Cells[0, 0], "BSlime", 2); break;
                //    case 3: Slime = new Slime(Cells[0, 0], "YSlime", 3); break;
                //    case 4: Slime = new Slime(Cells[0, 0], "PSlime", 4); break;
                //}
                //Slime.Transform.Pos = new Vector2(-100, -100);
                //Slimes.Add(Slime);
                //Container.ObjectsToLoad.Add(Slime);
            //}
        }
        public override void Update()
        {
            MPos = Mouse.GetState().Position.ToVector2();
            //OldPaint();
            //NewPaint();
            //TestPaint();
            ShowPointer();
            Paint();
        }
        private void Paint()
        {
            if (Clicker.Hold())
            {
                Cell MouseCell = GetMouseCell();
                if (MouseCell != null)
                {
                    if (SelectedSlime >= 0)
                    {
                        Slime Slime = Slimes[SelectedSlime];
                        Slime.MoveToCell(MouseCell);
                    }
                    else if (SelectedProperty >= 0)
                    {
                        if (MouseCell.Properties.Add(SelectedProperty))
                        {
                            MouseCell.NewParseProperties();
                        }
                    }
                    else if (MouseCell.Col != 0 && MouseCell.Col != Cols - 1 && MouseCell.Row != 0 && MouseCell.Row != Rows - 1)
                    {
                        ResetCell(MouseCell);
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
                //switch (SelectedProperty) 
                //{
                //    case CellObjects.WALL: PointerColor = Color.Black; break;
                //    case CellObjects.PIT: PointerColor = Color.Black; break;
                //    case CellObjects.ICE: PointerColor = ColorManager.Colors["Ice"]; break;
                //    case CellObjects.SWITCH: PointerColor = Color.Black; break;
                //    case CellObjects.RTINT: PointerColor = Color.Black; break;
                //    case CellObjects.BTINT: PointerColor = Color.Black; break;
                //    default: PointerColor = Color.Gray; break;
                //}
                PointerColor = DebugMenu.SelectedColor;
            }
            Pointer.Sprite.Color = PointerColor;
        }
        //private void OldPaint()
        //{
        //    Vector2 MPos = Mouse.GetState().Position.ToVector2();
        //    string Type = DebugMenu.SelectedButton;
        //    Color NewColor = ColorManager.Colors.ContainsKey(Type) ? ColorManager.Colors[Type] : Color.Gray;

        //    if (Transform.Rect.Contains(MPos))
        //    {
        //        Pointer.Transform.Pos = MPos - new Vector2(5, 5);
        //        Pointer.Sprite.Color = NewColor;
        //    }
        //    else Pointer.Sprite.Color = ColorManager.None;

        //    Slime SelectedSlime = null;
        //    Cell SlimeCell = null;
        //    if (Type.Contains("Slime"))
        //    {
        //        SelectedSlime = GetSlimeByType(Type);
        //        SlimeCell = GetCell(SelectedSlime.Col, SelectedSlime.Row);
        //    }

        //    Text GoalText = new Text(new Transform(0, 0, 0, 0), "Goal");
        //    if (Type.Contains("Goal"))
        //    {
        //        GoalText.Color = ColorManager.Colors[Type.Substring(1, 1) + "Slime"];
        //    }

        //    if (Clicker.Hold())
        //    {
        //        foreach (Cell Cell in Cells)
        //        {
        //            if (Cell.Transform.Rect.Contains(MPos))
        //            {
        //                if (SelectedSlime != null)
        //                {
        //                    if (SlimeCell != null) SlimeCell.Properties.Remove(Type);
        //                    SelectedSlime.MoveToCell(Cell);
        //                    Cell.Properties.Add(Type);
        //                }
        //                else
        //                {
        //                    bool HasSlime = false;
        //                    foreach (string Property in Cell.Properties)
        //                    {
        //                        if (Property.Contains("Slime"))
        //                        {
        //                            Slime Slime = GetSlimeByType(Property);
        //                            if (Type != "Ice") Slime.MoveToCell(null);
        //                            HasSlime = true;
        //                            break;
        //                        }
        //                    }
        //                    if (!(HasSlime && Type == "Ice")) Cell.Properties.Clear();
        //                    Cell.Properties.Add(Type);
        //                    Cell.Sprite.Color = NewColor;
        //                    if (Type == "Pit") Cell.Sprite.Texture = Shapes.BackedCircle;
        //                    else Cell.Sprite.Texture = Shapes.Square;
        //                }
        //                break;
        //            }
        //        }
        //    }
        //}
        //private void NewPaint()
        //{
        //    Vector2 MPos = Mouse.GetState().Position.ToVector2();
        //    string Type = DebugMenu.SelectedButton;
        //    Color NewColor = ColorManager.Colors.ContainsKey(Type) ? ColorManager.Colors[Type] : Color.Gray;

        //    if (Transform.Rect.Contains(MPos))
        //    {
        //        Pointer.Transform.Pos = MPos - new Vector2(5, 5);
        //        Pointer.Sprite.Color = NewColor;
        //    }
        //    else Pointer.Sprite.Color = ColorManager.None;

        //    //bool Overrider = false;

        //    if (Type == "Wall" || Type == "Pit" || Type == "Switch" || Type == "Ice")
        //    {
        //        //Overrider = true;
        //    }

        //    Slime SelectedSlime = null;
        //    Cell SlimeCell = null;
        //    if (Type.Contains("Slime"))
        //    {
        //        SelectedSlime = GetSlimeByType(Type);
        //        SlimeCell = GetCell(SelectedSlime.Col, SelectedSlime.Row);
        //    }

        //    if (Clicker.Hold())
        //    {
        //        foreach (Cell Cell in Cells)
        //        {
        //            if (Cell.Transform.Rect.Contains(MPos))
        //            {
        //                //Move the selected slime from where they are to the selected cell
        //                if (SelectedSlime != null)
        //                {
        //                    if (SlimeCell != null) SlimeCell.Properties.Remove(Type);
        //                    SelectedSlime.MoveToCell(Cell);
        //                    Cell.Properties.Add(Type);
        //                }
        //                else
        //                {
        //                    bool HasSlime = false;
        //                    foreach (string Property in Cell.Properties)
        //                    {
        //                        if (Property.Contains("Slime"))
        //                        {
        //                            Slime Slime = GetSlimeByType(Property);
        //                            if (Type != "Ice") Slime.MoveToCell(null);
        //                            HasSlime = true;
        //                            break;
        //                        }
        //                    }
        //                    if (!(HasSlime && Type == "Ice")) Cell.Properties.Clear();
        //                    Cell.Properties.Add(Type);
        //                    Cell.Sprite.Color = NewColor;
        //                    if (Type == "Pit") Cell.Sprite.Texture = Shapes.BackedCircle;
        //                    else Cell.Sprite.Texture = Shapes.Square;
        //                }
        //                break;
        //            }
        //        }
        //    }
        //}
        //private void TestPaint()
        //{
        //    Vector2 MPos = Mouse.GetState().Position.ToVector2();
        //    string Type = DebugMenu.SelectedButton;
        //    Color NewColor = ColorManager.Colors.ContainsKey(Type) ? ColorManager.Colors[Type] : Color.Gray;

        //    if (Transform.Rect.Contains(MPos))
        //    {
        //        Pointer.Transform.Pos = MPos - new Vector2(5, 5);
        //        Pointer.Sprite.Color = NewColor;
        //    }
        //    else Pointer.Sprite.Color = ColorManager.None;

        //    Slime SelectedSlime = null;
        //    Cell SlimeCell = null;
        //    if (Type.Contains("Slime"))
        //    {
        //        SelectedSlime = GetSlimeByType(Type);
        //        SlimeCell = GetCell(SelectedSlime.Col, SelectedSlime.Row);
        //    }

        //    if (Clicker.Hold())
        //    {
        //        foreach (Cell Cell in Cells)
        //        {
        //            if (Cell.Transform.Rect.Contains(MPos))
        //            {
        //                //Move the selected slime from where they are to the selected cell
        //                //Cell.Properties.Clear();    
        //                if (SelectedSlime != null)
        //                {
        //                    if (SlimeCell != null) SlimeCell.Properties.Remove(Type);
        //                    SelectedSlime.MoveToCell(Cell);
        //                }
        //                Cell.Properties.Add(Type);
        //                Cell.ParseProperties();
        //                break;
        //            }
        //        }
        //    }
        //}
        public void ResetGrid()
        {
            //for (int i = 0; i < Cells.Length; i++) 
            //{
                //int Col = i % Cols;
                //int Row = (int)MathF.Floor(i / Cols);
                //Cell Cell = Cells[Col, Row];
                //Cell.Container.Destroy = true;
                //if (Cell.Col == 0 || Cell.Row == 0 || Cell.Col == 10 || Cell.Row == 10)
                //{
                //    Cell.NewProperties.Add(CellObjects.WALL);
                //    Cell.NewParseProperties();
                //}
                //ResetCell(Cell);
            //}
        }
        private void ResetCell(Cell Cell)
        {
            if (Cell.Properties.Count > 0)
            {
                Cell.Container.Destroy = true;
                Cell = new Cell(Cell.Transform, Cell.Sprite.Color, Cell.Col, Cell.Row);
                Cells[Cell.Col, Cell.Row] = Cell;
                Container.ObjectsToLoad.Add(Cell);
            }
        }
    }
}
