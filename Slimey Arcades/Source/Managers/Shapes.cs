using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Slimey_Arcades
{
    public static class Shapes
    {
        public static Texture2D Square;
        public static Texture2D Circle;
        public static Texture2D Triangle;
        public static Texture2D ETriangle;
        public static Texture2D BackedCircle;
        public static Texture2D XCross;
        public static Texture2D HueSlider;
        private static Color[] RawPixels;
        private static Color Filled = Color.White;
        private static Color Empty = new Color(0, 0, 0, 0);
        private static GraphicsDevice GD;
        public static void MakeShapes(GraphicsDevice GraphicsDevice)
        {
            GD = GraphicsDevice;
            int Res = 1000;

            //Square
            Square = new Texture2D (GraphicsDevice, 1, 1);
            RawPixels = new Color[1];
            for (int i = 0; i < 1; i++) { RawPixels[i] = Filled; }
            Square.SetData(RawPixels);

            //High-Res Circle
            int Size = Res * Res;
            Circle = new Texture2D(GraphicsDevice, Res, Res);
            RawPixels = new Color[Size];
            for (int i = 0; i < Size; i++)
            {
                int X = (i % Res) - (Res/2);
                int Y = (int)MathF.Floor((float)(i / Res)) - (Res / 2);
                RawPixels[i] = Empty;
                if ( (X * X) + (Y * Y) <= (Res / 2) * (Res / 2)){  RawPixels[i] = Filled; }
            }
            Circle.SetData(RawPixels);

            //Backed Circle
            BackedCircle = new Texture2D(GraphicsDevice, Res, Res);
            RawPixels = new Color[Size];
            for (int i = 0; i < Size; i++)
            {
                int X = (i % Res) - (Res / 2);
                int Y = (int)MathF.Floor((float)(i / Res)) - (Res / 2);
                RawPixels[i] = Color.Gray;
                if ((X * X) + (Y * Y) <= (Res / 2) * (Res / 2)) { RawPixels[i] = Color.Black; }
            }
            BackedCircle.SetData(RawPixels);

            //Right Triangle
            Triangle = new Texture2D(GraphicsDevice, Res, Res);
            RawPixels = new Color[Res*Res];
            for (int i = 0; i < Res * Res; i++)
            {
                int X = i % Res;
                int Y = (int)MathF.Floor((float)(i / Res));
                RawPixels[i] = Empty;
                if (X <= Y) { RawPixels[i] = Filled; }
            }
            Triangle.SetData(RawPixels);

            //Equilateral Triangle
            ETriangle = new Texture2D(GraphicsDevice, Res + 1, Res + 1);
            RawPixels = new Color[(Res + 1) * (Res + 1)];
            int Min = Res / 2;
            int Max = Res / 2;
            for (int i = 0; i < (Res + 1) * (Res + 1); i++)
            {
                int X = i % (Res + 1);
                if (i > 0 && i % ((Res + 1) * 2) == 0) { Min--; Max++; }
                RawPixels[i] = Empty;
                if ((X >= Min) && (X <= Max)) { RawPixels[i] = Filled; }
            }
            ETriangle.SetData(RawPixels);

            //HueSlider
            HueSlider = new Texture2D(GraphicsDevice, 1, 360);
            RawPixels = new Color[360];
            for (int i = 0; i < 360; i++)
            {
                Color NewColor = ColorManager.HsvToRgb(new Vector3((float)i, 1, 1));
                RawPixels[i] = NewColor;
            }
            HueSlider.SetData(RawPixels);

            //XCross
            XCross = new Texture2D(GraphicsDevice, Res, Res);
            RawPixels = new Color[Res * Res];
            for (int i = 0; i < Res * Res; i++)
            {
                int X = i % Res;
                int Y = (int)MathF.Floor((float)(i / Res));
                //int Weight = 150;
                int Weight = 100;
                RawPixels[i] = Filled;
                if ((X + Weight < Y || Y + Weight < X)) RawPixels[i] = Empty;
                if (X + Y < Res + Weight && X + Y > Res - Weight) RawPixels[i] = Filled;
            }
            XCross.SetData(RawPixels);
        }
        public static Texture2D MakeOutline(Rectangle Rect, int OutlineWeight = 5)
        {
            Texture2D Outline;
            if (Rect.Width >= OutlineWeight && Rect.Height >= OutlineWeight) Outline = new Texture2D(GD, Rect.Width + OutlineWeight, Rect.Height + OutlineWeight);
            else return new Texture2D(GD, 1, 1);
            int TotalPixels = (Rect.Width + OutlineWeight) * (Rect.Height + OutlineWeight);
            RawPixels = new Color[TotalPixels];
            for (int i = 0; i < TotalPixels; i++)
            {
                int X = i % (Rect.Width + OutlineWeight);
                int Y = (int)MathF.Floor((float)(i / (Rect.Width + OutlineWeight)));
                RawPixels[i] = Empty;
                if (X <= OutlineWeight || X >= Rect.Width || Y <= OutlineWeight || Y >= Rect.Height) { RawPixels[i] = Filled; }
            }
            Outline.SetData(RawPixels);
            return Outline;
        }
        public static Texture2D MakeColorPicker(float H)
        {
            Texture2D NewTexture = new Texture2D(GD, 100, 100);
            RawPixels = new Color[10000];
            Color NewColor;
            for (int i = 0; i < 10000; i++)
            {
                double S = (i % 100) ;
                S /= 100;
                double V = (MathF.Floor((i / 100))) / 100;
                NewColor = ColorManager.HsvToRgb(new Vector3(H, (float)S, (float)V));
                RawPixels[9999-i] = NewColor;
            }
            NewTexture.SetData(RawPixels);
            return NewTexture;
        }
    }
}
