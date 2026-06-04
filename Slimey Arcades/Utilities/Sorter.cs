using System;
using System.Collections;
using System.Collections.Generic;

namespace Slimey_Arcades.Utilities
{
    public class Sorter
    {
        public List<IUpdate> Updates { get; set; } = new();
        public List<IDraw> Draws { get; set; } = new();
        public List<IContainer> Containers { get; set; } = new();
        public List<Text> Texts { get; set; } = new();
        private ArrayList Objects { get; set; } = new();
        public Sorter(ArrayList AllObjects) 
        {
            Objects = AllObjects;
            foreach (object Objects in AllObjects)
            {
                if (typeof(IUpdate).IsAssignableFrom(Objects.GetType()))
                {
                    IUpdate Object = (IUpdate)Objects;
                    Updates.Add(Object);
                }
                if (typeof(IDraw).IsAssignableFrom(Objects.GetType()))
                {
                    IDraw Object = (IDraw)Objects;
                    Draws.Add(Object);
                }
                if (typeof(IContainer).IsAssignableFrom(Objects.GetType()))
                {
                    IContainer Object = (IContainer)Objects;
                    Containers.Add(Object);
                }
                if (typeof(Text).IsAssignableFrom(Objects.GetType()))
                {
                    Text Object = (Text)Objects;
                    Texts.Add(Object);
                }
            }
        }
        public ArrayList GetAllChildren()
        {
            ArrayList Children = new(Objects);
            List<IContainer> ChildContainers = new(Containers);
            for (int i = 0; i < ChildContainers.Count; i++)
            {
                IContainer Container = ChildContainers[i];
                Children.AddRange(Container.SubObjects);
                if (Container.Obj.Containers.Count > 0) ChildContainers.AddRange(Container.Obj.Containers);
            }
            return Children;
        }
        public void SetAllChildren(IContainer Container, Action<object> Function, string Type = null)
        {
            List<IContainer> ChildContainers = new(); 
            ChildContainers.Add(Container);
            ChildContainers.AddRange(Containers);
            for (int i = 0; i < ChildContainers.Count; i++)
            {
                IContainer ChildContainer = ChildContainers[i];
                if (Type != null)
                {
                    if (Type.Contains("Draw"))
                    {
                        foreach (IDraw Object in ChildContainer.Obj.Draws) Function(Object);
                    }
                    if (Type.Contains("Text"))
                    {
                        foreach (Text Object in ChildContainer.Obj.Draws) Function(Object);
                    }
                    if (Type.Contains("Update"))
                    {
                        foreach (IUpdate Object in ChildContainer.Obj.Draws) Function(Object);
                    }
                }
                else
                {
                    foreach (object Object in ChildContainer.SubObjects) Function(Object);
                }
                if (Container.Obj.Containers.Count > 0) ChildContainers.AddRange(Container.Obj.Containers);
            }
        }
    }
}
