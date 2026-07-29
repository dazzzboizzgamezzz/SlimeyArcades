using Microsoft.Xna.Framework;
using System.Collections;

//namespace Slimey_Arcades.Windows
namespace Slimey_Arcades
{
    public class Header : IContainer, IDraw, IUpdate
    {
        public Container Container { get; init; } = new();
        public int LoadedCount { get; set; }
        public ArrayList SubObjects { get => Container.ObjectsToLoad; }
        public Sprite Sprite { get; set; }
        public Transform Transform { get; set; }
        private Button MinButton { get; set; }
        private Button CloseButton { get; set; }
        public bool Closed { get; set; } = false;
        public bool Minimized { get; set; } = false;
        public bool Destroy {  get; set; } = false;
        public Header(Transform ParentTransform)
        {
            Transform = new Transform(ParentTransform.X, ParentTransform.Y, ParentTransform.Width, 25);
            Sprite = new Sprite(Shapes.Square, Color.Gray);
            MinButton = new Button(Transform.X + Transform.Width - 35, Transform.Y + 5, 15, 15, Shapes.Circle, Color.Yellow, "");
            MinButton.Function = () => { if (Minimized) Minimized = false; else Minimized = true; };
            CloseButton = new Button(Transform.X + Transform.Width - 15, Transform.Y + 5, 15, 15, Shapes.Circle, Color.Red, "");
            CloseButton.Function = () => { Closed = true; };
            SubObjects.Add(CloseButton);
        }
        public virtual void Update()
        {
            foreach (IUpdate Object in Container.Updates)
            {
                Object.Update();
            } 
        }
    }
}
