using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Slimey_Arcades
{
    public abstract class Scene : IContainer, INotifier
    {
        public Notifier Notifier { get; init; } = new Notifier();
        public Container Container { get; init; } = new();
        public virtual Scene NextScene { get; set; } = null;
        private Vector3 ScreenTarget {  get; set; } = Vector3.Zero;
        private Matrix TransformMatrix { get; set; } = Matrix.Identity;
        private float WindowScale { get; set; } = 1;
        public Scene(Color BGColor)
        {
            Transform BGTransform = new Transform(0, 0, (int)SETTINGS.SCREENWIDTH * 2, (int)SETTINGS.SCREENHEIGHT * 2);
            Polygon BackGround = new Polygon(BGTransform, BGColor);
            BackGround.Sprite.LayerData.LayerIndex = 9;
            Container.ObjectsToLoad.Add(BackGround);
            Notifier.ProcessNotifications = ProcessNotifications;
        }
        public void Draw(SpriteBatch SpriteBatch, float NewWindowScale, Vector2 ScreenOffset)
        {
            SpriteBatch.Begin(sortMode: SpriteSortMode.BackToFront, transformMatrix: TransformMatrix);
            Container.Draw(SpriteBatch, NewWindowScale, ScreenOffset);
            SpriteBatch.End();
        }
        public void Update()
        {
            Notifier.Broadcast(this);
            Container.Update();
            Container.LoadObjects(this);
            Container.DestroyObjects();
            MoveScreen();
            //MouseControl.Reset();
        }
        public abstract void Load();
        public virtual void ProcessNotifications(Notification Notification)
        {
            switch (Notification.Type)
            {
                case NOTTYPES.CHANGELEVEL:
                    NextScene = (Scene)Notification.Data;
                    break;
                case NOTTYPES.SCREENCHANGE:
                    switch (Notification.Data)
                    {
                        case "Left":
                            ScreenTarget += new Vector3(-(int)SETTINGS.SCREENWIDTH * WindowScale, 0, 0);
                            break;
                        case "Right":
                            ScreenTarget += new Vector3((int)SETTINGS.SCREENWIDTH * WindowScale, 0, 0);
                            break;
                        case "Up":
                            ScreenTarget += new Vector3(0, (int)SETTINGS.SCREENHEIGHT * WindowScale, 0);
                            break;
                        case "Down":
                            ScreenTarget += new Vector3(0, -(int)SETTINGS.SCREENHEIGHT * WindowScale, 0);
                            break;
                    }
                    break;
            }
        }
        private void MoveScreen()
        {
            if (ScreenTarget != new Vector3(TransformMatrix.M41, TransformMatrix.M42, 0))
            {
                Matrix TargetMatrix = Matrix.CreateTranslation(ScreenTarget);
                float XDiff = Math.Abs(ScreenTarget.X - TransformMatrix.M41);
                float YDiff = Math.Abs(ScreenTarget.Y - TransformMatrix.M42);
                float Speed = 0.1f;
                if (XDiff < Speed * 3 && YDiff < Speed * 3)
                {
                    TransformMatrix = TargetMatrix;
                }
                else
                {
                    TransformMatrix = Matrix.Lerp(TransformMatrix, TargetMatrix, Speed);
                }
                MouseControl.MouseOffset = new Vector2(-TransformMatrix.M41, -TransformMatrix.M42);
            }
        }
    }
}
