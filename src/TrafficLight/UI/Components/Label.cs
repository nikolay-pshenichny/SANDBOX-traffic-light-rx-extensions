using System;

namespace TrafficLight.UI.Components
{
    public class Label<T> : IObserver<T>
    {
        private readonly int _left;
        private readonly int _top;
        private readonly string _prefix;
        private readonly object _uiLockObject;

        public Label(int left, int top, string prefix, object uiLockObject)
        {
            _left = left;
            _top = top;
            _prefix = prefix;
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

        public void OnNext(T notification)
        {
            this.Draw(notification);
        }

        public void Draw(T value)
        {
            lock(_uiLockObject)
            {
                Console.SetCursorPosition(_left, _top);
                Console.ResetColor();
                Console.Write(value == null ? _prefix : string.Format("{0}{1}", _prefix, value));
            }
        }
    }
}
