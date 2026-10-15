using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Slimey_Arcades
{
    public static class FontManager
    {
        private static ContentManager Content;
        public static Dictionary<string, SpriteFont> Fonts;
        public static void Initialize(ContentManager ContentManager) 
        {
            Content = ContentManager;
            Fonts = new();
            DirectoryInfo Directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Content/Fonts");
            if (Directory.Exists)
            {
                FileInfo[] FontFiles = Directory.GetFiles("*.xnb");
                foreach (FileInfo FontFile in FontFiles)
                {
                    string FontName = Path.GetFileNameWithoutExtension(FontFile.Name);
                    SpriteFont NewFont = Content.Load<SpriteFont>("Content/Fonts/" + FontName);
                    Fonts.Add(FontName, NewFont);
                }
            }
            else throw new Exception("This directory doesn't exist!!!");
        }
        public static string GetFontName(SpriteFont Font) 
        {
            string FontName = "";
            foreach(KeyValuePair<string, SpriteFont> Data in Fonts)
            {
                if (Data.Value == Font) { FontName = Data.Key; break; }
            }
            if (FontName == "") throw new Exception("This font doesn't exist!!!");
            return FontName;
        }
    }
}
