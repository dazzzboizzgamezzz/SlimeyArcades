using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Slimey_Arcades
{
    public class Grid : IDraw, IContainer, IUpdate
    {
        public Cell[,] Cells { get; set; }
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public Container Container { get; init; } = new();
        protected int Rows { get; init; }
        protected int Cols { get; init; }
        protected int CellSize { get; init; }
        protected int CellGap { get; init; }
        //protected List<Slime> Slimes { get; init; } = new();
        protected Slime[] Slimes { get; init; } = new Slime[5];
        public Grid(int X, int Y, int NewCols, int NewRows, Color BGColor, int NewCellGap = 5, int NewCellSize = 50, int BorderWidth = 5, Color? CellColor = null)
        {
            Rows = NewRows;
            Cols = NewCols;
            CellSize = NewCellSize;
            CellGap = NewCellGap;
            int Width = ((CellSize + CellGap) * Cols) - CellGap;
            int Height = ((CellSize + CellGap) * Rows) - CellGap;
            Transform = new Transform(X - BorderWidth, Y - BorderWidth, Width + BorderWidth * 2, Height + BorderWidth * 2);
            Sprite = new Sprite(Shapes.Square, BGColor);
            Sprite.LayerData.LayerIndex = 9;

            Cells = new Cell[Cols, Rows];
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    int CellX = X + (i * (CellSize + CellGap));
                    int CellY = Y + (j * (CellSize + CellGap));
                    Transform CellPosition = new Transform(CellX, CellY, CellSize, CellSize);
                    Color NewCellColor = CellColor == null ? Color.Gray : (Color)CellColor;
                    Cells[i, j] = new Cell(CellPosition, NewCellColor, i, j);
                    Cells[i, j].Sprite.LayerData.LayerIndex = 8;
                    Container.ObjectsToLoad.Add(Cells[i, j]);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Slime Slime = new Slime(Cells[0, 0], "", i);
                Slime.MoveToCell(null);
                Slime.Sprite.LayerData.LayerDepth += 4;
                Slimes[i] = Slime;
                Container.ObjectsToLoad.Add(Slimes[i]);
            }
        }
        public virtual void Update() { }
        public virtual Cell GetCell(int Col, int Row)
        {
            Cell TargetCell = null;
            if ((Col < Cols && Col > -1) && (Row < Rows && Row > -1))
            {
                TargetCell = Cells[Col, Row];
            }
            return TargetCell;
        }
        public Cell GetMouseCell()
        {
            Vector2 MPos = Mouse.GetState().Position.ToVector2();
            Cell MouseCell = null;
            if (Transform.Rect.Contains(MPos))
            {
                Vector2 Offset = MPos - Transform.Pos;
                int Col = (int)MathF.Floor(Offset.X / (CellSize + CellGap));
                int Row = (int)MathF.Floor(Offset.Y / (CellSize + CellGap));
                MouseCell = GetCell(Col, Row);
            }
            return MouseCell;
        }
        //public Slime GetSlimeByType(string Type)
        //{
        //    Slime NewSlime = null;
        //    foreach (Slime Slime in Slimes)
        //    {
        //        //if (Slime.Type == Type)
        //        //{
        //        //    NewSlime = Slime;
        //        //    break;
        //        //}
        //    }
        //    return NewSlime;
        //}
        public void SaveLevel(int Level)
        {
            string SceneFolder = Directory.GetCurrentDirectory() + "\\Levels";
            Directory.CreateDirectory(SceneFolder);
            string LevelFile = SceneFolder + "\\Level" + Level.ToString() + ".csv";
            string Data = "";
            using (StreamWriter Writer = new StreamWriter(LevelFile))
            {
                foreach (Slime Slime in Slimes)
                {
                    Data = Slime.Col.ToString() + "," + Slime.Row.ToString() + "," + Slime.Type.ToString();
                    Writer.WriteLine(Data);
                    Writer.Flush();
                }
                foreach (Cell Cell in Cells)
                {
                    //if (Cell.Properties.Count > 0)
                    //{
                    //    Properties = Cell.Col.ToString() + "," + Cell.Row.ToString() + ",";
                    //    foreach (string Property in Cell.Properties)
                    //    {
                    //        Properties = Properties + Property + ",";
                    //    }
                    //    Properties = Properties.Remove(Properties.Length - 1);
                    //    Writer.WriteLine(Properties);
                    //    Writer.Flush();
                    //}
                    if (Cell.Properties.Count > 0)
                    {
                        Data = Cell.Col.ToString() + "," + Cell.Row.ToString() + ",";
                        foreach (int Property in Cell.Properties)
                        {
                            Data = Data + Property.ToString() + ",";
                        }
                        Data = Data.Remove(Data.Length - 1);
                        Writer.WriteLine(Data);
                        Writer.Flush();
                    }
                }
            }
        }
        public void LoadLevel(int Level)
        {
            string LevelFile = Directory.GetCurrentDirectory() + "\\Levels" + "\\Level" + Level.ToString() + ".csv";
            if (File.Exists(LevelFile))
            {
                using (StreamReader Reader = new StreamReader(LevelFile))
                {
                    string Line;
                    int LineCount = 0;
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        List<string> Data = Line.Split(",").ToList();
                        Cell NextCell = GetCell(int.Parse(Data[0]), int.Parse(Data[1]));
                        if (LineCount < 5)
                        {
                            Slime Slime = Slimes[LineCount];
                            Slime.MoveToCell(NextCell);
                        }
                        else if (Data.Count > 2)
                        {
                            //Data.RemoveRange(0, 2);
                            //NextCell.Properties.AddRange(Data);
                            //foreach (string Property in Data)
                            //{
                            //    if (Property.Contains("Slime"))
                            //    {
                            //        if (Slimes.Count > 0)
                            //        {
                            //            Slime OldSlime = GetSlimeByType(Property);
                            //            if (OldSlime != null) Slimes.Remove(OldSlime);
                            //        }
                            //        Slime NewSlime = new Slime(NextCell, Property);
                            //        Slimes.Add(NewSlime);
                            //    }
                            //}
                            //NextCell.ParseProperties();
                            Data.RemoveRange(0, 2);
                            foreach (string Property in Data)
                            {
                                int PropertyValue = int.Parse(Property);
                                NextCell.Properties.Add((CellObjects)PropertyValue);
                            }
                            if (NextCell.Properties.Count > 0) NextCell.NewParseProperties();
                        }
                        LineCount++;
                    }
                }
                //Container.ObjectsToLoad.AddRange(Slimes);
            }
            else throw new Exception("This level doesn't exist!!!");
        }
    }
}
