using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Slimey_Arcades.Utilities;
using Slimey_Arcades.Objects;

namespace Slimey_Arcades.Scenes
{
    public abstract class Scene : IDraw, IContainer
    {
        public int LoadedCount { get; set; }
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public ArrayList SubObjects { get; set; } = new();
        public Sorter Obj { get => new Sorter(SubObjects); }
        public Scene NextScene { get; set; } = null;
        //private float CurrentLayer { get; set; } = 0f;
        public Scene(Color BGColor)
        {
            Transform = new Transform(0, 0, (int)SETTINGS.SCREENWIDTH, (int)SETTINGS.SCREENHEIGHT);
            Sprite = new Sprite(Shapes.Square, BGColor);
            Sprite.LayerData.LayerIndex = 9;
        }
        public void LoadObjects()
        {
            List<IContainer> AllContainers = new();
            AllContainers.Add(this);
            //float LayerDepth = (float)Math.Pow(10, -SubObjects.Count.ToString().Length);
            int LayerDepth = Sprite.LayerData.LayerDepth;
            //float CurrentLayer = LayerDepth;
            int IncreaseLayer = 0;
            //int NextLayerObjects = 0;

            for (int i = 0; i < AllContainers.Count; i++)
            {
                IContainer Container = AllContainers[i];
                foreach (IDraw ObjDraw in Container.Obj.Draws)
                {
                    ObjDraw.LayerData.LayerDepth = LayerDepth;
                    //ObjDraw.Layer = ObjDraw.Layer == 0 ? CurrentLayer : (float)Math.Pow(ObjDraw.Layer, -1) * LayerDepth;
                    //CurrentLayer += LayerDepth;
                }
                foreach (Text ObjText in Container.Obj.Texts)
                {
                    ObjText.LayerData.LayerDepth = LayerDepth;
                    //ObjText.Layer = ObjText.Layer == 0 ? CurrentLayer : (float)Math.Pow(ObjText.Layer, -1) * LayerDepth;
                    //CurrentLayer += LayerDepth;
                }
                foreach (IContainer SubContainer in Container.Obj.Containers)
                {
                    AllContainers.Add(SubContainer);
                    SubContainer.LoadedCount = SubContainer.SubObjects.Count;
                    SubObjects.AddRange(SubContainer.SubObjects);
                    //NextLayerObjects += SubContainer.SubObjects.Count;
                }
                if (i == IncreaseLayer)
                {
                    //LayerDepth *= (float)Math.Pow(10, -NextLayerObjects.ToString().Length);
                    //CurrentLayer = LayerDepth;
                    //NextLayerObjects = 0;
                    LayerDepth++;
                    IncreaseLayer += AllContainers.Count - 1;
                }
            }
            LoadedCount = Obj.GetAllChildren().Count;
        }
        public void OldLoadObjects()
        {
            List<IContainer> AllContainers = new();
            AllContainers.Add(this);
            float LayerDepth = (float)Math.Pow(10, -SubObjects.Count.ToString().Length);
            float CurrentLayer = LayerDepth;
            int IncreaseLayer = 0;
            int NextLayerObjects = 0;

            for (int i = 0; i < AllContainers.Count; i++)
            {
                IContainer Container = AllContainers[i];
                foreach (IDraw ObjDraw in Container.Obj.Draws)
                {
                    //ObjDraw.Layer = ObjDraw.Layer == 0 ? CurrentLayer : (float)Math.Pow(ObjDraw.Layer, -1) * LayerDepth;
                    CurrentLayer += LayerDepth;
                }
                foreach (Text ObjText in Container.Obj.Texts)
                {
                    //ObjText.Layer = ObjText.Layer == 0 ? CurrentLayer : (float)Math.Pow(ObjText.Layer, -1) * LayerDepth;
                    CurrentLayer += LayerDepth;
                }
                foreach (IContainer SubContainer in Container.Obj.Containers)
                {
                    AllContainers.Add(SubContainer);
                    SubContainer.LoadedCount = SubContainer.SubObjects.Count;
                    SubObjects.AddRange(SubContainer.SubObjects);
                    NextLayerObjects += SubContainer.SubObjects.Count;
                }
                if (i == IncreaseLayer)
                {
                    LayerDepth *= (float)Math.Pow(10, -NextLayerObjects.ToString().Length);
                    CurrentLayer = LayerDepth;
                    NextLayerObjects = 0;
                    IncreaseLayer += AllContainers.Count - 1;
                }
            }
        }
        public abstract void Load();
    }
}
