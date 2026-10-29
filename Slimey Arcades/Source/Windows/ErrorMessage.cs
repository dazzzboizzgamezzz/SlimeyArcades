using Microsoft.Xna.Framework;
using System;

namespace Slimey_Arcades
{
    public class ErrorMessage: IDraw, IContainer, IUpdate, INotifier
    {
        public Transform Transform { get; set; }
        public Sprite Sprite {  get; set; }
        public Container Container { get; init; } = new();
        public Notifier Notifier { get; init; } = new();
        public ErrorMessage(object Parent)
        {
            Sprite = new Sprite(Shapes.Square, Color.DarkGray);
            Transform = new Transform((int)SETTINGS.SCREENWIDTH / 2 - 150, (int)SETTINGS.SCREENHEIGHT /2 - 100, 300, 150);
            
            Transform TextTransform = new Transform(Transform.X + 25, Transform.Y + 10, 0, 0);
            string ErrorMessage = "There was an error with the " + Environment.NewLine + 
                                    Parent.GetType().ToString() + Environment.NewLine + 
                                    ". Please return to the title screen.";
            Text ErrorText = new Text(TextTransform, ErrorMessage);
            Container.ObjectsToLoad.Add(ErrorText);

            Transform ButtonTransform = new Transform(Transform.X + Transform.Width / 2 - 50, Transform.Y + Transform.Height - 75, 100, 50);
            Button TitleScreenButton = new Button(ButtonTransform, Shapes.Square, Color.Gray, "Title Screen", true);
            TitleScreenButton.Function = () => { Notifier.SendNotification(new Notification(NOTTYPES.CHANGELEVEL, new TitleScene())); };
            Container.ObjectsToLoad.Add(TitleScreenButton);
        }
        public void Update()
        {

        }
    }
}
