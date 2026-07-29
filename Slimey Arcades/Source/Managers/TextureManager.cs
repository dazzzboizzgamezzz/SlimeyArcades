using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
//using Slimey_Arcades.Objects;

namespace Slimey_Arcades.Managers
{
    public static class TextureManager
    {
        private static ContentManager Content;
        public static Dictionary<string, Texture2D> Textures;
        public static void Initialize(ContentManager ContentManager)
        {
            Content = ContentManager;
            Textures = new();
            DirectoryInfo Directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory.ToString() + "Content/Textures");
            if (Directory.Exists)
            {
                FileInfo[] TextureFiles;
                TextureFiles = Directory.GetFiles("*.xnb");
                foreach (FileInfo TextureFile in TextureFiles)
                {
                    string TextureName = Path.GetFileNameWithoutExtension(TextureFile.Name);
                    Texture2D NewTexture = Content.Load<Texture2D>("Content/Fonts/" + TextureName);
                    Textures.Add(TextureName, NewTexture);
                }
            }
            Textures.Add("Square", Shapes.Square);
            Textures.Add("Circle", Shapes.Circle);
            Textures.Add("Triangle", Shapes.Triangle);
        }
        public static string GetTextureName(Texture2D Texture)
        {
            string TextureName = "";
            foreach (KeyValuePair<string, Texture2D> Data in Textures)
            {
                if (Data.Value == Texture) { TextureName = Data.Key; break; }
            }
            if (TextureName == "") throw new Exception("This Texture doesn't exist!!!");
            return TextureName;
        }
    }
}
