using System;
using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Linq;

namespace Slimey_Arcades
{
    public interface INotifier
    {
        public Notifier Notifier { get; init; }
        public List<Notification> SentNotifications { get => Notifier.NotificationsToBroadcast; }
        public Action<Notification> ProcessNotifications { get => Notifier.ProcessNotifications; }
    }
    public class Notifier
    {
        public INotifier RootNotifier { get; set; } = null;
        public List<Notification> NotificationsToBroadcast { get; set; } = new();
        public Action<Notification> ProcessNotifications {  get; set; }
        public Notifier(Action<Notification> NewProcess = null)
        {
            ProcessNotifications = NewProcess;
        }
        public void SendNotification(Notification Notification)
        {
            if (RootNotifier == null) NotificationsToBroadcast.Add(Notification);
            else RootNotifier.SentNotifications.Add(Notification);
            //RootNotifier.SentNotifications.Add(Notification);
        }
        public void Broadcast(IContainer BaseScene)
        {
            if (NotificationsToBroadcast.Count > 0)
            {
                foreach (Notification Notification in NotificationsToBroadcast) ProcessNotifications(Notification);
                List<IContainer> AllContainers = new();
                AllContainers.Add(BaseScene);
                for (int i = 0; i < AllContainers.Count; i++)
                {
                    IContainer Container = AllContainers[i];
                    AllContainers.AddRange(Container.Containers);
                    foreach (INotifier Notifier in Container.Notifiers)
                    {
                        foreach (Notification Notification in NotificationsToBroadcast) Notifier.ProcessNotifications(Notification);
                    }
                }
                NotificationsToBroadcast.Clear();
            }
        }
    }
    public class Notification
    {
        public NOTTYPES Type { get; init; }
        public object Data { get; init; }
        public Notification(NOTTYPES NewType = NOTTYPES.NONE, object NewData = null)
        {
            Type = NewType;
            Data = NewData;
        }
    }
    public enum NOTTYPES
    {
        NONE = -1,
        PAUSED = 0,
        CHANGELEVEL = 1,
        WIN = 2,
        LOSE = 3,
        SCREENCHANGE = 4,
    }
}
