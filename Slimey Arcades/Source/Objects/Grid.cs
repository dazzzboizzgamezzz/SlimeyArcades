using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Slimey_Arcades
{
    public class Grid : IDraw, IContainer, IUpdate, INotifier
    {
        public Cell[,] Cells { get; set; }
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public Container Container { get; init; } = new();
        public Notifier Notifier { get; init; } = new();
        protected int Rows { get; init; }
        protected int Cols { get; init; }
        protected int CellSize { get; init; }
        protected int CellGap { get; init; }
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
            Notifier.ProcessNotifications = ProcessNotifications;

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
                    Cells[i, j].CellGap = CellGap;
                    Container.ObjectsToLoad.Add(Cells[i, j]);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Slime Slime = new Slime(Cells[0, 0], i);
                Slime.MoveToCell(null);
                Slime.Sprite.LayerData.LayerDepth += 1;
                Slimes[i] = Slime;
                Container.ObjectsToLoad.Add(Slimes[i]);
            }
        }
        public virtual void Update() { }
        protected virtual void ProcessNotifications(Notification Notification) { }
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
                            Data.RemoveRange(0, 2);
                            foreach (string Property in Data)
                            {
                                int PropertyValue = int.Parse(Property);
                                NextCell.Properties.Add((CELLOBJECTS)PropertyValue);
                            }
                            if (NextCell.Properties.Count > 0) NextCell.ParseProperties();
                        }
                        LineCount++;
                    }
                }
            }
            else
            {
                LoadDefaultLevel();
                LoadLevel(-1);
            }
        }
        private void LoadDefaultLevel()
        {
            Grid DefaultGrid = new Grid(0, 0, 11, 11, Color.Black);
            for (int i = 0; i < 11; i++)
            {
                DefaultGrid.Cells[i, 0].Properties.Add(CELLOBJECTS.WALL);
                DefaultGrid.Cells[0, i].Properties.Add(CELLOBJECTS.WALL);
                DefaultGrid.Cells[10, i].Properties.Add(CELLOBJECTS.WALL);
                DefaultGrid.Cells[i, 10].Properties.Add(CELLOBJECTS.WALL);
            }
            DefaultGrid.Slimes[0].MoveToCell(Cells[5, 5]);
            DefaultGrid.SaveLevel(-1);
        }
    }
}
