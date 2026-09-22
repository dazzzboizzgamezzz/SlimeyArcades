using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
//using Slimey_Arcades.Managers;

//namespace Slimey_Arcades.Windows
namespace Slimey_Arcades
{
    public class NewColorWindow : Window
    {
        private Dragger Dragger {  get; init; }
        private Polygon HueSelectBox { get; init; }
        private Polygon Selector { get; init; }
        private Polygon ColorSelectBox { get; init; }
        private Polygon HueSlider { get; init; }
        private Vector3 HSV { get; set; }
        private List<Text> DisValues { get; init; } = new();
        private int Alpha { get => alpha; set => alpha = Math.Clamp(value, 0, 255); }
        private int alpha;
        public Color PickedColor { get => new Color(ColorManager.HsvToRgb(HSV).R, ColorManager.HsvToRgb(HSV).G, ColorManager.HsvToRgb(HSV).B, Alpha); }

        public NewColorWindow(Vector2 Pos, Color StartingColor) : base(new Transform((int)Pos.X, (int)Pos.Y, 350, 250), Color.DarkGray)
        {
            HSV = ColorManager.RGBToHSV(StartingColor);
            Alpha = StartingColor.A;
            Dragger = new Dragger(Header.Transform);

            HueSlider = new Polygon(new Transform((int)Pos.X + 230, (int)Pos.Y + 45, 20, 180), Color.White);
            HueSelectBox = new Polygon(new Transform((int)Pos.X + 225, (int)(Pos.Y + 45 + HSV.X/2), 30, 5), Color.White);
            ColorSelectBox = new Polygon(new Transform((int)Pos.X + 15, (int)Pos.Y + 35, 200, 200), Color.White);
            Selector = new Polygon(new Transform((int)(Pos.X + 15 + (1 - HSV.Y * 0.005)), (int)(Pos.Y + 35 + (1 - HSV.Z * 0.005)), 5, 5), Color.White, "Circle");

            HueSelectBox.Sprite = new Sprite(Shapes.MakeOutline(HueSelectBox.Transform.Rect, 2), Color.Black);
            HueSlider.Sprite.Texture = Shapes.HueSlider;
            ColorSelectBox.Sprite.Texture = Shapes.MakeColorPicker(HSV.X);

            List<Polygon> TextBoxes = new();

            for (int i = 0; i < 7; i++)
            {
                Transform NewPos = new Transform((int)Pos.X + 260, (int)Pos.Y + 30 + i * 30, 55, 25);
                string NewTextString = "";
                switch (i)
                {
                    case 0: NewTextString = "R:"; break;
                    case 1: NewTextString = "G:"; break;
                    case 2: NewTextString = "B:"; break;
                    case 3: NewTextString = "A:"; break;
                    case 4: NewTextString = "H:"; break;
                    case 5: NewTextString = "S:"; break;
                    case 6: NewTextString = "V:"; break;
                }
                Text NewText = new Text(NewPos, NewTextString);
                SubObjects.Add(NewText);
            }
            for (int i = 0; i < 7; i++)
            {
                Transform NewPos = new Transform((int)Pos.X + 280, (int)Pos.Y + 30 + i * 30, 62, 25);
                Transform NewTextPos = new Transform((int)Pos.X + 285, (int)Pos.Y + 32 + i * 30, 55, 25);
                Polygon TextBox = new Polygon(NewPos, Color.White);
                Text NewText = new Text(NewTextPos, "");
                //NewText.Layer = 2f;
                DisValues.Add(NewText);
                SubObjects.Add(TextBox);
                SubObjects.Add(NewText);
            }
            SetTextboxes();

            SubObjects.Add(ColorSelectBox);
            SubObjects.Add(HueSlider);
            SubObjects.Add(Selector);
            SubObjects.Add(HueSelectBox);
        }
        private bool Click { get; set; } = false;
        public override void Update()
        {
            Dragger.Drag(this);
            if (!Dragger.Dragging)
            {
                base.Update();
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) Click = true;
                if (Mouse.GetState().LeftButton == ButtonState.Released) Click = false;

                Vector2 MPos = Mouse.GetState().Position.ToVector2();

                if (ColorSelectBox.Transform.Rect.Contains(MPos) && Click) 
                {
                    Selector.Transform.Pos = MPos;
                    int XOffset = ColorSelectBox.Transform.X - (int)MPos.X;
                    int YOffset = ColorSelectBox.Transform.Y - (int)MPos.Y;
                    HSV = new Vector3(HSV.X, 1 + (XOffset * 0.005f), 1 + (YOffset * 0.005f));
                    SetTextboxes();
                }
                if (HueSlider.Transform.Rect.Contains(MPos) && Click) 
                {
                    HueSelectBox.Transform.Y = (int)MPos.Y;
                    int HueValue = (HueSelectBox.Transform.Y - HueSlider.Transform.Y)*2;
                    HSV = new Vector3(HueValue, HSV.Y, HSV.Z);
                    ColorSelectBox.Sprite.Texture = Shapes.MakeColorPicker(HSV.X);
                    SetTextboxes();
                }
            }
        }
        private void SetTextboxes()
        {
            for (int i = 0; i < 7; i++)
            {
                string NewTextString = "";
                switch (i)
                {
                    case 0: NewTextString = PickedColor.R.ToString(); break;
                    case 1: NewTextString = PickedColor.G.ToString(); break;
                    case 2: NewTextString = PickedColor.B.ToString(); break;
                    case 3: NewTextString = Alpha.ToString(); break;
                    case 4: NewTextString = HSV.X.ToString(); break;
                    case 5: NewTextString = HSV.Y.ToString(); break;
                    case 6: NewTextString = HSV.Z.ToString(); break;
                }
                if (NewTextString.Length > 6) NewTextString = NewTextString.Remove(6);
                DisValues[i].Txt = NewTextString;
            }
        }
    }
}
