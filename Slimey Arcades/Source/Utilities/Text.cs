using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slimey_Arcades.Managers;

namespace Slimey_Arcades
{
    public interface IText
    {
        public Text TextObject { get; set; }
        public SpriteFont Font { get => TextObject.Font; }
        public SpriteEffects Effect { get => TextObject.Effect; set => TextObject.Effect = value; }
        public string Text { get => TextObject.Txt; set => TextObject.Txt = value; }
        public Color Color { get => TextObject.Color; set => TextObject.Color = value; }
        public int LayerDepth { get => TextObject.LayerData.LayerDepth; set => TextObject.LayerData.LayerDepth = value; }
        public float Layer { get => TextObject.LayerData.Layer; }
        public Transform Transform { get => TextObject.Transform; }
        public float Rotation { get => Transform.Rotation; set => Transform.Rotation = value; }
        public Vector2 Origin { get => Transform.Origin; set => Transform.Origin = value; }
        public float Scale { get => Transform.Scale; set => Transform.Scale = value; }
        public Vector2 Pos { get => Transform.Pos; set { Transform.X = (int)value.X; Transform.Y = (int)value.Y; } }
        public int TextWidth { get => (int)(Font.MeasureString(Text).X / (Scale * 100)); }
        public int TextHeight { get => (int)(Scale * 100); }
    }
    public class Text
    {
        public SpriteFont Font { get; init; }
        public SpriteEffects Effect { get; set; }
        public string Txt { get; set; }
        public Color Color { get; set; }
        public LayerData LayerData { get; set; } = new();
        public float Layer { get => LayerData.Layer; }
        public Transform Transform { get; init; }
        public float Rotation { get => Transform.Rotation; set => Transform.Rotation = value; }
        public Vector2 Origin { get => Transform.Origin; set => Transform.Origin = value; }
        public float Scale { get => Transform.Scale; set => Transform.Scale = value; }
        public Vector2 Pos { get => Transform.Pos; set { Transform.X = (int)value.X; Transform.Y = (int)value.Y; } }
        public int TextWidth { get => (int)(Font.MeasureString(Txt).X / (Scale * 100)); }
        public int TextHeight { get => (int)(Scale * 100); }
        public Text(Transform NewTransform = null, string NewText = "", Color? NewColor = null, float NewScale = 0.12f, SpriteFont NewFont = null, SpriteEffects NewEffect = SpriteEffects.None)
        {
            Transform = NewTransform == null ? Transform.None : NewTransform;
            Txt = NewText;
            Color = NewColor == null ? Color.Black : (Color)NewColor;
            Font = NewFont == null ? FontManager.Fonts["Arial"] : NewFont;
            Scale = NewScale;
            Effect = NewEffect;
        }
    }
}
