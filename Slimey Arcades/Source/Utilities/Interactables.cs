using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Slimey_Arcades
{
    public interface IUpdate
    {
        public void Update();
    }
    public class Clicker
    {
        private bool Clicked { get; set; } = false;
        private Transform ClickArea { get; init; }
        public Clicker(Transform NewArea)
        {
            ClickArea = NewArea;
        }
        public bool Click(string Button = "Left")
        {
            ButtonState CurrentPress = ButtonState.Released;
            Point MousePos = Mouse.GetState().Position;

            switch (Button)
            {
                case "Left": CurrentPress = Mouse.GetState().LeftButton; break;
                case "Right": CurrentPress = Mouse.GetState().RightButton; break;
            }

            if (ClickArea.Rect.Contains(MousePos) && CurrentPress == ButtonState.Pressed && !Clicked)
            {
                Clicked = true;
                return true;
            }
            if (CurrentPress == ButtonState.Released && Clicked)
            {
                Clicked = false;
            }
            return false;
        }
        public bool Hold()
        {
            Point MousePos = Mouse.GetState().Position;

            if (ClickArea.Rect.Contains(MousePos) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class Dragger
    {
        private Transform Transform { get; set; }
        public bool Dragging { get; set; } = false;
        private bool Click { get; set; } = false;
        Vector2 MouseOffset {  get; set; }
        public Dragger(Transform NewTransform)
        {
            Transform = NewTransform;
        }
        public void Drag(IDraw Object) 
        {
            Vector2 MPos = Mouse.GetState().Position.ToVector2();
            if (Transform.Rect.Contains(MPos)) Mouse.SetCursor(MouseCursor.Hand);
            else Mouse.SetCursor(MouseCursor.Arrow);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (Transform.Rect.Contains(MPos) && !Click)
                {
                    Dragging = true;
                    Click = true;
                    MouseOffset = MPos - Object.Pos;
                }
                if (Dragging)
                {
                    OldObjectPos = Object.Pos;
                    Object.Pos = MPos - MouseOffset;
                    MoveChildren(Object);
                }
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released) { Dragging = false; Click = false; }
        }
        private Vector2 OldObjectPos;
        private void MoveChildren(IDraw Object)
        {
            if (typeof(IContainer).IsAssignableFrom(Object.GetType()))
            {
                IContainer Container = (IContainer)Object;
                List<IContainer> ChildContainers = new();
                ChildContainers.Add(Container);
                Vector2 Offset = Object.Pos - OldObjectPos;
                for (int i = 0; i < ChildContainers.Count; i++)
                {
                    IContainer ChildContainer = ChildContainers[i];
                    foreach (IDraw SubObject in ChildContainer.Draws)
                    {
                        if (!typeof(Button).IsAssignableFrom(SubObject.GetType()))
                        {
                            SubObject.Pos = SubObject.Pos + Offset;
                        }
                    }
                    foreach (Text SubObject in ChildContainer.Texts)
                    {
                        SubObject.Pos = SubObject.Pos + (Object.Pos - OldObjectPos);
                    }
                    if (ChildContainer.Containers.Count > 0)
                    {
                        ChildContainers.AddRange(ChildContainer.Containers);
                    }
                }
            }
        }
    }
    //public class Keyer
    //{
    //    private List<Keys> PressedKeys { get; set; } = new();
    //    public Keyer()
    //    {

    //    }
    //}
}
