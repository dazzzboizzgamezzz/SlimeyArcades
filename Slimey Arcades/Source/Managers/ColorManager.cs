using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Slimey_Arcades.Managers
{
    public static class ColorManager
    {
        public static Dictionary<string, Color> Colors = new()
        {
            {"GSlime", new Color(0, 200, 50) },
            {"RSlime", new Color(250, 50, 50) },
            {"BSlime", new Color(50, 115, 230) },
            {"PSlime", new Color(240, 80, 240) },
            {"YSlime", new Color(240, 240, 80) },
            {"Ice", new Color(50, 225, 225) },
            {"Barrel", new Color(225, 100, 50) },
            {"Pit", new Color(254, 254, 254) },
            {"Wall", Color.Black },
            {"None", new Color(0, 0, 0, 0) }
        };

        public static Color None = Colors["None"];
        public static Color InvertColor(Color OldColor)
        {
            Color NewColor = new Color(255 - OldColor.R, 255 - OldColor.G, 255 - OldColor.B);
            NewColor.A = OldColor.A;
            return NewColor;
        }

        public static Vector3 RGBToHSV(Color Color)
        {
            // R, G, B values are divided by 255
            // to change the range from 0..255 to 0..1
            // h, s, v = hue, saturation, value
            double R = Color.R / 255.0;
            double G = Color.G / 255.0;
            double B = Color.B / 255.0;
            double H = -1; 
            double S = -1;
            double V = 0;

            double CMax = Math.Max(R, Math.Max(G, B)); // maximum of r, g, b
            double CMin = Math.Min(R, Math.Min(G, B)); // minimum of r, g, b
            double Diff = CMax - CMin; // diff of cmax and cmin.

            // if cmax and cmax are equal then h = 0
            if (CMax == CMin)
                H = 0;

            // if cmax equal r then compute h
            else if (CMax == R)
                H = (60 * ((G - B) / Diff) + 360) % 360;

            // if cmax equal g then compute h
            else if (CMax == G)
                H = (60 * ((B - R) / Diff) + 120) % 360;

            // if cmax equal b then compute h
            else if (CMax == B)
                H = (60 * ((R - G) / Diff) + 240) % 360;

            // if cmax equal zero
            if (CMax == 0)
                S = 0;
            else
                S = (Diff / CMax);

            // compute v
            V = CMax;

            return new Vector3((float)H, (float)S, (float)V);
        }

        public static Color HsvToRgb(Vector3 HSVValue)
        {

            double H = Math.Abs((double)HSVValue.X % 360);
            double S = (double)HSVValue.Y;
            double V = (double)HSVValue.Z;
            double R = 0;
            double G = 0;
            double B = 0;
            if (V <= 0)
            { R = 0;  G = 0; B = 0; }
            else if (S <= 0)
            { R = V; G = V; B = V;}
            else
            {
                double Hf = H / 60.0;
                int i = (int)Math.Floor(Hf);
                double F = Hf - i;
                double Pv = V * (1 - S);
                double Qv = V * (1 - S * F);
                double Tv = V * (1 - S * (1 - F));
                switch (i)
                {
                    // Red is the dominant color
                    case 0:
                        R = V;
                        G = Tv;
                        B = Pv;
                        break;
                    // Green is the dominant color
                    case 1:
                        R = Qv;
                        G = V;
                        B = Pv;
                        break;
                    case 2:
                        R = Pv;
                        G = V;
                        B = Tv;
                        break;
                    // Blue is the dominant color
                    case 3:
                        R = Pv;
                        G = Qv;
                        B = V;
                        break;
                    case 4:
                        R = Tv;
                        G = Pv;
                        B = V;
                        break;
                    // Red is the dominant color
                    case 5:
                        R = V;
                        G = Pv;
                        B = Qv;
                        break;
                    // Just in case we overshoot on our math by a little, we put these here.
                    // Since its a switch it won't slow us down at all to put these here.
                    case 6:
                        R = V;
                        G = Tv;
                        B = Pv;
                        break;
                    case -1:
                        R = V;
                        G = Pv;
                        B = Qv;
                        break;
                }
            }

            R = R * 255;
            G = G * 255;
            B = B * 255;

            R = Math.Clamp(R, 0, 255);
            G = Math.Clamp(G, 0, 255);
            B = Math.Clamp(B, 0, 255);

            Color NewColor = new((int)R, (int)G, (int)B);
            return NewColor;
        }

        public static Color StringToColor(string Color)
        {
            Color NewColor = new Color(255, 255, 255, 255);
            int NewRed;
            int NewGre;
            int NewBlu;
            int NewAlp;
            if (!int.TryParse(Color.Split(" ")[0].Substring(3), out NewRed)) return NewColor;
            if (!int.TryParse(Color.Split(" ")[1].Substring(2), out NewGre)) return NewColor;
            if (!int.TryParse(Color.Split(" ")[2].Substring(2), out NewBlu)) return NewColor;
            if (!int.TryParse(Color.Split(" ")[3].Substring(2, Color.Split(" ")[3].Length - 3), out NewAlp)) return NewColor;
            NewColor = new Color(NewRed, NewGre, NewBlu, NewAlp);
            return NewColor;
        }

        public static Color GetWhiteOrBlack(Color BGColor)
        {
            Color NewColor = None;

            Vector3 BGHSV = RGBToHSV(BGColor);

            NewColor = BGHSV.Z >= 0.5 ? Color.Black : Color.White;

            return NewColor;
        }
    }
}
