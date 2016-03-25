using System;
using TrafficLight.Configuration;
using TrafficLight.Notifications;
using TrafficLight.Entities;
using System.Collections.Generic;

namespace TrafficLight
{

    /// <summary>
    /// http://www.555-timer-circuits.com/traffic-lights-4-way.html
    /// </summary>
    public class TwoWayTrafficLightLogic : ITwoWayTrafficLightLogic
    {
        private readonly ITimingsConfiguration _timingsConfiguration;

        private readonly List<IObserver<LightChangedNotification>> _subscribers;

        private int _counter;

        private bool _initialized;

        public TwoWayTrafficLightLogic(ITimingsConfiguration timingsConfiguration)
        {
            _timingsConfiguration = timingsConfiguration;

            _counter = 0;

            _initialized = false;

            _subscribers = new List<IObserver<LightChangedNotification>>();
        }

        public Light NorthSouthLight
        {
            get
            {
                // Primary direction starts (turns green) from 0'th second
                int delta = 0;
                return DetermineLight(delta);
            }
        }
        public Light EastWestLight
        {
            get
            {
                // Secondary direction starts (turns green) when primary direction swithed Red and after a short delay
                int delta = _timingsConfiguration.YellowSeconds + _timingsConfiguration.GreenSeconds + _timingsConfiguration.DelayBetweenDirections;
                return DetermineLight(delta);
            }
        }

        public int Counter { get { return _counter; } }


        public void /*IObserver*/ OnCompleted()
        {
            throw new NotImplementedException();
        }
        public void /*IObserver*/ OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void /*IObserver*/ OnNext(TimerTickNotification _)
        {
            var oldNSLight = this.NorthSouthLight;
            var oldEWLight = this.EastWestLight;

            _counter++;

            if ((!_initialized) && (_counter == _timingsConfiguration.InitializationSeconds))
            {
                _counter = 0;
                _initialized = true;
            }

            if (_counter == SingleDirectionFullCycleTime)
            {
                _counter = 0;
            }

            var newNSLight = this.NorthSouthLight;
            var newEWLight = this.EastWestLight;

            if ((oldNSLight != newNSLight) || (!_initialized && _counter == 1)) { NotifySubscribers(Direction.NorthSouth, newNSLight); }
            if ((oldEWLight != newEWLight) || (!_initialized && _counter == 1)) { NotifySubscribers(Direction.EastWest, newEWLight); }
        }

        public IDisposable /*IObservable*/ Subscribe(IObserver<LightChangedNotification> observer)
        {
            _subscribers.Add(observer);
            //TODO: Implement Unsubscriber
            return null;
        }

        private int SingleDirectionFullCycleTime
        {
            get
            {
                // Red is active for (Yellow+Green) seconds, that is why we multiply by 2
                return ((_timingsConfiguration.YellowSeconds + _timingsConfiguration.GreenSeconds) * 2)
                    + _timingsConfiguration.DelayBetweenDirections * 2;
            }
        }

        private void NotifySubscribers(Direction direction, Light value)
        {
            var notification = new LightChangedNotification { Direction = direction, Value = value };
            foreach(var subscriber in _subscribers)
            {
                subscriber.OnNext(notification);
            }
        }

        private Light DetermineLight(int delta)
        {
            if (_initialized)
            {
                if ((_counter >= delta) && (_counter < (delta + _timingsConfiguration.GreenSeconds)))
                {
                    return Light.Green;
                }
                else if ((_counter >= (delta + _timingsConfiguration.GreenSeconds)) 
                    && (_counter < (delta + _timingsConfiguration.YellowSeconds + _timingsConfiguration.GreenSeconds)))
                {
                    return Light.Yellow;
                }
            }

            return Light.Red;
        }
    }

    public interface ITwoWayTrafficLightLogic : IObserver<TimerTickNotification>, IObservable<LightChangedNotification>
    {
        Light NorthSouthLight { get; }
        Light EastWestLight { get; }
        int Counter { get; }
    }
}
