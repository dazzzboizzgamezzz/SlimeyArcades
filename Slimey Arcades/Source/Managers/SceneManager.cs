using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Slimey_Arcades.Scenes;

namespace Slimey_Arcades.Managers
{
    public static class SceneManager    
    {
        public static Dictionary<string, Scene> Scenes = new();
        public static void Initialize()
        {
            List<string> SceneFiles = Directory.GetFiles("C:\\Users\\nokin\\source\\repos\\Slimey Arcades\\Slimey Arcades\\Scenes").ToList();
            foreach (string SceneFile in SceneFiles)
            {
                string SceneName = Path.GetFileNameWithoutExtension(SceneFile);
                Type SceneType = typeof(Scene).Assembly.GetType("Slimey_Arcades.Scenes." + SceneName);
                if (SceneType != null)
                {
                    Scenes.Add(SceneName, (Scene)Activator.CreateInstance(SceneType));
                }
            }
        }
    }
}
