using Microsoft.Xna.Framework.Graphics;
using Slimey_Arcades.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Slimey_Arcades.Scenes
{
    public class SceneController
    {
        private SpriteBatch SpriteBatch {  get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        private Scene ActiveScene { get; set; }
        public SceneController(GraphicsDevice NewGraphicsDevice, Scene StartScene) 
        {  
            GraphicsDevice = NewGraphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            StartScene.Load();
            StartScene.LoadObjects();
            ActiveScene = StartScene;
        }
        public void Draw()
        {
            //back is 1, front is 0
            SpriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

            IDraw DrawScene = (IDraw)ActiveScene;
            SpriteBatch.Draw(DrawScene.Texture,
                             DrawScene.Rect,
                             null,
                             DrawScene.Color,
                             DrawScene.Rotation,
                             DrawScene.Origin,
                             DrawScene.Effect,
                             1); //Draw Scene

            RenderObjects();

            SpriteBatch.End();
        }
        public void Update()
        {
            if (ActiveScene.NextScene != null)
            {
                ActiveScene = ActiveScene.NextScene;
                ActiveScene.LoadObjects();
            }
            foreach (IUpdate Object in ActiveScene.Obj.Updates)
            {
                Object.Update();
                DestroyObjects(Object);
            }
            if (ActiveScene.LoadedCount != ActiveScene.Obj.GetAllChildren().Count)
            {
                ActiveScene.LoadObjects();
            }
        }
        private void DestroyObjects(IUpdate Object)
        {
            if (Object.Destroy == true)
            {
                ActiveScene.SubObjects.Remove(Object);
                if (typeof(IContainer).IsAssignableFrom(Object.GetType()))
                {
                    IContainer Container = (IContainer)Object;
                    foreach (object ToDestroy in Container.Obj.GetAllChildren())
                    {
                        for (int i = 0; i < ActiveScene.SubObjects.Count; i++)
                        {
                            object SubObject = ActiveScene.SubObjects[i];
                            if (ToDestroy == SubObject)
                            {
                                ActiveScene.SubObjects.Remove(SubObject);
                                break;
                            }
                        }
                    }
                }
            }
        }
        private void RenderObjects()
        {
            foreach (IDraw Object in ActiveScene.Obj.Draws)
            {
                SpriteBatch.Draw(Object.Texture,
                                 Object.Rect,
                                 null,
                                 Object.Color,
                                 Object.Rotation,
                                 Object.Origin,
                                 Object.Effect,
                                 Object.Layer); //Draw Objects
            }
            foreach (Text Object in ActiveScene.Obj.Texts) 
            {
                SpriteBatch.DrawString(Object.Font,
                                       Object.Txt,
                                       Object.Pos,
                                       Object.Color,
                                       Object.Rotation,
                                       Object.Origin,
                                       Object.Scale,
                                       Object.Effect,
                                       Object.Layer); //Draw Text
            }
        }
        //private void LoadNewObjects()
        //{
        //    foreach (IContainer Object in ActiveScene.Obj.Containers)
        //    {
        //        if (Object.SubObjects.Count != Object.LoadedCount)
        //        {
        //            //LoadObjects(Object, DrawObject.Layer);
        //            foreach (object SubObject in Object.SubObjects)
        //            {
        //                if (!ActiveScene.SubObjects.Contains(SubObject))
        //                {
        //                    ActiveScene.SubObjects.Add(SubObject);
        //                    //if (typeof(IContainer).IsAssignableFrom(SubObject.GetType()))
        //                    //{
        //                    //    IContainer SubContainer = (IContainer)SubObject;
        //                    //    //SubContainer.LoadedCount = 0;
        //                    //    ActiveScene.SubObjects.AddRange(SubContainer.Obj.GetAllChildren());
        //                    //}
        //                }
        //                //if (typeof(IDraw).IsAssignableFrom(Object.GetType()))
        //                //{
        //                //    IDraw DrawObject = (IDraw)Object;
        //                //    IDraw DrawSubObject = (IDraw)SubObject;
        //                //    if (DrawSubObject.Layer == 0)
        //                //    {
        //                //        float ParentLayer = DrawObject.Layer;
        //                //        int ChildCount = Object.SubObjects.Count;
        //                //        int LayerIndex = ChildCount.ToString().Length;
        //                //        while (ParentLayer < 1)
        //                //        {
        //                //            ParentLayer *= 10;
        //                //            LayerIndex++;
        //                //        }
        //                //        float ChildLayer = (float)Math.Pow(10, -LayerIndex) * (ChildCount * (float)Math.Pow(10, -ChildCount));
        //                //    }
        //                //}
        //            }
        //            ActiveScene.LoadObjects();
        //        }
        //    }
        //}
        //private void LoadObjects(IContainer ContainerToLoad, float StartingLayer)
        //{
        //    List<IContainer> AllContainers = new();
        //    AllContainers.Add(ContainerToLoad);
        //    bool SceneFlag = typeof(Scene).IsAssignableFrom(ContainerToLoad.GetType());
        //    if (typeof(IDraw).IsAssignableFrom(ContainerToLoad.GetType()))
        //    {
        //        IDraw DrawContainer = (IDraw)ContainerToLoad;
        //        DrawContainer.Layer = StartingLayer;
        //    } 
        //    //float LayerDepth = (float)Math.Pow(10, -ContainerToLoad.SubObjects.Count.ToString().Length);
        //    //float CurrentLayer = LayerDepth;
        //    float CurrentLayer = StartingLayer;
        //    float LayerDepth = ContainerToLoad.SubObjects.Count.ToString().Length;
        //    while (CurrentLayer < 1)
        //    {
        //        CurrentLayer *= 10;
        //        LayerDepth++;
        //    }
        //    LayerDepth = (float)Math.Pow(10, -LayerDepth);
        //    CurrentLayer = LayerDepth;
        //    int IncreaseLayer = 0;
        //    int NextLayerObjects = 0;

        //    for (int i = 0; i < AllContainers.Count; i++)
        //    {
        //        IContainer Container = AllContainers[i];
        //        Container.LoadedCount = Container.SubObjects.Count;
        //        foreach (IDraw ObjDraw in Container.Obj.Draws)
        //        {
        //            //ObjDraw.Layer = ObjDraw.Layer == 0 ? CurrentLayer : (float)Math.Pow(ObjDraw.Layer, -1) * LayerDepth;
        //            ObjDraw.Layer = CurrentLayer;
        //            CurrentLayer += LayerDepth;
        //        }
        //        foreach (Text ObjText in Container.Obj.Texts)
        //        {
        //            //ObjText.Layer = ObjText.Layer == 0 ? CurrentLayer : (float)Math.Pow(ObjText.Layer, -1) * LayerDepth;
        //            ObjText.Layer = CurrentLayer;
        //            CurrentLayer += LayerDepth;
        //        }
        //        foreach (IContainer SubContainer in Container.Obj.Containers)
        //        {
        //            AllContainers.Add(SubContainer);
        //            SubContainer.LoadedCount = SubContainer.SubObjects.Count;
        //            if (SceneFlag) ContainerToLoad.SubObjects.AddRange(SubContainer.SubObjects);
        //            NextLayerObjects += SubContainer.SubObjects.Count;
        //        }
        //        if (i == IncreaseLayer)
        //        {
        //            LayerDepth *= (float)Math.Pow(10, -NextLayerObjects.ToString().Length);
        //            CurrentLayer = LayerDepth;
        //            NextLayerObjects = 0;
        //            IncreaseLayer += AllContainers.Count - 1;
        //        }
        //    }
        //}
    }
}
