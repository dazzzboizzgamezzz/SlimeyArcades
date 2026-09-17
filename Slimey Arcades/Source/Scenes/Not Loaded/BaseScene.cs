using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slimey_Arcades
{
    public abstract class Scene : IContainer, INotifier
    {
        public Notifier Notifier { get; init; } = new Notifier();
        public Container Container { get; init; } = new();
        public virtual Scene NextScene { get; set; } = null;
        public Scene(Color BGColor)
        {
            Transform BGTransform = new Transform(0, 0, (int)SETTINGS.SCREENWIDTH, (int)SETTINGS.SCREENHEIGHT);
            Polygon BackGround = new Polygon(BGTransform, BGColor);
            BackGround.Sprite.LayerData.LayerIndex = 9;
            Container.ObjectsToLoad.Add(BackGround);
            Notifier.ProcessNotifications = ProcessNotifications;
        }
        public void Draw(SpriteBatch SpriteBatch)
        {
            Container.Draw(SpriteBatch);
        }
        public void Update()
        {
            Notifier.Broadcast(this);
            Container.Update();
            Container.LoadObjects(this);
            Container.DestroyObjects();
        }
        public abstract void Load();
        public virtual void ProcessNotifications(Notification Notification)
        {
            switch (Notification.Type)
            {
                case NOTTYPES.CHANGELEVEL:
                    NextScene = (Scene)Notification.Data;
                    break;
            }
        }
    }
}
