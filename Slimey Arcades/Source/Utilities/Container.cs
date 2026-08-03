using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Slimey_Arcades
{
    public interface IContainer
    {
        public Container Container { get; init; }
        public ArrayList ObjectsToLoad { get => Container.ObjectsToLoad; }
        public ArrayList LoadedObjects { get => Container.LoadedObjects; }
        public List<IUpdate> Updates { get => Container.Updates; }
        public List<IDraw> Draws { get => Container.Draws; }
        public List<IContainer> Containers { get => Container.Containers; }
        public List<Text> Texts { get => Container.Texts; }
        public List<INotifier> Notifiers {get => Container.Notifiers; }
        public bool Destroy { get => Container.Destroy; set => Container.Destroy = value; }
        public IContainer Parent { get => Container.Parent; set => Container.Parent = value; }
        public int ContainerDepth { get => Container.ContainerDepth; set => Container.ContainerDepth = value; }
        public Action<IContainer, int> LoadObjects { get => (IContainer ParentContainer, int LayerOffset) => Container.LoadObjects(ParentContainer, LayerOffset); }
        public Action Update { get => () => Container.Update(); }
        public Action<SpriteBatch> Draw { get => (SpriteBatch SpriteBatch) => Container.Draw(SpriteBatch); }
        public Action DestroyObjects { get => () => Container.DestroyObjects(); }
    }
    public class Container
    {
        public List<IUpdate> Updates { get; set; } = new();
        public List<IDraw> Draws { get; set; } = new();
        public List<IContainer> Containers { get; set; } = new();
        public List<Text> Texts { get; set; } = new();
        public List<INotifier> Notifiers { get; set; } = new();
        public ArrayList ObjectsToLoad { get; set; } = new();
        public ArrayList LoadedObjects { get; set; } = new();
        public IContainer Parent { get; set; } = null;
        public int ContainerDepth { get; set; } = 1;
        public bool Destroy { get; set; } = false;
        public Container() { }
        public void LoadObjects(IContainer ParentContainer, int LayerOffset = 0)
        {
            for (int i = 0; i < ObjectsToLoad.Count; i++) 
            {
                object Objects = ObjectsToLoad[i];
                if (typeof(IUpdate).IsAssignableFrom(Objects.GetType()))
                {
                    IUpdate Object = (IUpdate)Objects;
                    Updates.Add(Object);
                }
                if (typeof(IDraw).IsAssignableFrom(Objects.GetType()))
                {
                    IDraw Object = (IDraw)Objects;
                    Object.LayerDepth = Object.LayerDepth + ContainerDepth + LayerOffset;
                    Draws.Add(Object);
                }
                if (typeof(Text).IsAssignableFrom(Objects.GetType()))
                {
                    Text Object = (Text)Objects;
                    Object.LayerData.LayerDepth = Object.LayerData.LayerDepth + ContainerDepth + LayerOffset;
                    Texts.Add(Object);
                }
                if (typeof(INotifier).IsAssignableFrom(Objects.GetType()))
                {
                    INotifier Object = (INotifier)Objects;
                    IContainer TargetContainer = ParentContainer;
                    while (TargetContainer != null)
                    {
                        if (TargetContainer.Parent == null) break;
                        else TargetContainer = TargetContainer.Parent;
                    }
                    Object.Notifier.RootNotifier = (INotifier)TargetContainer;
                    Notifiers.Add(Object);
                }
                if (typeof(IContainer).IsAssignableFrom(Objects.GetType()))
                {
                    IContainer Object = (IContainer)Objects;
                    Object.ContainerDepth = ContainerDepth + 1 + LayerOffset;
                    Object.Parent = ParentContainer;
                    Containers.Add(Object);
                }
                LoadedObjects.Add(Objects);
                ObjectsToLoad.Remove(Objects);
                i--;
            }
            foreach (IContainer Container in Containers)
            {
                Container.LoadObjects(Container, 0);
            }
        }
        public void DestroyObjects()
        {
            for (int i = 0; i < Containers.Count; i++)
            {
                IContainer SubContainer = Containers[i];
                if (SubContainer.Destroy)
                {
                    LoadedObjects.Remove(SubContainer);
                    Containers.Remove(SubContainer);
                    if (typeof(IUpdate).IsAssignableFrom(SubContainer.GetType()))
                    {
                        Updates.Remove((IUpdate)SubContainer);
                    }
                    if (typeof(IDraw).IsAssignableFrom(SubContainer.GetType()))
                    {
                        Draws.Remove((IDraw)SubContainer);
                    }
                    i--;
                }
                SubContainer.DestroyObjects();
            }
        }
        public void Update()
        {
            foreach (IUpdate Update in Updates)
            {
                Update.Update();
                if (typeof(IContainer).IsAssignableFrom(Update.GetType()))
                {
                    IContainer SubContainer = (IContainer)Update;
                    SubContainer.Update();
                }
            }
        }
        public void Draw(SpriteBatch SpriteBatch)
        {
            foreach(Text Text in Texts)
            {
                if (Text.Txt != "")
                {
                    SpriteBatch.DrawString(Text.Font, Text.Txt, Text.Pos, Text.Color, Text.Rotation, Text.Origin, Text.Scale, Text.Effect, Text.Layer);
                }
            } 
            foreach (IDraw Draw in Draws)
            {
                SpriteBatch.Draw(Draw.Texture, Draw.Rect, null, Draw.Color, Draw.Rotation, Draw.Origin, Draw.Effect, Draw.Layer);
                if (typeof(IContainer).IsAssignableFrom(Draw.GetType()))
                {
                    IContainer SubContainer = (IContainer)Draw;
                    SubContainer.Draw(SpriteBatch);
                }
            }
        }
        public void MoveAllChildren(IDraw Parent, Vector2 NewPos)
        {
            Vector2 OldPos = Parent.Transform.Pos;
            Vector2 Offset = Vector2.Zero;
            Parent.Transform.Pos = NewPos;
            foreach (IDraw Draw in Draws)
            {
                Offset = Draw.Transform.Pos - OldPos;
                Draw.Transform.Pos = NewPos + Offset;
            }
            foreach (Text Text in Texts)
            {
                Offset = Text.Transform.Pos - OldPos;
                Text.Transform.Pos = NewPos + Offset;
            }
            List<IContainer> SubContainers = new();
            SubContainers.AddRange(Containers);
            for (int i = 0; i < SubContainers.Count; i++)
            {
                IContainer SubContainer = SubContainers[i];
                foreach (IDraw Draw in SubContainer.Draws)
                {
                    Offset = Draw.Transform.Pos - OldPos;
                    Draw.Transform.Pos = NewPos + Offset;
                }
                foreach (Text Text in SubContainer.Texts)
                {
                    Offset = Text.Transform.Pos - OldPos;
                    Text.Transform.Pos = NewPos + Offset;
                }
            }
        }
    }
}
