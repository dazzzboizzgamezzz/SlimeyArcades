using Microsoft.Xna.Framework;
using Slimey_Arcades.Objects;
using Slimey_Arcades.Utilities;
using Slimey_Arcades.Windows;
using System.Runtime.CompilerServices;

namespace Slimey_Arcades.Scenes
{
    public class TitleScene :Scene
    {
        public TitleScene() : base (Color.CornflowerBlue)
        {

        }
        public override void Load()
        {
            //PauseWindow PauseWindow = new(new Vector2(100, 100), 0);
            //Container.ObjectsToLoad.Add(PauseWindow);
        }
    }
}
