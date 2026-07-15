using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slimey_Arcades.Managers;
using System;

namespace Slimey_Arcades
{
    public interface IDraw
    {
        public Transform Transform { get; set; }
        protected Sprite Sprite { get; set; }
        public Texture2D Texture { get => Sprite.Texture; }
        public Color Color { get => Sprite.Color; }
        public float Layer { get => Sprite.Layer; }
        public int LayerIndex { get => Sprite.LayerData.LayerIndex; set => Sprite.LayerData.LayerIndex = value; }
        public int LayerDepth { get => Sprite.LayerData.LayerDepth; set => Sprite.LayerData.LayerDepth = value; }
        public SpriteEffects Effect { get => Sprite.Effect; }
        public float Rotation { get => Transform.Rotation; }
        public Vector2 Origin { get => Transform.Origin; }
        public Rectangle Rect { get => Transform.Rect; }
        public Vector2 Pos { get => Transform.Pos; set { Transform.X = (int)value.X; Transform.Y = (int)value.Y; } }
    }
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public SpriteEffects Effect { get; set; }
        public LayerData LayerData { get; set; } = new();
        public float Layer { get => LayerData.Layer; }
        public Sprite(Texture2D NewTexture, Color NewColor) 
        { 
            Texture = NewTexture;
            Color = NewColor;
            Effect = SpriteEffects.None;
        }
    }
    public class Transform
    {
        public Rectangle Rect { get => new Rectangle(X, Y, Width, Height); }
        public Vector2 Pos { get => new Vector2(X, Y); set { X = (int)value.X; Y = (int)value.Y; } }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Center { get => new Vector2(X + Width / 2, Y + Height / 2); }
        public static Transform None { get => new Transform(0, 0, 0, 0); }
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
    }
    public class LayerData
    {
        public int LayerDepth {  get; set; } = 1;
        public int LayerIndex { get; set; } = 1;
        public float Layer { get => LayerIndex * (float)Math.Pow(10, -LayerDepth); }
        public LayerData() { }
    }
    //public class Text
    //{
    //    public SpriteFont Font { get; init; }
    //    public string Txt { get; set; }
    //    public Color Color { get; set; }
    //    public SpriteEffects Effect { get; set; }
    //    //public float Layer { get; set; } = 0;
    //    public LayerData LayerData { get; set; } = new();
    //    public float Layer { get => LayerData.Layer; }
    //    public Transform Transform { get; init; }
    //    public float Rotation { get => Transform.Rotation; set => Transform.Rotation = value; }
    //    public Vector2 Origin { get => Transform.Origin; set => Transform.Origin = value; }
    //    public float Scale { get => Transform.Scale; set => Transform.Scale = value; }
    //    public Vector2 Pos { get => Transform.Pos; set { Transform.X = (int)value.X; Transform.Y = (int)value.Y; } }
    //    public int TextWidth { get => (int)(Font.MeasureString(Txt).X/(Scale*100)); }
    //    public int TextHeight { get => (int)(Scale * 100); }
    //    public Text(Transform NewTransform, string NewText, float NewScale = 0.12f, SpriteFont NewFont = null) 
    //    { 
    //        Transform = NewTransform;
    //        Txt = NewText;
    //        Color = Color.Black;
    //        if (NewFont == null) NewFont = FontManager.Fonts["Arial"];
    //        Font = NewFont;
    //        Scale = NewScale;
    //        Effect = SpriteEffects.None;
    //    }
    //}
}
