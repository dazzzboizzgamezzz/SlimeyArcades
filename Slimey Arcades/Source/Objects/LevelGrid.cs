using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slimey_Arcades
{
    public class LevelGrid : Grid
    {
        private List<Keys> PressedKeys { get; set; } = new();
        private List<Slime> ActiveSlimes { get; set; } = new();
        private Slime Player { get; set; }
        private Polygon WallOutline { get; set; }
        private int Timer { get; set; } = 0;
        private bool Paused { get; set; } = false;
        //private Queue<Slime> RotationFusionSlimes { get; set; } = new();
        public LevelGrid(int X, int Y, int Level) : base(X, Y, 11, 11, Color.DarkMagenta, 2)
        {
            LoadLevel(Level);

            Player = Slimes[0];
            ActiveSlimes.Add(Player);
            MoveSlimesToTargets();

            WallOutline = new Polygon(new Transform(-300, -200, 50, 50), Color.Red);
            WallOutline.Sprite.Texture = Shapes.MakeOutline(Cells[0, 0].Transform.Rect);
            WallOutline.Sprite.LayerData.LayerDepth += 2;
            Container.ObjectsToLoad.Add(WallOutline);
        }
        public override void Update()
        {
            if (!Paused)
            {
                List<Keys> CurrentKeys = Keyboard.GetState().GetPressedKeys().ToList();
                foreach (Keys Key in CurrentKeys) 
                { 
                    if (KeyManager.ArrowKeys.ContainsKey(Key) && !PressedKeys.Contains(Key))
                    {
                        PressedKeys.Add(Key);
                        Move(KeyManager.ArrowKeys[Key]);
                    }
                    bool CanRotate = true;
                    foreach (Keys OtherKey in PressedKeys) 
                    {
                        if (KeyManager.ArrowKeys.ContainsKey(OtherKey)) { CanRotate = false; break; }
                    }
                    if (CanRotate && !PressedKeys.Contains(Key))
                    {
                        if (Key == Keys.Q) { Rotate("Counterclockwise"); PressedKeys.Add(Key); }
                        if (Key == Keys.E) { Rotate("Clockwise"); PressedKeys.Add(Key); }
                    }
                }
                //Prevents the key buttons from being held down
                for (int i = 0; i < PressedKeys.Count; i++)
                {
                    Keys Key = PressedKeys[i];
                    if (Keyboard.GetState().IsKeyUp(Key)) { PressedKeys.Remove(Key); i--; }
                }
            }
            //Reset the wall outline after a certain time, which is set in the rotation function
            if (Timer >= 0)
            {
                Timer--;
                if (Timer == 0) WallOutline.Transform.Pos = new Vector2(-100, -100);
            }
        }
        protected override void ProcessNotifications(Notification Notification)
        {
            switch (Notification.Type)
            {
                case NOTTYPES.PAUSED:
                    Paused = Paused ? false : true;
                    break;
            }
        }
        private void Move(string Direction)
        {
            List<Cell> TargetCells = new();

            foreach (Slime Slime in ActiveSlimes)
            {
                //Cell NewTarget = GetTargetCell(Slime.TargetCell, Direction);
                //if (NewTarget == null) return;
                //Slime.TargetCell = NewTarget;
                //TargetCells.Add(NewTarget);
                Slime.TargetCell = GetTargetCell(Slime.TargetCell, Direction);
            }

            //if (HitWall(TargetCells, Direction)) return;
            if (GetCellCount(CELLOBJECTS.WALL) > 0)
            {
                foreach(Slime Slime in ActiveSlimes)
                {
                    Slime.TargetCell = GetCell(Slime.Col, Slime.Row);
                }
                return;
            }

            foreach (Slime Slime in ActiveSlimes)
            {
                if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.RSWITCH)) { SwitchRedBlue(CELLOBJECTS.RSWITCH); break; }
                else if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.BSWITCH)) { SwitchRedBlue(CELLOBJECTS.BSWITCH); break; }
            }

            CutSlimes();

            //IceCheck(Direction);

            if (PitCheck()) return;

            MoveSlimesToTargets();

            if (GetCellCount(CELLOBJECTS.ICE) > 0)
            {
                Move(Direction);
            }
        }
        private int GetCellCount(CELLOBJECTS CellType)
        {
            int CellCount = 0;

            foreach(Slime Slime in ActiveSlimes)
            {
                if (Slime.TargetCell.Properties.Contains(CellType))
                {
                    CellCount++;
                }
            }

            return CellCount;
        }
        //private void IceCheck(string Direction)
        //{
        //    for (int i = 0; i < ActiveSlimes.Count; i++)
        //    {
        //        Slime Slime = ActiveSlimes[i];
        //        if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.ICE))
        //        {
        //            //Fusion(Slime.TargetCell, Slime.CanStick, false);
        //            Fusion(Slime);
        //            Move(Direction);
        //        }
        //    }
        //}
        //private bool PitCheck()
        //{
        //    bool HitPit = true;
        //    foreach (Slime Slime in ActiveSlimes)
        //    {
        //        if (!Slime.TargetCell.Properties.Contains(CELLOBJECTS.PIT)) 
        //        { 
        //            HitPit = false; 
        //            break; 
        //        }
        //    }
        //    if (HitPit)
        //    {
        //        foreach (Slime Slime in ActiveSlimes)
        //        {
        //            Slime.TargetCell = null;
        //            //Slime.MoveToCell(Slime.TargetCell);
        //            Slime.MoveToTarget();
        //        }
        //        Notification Notification = new Notification(NOTTYPES.LOSE);
        //        Notifier.SendNotification(Notification);
        //    }
        //    return HitPit;
        //}
        private bool PitCheck()
        {
            bool HitPit = false;
            if (GetCellCount(CELLOBJECTS.PIT) == ActiveSlimes.Count)
            {
                foreach (Slime Slime in ActiveSlimes)
                {
                    Slime.TargetCell = null;
                    Slime.MoveToTarget();
                }
                Notification Notification = new Notification(NOTTYPES.LOSE);
                Notifier.SendNotification(Notification);
                HitPit = true;
            }
            return HitPit;
        }
        private void MoveSlimesToTargets()
        {
            for (int i = 0; i < ActiveSlimes.Count; i++)
            {
                Slime Slime = ActiveSlimes[i];
                //Slime.MoveToCell(Slime.TargetCell);
                Slime.MoveToTarget();
                Fusion(Slime);
            }
            foreach (Slime Slime in Slimes) { Slime.WasCut = false; }
            ActiveSlimes = ActiveSlimes.OrderBy(Slime => Slime.DistanceToCenter).ToList();
            StretchSlimes();
            CheckWin();
        }
        private void StretchSlimes()
        {
            foreach (Slime Slime in ActiveSlimes)
            {
                Slime.Transform.Width = 40;
                Slime.Transform.Height = 40;
            }
            
            List<Slime> SlimesToSort;
            SlimesToSort = new(ActiveSlimes.OrderBy(o => o.Col).ThenBy(o => o.Row));
            for (int i = 0; i < SlimesToSort.Count; i++)
            {
                Slime Slime = SlimesToSort[i];
                for (int j = i + 1; j < SlimesToSort.Count; j++)
                {
                    Slime OtherSlime = SlimesToSort[j];
                    Vector2 RelativePos = new Vector2(OtherSlime.Col - Slime.Col, OtherSlime.Row - Slime.Row);
                    if (RelativePos == new Vector2(1, 0))
                    {
                        Slime.Transform.Width += 6;
                        OtherSlime.Transform.Width += 6;
                        OtherSlime.Transform.X += -6;
                    }
                    if (RelativePos == new Vector2(0, 1))
                    {
                        Slime.Transform.Height += 6;
                        OtherSlime.Transform.Height += 6;
                        OtherSlime.Transform.Y += -6;
                    }
                }
            }
        }
        private Cell GetTargetCell(Cell StartingCell, string Direction)
        {
            Cell TargetCell;
            int ColOffset = 0;
            int RowOffset = 0;
            switch (Direction)
            {
                case "Down": RowOffset = 1; break;
                case "Up": RowOffset = -1; break;
                case "Right": ColOffset = 1; break;
                case "Left": ColOffset = -1; break;
            }
            TargetCell = GetCell(StartingCell.Col + ColOffset, StartingCell.Row + RowOffset);
            return TargetCell;
        }
        //private bool HitWall(List<Cell> TargetCells, string Direction)
        //{
        //    foreach (Cell Cell in TargetCells)
        //    {
        //        if (Cell.Properties.Contains(CELLOBJECTS.WALL))
        //        {
        //            string NewDirection = "";
        //            switch (Direction)
        //            {
        //                case "Down": NewDirection = "Up"; break;
        //                case "Up": NewDirection = "Down"; break;
        //                case "Right": NewDirection = "Left"; break;
        //                case "Left": NewDirection = "Right"; break;
        //            }
        //            foreach (Slime Slime in ActiveSlimes)
        //            {
        //                Slime.TargetCell = GetTargetCell(Slime.TargetCell, NewDirection);
        //            }
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //private void NewCutSlimes()
        private void CutSlimes()
        {
            if (ActiveSlimes.Count == 1) return;

            HashSet<Slime> SlimesToCheck = new();
            SlimesToCheck.Add(Player);

            Dictionary<Vector2, Slime> SlimeTargetCells = new();
            foreach (Slime Slime in ActiveSlimes) SlimeTargetCells.Add(Slime.TargetCell.ColRowVec, Slime);

            for (int i = 0; i < SlimesToCheck.Count; i++)
            {
                Slime Slime = SlimesToCheck.Take(i + 1).Last();
                foreach (Cell Neighbor in GetNeighbors(Slime.TargetCell))
                {
                    if (SlimeTargetCells.Keys.Contains(Neighbor.ColRowVec))
                    {
                        Slime NeighborSlime = SlimeTargetCells[Neighbor.ColRowVec];
                        if (!CompareCutters(Slime.TargetCell, Neighbor))
                        {
                            SlimesToCheck.Add(NeighborSlime);
                        }
                        else if (!SlimesToCheck.Contains(SlimeTargetCells[Neighbor.ColRowVec]))
                        {
                            NeighborSlime.WasCut = true;
                            SlimeTargetCells.Remove(NeighborSlime.TargetCell.ColRowVec);
                            //NeighborSlime.MoveToCell(NeighborSlime.TargetCell);
                            NeighborSlime.MoveToTarget();
                            i--;
                        }
                    }
                }
            }

            if (SlimesToCheck.Count != ActiveSlimes.Count)
            {
                foreach(Slime Slime in ActiveSlimes)
                {
                    //Slime.MoveToCell(Slime.TargetCell);
                    Slime.MoveToTarget();
                }
                ActiveSlimes = SlimesToCheck.ToList();
            }
        }
        private bool CompareCutters(Cell StartCell, Cell Neighbor)
        {
            foreach(CELLOBJECTS Cutter in StartCell.GetCutters())
            {
                foreach (CELLOBJECTS NeighborCutter in Neighbor.GetCutters())
                {
                    if (Cutter == CELLOBJECTS.RCUTTER && NeighborCutter == CELLOBJECTS.LCUTTER
                     || Cutter == CELLOBJECTS.LCUTTER && NeighborCutter == CELLOBJECTS.RCUTTER
                     || Cutter == CELLOBJECTS.UCUTTER && NeighborCutter == CELLOBJECTS.DCUTTER
                     || Cutter == CELLOBJECTS.DCUTTER && NeighborCutter == CELLOBJECTS.UCUTTER)
                        return true;
                }
            }
            return false;
        }
        private void Fusion(Slime StartingSlime)
        {
            bool IsSlime = StartingSlime.CanStick;
            bool WasCut = StartingSlime.WasCut;
            Cell StartingCell = StartingSlime.TargetCell;
            List<Cell> Neighbors = new()
            {
                GetTargetCell(StartingCell, "Up"),
                GetTargetCell(StartingCell, "Down"),
                GetTargetCell(StartingCell, "Left"),
                GetTargetCell(StartingCell, "Right"),
            };
            foreach(Cell Neighbor in Neighbors)
            {
                if (WasCut && StartingCell.HasCutter() && Neighbor.HasCutter())
                {
                    continue;
                }
                if (Neighbor.Barrel != null && IsSlime)
                {
                    ActiveSlimes.Add(Neighbor.Barrel);
                    Neighbor.Barrel = null;
                }
                foreach(Slime Slime in Slimes)
                {
                    //if (ActiveSlimes.Contains(Slime) || Slime.ColRowVec == new Vector2(-1, -1)) continue;
                    if (ActiveSlimes.Contains(Slime) || Slime.ColRowVec == new Vector2(-1, -1) || Slime.WasCut) continue;
                    if (Slime.ColRowVec == Neighbor.ColRowVec)
                    {
                        Slime.DistanceToCenter = StartingSlime.DistanceToCenter + 1;
                        ActiveSlimes.Add(Slime);
                    }
                }
            }
        }
        private void CheckWin()
        {
            bool Winning = false;
            foreach (Slime Slime in Slimes)
            {
                if (Slime.ColRowVec != new Vector2(-1, -1))
                {
                    if (!ActiveSlimes.Contains(Slime)) { Winning = false; break; }
                    
                    Cell SlimeCell = GetCell(Slime.Col, Slime.Row);
                    if (SlimeCell.Properties.Count == 0) { Winning = false; break; }

                    CELLOBJECTS SlimeType = (CELLOBJECTS)(-1);
                    switch (Slime.Type)
                    {
                        case 0: SlimeType = CELLOBJECTS.GGOAL; break;
                        case 1: SlimeType = CELLOBJECTS.RGOAL; break;
                        case 2: SlimeType = CELLOBJECTS.BGOAL; break;
                        case 3: SlimeType = CELLOBJECTS.YGOAL; break;
                        case 4: SlimeType = CELLOBJECTS.PGOAL; break;
                    }

                    foreach(CELLOBJECTS Property in SlimeCell.Properties)
                    {
                        if (Property == SlimeType) Winning = true;
                        else Winning = false;
                    }

                    if (!Winning) break;
                }
            }
            if (Winning)
            {
                Notification Notification = new Notification(NOTTYPES.WIN);
                Notifier.SendNotification(Notification);
            }
        }
        private Slime CheckForSlimes(Cell TargetCell)
        {
            Slime FoundSlime = null;
            foreach (Slime Slime in Slimes)
            {
                if (Slime.ColRowVec == TargetCell.ColRowVec)
                {
                    FoundSlime = Slime;
                    break;
                }
            }
            //foreach(Slime Barrel in Barrels)
            //{
            //    if (Barrel.ColRowVec == TargetCell.ColRowVec)
            //    {
            //        FoundSlime = Barrel;
            //        break;
            //    }
            //}
            return FoundSlime;
        }
        private void Rotate(string Direction)
        {
            if (ActiveSlimes.Count == 1) return;

            List<Slime> SlimesToRotate = new(ActiveSlimes);

            for (int i = 1; i < SlimesToRotate.Count; i++)
            {
                Slime Slime = SlimesToRotate[i];

                Vector2 RelPos = Slime.ColRowVec - Player.ColRowVec;
                Vector2 TargetPos = Vector2.Zero;

                if (Direction == "Clockwise") TargetPos = new Vector2(-RelPos.Y, RelPos.X);
                if (Direction == "CounterClockwise") TargetPos = new Vector2(RelPos.Y, -RelPos.X);

                Vector2 NextPos = RelPos;
                Vector2 MovePos = (Math.Abs(RelPos.X) < Math.Abs(NextPos.Y)) ?
                    MovePos = (NextPos.X < TargetPos.X) ? new Vector2(1, 0) : new Vector2(-1, 0) :
                    MovePos = (NextPos.Y < TargetPos.Y) ? new Vector2(0, 1) : new Vector2(0, -1);

                bool FoundSlime = false;

                while (NextPos != TargetPos)
                {
                    if (Math.Abs(MovePos.X) == 1 && NextPos.X == TargetPos.X)
                    {
                        MovePos = (NextPos.Y < TargetPos.Y) ? new Vector2(0, 1) : new Vector2(0, -1);
                    }
                    if (Math.Abs(MovePos.Y) == 1 && NextPos.Y == TargetPos.Y)
                    {
                        MovePos = (NextPos.X < TargetPos.X) ? new Vector2(1, 0) : new Vector2(-1, 0);
                    }

                    NextPos += MovePos;

                    Cell NextCell = GetCell(Player.ColRowVec + NextPos);

                    if (NextCell.Properties.Contains(CELLOBJECTS.WALL)) { StopRotating(NextCell); return; }
                    //there is a bug if you collide with two slimes before hitting a wall, only one slime will fuse, fix when adding in barrel on barrel collisions.
                    if (!FoundSlime)
                    {
                        Slime NeighborSlime = CheckForSlimes(NextCell);
                        if (NeighborSlime != null && !SlimesToRotate.Contains(NeighborSlime))
                        {
                            FoundSlime = true;
                            NeighborSlime.DistanceToCenter = Slime.DistanceToCenter + 1;
                            ActiveSlimes.Add(NeighborSlime);
                            SlimesToRotate.Add(NeighborSlime);
                            NeighborSlime.TargetCell = GetCell(Slime.ColRowVec + MovePos);
                            NeighborSlime.MoveToTarget();
                        }
                    }
                }
                Slime.TargetCell = GetCell(TargetPos + Player.ColRowVec);
            }

            ActiveSlimes = SlimesToRotate;

            foreach (Slime Slime in ActiveSlimes)
            {
                if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.RSWITCH)) { SwitchRedBlue(CELLOBJECTS.RSWITCH); break; }
                else if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.BSWITCH)) { SwitchRedBlue(CELLOBJECTS.BSWITCH); break; }
            }

            if (PitCheck()) return;

            MoveSlimesToTargets();
        }
        //private void OldRotate(string Direction)
        //{
        //    if (ActiveSlimes.Count > 1)
        //    {
        //        List<Slime> SlimesToSort = new();
        //        foreach (Slime Slime in ActiveSlimes)
        //        {
        //            if (Slime != Player)
        //            {
        //                int RelX = Slime.Col - Player.Col;
        //                int RelY = -(Slime.Row - Player.Row);
        //                Slime.DistanceToCenter = Math.Abs(RelX) + Math.Abs(RelY);
        //                SlimesToSort.Add(Slime);
        //            }
        //        }
        //        Stack<Slime> SlimesToRotate = new(SlimesToSort.OrderBy(o => o.DistanceToCenter));
        //        while (SlimesToRotate.Count > 0)
        //        {
        //            Slime Slime = SlimesToRotate.Pop();

        //            int RelX =   Slime.Col - Player.Col;
        //            int RelY = -(Slime.Row - Player.Row);

        //            string Quadrant = "";

        //            if (RelX + RelY > 0 && RelX - RelY >= 0) Quadrant = "Right";
        //            if (RelX + RelY <= 0 && RelX - RelY > 0) Quadrant = "Down";
        //            if (RelX + RelY < 0 && RelX - RelY <= 0) Quadrant = "Left";
        //            if (RelX + RelY >= 0 && RelX - RelY < 0) Quadrant = "Up";

        //            Vector2 RotationTarget = Vector2.Zero;
        //            Cell TargetCell = null;

        //            string MoveDirection1 = "";
        //            string MoveDirection2 = "";

        //            if (Direction == "Clockwise")
        //            {
        //                RotationTarget = new Vector2(RelY, -RelX);
        //                TargetCell = GetCell((int)RotationTarget.X + Player.Col, -(int)RotationTarget.Y + Player.Row);
        //                if (TargetCell == null) return;
        //                switch (Quadrant)
        //                {
        //                    case "Up":    MoveDirection1 = "Right"; MoveDirection2 = "Down";  break;
        //                    case "Right": MoveDirection1 = "Down";  MoveDirection2 = "Left";  break;
        //                    case "Down":  MoveDirection1 = "Left";  MoveDirection2 = "Up";    break;
        //                    case "Left":  MoveDirection1 = "Up";    MoveDirection2 = "Right"; break;
        //                }
        //            }
        //            else
        //            {
        //                RotationTarget = new Vector2(-RelY, RelX);
        //                TargetCell = GetCell((int)RotationTarget.X + Player.Col, -(int)RotationTarget.Y + Player.Row);
        //                if (TargetCell == null) return;
        //                switch (Quadrant)
        //                {
        //                    case "Up":    MoveDirection1 = "Left";  MoveDirection2 = "Down";  break;
        //                    case "Left":  MoveDirection1 = "Down";  MoveDirection2 = "Right"; break;
        //                    case "Down":  MoveDirection1 = "Right"; MoveDirection2 = "Up";    break;
        //                    case "Right": MoveDirection1 = "Up";    MoveDirection2 = "Left";  break;
        //                }
        //            }

        //            bool HitWall = false;
        //            Cell OtherTarget = GetCell(Slime.Col, Slime.Row);
        //            if (MoveForRotating(Slime, TargetCell, MoveDirection1)) HitWall = true;
        //            if (RotationFusionSlimes.Count > 0)
        //            {
        //                while (RotationFusionSlimes.Count > 0)
        //                {
        //                    Slime OtherSlime = RotationFusionSlimes.Dequeue();
        //                    OtherTarget = GetTargetCell(OtherTarget, MoveDirection1);
        //                    //OtherSlime.MoveToCell(OtherTarget);
        //                    OtherSlime.TargetCell = OtherTarget;
        //                    OtherSlime.MoveToTarget();
        //                    ActiveSlimes.Add(OtherSlime);
        //                    SlimesToRotate.Push(OtherSlime);
        //                }
        //            }
        //            if (HitWall) break;
        //            if (MoveForRotating(Slime, TargetCell, MoveDirection2)) HitWall = true;
        //            if (RotationFusionSlimes.Count > 0)
        //            {
        //                while (RotationFusionSlimes.Count > 0)
        //                {
        //                    Slime OtherSlime = RotationFusionSlimes.Dequeue();
        //                    OtherTarget = GetTargetCell(OtherTarget, MoveDirection1);
        //                    //OtherSlime.MoveToCell(OtherTarget);
        //                    OtherSlime.TargetCell = OtherTarget;
        //                    OtherSlime.MoveToTarget();
        //                    ActiveSlimes.Add(OtherSlime);
        //                    SlimesToRotate.Push(OtherSlime);
        //                }
        //            }
        //            if (HitWall) break;
        //        }
        //        MoveSlimesToTargets();
        //    }
        //}
        //private bool MoveForRotating(Slime Slime, Cell TargetCell, string MoveDirection)
        //{
        //    bool HitWall = false;

        //    int CurrentPosition = 0;
        //    int TargetPosition = 0;
        //    int MoveOffset = 0;
        //    switch (MoveDirection)
        //    {
        //        case "Up":
        //            CurrentPosition = Slime.Row;
        //            TargetPosition = TargetCell.Row;
        //            MoveOffset = -1;
        //            break;
        //        case "Down":
        //            CurrentPosition = Slime.Row;
        //            TargetPosition = TargetCell.Row;
        //            MoveOffset = 1;
        //            break;
        //        case "Right":
        //            CurrentPosition = Slime.Col;
        //            TargetPosition = TargetCell.Col;
        //            MoveOffset = 1;
        //            break;
        //        case "Left":
        //            CurrentPosition = Slime.Col;
        //            TargetPosition = TargetCell.Col;
        //            MoveOffset = -1;
        //            break;
        //    }

        //    while (CurrentPosition != TargetPosition)
        //    {
        //        Slime.TargetCell = GetTargetCell(Slime.TargetCell, MoveDirection);
        //        if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.WALL))
        //        {
        //            StopRotating(Slime.TargetCell);
        //            HitWall = true;
        //            return HitWall;
        //        }
        //        foreach (Slime OtherSlime in Slimes)
        //        {
        //            if (!ActiveSlimes.Contains(OtherSlime) && OtherSlime.ColRowVec == Slime.TargetCell.ColRowVec)
        //            {
        //                RotationFusionSlimes.Enqueue(OtherSlime);
        //            }
        //        }
        //        CurrentPosition += MoveOffset;
        //    }

        //    return HitWall;
        //}
        private void StopRotating(Cell TargetCell) 
        {
            WallOutline.Transform.Pos = new Vector2(TargetCell.Transform.Pos.X, TargetCell.Transform.Pos.Y);
            Timer = 60;
            foreach (Slime Slime in ActiveSlimes)
            {
                Slime.TargetCell = GetCell(Slime.Col, Slime.Row);
            }
        }
    }
}
