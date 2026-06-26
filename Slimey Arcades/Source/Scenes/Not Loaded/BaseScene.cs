using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Slimey_Arcades.Utilities;

namespace Slimey_Arcades
{
    public abstract class Scene : IDraw, IContainer
    {
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public Container Container { get; init; } = new();
        public virtual Scene NextScene { get; set; } = null;
        public Scene(Color BGColor)
        {
            Transform = new Transform(0, 0, (int)SETTINGS.SCREENWIDTH, (int)SETTINGS.SCREENHEIGHT);
            Sprite = new Sprite(Shapes.Square, BGColor);
            Sprite.LayerData.LayerIndex = 9;
        }
        public void LoadObjects(IContainer StartingContainer)
        {
            List<IContainer> AllContainers = new();
            AllContainers.Add(StartingContainer);
            int IncreaseLayer = 0;
            int CurrentDepth = StartingContainer.Container.ContainerDepth + 1;

            for (int i = 0; i < AllContainers.Count; i++)
            {
                IContainer NewContainer = AllContainers[i];
                if (NewContainer != this) Container.ObjectsToLoad.AddRange(NewContainer.Container.ObjectsToLoad);
                NewContainer.Container.LoadObjects();
                foreach (IDraw ObjDraw in NewContainer.Container.Draws)
                {
                    ObjDraw.LayerDepth += CurrentDepth;
                }
                foreach (Text ObjText in NewContainer.Container.Texts)
                {
                    ObjText.LayerData.LayerDepth += CurrentDepth;
                }
                foreach (IContainer SubContainer in NewContainer.Container.Containers)
                {
                    AllContainers.Add(SubContainer);
                    SubContainer.Container.ContainerDepth = CurrentDepth;
                }
                if (i == IncreaseLayer)
                {
                    IncreaseLayer += AllContainers.Count - 1;
                    CurrentDepth++;
                }
            }
            Container.LoadObjects();
        }
        public abstract void Load();
        public virtual void PostLoad() { }
    }
}
