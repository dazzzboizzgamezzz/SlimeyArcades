using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Slimey_Arcades.Managers;
using Slimey_Arcades.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slimey_Arcades
{
    public class LevelGrid : Grid
    {
        //public bool Paused { get; set; } = false;
        private List<Keys> PressedKeys { get; set; } = new();
        private List<Slime> ActiveSlimes { get; set; } = new();
        private Slime Player { get; set; }
        private Polygon WallOutline { get; set; }
        private int Timer { get; set; } = 0;
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
            if (LevelScene.PauseState == "")
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
        private void Move(string Direction)
        {
            List<Cell> TargetCells = new();

            foreach (Slime Slime in ActiveSlimes)
            {
                Cell NewTarget = GetTargetCell(Slime.TargetCell, Direction);
                if (NewTarget == null) return;
                Slime.TargetCell = NewTarget;
                TargetCells.Add(NewTarget);
            }

            if (!HitWall(TargetCells, Direction))
            {
                for(int i = 0; i < ActiveSlimes.Count; i++)
                {
                    Slime Slime = ActiveSlimes[i];
                    if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.ICE))
                    {
                        Fusion(Slime.TargetCell);
                        Move(Direction);
                    }
                }
                bool HitPit = true;
                foreach (Cell Cell in TargetCells)
                {
                    if (Cell.Properties.Contains(CELLOBJECTS.PIT)) continue;
                    HitPit = false;
                    break;
                }
                if (HitPit)
                {
                    foreach (Slime Slime in ActiveSlimes)
                    {
                        Slime.TargetCell = null;
                        Slime.MoveToCell(Slime.TargetCell);
                    }
                }
            }
            MoveSlimesToTargets();
        }
        private void MoveSlimesToTargets()
        {
            for (int i = 0; i < ActiveSlimes.Count; i++)
            {
                Slime Slime = ActiveSlimes[i];
                Slime.MoveToCell(Slime.TargetCell);
                Fusion(Slime);
            }
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
        private bool HitWall(List<Cell> TargetCells, string Direction)
        {
            foreach (Cell Cell in TargetCells)
            {
                if (Cell.Properties.Contains(CELLOBJECTS.WALL))
                {
                    string NewDirection = "";
                    switch (Direction)
                    {
                        case "Down": NewDirection = "Up"; break;
                        case "Up": NewDirection = "Down"; break;
                        case "Right": NewDirection = "Left"; break;
                        case "Left": NewDirection = "Right"; break;
                    }
                    foreach (Slime Slime in ActiveSlimes)
                    {
                        Slime.TargetCell = GetTargetCell(Slime.TargetCell, NewDirection);
                    }
                    return true;
                }
            }
            return false;
        }
        private void Fusion(Cell StartingCell)
        {
            List<Cell> Neighbors = new()
            {
                GetTargetCell(StartingCell, "Up"),
                GetTargetCell(StartingCell, "Down"),
                GetTargetCell(StartingCell, "Left"),
                GetTargetCell(StartingCell, "Right"),
            };
            //for (int i = 0; i < Slimes.Count; i++)
            //{
            //Slime Slime = Slimes[i];
            //string SlimeType = Slime.Properties[0];
            //foreach (Cell Neighbor in Neighbors)
            //{
            //    if (Neighbor != null && Neighbor.Properties.Contains(SlimeType))
            //    {
            //        ActiveSlimes.Add(Slime);
            //        Slimes.Remove(Slime);
            //        i--;
            //        Neighbor.Properties.Remove(SlimeType);
            //        Slime.TargetCell = Neighbor;
            //        break;
            //    }
            //}
            //}
            foreach (Slime Slime in Slimes)
            {
                if (!ActiveSlimes.Contains(Slime) && Slime.ColRowVec != new Vector2(-1, -1))
                {
                    foreach(Cell Neighbor in Neighbors)
                    {
                        if (Slime.ColRowVec == Neighbor.ColRowVec)
                        {
                            ActiveSlimes.Add(Slime);
                        }
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
            if (Winning) LevelScene.PauseState = "Winning";
        }
        /// <summary>
        /// 1. Sort each slime by their distance from the player (old code, can be depricated)
        /// 2. For each slime other than the player, establish their 'quadrant', determined by placing an 'X' over the player and seeing which section the slime is in
        /// 3. Set the target rotational target as (x, y) => (y, -x) for clockwise and (x, y) => (-y, x) for counterclockwise
        /// 4. Set the slime TargetCell one cell closer to the rotational target, moving away from the player (i.e. upper quadrant clockwise moves left then down)
        /// 5. Repeatedly move the TargetCell until you reach the rotational target
        /// 6. If the moving slime collides with an inactive slime, the inactive slime is added to the active slimes, and it's position is placed next to the slimes starting position
        ///     After the slime is done rotating, the added slime also rotates. This allows the player to collide with a slime then a wall and have the slime attach properly
        /// 7. If the slime collides with a wall, reset the TargetCell of every slime to their respective positions and stop rotating
        /// 8. Move all of the slimes to their TargetCells, which includes fusion and stretching
        /// </summary>
        /// <param name="Direction"></param>
        private void Rotate(string Direction)
        {
            if (ActiveSlimes.Count > 1)
            {
                List<Slime> SlimesToSort = new();
                foreach (Slime Slime in ActiveSlimes)
                {
                    if (Slime != Player)
                    {
                        int RelX = Slime.Col - Player.Col;
                        int RelY = -(Slime.Row - Player.Row);
                        Slime.DistanceToCenter = Math.Abs(RelX) + Math.Abs(RelY);
                        SlimesToSort.Add(Slime);
                    }
                }
                Stack<Slime> SlimesToRotate = new(SlimesToSort.OrderBy(o => o.DistanceToCenter));
                while (SlimesToRotate.Count > 0)
                {
                    Slime Slime = SlimesToRotate.Pop();

                    int RelX =   Slime.Col - Player.Col;
                    int RelY = -(Slime.Row - Player.Row);

                    string Quadrant = "";

                    if (RelX + RelY > 0 && RelX - RelY >= 0) Quadrant = "Right";
                    if (RelX + RelY <= 0 && RelX - RelY > 0) Quadrant = "Down";
                    if (RelX + RelY < 0 && RelX - RelY <= 0) Quadrant = "Left";
                    if (RelX + RelY >= 0 && RelX - RelY < 0) Quadrant = "Up";

                    Vector2 RotationTarget = Vector2.Zero;
                    Cell TargetCell = null;

                    string MoveDirection1 = "";
                    string MoveDirection2 = "";

                    if (Direction == "Clockwise")
                    {
                        RotationTarget = new Vector2(RelY, -RelX);
                        TargetCell = GetCell((int)RotationTarget.X + Player.Col, -(int)RotationTarget.Y + Player.Row);
                        if (TargetCell == null) return;
                        switch (Quadrant)
                        {
                            case "Up":    MoveDirection1 = "Right"; MoveDirection2 = "Down";  break;
                            case "Right": MoveDirection1 = "Down";  MoveDirection2 = "Left";  break;
                            case "Down":  MoveDirection1 = "Left";  MoveDirection2 = "Up";    break;
                            case "Left":  MoveDirection1 = "Up";    MoveDirection2 = "Right"; break;
                        }
                    }
                    else
                    {
                        RotationTarget = new Vector2(-RelY, RelX);
                        TargetCell = GetCell((int)RotationTarget.X + Player.Col, -(int)RotationTarget.Y + Player.Row);
                        if (TargetCell == null) return;
                        switch (Quadrant)
                        {
                            case "Up":    MoveDirection1 = "Left";  MoveDirection2 = "Down";  break;
                            case "Left":  MoveDirection1 = "Down";  MoveDirection2 = "Right"; break;
                            case "Down":  MoveDirection1 = "Right"; MoveDirection2 = "Up";    break;
                            case "Right": MoveDirection1 = "Up";    MoveDirection2 = "Left";  break;
                        }
                    }

                    bool HitWall = false;
                    Cell OtherTarget = GetCell(Slime.Col, Slime.Row);
                    if (MoveForRotating(Slime, TargetCell, MoveDirection1)) HitWall = true;
                    if (RotationFusionSlimes.Count > 0)
                    {
                        while (RotationFusionSlimes.Count > 0)
                        {
                            Slime OtherSlime = RotationFusionSlimes.Dequeue();
                            OtherTarget = GetTargetCell(OtherTarget, MoveDirection1);
                            OtherSlime.MoveToCell(OtherTarget);
                            ActiveSlimes.Add(OtherSlime);
                            SlimesToRotate.Push(OtherSlime);
                        }
                    }
                    if (HitWall) break;
                    if (MoveForRotating(Slime, TargetCell, MoveDirection2)) HitWall = true;
                    if (RotationFusionSlimes.Count > 0)
                    {
                        while (RotationFusionSlimes.Count > 0)
                        {
                            Slime OtherSlime = RotationFusionSlimes.Dequeue();
                            OtherTarget = GetTargetCell(OtherTarget, MoveDirection1);
                            OtherSlime.MoveToCell(OtherTarget);
                            ActiveSlimes.Add(OtherSlime);
                            SlimesToRotate.Push(OtherSlime);
                        }
                    }
                    if (HitWall) break;
                }
                MoveSlimesToTargets();
            }
        }
        private Queue<Slime> RotationFusionSlimes = new();
        private bool MoveForRotating(Slime Slime, Cell TargetCell, string MoveDirection)
        {
            bool HitWall = false;

            int CurrentPosition = 0;
            int TargetPosition = 0;
            int MoveOffset = 0;
            switch (MoveDirection)
            {
                case "Up":
                    CurrentPosition = Slime.Row;
                    TargetPosition = TargetCell.Row;
                    MoveOffset = -1;
                    break;
                case "Down":
                    CurrentPosition = Slime.Row;
                    TargetPosition = TargetCell.Row;
                    MoveOffset = 1;
                    break;
                case "Right":
                    CurrentPosition = Slime.Col;
                    TargetPosition = TargetCell.Col;
                    MoveOffset = 1;
                    break;
                case "Left":
                    CurrentPosition = Slime.Col;
                    TargetPosition = TargetCell.Col;
                    MoveOffset = -1;
                    break;
            }

            while (CurrentPosition != TargetPosition)
            {
                Slime.TargetCell = GetTargetCell(Slime.TargetCell, MoveDirection);
                if (Slime.TargetCell.Properties.Contains(CELLOBJECTS.WALL))
                {
                    StopRotating(Slime.TargetCell);
                    HitWall = true;
                    return HitWall;
                }
                //Slime collision logic
                //if (Slimes.Count > 0)
                //{
                //    for (int i = 0; i < Slimes.Count; i++)
                //    {
                //        Slime OtherSlime = Slimes[i];
                //        //if (Slime.TargetCell.Properties.Contains(OtherSlime.Type))
                //        //{
                //        //    Slimes.Remove(OtherSlime);
                //        //    RotationFusionSlimes.Enqueue(OtherSlime);
                //        //}
                //    }
                //}
                foreach (Slime OtherSlime in Slimes)
                {
                    if (!ActiveSlimes.Contains(OtherSlime) && OtherSlime.ColRowVec == Slime.TargetCell.ColRowVec)
                    {
                        RotationFusionSlimes.Enqueue(OtherSlime);
                    }
                }
                CurrentPosition += MoveOffset;
            }

            return HitWall;
        }
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
