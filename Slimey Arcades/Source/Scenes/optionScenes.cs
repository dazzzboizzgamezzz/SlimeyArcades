using Microsoft.Xna.Framework;
using Slimey_Arcades.Objects;
using Slimey_Arcades.Windows;
using Slimey_Arcades.Utilities;
using Slimey_Arcades.Managers;

namespace Slimey_Arcades.Scenes
{
    public class optionScences :Scene
    {
        public bool LoadLevel { get; init; }
        public optionScences() : base (Color.Blue)
        {

        }
        public override void Load()
        {
            Polygon test = new Polygon(new Transform(100,100,100,100), Color.Red);
            Container.ObjectsToLoad.Add(test);
        }
    }
}
