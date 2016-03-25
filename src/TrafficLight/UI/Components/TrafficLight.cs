using System;
using TrafficLight.Entities;
using TrafficLight.Notifications;

namespace TrafficLight.UI.Components
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Block_Elements
    /// https://en.wikipedia.org/wiki/Box-drawing_character
    /// </summary>
    public class TrafficLight : IObserver<LightChangedNotification>
    {
        private const char MediumShade = '\u2592';
        private const char FullBlock = '\u2588';

        private readonly int _left;
        private readonly int _top;
        private readonly string _caption;
        private readonly object _uiLockObject;

        public TrafficLight(int left, int top, string caption, object uiLockObject)
        {
            _left = left;
            _top = top;
            _caption = caption;
            _uiLockObject = uiLockObject;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(LightChangedNotification notification)
        {
            this.Draw(notification.Value);
        }

        public void Draw()
        {
            this.Draw(null);
        }

        private void Draw(Light? light)
        {
            lock(_uiLockObject)
            {
                Console.SetCursorPosition(_left, _top);
                Console.ResetColor();
                Console.Write(_caption);


                Console.SetCursorPosition(_left, _top + 1);
                Console.ForegroundColor = light == Light.Red ? ConsoleColor.Red : ConsoleColor.DarkRed;
                Console.Write(new string(light == Light.Red ? FullBlock : MediumShade, 2));

                Console.SetCursorPosition(_left, _top + 2);
                Console.ForegroundColor = light == Light.Yellow ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;
                Console.Write(new string(light == Light.Yellow ? FullBlock : MediumShade, 2));

                Console.SetCursorPosition(_left, _top + 3);
                Console.ForegroundColor = light == Light.Green ? ConsoleColor.Green : ConsoleColor.DarkGreen;
                Console.Write(new string(light == Light.Green ? FullBlock : MediumShade, 2));
            }
        }

    }
}
