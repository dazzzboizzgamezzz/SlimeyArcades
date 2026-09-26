using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
    public class AnimatedSprite : Sprite
    {
        private AnimationCycle CurrentAnimation { get; set; }
        public AnimatedSprite(Texture2D NewTexture, int NewCycleSpeed, Color? NewColor = null, Action NewProcedure = null, List<Sprite> NewKeyFrames = null) : 
            base(NewTexture, NewColor == null ? Color.White : (Color)NewColor)
        {
            CurrentAnimation = new(new Sprite(NewTexture, NewColor == null ? Color.White : (Color)NewColor), NewCycleSpeed, NewKeyFrames, NewProcedure);
        }
        public void Update(int AnimationClock)
        {
            if (CurrentAnimation != null)
            {
                CurrentAnimation.Update(AnimationClock);
                Texture = CurrentAnimation.DisplaySprite.Texture;
                Color = CurrentAnimation.DisplaySprite.Color;
            }
        }
    }
    public class AnimationCycle
    {
        public Sprite DisplaySprite { get; set; } 
        public int CycleSpeed { get; set; }
        private List<Sprite> KeyFrameSprites { get; set; } = new();
        private int CurrentFrame { get; set; } = 0;
        private Action Procedure { get; set; }
        public AnimationCycle(Sprite NewSprite, int NewCycleSpeed, List<Sprite> NewKeyFrames = null, Action NewProcedure = null)
        {
            DisplaySprite = NewSprite;
            CycleSpeed = NewCycleSpeed;
            KeyFrameSprites = NewKeyFrames == null ? new() { NewSprite } : NewKeyFrames;
            Procedure = NewProcedure == null ? () => { } : NewProcedure;
        }
        public void Update(int AnimationClock)
        {
            if (AnimationClock % CycleSpeed == 0)
            {
                Procedure();
                CurrentFrame = (CurrentFrame + 1) % KeyFrameSprites.Count;
                DisplaySprite = KeyFrameSprites[CurrentFrame];
            }
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
    }
    public class LayerData
    {
        public int LayerDepth {  get; set; } = 1;
        public int LayerIndex { get; set; } = 1;
        public float Layer { get => LayerIndex * (float)Math.Pow(10, -LayerDepth); }
        public LayerData() { }
    }
}
