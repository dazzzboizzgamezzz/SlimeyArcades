using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slimey_Arcades.Utilities;

namespace Slimey_Arcades.Objects
{
    public class Polygon : IDraw
    {
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public Polygon(Transform NewTransform, Color BGColor, string Type = "Square")
        {
            Transform = NewTransform;
            Texture2D NewTexture = null;
            switch (Type)
            {
                case "Circle": NewTexture = Shapes.Circle; break;
                case "Square": NewTexture = Shapes.Square; break;
                case "RTriangle": NewTexture = Shapes.Triangle; break;
                case "ETriangle": NewTexture = Shapes.ETriangle; break;
                default: NewTexture = Shapes.Square; break;
            }
            Sprite = new Sprite(NewTexture, BGColor);
        }
    }
}
