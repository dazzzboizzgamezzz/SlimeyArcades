using Microsoft.Xna.Framework;
using System.Collections;
using Slimey_Arcades.Managers;

//namespace Slimey_Arcades.Windows
namespace Slimey_Arcades
{
    public class Window : IContainer, IDraw, IUpdate
    {
        public int LoadedCount { get; set; }
        public ArrayList SubObjects { get => Container.ObjectsToLoad; } 
        private ArrayList MinimizedObjects { get; set; } = new();
        public Sprite Sprite { get; set; }
        public Transform Transform { get; set; }
        public Container Container { get; init; } = new();
        public Header Header { get; set; }
        protected Color Color {  get; init; }
        public bool Destroy {  get; set; }
        public Window(Transform NewTransform, Color BGColor)
        {
            Transform = NewTransform;
            Sprite = new Sprite(Shapes.Square, BGColor);
            Color = BGColor;
        }
        public virtual void Update()
        {
            if (Header != null)
            {
                MinimizeWindow();
            }
            foreach (IUpdate Object in Container.Updates)
            {
                if (Header != null && Header.Closed == true) Destroy = true;
                else Object.Update();
            } 
        }
        private void MinimizeWindow()
        {
            if (Header.Minimized)
            {
                Sprite.Color = ColorManager.None;
                MinimizedObjects.AddRange(SubObjects);
                MinimizedObjects.Remove(Header);
                SubObjects.Clear();
                SubObjects.Add(Header);
            }
            else
            {
                Sprite.Color = Color;
                SubObjects.AddRange(MinimizedObjects);
                MinimizedObjects.Clear();
            }
        }
    }
}
