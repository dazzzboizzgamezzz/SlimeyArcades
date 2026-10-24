using Microsoft.Xna.Framework;
using Slimey_Arcades.Scenes;
using System.Collections.Generic;

namespace Slimey_Arcades
{
    public class TestScene :Scene
    {
        public TestScene() : base (Color.CornflowerBlue)
        {

        }
        public override void Load()
        {
            DebugSelect Test = new DebugSelect();
            Container.ObjectsToLoad.Add(Test);

            //NewDebugGrid Test2 = new NewDebugGrid();
            //Container.ObjectsToLoad.Add(Test2);

            NewDebugScene Test2 = new NewDebugScene();
            Container.ObjectsToLoad.Add(Test2);
        }
    }
}
