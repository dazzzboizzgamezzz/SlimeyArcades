using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Slimey_Arcades
{
    public class Grid : IDraw, IContainer, IUpdate, INotifier
    {
        public Cell[,] Cells { get; set; }
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public Container Container { get; init; } = new();
        public Notifier Notifier { get; init; } = new();
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int CellSize { get; init; }
        protected int CellGap { get; init; }
        protected Slime[] Slimes { get; init; } = new Slime[5];
        private enum GRIDDATA
        {
            NONE = -1,
            COLROW = 1,
            SLIME = 2,
            CELLDATA = 3
        }
        public Grid(int X, int Y, int NewCols = 12, int NewRows = 12, Color? BGColor = null, int NewCellGap = 2, int NewCellSize = 50, int BorderWidth = 5, Color? CellColor = null, int? NewLevel = null)
        {
            if (NewLevel != null) GetStartingLevel((int)NewLevel);
            else
            {
                Rows = NewRows;
                Cols = NewCols;
            }
            CellSize = NewCellSize;
            CellGap = NewCellGap;
            int Width = ((CellSize + CellGap) * Cols) - CellGap;
            int Height = ((CellSize + CellGap) * Rows) - CellGap;
            Transform = new Transform(X - BorderWidth, Y - BorderWidth, Width + BorderWidth * 2, Height + BorderWidth * 2);
            Sprite = new Sprite(Shapes.Square, BGColor == null ? ColorHelp.None : (Color)BGColor);
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
                Slime.TargetCell = null;
                Slime.MoveToTarget();
                Slime.Sprite.LayerData.LayerDepth += 1;
                Slimes[i] = Slime;
                Container.ObjectsToLoad.Add(Slimes[i]);
            }
        }
        public virtual void Update() { }
        protected virtual void ProcessNotifications(Notification Notification) { }
        public Cell GetCell(Vector2 ColRowVec)
        {
            return GetCell((int)ColRowVec.X, (int)ColRowVec.Y);
        }
        public Cell GetCell(int Col, int Row)
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
            Cell MouseCell = null;
            if (Transform.Rect.Contains(MouseControl.MousePosition))
            {
                Vector2 Offset = MouseControl.MousePosition - Transform.Pos;
                int Col = (int)MathF.Floor(Offset.X / (CellSize + CellGap));
                int Row = (int)MathF.Floor(Offset.Y / (CellSize + CellGap));
                MouseCell = GetCell(Col, Row);
            }
            return MouseCell;
        }
        public Slime GetCellSlime(Cell Cell)
        {
            Slime Slime = null;
            foreach (Slime OtherSlime in Slimes)
            {
                if (OtherSlime.ColRowVec == Cell.ColRowVec) 
                { 
                    Slime = OtherSlime; 
                    break; 
                }
            }
            return Slime;
        }
        public List<Cell> GetNeighbors(Cell Start)
        {
            List<Cell> Neighbors = new();

            if (GetCell(Start.Col + 1, Start.Row) != null) Neighbors.Add(GetCell(Start.Col + 1, Start.Row));
            if (GetCell(Start.Col - 1, Start.Row) != null) Neighbors.Add(GetCell(Start.Col - 1, Start.Row));
            if (GetCell(Start.Col, Start.Row + 1) != null) Neighbors.Add(GetCell(Start.Col, Start.Row + 1));
            if (GetCell(Start.Col, Start.Row - 1) != null) Neighbors.Add(GetCell(Start.Col, Start.Row - 1));

            return Neighbors;
        }
        public void SwitchRedBlue(CELLOBJECTS SwitchType)
        {
            CELLOBJECTS OtherSwitch = SwitchType == CELLOBJECTS.RSWITCH ? CELLOBJECTS.BSWITCH : CELLOBJECTS.RSWITCH;
            foreach(Cell Cell in Cells)
            {
                if (Cell.Properties.Contains(SwitchType)) 
                {
                    Cell.Properties.Remove(SwitchType);
                    Cell.Properties.Add(OtherSwitch);
                    Cell.ParseProperties();
                }
                if (SwitchType == CELLOBJECTS.RSWITCH && Cell.RedProperties.Count > 0)
                {
                    Cell.Properties = new(Cell.RedProperties);
                    Cell.ParseProperties();
                }
                else if (SwitchType == CELLOBJECTS.BSWITCH && Cell.BlueProperties.Count > 0)
                {
                    Cell.Properties = new(Cell.BlueProperties);
                    Cell.ParseProperties();
                }
            }
        }
        private void GetStartingLevel(int NewLevel)
        {
            string LevelFile = Directory.GetCurrentDirectory() + "\\Levels" + "\\Level" + NewLevel.ToString() + ".csv";
            using (StreamReader Reader = new StreamReader(LevelFile))
            {
                string[] Data = Reader.ReadLine().Split(",");
                GRIDDATA DataType = (GRIDDATA)int.Parse(Data[0]);
                while (DataType != GRIDDATA.COLROW)
                {
                    Data = Reader.ReadLine().Split(",");
                    DataType = (GRIDDATA)int.Parse(Data[0]);
                }
                Cols = int.Parse(Data[1]);
                Rows = int.Parse(Data[2]);
            }
        }
        public void SaveLevel(int Level)
        {
            try
            {
                string SceneFolder = Directory.GetCurrentDirectory() + "\\Levels";
                Directory.CreateDirectory(SceneFolder);
                string LevelFile = SceneFolder + "\\Level" + Level.ToString() + ".csv";
                string Data = "";
                using (StreamWriter Writer = new StreamWriter(LevelFile))
                {
                    Data = ((int)GRIDDATA.COLROW).ToString() + "," + Cols.ToString() + "," + Rows.ToString();
                    Writer.WriteLine(Data);
                    Writer.Flush();
                    foreach (Slime Slime in Slimes)
                    {
                        Data = ((int)GRIDDATA.SLIME).ToString() + "," + Slime.Col.ToString() + "," + Slime.Row.ToString() + "," + Slime.Type.ToString();
                        Writer.WriteLine(Data);
                        Writer.Flush();
                    }
                    foreach (Cell Cell in Cells)
                    {
                        if (Cell.Properties.Count > 0)
                        {
                            Data = ((int)GRIDDATA.CELLDATA).ToString() + "," + Cell.Col.ToString() + "," + Cell.Row.ToString() + ",";
                            foreach (int Property in Cell.Properties)
                            {
                                Data += Property.ToString() + ",";
                            }
                            Data = Data.Remove(Data.Length - 1);
                            Writer.WriteLine(Data);
                            Writer.Flush();
                        }
                    }
                }
            }
            catch
            {
                Container.ObjectsToLoad.Add(new ErrorMessage(this));
                Container.LoadObjects(this, 1);
            }
        }
        public void LoadLevel(int Level)
        {
            try
            {
                string LevelFile = Directory.GetCurrentDirectory() + "\\Levels" + "\\Level" + Level.ToString() + ".csv";
                if (File.Exists(LevelFile))
                {
                    CELLOBJECTS SwitchType = 0;
                    using (StreamReader Reader = new StreamReader(LevelFile))
                    {
                        string Line;
                        int LineCount = 0;
                        while ((Line = Reader.ReadLine()) != null)
                        {
                            string[] Data = Line.Split(",");
                            GRIDDATA DataType = (GRIDDATA)int.Parse(Data[0]);

                            if (DataType == GRIDDATA.COLROW) continue;

                            Cell NextCell = GetCell(int.Parse(Data[1]), int.Parse(Data[2]));
                            switch (DataType)
                            {
                                case GRIDDATA.SLIME:
                                    Slime Slime = Slimes[int.Parse(Data[3])];
                                    Slime.TargetCell = NextCell;
                                    Slime.MoveToTarget();
                                    break;
                                case GRIDDATA.CELLDATA:
                                    for (int i = 3; i < Data.Length; i++)
                                    {
                                        CELLOBJECTS Property = (CELLOBJECTS)int.Parse(Data[i]);
                                        NextCell.Properties.Add(Property);
                                        if (Property == CELLOBJECTS.RSWITCH)
                                            SwitchType = CELLOBJECTS.BSWITCH;
                                        if (Property == CELLOBJECTS.BSWITCH) SwitchType = CELLOBJECTS.RSWITCH;
                                    }
                                    if (NextCell.Properties.Contains(CELLOBJECTS.BTINT))
                                    {
                                        NextCell.BlueProperties = new(NextCell.Properties);
                                        NextCell.RedProperties.Add(CELLOBJECTS.BTINT);
                                    }
                                    if (NextCell.Properties.Contains(CELLOBJECTS.RTINT))
                                    {
                                        NextCell.RedProperties = new(NextCell.Properties);
                                        NextCell.BlueProperties.Add(CELLOBJECTS.RTINT);
                                    }
                                    if (NextCell.Properties.Count > 0) NextCell.ParseProperties();
                                    break;
                            }
                            LineCount++;
                        }
                    }
                    if (SwitchType != 0) SwitchRedBlue(SwitchType);
                }
                else
                {
                    LoadDefaultLevel();
                    LoadLevel(-1);
                }
            }
            catch
            {
                Container.ObjectsToLoad.Add(new ErrorMessage(this));
                Container.LoadObjects(this, 1);
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
            DefaultGrid.Slimes[0].TargetCell = Cells[5, 5];
            DefaultGrid.Slimes[0].MoveToTarget();
            DefaultGrid.SaveLevel(-1);
        }
    }
}
