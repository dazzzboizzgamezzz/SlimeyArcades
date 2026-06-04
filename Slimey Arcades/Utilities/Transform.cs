using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Slimey_Arcades.Utilities
{
    public class Transform :ICloneable
    {
        public Rectangle Rect { get => new Rectangle(X, Y, Width, Height); }
        public Vector2 Pos { get => new Vector2(X, Y); set { X = (int)value.X; Y = (int)value.Y; } }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set;}
        public int Height { get; set;}
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Center { get => new Vector2(X + Width/2, Y + Height/2); }
        public Transform(int NewX, int NewY, int NewWidth, int NewHeight, float NewScale = 1)
        {
            X = NewX;
            Y = NewY;
            Width = NewWidth;
            Height = NewHeight;
            Scale = NewScale;
            Rotation = 0;
            Origin = Vector2.Zero;
        }
        public Transform(int NewX, int NewY, int NewWidth, int NewHeight, Vector2 NewOrigin, float NewScale = 1, float NewRotation = 0)
        {
            X = NewX;
            Y = NewY;
            Width = NewWidth;
            Height = NewHeight;
            Origin = NewOrigin;
            Scale = NewScale;
            Rotation = NewRotation;
        }
        public Transform(Rectangle NewTransform)
        {
            X = NewTransform.X;
            Y = NewTransform.Y;
            Width = NewTransform.Width;
            Height = NewTransform.Height;
            Scale = 1;
            Rotation = 0;
            Origin = Vector2.Zero;
        }
        public void ScaleObject()
        {
            float NewWidth = Width * Scale;
            float NewHeight = Height * Scale;
            Width = (int)Math.Round(NewWidth);
            Height = (int)Math.Round(NewHeight);
        }
        public object Clone() 
        { 
            return MemberwiseClone();
        }
    }
}
