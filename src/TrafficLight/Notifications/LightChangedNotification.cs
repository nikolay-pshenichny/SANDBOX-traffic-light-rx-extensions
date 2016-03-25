using TrafficLight.Entities;

namespace TrafficLight.Notifications
{
    public class LightChangedNotification
    {
        public Direction Direction { get; set; }

        public Light Value { get; set; }
    }
}
