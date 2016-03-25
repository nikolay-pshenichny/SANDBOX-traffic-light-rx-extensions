using System;
using System.Linq;
using System.Reactive.Linq;
using TrafficLight.Entities;
using TrafficLight.Configuration;
using TrafficLight.Notifications;

namespace TrafficLight
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            object uiLockObject = new object();

            // Logic
            var twoWayTrafficLightLogic = new TwoWayTrafficLightLogic(new TimingsConfiguration());

            // North-South traffic light UI component
            var northSouthTrafficLight = new TrafficLight.UI.Components.TrafficLight(10, 3, "N/S", uiLockObject);
            northSouthTrafficLight.Draw();
            twoWayTrafficLightLogic.Where(x => x.Direction == Direction.NorthSouth).Subscribe(northSouthTrafficLight);

            // East-West traffic light UI component
            var eastWestTrafficLight = new TrafficLight.UI.Components.TrafficLight(20, 3, "E/W", uiLockObject);
            eastWestTrafficLight.Draw();
            twoWayTrafficLightLogic.Where(x => x.Direction == Direction.EastWest).Subscribe(eastWestTrafficLight);

            // Timer Label
            var timerLabel = new TrafficLight.UI.Components.Label<string>(0, 1, "Timer: ", uiLockObject);
            timerLabel.Draw(null);

            // Timer events, that are going to drive our logic
            var timer = Observable.Interval(TimeSpan.FromSeconds(1));

            // Attach logic to the timer
            timer.Select(_ => new TimerTickNotification()).Subscribe(twoWayTrafficLightLogic);
            timer.Select(t => t.ToString()).Subscribe(timerLabel);


            lock(uiLockObject)
            {
                Console.ResetColor();
                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Press any key to exit...");
            }

            Console.ReadKey();
        }
    }
}
