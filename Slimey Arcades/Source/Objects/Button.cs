using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Slimey_Arcades
{
    public class Button : IContainer, IDraw, IUpdate
    {
        public Transform Transform { get; set; }
        public Sprite Sprite {  get; set; }
        public Text ButtonText { get; init; }
        public Container Container { get; init; } = new();
        private Color Highlight { get; set; }
        private Color UnHighlight { get; set; }
        public bool Highlightable {  get; set; }
        private bool Active = true;
        public bool IsActive { get => Active; set { Sprite.IsVisible = value; Active = value; } }
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
            ButtonText = new Text(new Transform(X, Y, NewWidth, NewHeight), Txt);
            Container.ObjectsToLoad.Add(ButtonText);
            Highlight = BGColor;
            Highlightable = true;
        }
        public Button(Transform NewTransform, Texture2D Texture = null, Color? BGColor = null, string Txt = "", bool TextCentered = false)
        //public Button(Transform NewTransform, Texture2D Texture = null, Color? BGColor = null, string Txt = "", bool TextCentered = true)
        {
            Sprite = Texture == null ? new Sprite(Shapes.Square, ColorManager.None) : new Sprite(Texture, ColorManager.None);
            Transform = NewTransform;
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
            if (!IsActive) return;
            if (Highlightable)
            {
                if (UnHighlight == ColorManager.None) UnHighlight = new Color(Sprite.Color.R - 30, Sprite.Color.G - 30, Sprite.Color.B - 30);
                Sprite.Color = MouseControl.Hovering(Transform) ? UnHighlight : Highlight;
            }
            if (MouseControl.LeftClick(Transform)) Function();
        }
        private void CenterText()
        {
            ButtonText.Pos = new Vector2(Transform.Center.X - ((ButtonText.TextWidth / 2) + 15), Transform.Center.Y - ((ButtonText.TextHeight / 2) + 5));
        }
    }
    public class LoopingButton : IContainer, IDraw, IUpdate
    {
        public Container Container { get; init; } = new();
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public Button Button { get => Buttons[CurrentButton]; }
        private List<Button> Buttons { get; init; }
        private int CurrentButton { get; set; } = 0;
        public LoopingButton(List<Button> NewButtons, Color? BGColor = null, Color? NewTriangleColor = null)
        {
            Buttons = NewButtons;
            Transform Base = Button.Transform;

            int ArrowSize = Base.Width < Base.Height ? (int)(Base.Width / 2) : (int)(Base.Height / 2);
            int HalfSize = (int)(ArrowSize / 2);
            //Color TriangleColor = NewTriangleColor == null ? Color.White : (Color)NewTriangleColor;
            //Polygon LeftTriangle = new Polygon(new Transform(Base.X - TSize / 2, (int)Base.Center.Y, TSize, TSize / 2), TriangleColor);
            //LeftTriangle.Sprite.Texture = Shapes.Square;
            //LeftTriangle.Transform.Origin = LeftTriangle.Transform.Center;
            //LeftTriangle.Transform.Rotation = -0.5f;
            //Button LeftButton = new Button(new Transform((int)LeftTriangle.Transform.X, (int)LeftTriangle.Transform.Y, LeftTriangle.Transform.Height, LeftTriangle.Transform.Width));
            //LeftButton.Function = () => { LoopButtons(-1); };
            //Container.ObjectsToLoad.Add(LeftButton);
            //Container.ObjectsToLoad.Add(LeftTriangle);
            //Button RightTriangle = new Button(new Transform(Base.X + Base.Width + 3, (int)Base.Center.Y, TSize, TSize / 2), Shapes.ETriangle, TriangleColor);
            if (Buttons.Count > 1)
            {
                Button RightTriangle = new Button(new Transform(Base.X + Base.Width + 3, (int)Base.Center.Y - HalfSize, HalfSize, ArrowSize), Txt: ">", TextCentered: false);
                //RightTriangle.Transform.Rotation = 0.5f;
                RightTriangle.Function = () => { LoopButtons(1); };
                Container.ObjectsToLoad.Add(RightTriangle);
                Button LeftTriangle = new Button(new Transform(Base.X - HalfSize - 3, (int)Base.Center.Y - HalfSize, HalfSize, ArrowSize), Txt: "<", TextCentered: false);
                //RightTriangle.Transform.Rotation = 0.5f;
                LeftTriangle.Function = () => { LoopButtons(-1); };
                Container.ObjectsToLoad.Add(LeftTriangle);
            }

            Transform = new Transform(Base.X - ArrowSize, Base.Y - 5, Base.Width + (ArrowSize * 2), Base.Height + 10);
            Sprite = new Sprite(Shapes.Square, BGColor == null ? ColorHelp.None : (Color)BGColor);
            foreach (Button NextButton in Buttons)
            {
                if (NextButton != Button) NextButton.IsActive = false;
                Container.ObjectsToLoad.Add(NextButton);
            }
        }
        private void LoopButtons(int Direction)
        {
            Button.IsActive = false;
            CurrentButton = (CurrentButton + Direction) % Buttons.Count;
            if (CurrentButton < 0) CurrentButton = Buttons.Count - 1;
            Button.IsActive = true;
            Button.Function();
        }
        public void Update() { }
    }

}
