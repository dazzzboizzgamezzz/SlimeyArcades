using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Managers;
using Slimey_Arcades.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Slimey_Arcades.Objects
{
    public class Grid : IDraw, IContainer, IUpdate
    {
        public int LoadedCount { get; set; }
        public Cell[,] Cells { get; set; }
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public ArrayList SubObjects { get; set; } = new();
        public Sorter Obj { get => new Sorter(SubObjects); }
        public bool Destroy { get; set; } = false;
        public List<Slime> Slimes { get; set; } = new(); 
        public List<Cell> Goals { get; set; } = new();
        private int Rows { get; init; }
        private int Cols { get; init; }
        public Grid(int X, int Y, int NewCols, int NewRows, Color BGColor, int CellGap = 5, int CellSize = 50, int BorderWidth = 5)
        {
            Rows = NewRows;
            Cols = NewCols;
            int Width = ((CellSize + CellGap) * Cols) - CellGap;
            int Height = ((CellSize + CellGap) * Rows) - CellGap;
            Transform = new Transform(X - BorderWidth, Y - BorderWidth, Width + BorderWidth * 2, Height + BorderWidth * 2);
            Sprite = new Sprite(Shapes.Square, BGColor);

            Cells = new Cell[Cols, Rows];
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    int CellX = X + (i * (CellSize + CellGap));
                    int CellY = Y + (j * (CellSize + CellGap));
                    Transform CellPosition = new Transform(CellX, CellY, CellSize, CellSize);
                    Cells[i, j] = new Cell(CellPosition, Color.Gray, i, j, i + j);
                    SubObjects.Add(Cells[i, j]);
                }
            }
        }
        public virtual void Update()
        {

        }
        public virtual Cell GetCell(int Col, int Row)
        {
            Cell TargetCell = null;
            if ((Col < Cols && Col > -1) && (Row < Rows && Row > -1))
            {
                TargetCell = Cells[Col, Row];
            }
            return TargetCell;
        }
        public Slime GetSlimeByType(string Type)
        {
            Slime NewSlime = null;
            foreach (Slime Slime in Slimes)
            {
                if (Slime.Type == Type)
                {
                    NewSlime = Slime;
                    break;
                }
            }
            return NewSlime;
        }
        public void SaveLevel(int Level)
        {
            string SceneFolder = Directory.GetCurrentDirectory() + "\\Levels";
            Directory.CreateDirectory(SceneFolder);
            string LevelFile = SceneFolder + "\\Level" + Level.ToString() + ".csv";
            string Properties = "";
            using (StreamWriter Writer = new StreamWriter(LevelFile))
            {
                foreach (Cell Cell in Cells)
                {
                    if (Cell.Properties.Count > 0)
                    {
                        Properties = Cell.Col.ToString() + "," + Cell.Row.ToString() + ",";
                        foreach (string Property in Cell.Properties)
                        {
                            Properties = Properties + Property + ",";
                        }
                        Properties = Properties.Remove(Properties.Length - 1);
                        Writer.WriteLine(Properties);
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
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        List<string> Data = Line.Split(",").ToList();
                        Cell NextCell = Cells[int.Parse(Data[0]), int.Parse(Data[1])];
                        if (Data.Count > 2)
                        {
                            Data.RemoveRange(0, 2);
                            NextCell.Properties.AddRange(Data);
                            foreach (string Property in Data)
                            {
                                if (Property.Contains("Slime"))
                                {
                                    if (Slimes.Count > 0)
                                    {
                                        Slime OldSlime = GetSlimeByType(Property);
                                        Slimes.Remove(OldSlime);
                                    }
                                    Slime NewSlime = new Slime(NextCell, Property);
                                    Slimes.Add(NewSlime);
                                }
                                //else
                                //{
                                //    NextCell.Sprite.Color = ColorManager.Colors.ContainsKey(Property) ? ColorManager.Colors[Property] : Color.Gray;
                                //    if (Property == "Pit") NextCell.Sprite.Texture = Shapes.BackedCircle;
                                //    else NextCell.Sprite.Texture = Shapes.Square;
                                //}
                            }
                            NextCell.ParseProperties();
                        }
                    }
                }
                SubObjects.AddRange(Slimes);
            }
            else 
            {
                SaveLevel(Level);
                LoadLevel(Level);
            }
        }
    }
}
