namespace TrafficLight.Configuration
{
/*
  Stts: | INIT  | GREEN | YL | DLY |
  Time: 0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20
  N/S :   R R R R G G G G Y  Y DR DR  R  R  R  R  R  R  R  R
  E/W :   R R R R R R R R R  R  R  R  G  G  G  G  Y  Y DR DR


  Full Cycle Time (R + G + Y) = (GreenSec + YelloSec + DelaySec) * 2
*/    


    public class TimingsConfiguration : ITimingsConfiguration
    {
        public int InitializationSeconds { get { return 5; } }
        public int YellowSeconds { get { return 2; } }
        public int GreenSeconds { get { return 4; } }
        public int DelayBetweenDirections { get { return 2; } }
    }

    public interface ITimingsConfiguration
    {
        /// <summary>
        /// When power is turned on, both red lights will be active for this amount of time (seconds)
        /// </summary>
        int InitializationSeconds { get; }

        /// <summary>
        /// Yellow light will be active for this abount of time (seconds)
        /// </summary>
        int YellowSeconds { get; }

        /// <summary>
        /// Green light will be active for this abount of time (seconds)
        /// </summary>
        int GreenSeconds { get; }

        /// <summary>
        /// When one directions switches Red, another will stay Red for this amount of time (seconds) before switching to Green
        /// </summary>
        int DelayBetweenDirections { get; }
    }
}