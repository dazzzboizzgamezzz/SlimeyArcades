using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Slimey_Arcades
{
    public interface IUpdate
    {
        public void Update();
    }
    public static class MouseControl
    {
        public static Vector2 MouseOffset { get; set; }
        public static Vector2 MousePosition { get => Mouse.GetState().Position.ToVector2() + MouseOffset; }
        private static bool LeftClicked { get; set; } = false;
        private static bool RightClicked { get; set; } = false;
        public static bool LeftClick(Transform ClickArea)
        {
            if (ClickArea.Rect.Contains(MousePosition) && Mouse.GetState().LeftButton == ButtonState.Pressed && !LeftClicked)
            {
                LeftClicked = true;
                return true;
            }
            return false;
        }
        public static bool LeftHold(Transform ClickArea)
        {
            Vector2 MousePos = Mouse.GetState().Position.ToVector2() + MouseOffset;
            return (ClickArea.Rect.Contains(MousePos) && Mouse.GetState().LeftButton == ButtonState.Pressed);
        }
        public static bool RightClick(Transform ClickArea)
        {
            if (ClickArea.Rect.Contains(MousePosition) && Mouse.GetState().RightButton == ButtonState.Pressed && !RightClicked)
            {
                RightClicked = true;
                return true;
            }
            return false;
        }
        public static bool RightHold(Transform ClickArea)
        {
            Vector2 MousePos = Mouse.GetState().Position.ToVector2() + MouseOffset;
            return (ClickArea.Rect.Contains(MousePos) && Mouse.GetState().RightButton == ButtonState.Pressed);
        }
        public static bool Hovering(Transform ActiveArea)
        {
            Vector2 MousePos = Mouse.GetState().Position.ToVector2() + MouseOffset;
            return ActiveArea.Rect.Contains(MousePos);
        }
        public static void Reset()
        {
            if (RightClicked && Mouse.GetState().RightButton == ButtonState.Released)
            {
                RightClicked = false;
            }

            if (LeftClicked && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                LeftClicked = false;
            }
        }
    }
}
