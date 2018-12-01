using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class SharedState
    {
        private readonly object _locker = new object();
        private bool _inUse = false;

        public int ConcurrentAccessCount { get; private set; }

        public IDisposable Use()
        {
            lock (_locker)
            {
                if (_inUse)
                {
                    ConcurrentAccessCount++;
                }
                _inUse = true;
            }

            return new UnlockDisposable(this);
        }

        public async ValueTask GetTask()
        {
            using (Use())
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
            }
        }

        public async ValueTask<T> GetTask<T>(T value)
        {
            using (Use())
            {
                await Task.Yield();
                return value;
            }
        }

        private class UnlockDisposable : IDisposable
        {
            private SharedState _state;

            public UnlockDisposable(SharedState state)
            {
                _state = state;
            }
            
            public void Dispose()
            {
                lock (_state._locker)
                {
                    _state._inUse = false;
                }
            }
        }
    }
}
