using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;
using Slimey_Arcades.Utilities;

namespace Slimey_Arcades
{
    public interface IContainer
    {
        public ArrayList SubObjects { get; set; }
        public Sorter Obj { get => new Sorter(SubObjects); }
        public int LoadedCount { get; set; }
    }
    public interface IUpdate
    {
        public void Update();
        public bool Destroy { get; set; }
    }
    public interface IDraw
    {
        protected Transform Transform { get; set; }
        protected Sprite Sprite { get; set; }
        public Texture2D Texture { get => Sprite.Texture; }
        public Color Color { get => Sprite.Color; }
        public LayerData LayerData { get => Sprite.LayerData; }
        public float Layer { get => Sprite.Layer; }
        public SpriteEffects Effect { get => Sprite.Effect; }
        public float Rotation { get => Transform.Rotation; }
        public Vector2 Origin { get => Transform.Origin; }
        public Rectangle Rect { get => Transform.Rect; }
        public Vector2 Pos { get => Transform.Pos; set { Transform.X = (int)value.X; Transform.Y = (int)value.Y; } }
        public float Scale { get => Transform.Scale; }
        public void ScaleObject() { Transform.ScaleObject(); }
    }
}
