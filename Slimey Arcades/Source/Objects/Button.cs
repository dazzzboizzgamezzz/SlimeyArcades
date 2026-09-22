using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
//using Slimey_Arcades.Utilities;
//using Slimey_Arcades.Managers;

namespace Slimey_Arcades
{
    public class Button : IContainer, IDraw, IUpdate
    {
        public Transform Transform { get; set; }
        protected Clicker Click {  get; init; }
        public Sprite Sprite {  get; set; }
        //protected Text ButtonText { get; init; }
        public Text ButtonText { get; init; }
        public Container Container { get; init; } = new();
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
            Container.ObjectsToLoad.Add(ButtonText);
            Highlight = BGColor;
            Highlightable = true;
        }
        public Button(Transform NewTransform, Texture2D Texture = null, Color? BGColor = null, string Txt = "", bool TextCentered = false)
        {
            Sprite = Texture == null ? new Sprite(Shapes.Square, ColorManager.None) : new Sprite(Texture, ColorManager.None);
            Transform = NewTransform;
            Click = new Clicker(NewTransform);
            //ButtonText = new Text(NewTransform, Txt);
            ButtonText = new Text(new Transform(NewTransform.X, NewTransform.Y, NewTransform.Width, NewTransform.Height, NewTransform.Scale), Txt);
            Container.ObjectsToLoad.Add(ButtonText);
            Color BackgroundColor = BGColor == null ? ColorManager.None : (Color)BGColor;
            Sprite.Color = BackgroundColor;
            Highlight = BackgroundColor;
            Highlightable = true;   
            if (TextCentered) CenterText();
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
            ButtonText.Pos = new Vector2(Transform.Center.X - ((ButtonText.TextWidth / 2) + 15), Transform.Center.Y - ((ButtonText.TextHeight / 2) + 5));
        }
    }
}
