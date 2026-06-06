using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Managers;
using Slimey_Arcades.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Slimey_Arcades.Objects
{
    public class DebugMenu : Grid 
    {
        private Polygon Outline { get; set; }
        public static string SelectedButton { get; set; }
        private Button SlimeButton { get; set; }
        private Button GoalButton { get; set; }
        public int SelectedSlime { get; set; } = 0;
        public int SelectedGoal { get; set; } = 0;
        private Clicker SlimeClicker { get; set; }
        private Clicker GoalClicker { get; set; }
        public DebugMenu(int X, int Y) : base(X, Y, 1, 10, Color.DarkMagenta)
        {
            SelectedButton = "GSlime";
            Outline = new Polygon(new Transform(X - 5, Y - 5, 60, 60), Color.Gold);
            Outline.Sprite.Texture = Shapes.MakeOutline(new Rectangle(X + 5, Y + 5, 50, 50));
            Container.ObjectsToLoad.Add(Outline);
            for (int i = 0; i < 10; i++)
            {
                Cells[0, i].Sprite.Color = ColorManager.None;
                Button NewButton = new Button(Cells[0, i].Transform, BGColor: Color.Gray);
                string ButtonName = "";
                Color ButtonColor = Color.Gray;
                switch (i)
                {
                    case 0: 
                        ButtonName = "GSlime"; 
                        ButtonColor = ColorManager.Colors["GSlime"];
                        SlimeButton = NewButton;
                        break;
                    case 1:
                        ButtonName = "GGoal";
                        NewButton.Text = "Goal";
                        NewButton.TextColor = ColorManager.Colors["GSlime"];
                        GoalButton = NewButton;
                        break;
                    case 2: 
                        ButtonName = "Wall"; 
                        ButtonColor = Color.Black;
                        break;
                    case 3: 
                        ButtonName = "Ice"; 
                        ButtonColor = ColorManager.Colors["Ice"];
                        break;
                    case 4:
                        ButtonName = "Pit";
                        ButtonColor = ColorManager.Colors["Pit"];
                        NewButton.Sprite.Texture = Shapes.BackedCircle;
                        break;
                    case 5:
                        ButtonName = "Cutter";
                        break;
                    case 6:
                        ButtonName = "Barrel";
                        ButtonColor = ColorManager.Colors["Barrel"];
                        break;
                    case 7: 
                        ButtonName = "Switch";
                        break;
                    case 8: 
                        ButtonName = "RedBlue";
                        break;
                }
                NewButton.Function = () => 
                {
                    Outline.Transform.Pos = NewButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = ButtonName;
                };
                NewButton.Sprite.Color = ButtonColor;
                NewButton.Highlightable = false;
                Container.ObjectsToLoad.Add(NewButton);
            }
            SlimeClicker = new Clicker(Cells[0, 0].Transform);
            GoalClicker = new Clicker(Cells[0, 1].Transform);
        }
        public override void Update()
        {
            if (SlimeClicker.Click("Right"))
            {
                int NextSlime = (SelectedSlime + 1) % 5;
                string SlimeName = "";
                switch (NextSlime)
                {
                    case 0: SlimeName = "GSlime"; break;
                    case 1: SlimeName = "RSlime"; break;
                    case 2: SlimeName = "BSlime"; break;
                    case 3: SlimeName = "PSlime"; break;
                    case 4: SlimeName = "YSlime"; break;
                }
                if (SelectedButton.Contains("Slime")) SelectedButton = SlimeName;
                SlimeButton.Sprite.Color = ColorManager.Colors[SlimeName];
                SlimeButton.Function = () =>
                {
                    Outline.Transform.Pos = SlimeButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = SlimeName;
                };
                SelectedSlime = NextSlime;
            }
            if (GoalClicker.Click("Right"))
            {
                int NextGoal = (SelectedGoal + 1) % 5;
                string GoalName = "";
                //string SlimeName = "";
                switch (NextGoal)
                {
                    case 0: GoalName = "GGoal"; break;
                    case 1: GoalName = "RGoal"; break;
                    case 2: GoalName = "BGoal"; break;
                    case 3: GoalName = "PGoal"; break;
                    case 4: GoalName = "YGoal"; break;
                }
                if (SelectedButton.Contains("Goal")) SelectedButton = GoalName;
                GoalButton.TextColor = ColorManager.Colors[GoalName.Substring(0, 1) + "Slime"];
                //GoalButton.Sprite.Color = ColorManager.Colors[SlimeName];
                GoalButton.Function = () =>
                {
                    Outline.Transform.Pos = GoalButton.Transform.Pos - new Vector2(5, 5);
                    SelectedButton = GoalName;
                };
                SelectedGoal = NextGoal;
            }
        }
    }
    public class DebugGrid : Grid
    {
        private Clicker Clicker {  get; set; }
        private Polygon Pointer {  get; set; }
        public DebugGrid(int X, int Y) : base(X, Y, 11, 11, Color.DarkMagenta, 2)
        {
            Sprite.LayerData.LayerIndex = 9;
            Clicker = new Clicker(Transform);
            Pointer = new Polygon(new Transform(0, 0, 10, 10), ColorManager.None, "Circle");
            Container.ObjectsToLoad.Add(Pointer);
            foreach (Cell Cell in Cells)
            {
                if (Cell.Col == 0 || Cell.Row == 0 || Cell.Col == 10 || Cell.Row == 10)
                {
                    Cell.Sprite.Color = Color.Black;
                    Cell.Properties.Add("Wall");
                }
                Cell.Sprite.LayerData.LayerIndex = 8;
            }
            for (int i = 0; i < 5; i++)
            {
                Slime Slime = null;
                switch (i)
                {
                    case 0: Slime = new Slime(Cells[0, 0], "GSlime"); break;
                    case 1: Slime = new Slime(Cells[0, 0], "RSlime"); break;
                    case 2: Slime = new Slime(Cells[0, 0], "BSlime"); break;
                    case 3: Slime = new Slime(Cells[0, 0], "YSlime"); break;
                    case 4: Slime = new Slime(Cells[0, 0], "PSlime"); break;
                }
                Slime.Transform.Pos = new Vector2(-100, -100);
                Slimes.Add(Slime);
                Container.ObjectsToLoad.Add(Slime);
            }
        }
        public override void Update()
        {
            OldPaint();
            //NewPaint();
            //TestPaint();
        }
        private void OldPaint()
        {
            Vector2 MPos = Mouse.GetState().Position.ToVector2();
            string Type = DebugMenu.SelectedButton;
            Color NewColor = ColorManager.Colors.ContainsKey(Type) ? ColorManager.Colors[Type] : Color.Gray;

            if (Transform.Rect.Contains(MPos))
            {
                Pointer.Transform.Pos = MPos - new Vector2(5, 5);
                Pointer.Sprite.Color = NewColor;
            }
            else Pointer.Sprite.Color = ColorManager.None;

            Slime SelectedSlime = null;
            Cell SlimeCell = null;
            if (Type.Contains("Slime"))
            {
                SelectedSlime = GetSlimeByType(Type);
                SlimeCell = GetCell(SelectedSlime.Col, SelectedSlime.Row);
            }

            Text GoalText = new Text(new Transform(0, 0, 0, 0), "Goal");
            if (Type.Contains("Goal"))
            {
                GoalText.Color = ColorManager.Colors[Type.Substring(1, 1) + "Slime"];
            }

            if (Clicker.Hold())
            {
                foreach (Cell Cell in Cells)
                {
                    if (Cell.Transform.Rect.Contains(MPos))
                    {
                        if (SelectedSlime != null)
                        {
                            if (SlimeCell != null) SlimeCell.Properties.Remove(Type);
                            SelectedSlime.MoveToCell(Cell);
                            Cell.Properties.Add(Type);
                        }
                        else
                        {
                            bool HasSlime = false;
                            foreach (string Property in Cell.Properties)
                            {
                                if (Property.Contains("Slime"))
                                {
                                    Slime Slime = GetSlimeByType(Property);
                                    if (Type != "Ice") Slime.MoveToCell(null);
                                    HasSlime = true;
                                    break;
                                }
                            }
                            if (!(HasSlime && Type == "Ice")) Cell.Properties.Clear();
                            Cell.Properties.Add(Type);
                            Cell.Sprite.Color = NewColor;
                            if (Type == "Pit") Cell.Sprite.Texture = Shapes.BackedCircle;
                            else Cell.Sprite.Texture = Shapes.Square;
                        }
                        break;
                    }
                }
            }
        }
        private void NewPaint()
        {
            Vector2 MPos = Mouse.GetState().Position.ToVector2();
            string Type = DebugMenu.SelectedButton;
            Color NewColor = ColorManager.Colors.ContainsKey(Type) ? ColorManager.Colors[Type] : Color.Gray;

            if (Transform.Rect.Contains(MPos))
            {
                Pointer.Transform.Pos = MPos - new Vector2(5, 5);
                Pointer.Sprite.Color = NewColor;
            }
            else Pointer.Sprite.Color = ColorManager.None;

            //bool Overrider = false;

            if (Type == "Wall" || Type == "Pit" || Type == "Switch" || Type == "Ice")
            {
                //Overrider = true;
            }

            Slime SelectedSlime = null;
            Cell SlimeCell = null;
            if (Type.Contains("Slime"))
            {
                SelectedSlime = GetSlimeByType(Type);
                SlimeCell = GetCell(SelectedSlime.Col, SelectedSlime.Row);
            }

            if (Clicker.Hold())
            {
                foreach (Cell Cell in Cells)
                {
                    if (Cell.Transform.Rect.Contains(MPos))
                    {
                        //Move the selected slime from where they are to the selected cell
                        if (SelectedSlime != null)
                        {
                            if (SlimeCell != null) SlimeCell.Properties.Remove(Type);
                            SelectedSlime.MoveToCell(Cell);
                            Cell.Properties.Add(Type);
                        }
                        else
                        {
                            bool HasSlime = false;
                            foreach (string Property in Cell.Properties)
                            {
                                if (Property.Contains("Slime"))
                                {
                                    Slime Slime = GetSlimeByType(Property);
                                    if (Type != "Ice") Slime.MoveToCell(null);
                                    HasSlime = true;
                                    break;
                                }
                            }
                            if (!(HasSlime && Type == "Ice")) Cell.Properties.Clear();
                            Cell.Properties.Add(Type);
                            Cell.Sprite.Color = NewColor;
                            if (Type == "Pit") Cell.Sprite.Texture = Shapes.BackedCircle;
                            else Cell.Sprite.Texture = Shapes.Square;
                        }
                        break;
                    }
                }
            }
        }
        private void TestPaint()
        {
            Vector2 MPos = Mouse.GetState().Position.ToVector2();
            string Type = DebugMenu.SelectedButton;
            Color NewColor = ColorManager.Colors.ContainsKey(Type) ? ColorManager.Colors[Type] : Color.Gray;

            if (Transform.Rect.Contains(MPos))
            {
                Pointer.Transform.Pos = MPos - new Vector2(5, 5);
                Pointer.Sprite.Color = NewColor;
            }
            else Pointer.Sprite.Color = ColorManager.None;

            Slime SelectedSlime = null;
            Cell SlimeCell = null;
            if (Type.Contains("Slime"))
            {
                SelectedSlime = GetSlimeByType(Type);
                SlimeCell = GetCell(SelectedSlime.Col, SelectedSlime.Row);
            }

            if (Clicker.Hold())
            {
                foreach (Cell Cell in Cells)
                {
                    if (Cell.Transform.Rect.Contains(MPos))
                    {
                        //Move the selected slime from where they are to the selected cell
                        //Cell.Properties.Clear();    
                        if (SelectedSlime != null)
                        {
                            if (SlimeCell != null) SlimeCell.Properties.Remove(Type);
                            SelectedSlime.MoveToCell(Cell);
                        }
                        Cell.Properties.Add(Type);
                        Cell.ParseProperties();
                        break;
                    }
                }
            }
        }
    }
}
