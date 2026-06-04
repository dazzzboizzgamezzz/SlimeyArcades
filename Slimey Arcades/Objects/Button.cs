using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using Slimey_Arcades.Utilities;
using Slimey_Arcades.Managers;

namespace Slimey_Arcades.Objects
{
    public class Button : IContainer, IDraw, IUpdate
    {
        public int LoadedCount { get; set; }
        public bool Destroy { get; set; } = false;
        public Transform Transform { get; set; }
        protected Clicker Click {  get; init; }
        public Sprite Sprite {  get; set; }
        public ArrayList SubObjects { get; set; } = new();
        protected Text ButtonText { get; init; }
        public Sorter Obj { get => new Sorter(SubObjects); }
        private Color Highlight { get; set; }
        private Color UnHighlight { get; set; }
        public bool Highlightable {  get; set; }
        public string Text
        { 
            get => ButtonText.Txt; 
            set => ButtonText.Txt = value; 
        }
        public Color TextColor
        {
            get => ButtonText.Color;
            set => ButtonText.Color = value;
        }
        public Action Function { get; set; } 
        public Button(int X, int Y, int NewWidth, int NewHeight, Texture2D Texture, Color BGColor, string Txt)
        {
            Sprite = new Sprite(Texture, BGColor);
            Transform = new Transform(X, Y, NewWidth, NewHeight);
            Click = new Clicker(Transform);
            ButtonText = new Text(new Transform(X, Y, NewWidth, NewHeight), Txt);
            SubObjects.Add(ButtonText);
            Highlight = BGColor;
            Highlightable = true;
            //UnHighlight = new Color(Sprite.Color.R - 30, Sprite.Color.G - 30, Sprite.Color.B - 30);
        }
        public Button(Transform NewTransform, Texture2D Texture = null, Color? BGColor = null, string Txt = "", bool TextCentered = false)
        {
            Sprite = Texture == null ? new Sprite(Shapes.Square, ColorManager.None) : new Sprite(Texture, ColorManager.None);
            Transform = (Transform)NewTransform.Clone();
            Click = new Clicker((Transform)NewTransform.Clone());
            ButtonText = new Text((Transform)NewTransform.Clone(), Txt);
            SubObjects.Add(ButtonText);
            Color BackgroundColor = BGColor == null ? ColorManager.None : (Color)BGColor;
            Sprite.Color = BackgroundColor;
            Highlight = BackgroundColor;
            Highlightable = true;   
            if (TextCentered) CenterText();
            //UnHighlight = new Color(Sprite.Color.R - 30, Sprite.Color.G - 30, Sprite.Color.B - 30);
        }
        public void Update()
        {
            Point MPos = Mouse.GetState().Position;
            if (Highlightable)
            {
                if (UnHighlight == ColorManager.None) UnHighlight = new Color(Sprite.Color.R - 30, Sprite.Color.G - 30, Sprite.Color.B - 30);
                Sprite.Color = (Transform.Rect.Contains(MPos)) ? UnHighlight : Highlight;
            }
            if (Click.Click()) Function();
        }
        public void CenterText()
        {
            ButtonText.Pos = new Vector2(Transform.Center.X - (ButtonText.TextWidth / 2), Transform.Center.Y - (ButtonText.TextHeight / 2));
        }
    }
}
