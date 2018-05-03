// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    internal sealed class ActiveAsyncPlan<TSource1> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, Func<TSource1, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;

            AddJoinObserver(observer1);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, Func<TSource1, TSource2, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, Func<TSource1, TSource2, TSource3, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, Func<TSource1, TSource2, TSource3, TSource4, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;
        private readonly AsyncJoinObserver<TSource10> _observer10;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, AsyncJoinObserver<TSource10> observer10, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;
            _observer10 = observer10;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
            AddJoinObserver(observer10);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0 && _observer10.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();
                var notification10 = _observer10.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;
        private readonly AsyncJoinObserver<TSource10> _observer10;
        private readonly AsyncJoinObserver<TSource11> _observer11;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, AsyncJoinObserver<TSource10> observer10, AsyncJoinObserver<TSource11> observer11, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;
            _observer10 = observer10;
            _observer11 = observer11;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
            AddJoinObserver(observer10);
            AddJoinObserver(observer11);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0 && _observer10.Queue.Count > 0 && _observer11.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();
                var notification10 = _observer10.Queue.Peek();
                var notification11 = _observer11.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted || notification11.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value, notification11.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;
        private readonly AsyncJoinObserver<TSource10> _observer10;
        private readonly AsyncJoinObserver<TSource11> _observer11;
        private readonly AsyncJoinObserver<TSource12> _observer12;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, AsyncJoinObserver<TSource10> observer10, AsyncJoinObserver<TSource11> observer11, AsyncJoinObserver<TSource12> observer12, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;
            _observer10 = observer10;
            _observer11 = observer11;
            _observer12 = observer12;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
            AddJoinObserver(observer10);
            AddJoinObserver(observer11);
            AddJoinObserver(observer12);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0 && _observer10.Queue.Count > 0 && _observer11.Queue.Count > 0 && _observer12.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();
                var notification10 = _observer10.Queue.Peek();
                var notification11 = _observer11.Queue.Peek();
                var notification12 = _observer12.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted || notification11.Kind == NotificationKind.OnCompleted || notification12.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value, notification11.Value, notification12.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;
        private readonly AsyncJoinObserver<TSource10> _observer10;
        private readonly AsyncJoinObserver<TSource11> _observer11;
        private readonly AsyncJoinObserver<TSource12> _observer12;
        private readonly AsyncJoinObserver<TSource13> _observer13;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, AsyncJoinObserver<TSource10> observer10, AsyncJoinObserver<TSource11> observer11, AsyncJoinObserver<TSource12> observer12, AsyncJoinObserver<TSource13> observer13, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;
            _observer10 = observer10;
            _observer11 = observer11;
            _observer12 = observer12;
            _observer13 = observer13;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
            AddJoinObserver(observer10);
            AddJoinObserver(observer11);
            AddJoinObserver(observer12);
            AddJoinObserver(observer13);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0 && _observer10.Queue.Count > 0 && _observer11.Queue.Count > 0 && _observer12.Queue.Count > 0 && _observer13.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();
                var notification10 = _observer10.Queue.Peek();
                var notification11 = _observer11.Queue.Peek();
                var notification12 = _observer12.Queue.Peek();
                var notification13 = _observer13.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted || notification11.Kind == NotificationKind.OnCompleted || notification12.Kind == NotificationKind.OnCompleted || notification13.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value, notification11.Value, notification12.Value, notification13.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;
        private readonly AsyncJoinObserver<TSource10> _observer10;
        private readonly AsyncJoinObserver<TSource11> _observer11;
        private readonly AsyncJoinObserver<TSource12> _observer12;
        private readonly AsyncJoinObserver<TSource13> _observer13;
        private readonly AsyncJoinObserver<TSource14> _observer14;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, AsyncJoinObserver<TSource10> observer10, AsyncJoinObserver<TSource11> observer11, AsyncJoinObserver<TSource12> observer12, AsyncJoinObserver<TSource13> observer13, AsyncJoinObserver<TSource14> observer14, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;
            _observer10 = observer10;
            _observer11 = observer11;
            _observer12 = observer12;
            _observer13 = observer13;
            _observer14 = observer14;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
            AddJoinObserver(observer10);
            AddJoinObserver(observer11);
            AddJoinObserver(observer12);
            AddJoinObserver(observer13);
            AddJoinObserver(observer14);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0 && _observer10.Queue.Count > 0 && _observer11.Queue.Count > 0 && _observer12.Queue.Count > 0 && _observer13.Queue.Count > 0 && _observer14.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();
                var notification10 = _observer10.Queue.Peek();
                var notification11 = _observer11.Queue.Peek();
                var notification12 = _observer12.Queue.Peek();
                var notification13 = _observer13.Queue.Peek();
                var notification14 = _observer14.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted || notification11.Kind == NotificationKind.OnCompleted || notification12.Kind == NotificationKind.OnCompleted || notification13.Kind == NotificationKind.OnCompleted || notification14.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value, notification11.Value, notification12.Value, notification13.Value, notification14.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;
        private readonly AsyncJoinObserver<TSource10> _observer10;
        private readonly AsyncJoinObserver<TSource11> _observer11;
        private readonly AsyncJoinObserver<TSource12> _observer12;
        private readonly AsyncJoinObserver<TSource13> _observer13;
        private readonly AsyncJoinObserver<TSource14> _observer14;
        private readonly AsyncJoinObserver<TSource15> _observer15;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, AsyncJoinObserver<TSource10> observer10, AsyncJoinObserver<TSource11> observer11, AsyncJoinObserver<TSource12> observer12, AsyncJoinObserver<TSource13> observer13, AsyncJoinObserver<TSource14> observer14, AsyncJoinObserver<TSource15> observer15, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;
            _observer10 = observer10;
            _observer11 = observer11;
            _observer12 = observer12;
            _observer13 = observer13;
            _observer14 = observer14;
            _observer15 = observer15;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
            AddJoinObserver(observer10);
            AddJoinObserver(observer11);
            AddJoinObserver(observer12);
            AddJoinObserver(observer13);
            AddJoinObserver(observer14);
            AddJoinObserver(observer15);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0 && _observer10.Queue.Count > 0 && _observer11.Queue.Count > 0 && _observer12.Queue.Count > 0 && _observer13.Queue.Count > 0 && _observer14.Queue.Count > 0 && _observer15.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();
                var notification10 = _observer10.Queue.Peek();
                var notification11 = _observer11.Queue.Peek();
                var notification12 = _observer12.Queue.Peek();
                var notification13 = _observer13.Queue.Peek();
                var notification14 = _observer14.Queue.Peek();
                var notification15 = _observer15.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted || notification11.Kind == NotificationKind.OnCompleted || notification12.Kind == NotificationKind.OnCompleted || notification13.Kind == NotificationKind.OnCompleted || notification14.Kind == NotificationKind.OnCompleted || notification15.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value, notification11.Value, notification12.Value, notification13.Value, notification14.Value, notification15.Value);
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ActiveAsyncPlan<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16> : ActiveAsyncPlan
    {
        private readonly Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, Task> _onNext;
        private readonly Func<Task> _onCompleted;

        private readonly AsyncJoinObserver<TSource1> _observer1;
        private readonly AsyncJoinObserver<TSource2> _observer2;
        private readonly AsyncJoinObserver<TSource3> _observer3;
        private readonly AsyncJoinObserver<TSource4> _observer4;
        private readonly AsyncJoinObserver<TSource5> _observer5;
        private readonly AsyncJoinObserver<TSource6> _observer6;
        private readonly AsyncJoinObserver<TSource7> _observer7;
        private readonly AsyncJoinObserver<TSource8> _observer8;
        private readonly AsyncJoinObserver<TSource9> _observer9;
        private readonly AsyncJoinObserver<TSource10> _observer10;
        private readonly AsyncJoinObserver<TSource11> _observer11;
        private readonly AsyncJoinObserver<TSource12> _observer12;
        private readonly AsyncJoinObserver<TSource13> _observer13;
        private readonly AsyncJoinObserver<TSource14> _observer14;
        private readonly AsyncJoinObserver<TSource15> _observer15;
        private readonly AsyncJoinObserver<TSource16> _observer16;

        internal ActiveAsyncPlan(AsyncJoinObserver<TSource1> observer1, AsyncJoinObserver<TSource2> observer2, AsyncJoinObserver<TSource3> observer3, AsyncJoinObserver<TSource4> observer4, AsyncJoinObserver<TSource5> observer5, AsyncJoinObserver<TSource6> observer6, AsyncJoinObserver<TSource7> observer7, AsyncJoinObserver<TSource8> observer8, AsyncJoinObserver<TSource9> observer9, AsyncJoinObserver<TSource10> observer10, AsyncJoinObserver<TSource11> observer11, AsyncJoinObserver<TSource12> observer12, AsyncJoinObserver<TSource13> observer13, AsyncJoinObserver<TSource14> observer14, AsyncJoinObserver<TSource15> observer15, AsyncJoinObserver<TSource16> observer16, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, Task> onNext, Func<Task> onCompleted)
        {
            _onNext = onNext;
            _onCompleted = onCompleted;

            _observer1 = observer1;
            _observer2 = observer2;
            _observer3 = observer3;
            _observer4 = observer4;
            _observer5 = observer5;
            _observer6 = observer6;
            _observer7 = observer7;
            _observer8 = observer8;
            _observer9 = observer9;
            _observer10 = observer10;
            _observer11 = observer11;
            _observer12 = observer12;
            _observer13 = observer13;
            _observer14 = observer14;
            _observer15 = observer15;
            _observer16 = observer16;

            AddJoinObserver(observer1);
            AddJoinObserver(observer2);
            AddJoinObserver(observer3);
            AddJoinObserver(observer4);
            AddJoinObserver(observer5);
            AddJoinObserver(observer6);
            AddJoinObserver(observer7);
            AddJoinObserver(observer8);
            AddJoinObserver(observer9);
            AddJoinObserver(observer10);
            AddJoinObserver(observer11);
            AddJoinObserver(observer12);
            AddJoinObserver(observer13);
            AddJoinObserver(observer14);
            AddJoinObserver(observer15);
            AddJoinObserver(observer16);
        }

        internal override Task Match()
        {
            if (_observer1.Queue.Count > 0 && _observer2.Queue.Count > 0 && _observer3.Queue.Count > 0 && _observer4.Queue.Count > 0 && _observer5.Queue.Count > 0 && _observer6.Queue.Count > 0 && _observer7.Queue.Count > 0 && _observer8.Queue.Count > 0 && _observer9.Queue.Count > 0 && _observer10.Queue.Count > 0 && _observer11.Queue.Count > 0 && _observer12.Queue.Count > 0 && _observer13.Queue.Count > 0 && _observer14.Queue.Count > 0 && _observer15.Queue.Count > 0 && _observer16.Queue.Count > 0)
            {
                var notification1 = _observer1.Queue.Peek();
                var notification2 = _observer2.Queue.Peek();
                var notification3 = _observer3.Queue.Peek();
                var notification4 = _observer4.Queue.Peek();
                var notification5 = _observer5.Queue.Peek();
                var notification6 = _observer6.Queue.Peek();
                var notification7 = _observer7.Queue.Peek();
                var notification8 = _observer8.Queue.Peek();
                var notification9 = _observer9.Queue.Peek();
                var notification10 = _observer10.Queue.Peek();
                var notification11 = _observer11.Queue.Peek();
                var notification12 = _observer12.Queue.Peek();
                var notification13 = _observer13.Queue.Peek();
                var notification14 = _observer14.Queue.Peek();
                var notification15 = _observer15.Queue.Peek();
                var notification16 = _observer16.Queue.Peek();

                if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted || notification11.Kind == NotificationKind.OnCompleted || notification12.Kind == NotificationKind.OnCompleted || notification13.Kind == NotificationKind.OnCompleted || notification14.Kind == NotificationKind.OnCompleted || notification15.Kind == NotificationKind.OnCompleted || notification16.Kind == NotificationKind.OnCompleted)
                {
                    return _onCompleted();
                }

                Dequeue();

                return _onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value, notification11.Value, notification12.Value, notification13.Value, notification14.Value, notification15.Value, notification16.Value);
            }

            return Task.CompletedTask;
        }
    }

}
