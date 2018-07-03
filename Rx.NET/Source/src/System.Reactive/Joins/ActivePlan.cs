// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Joins
{
    internal abstract class ActivePlan
    {
        private readonly Dictionary<IJoinObserver, IJoinObserver> _joinObservers = new Dictionary<IJoinObserver, IJoinObserver>();

        protected readonly Action _onCompleted;

        internal abstract void Match();

        protected ActivePlan(Action onCompleted)
        {
            _onCompleted = onCompleted;
        }
        protected void AddJoinObserver(IJoinObserver joinObserver)
        {
            if (!_joinObservers.ContainsKey(joinObserver))
            {
                _joinObservers.Add(joinObserver, joinObserver);
            }
        }

        protected void Dequeue()
        {
            foreach (var joinObserver in _joinObservers.Values)
            {
                joinObserver.Dequeue();
            }
        }
    }

    internal class ActivePlan<T1> : ActivePlan
    {
        private readonly Action<T1> _onNext;
        private readonly JoinObserver<T1> _first;

        internal ActivePlan(JoinObserver<T1> first, Action<T1> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            AddJoinObserver(first);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted)
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value);
                }
            }
        }
    }

    internal class ActivePlan<T1, T2> : ActivePlan
    {
        private readonly Action<T1, T2> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, Action<T1, T2> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            AddJoinObserver(first);
            AddJoinObserver(second);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted)
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3> : ActivePlan
    {
        private readonly Action<T1, T2, T3> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third, Action<T1, T2, T3> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted)
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third, JoinObserver<T4> fourth, Action<T1, T2, T3, T4> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted)
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, Action<T1, T2, T3, T4, T5> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted)
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth,
            Action<T1, T2, T3, T4, T5, T6> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted)
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            Action<T1, T2, T3, T4, T5, T6, T7> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted)
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0)
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;
        private readonly JoinObserver<T10> _tenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            _tenth = tenth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
            AddJoinObserver(tenth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
             && _tenth.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();
                var n10 = _tenth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    || n10.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value,
                           n10.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;
        private readonly JoinObserver<T10> _tenth;
        private readonly JoinObserver<T11> _eleventh;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            _tenth = tenth;
            _eleventh = eleventh;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
            AddJoinObserver(tenth);
            AddJoinObserver(eleventh);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
             && _tenth.Queue.Count > 0
             && _eleventh.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();
                var n10 = _tenth.Queue.Peek();
                var n11 = _eleventh.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    || n10.Kind == NotificationKind.OnCompleted
                    || n11.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value,
                           n10.Value,
                           n11.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;
        private readonly JoinObserver<T10> _tenth;
        private readonly JoinObserver<T11> _eleventh;
        private readonly JoinObserver<T12> _twelfth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            _tenth = tenth;
            _eleventh = eleventh;
            _twelfth = twelfth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
            AddJoinObserver(tenth);
            AddJoinObserver(eleventh);
            AddJoinObserver(twelfth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
             && _tenth.Queue.Count > 0
             && _eleventh.Queue.Count > 0
             && _twelfth.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();
                var n10 = _tenth.Queue.Peek();
                var n11 = _eleventh.Queue.Peek();
                var n12 = _twelfth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    || n10.Kind == NotificationKind.OnCompleted
                    || n11.Kind == NotificationKind.OnCompleted
                    || n12.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value,
                           n10.Value,
                           n11.Value,
                           n12.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;
        private readonly JoinObserver<T10> _tenth;
        private readonly JoinObserver<T11> _eleventh;
        private readonly JoinObserver<T12> _twelfth;
        private readonly JoinObserver<T13> _thirteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            _tenth = tenth;
            _eleventh = eleventh;
            _twelfth = twelfth;
            _thirteenth = thirteenth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
            AddJoinObserver(tenth);
            AddJoinObserver(eleventh);
            AddJoinObserver(twelfth);
            AddJoinObserver(thirteenth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
             && _tenth.Queue.Count > 0
             && _eleventh.Queue.Count > 0
             && _twelfth.Queue.Count > 0
             && _thirteenth.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();
                var n10 = _tenth.Queue.Peek();
                var n11 = _eleventh.Queue.Peek();
                var n12 = _twelfth.Queue.Peek();
                var n13 = _thirteenth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    || n10.Kind == NotificationKind.OnCompleted
                    || n11.Kind == NotificationKind.OnCompleted
                    || n12.Kind == NotificationKind.OnCompleted
                    || n13.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value,
                           n10.Value,
                           n11.Value,
                           n12.Value,
                           n13.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;
        private readonly JoinObserver<T10> _tenth;
        private readonly JoinObserver<T11> _eleventh;
        private readonly JoinObserver<T12> _twelfth;
        private readonly JoinObserver<T13> _thirteenth;
        private readonly JoinObserver<T14> _fourteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth, JoinObserver<T14> fourteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            _tenth = tenth;
            _eleventh = eleventh;
            _twelfth = twelfth;
            _thirteenth = thirteenth;
            _fourteenth = fourteenth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
            AddJoinObserver(tenth);
            AddJoinObserver(eleventh);
            AddJoinObserver(twelfth);
            AddJoinObserver(thirteenth);
            AddJoinObserver(fourteenth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
             && _tenth.Queue.Count > 0
             && _eleventh.Queue.Count > 0
             && _twelfth.Queue.Count > 0
             && _thirteenth.Queue.Count > 0
             && _fourteenth.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();
                var n10 = _tenth.Queue.Peek();
                var n11 = _eleventh.Queue.Peek();
                var n12 = _twelfth.Queue.Peek();
                var n13 = _thirteenth.Queue.Peek();
                var n14 = _fourteenth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    || n10.Kind == NotificationKind.OnCompleted
                    || n11.Kind == NotificationKind.OnCompleted
                    || n12.Kind == NotificationKind.OnCompleted
                    || n13.Kind == NotificationKind.OnCompleted
                    || n14.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value,
                           n10.Value,
                           n11.Value,
                           n12.Value,
                           n13.Value,
                           n14.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;
        private readonly JoinObserver<T10> _tenth;
        private readonly JoinObserver<T11> _eleventh;
        private readonly JoinObserver<T12> _twelfth;
        private readonly JoinObserver<T13> _thirteenth;
        private readonly JoinObserver<T14> _fourteenth;
        private readonly JoinObserver<T15> _fifteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth, JoinObserver<T14> fourteenth,
            JoinObserver<T15> fifteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            _tenth = tenth;
            _eleventh = eleventh;
            _twelfth = twelfth;
            _thirteenth = thirteenth;
            _fourteenth = fourteenth;
            _fifteenth = fifteenth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
            AddJoinObserver(tenth);
            AddJoinObserver(eleventh);
            AddJoinObserver(twelfth);
            AddJoinObserver(thirteenth);
            AddJoinObserver(fourteenth);
            AddJoinObserver(fifteenth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
             && _tenth.Queue.Count > 0
             && _eleventh.Queue.Count > 0
             && _twelfth.Queue.Count > 0
             && _thirteenth.Queue.Count > 0
             && _fourteenth.Queue.Count > 0
             && _fifteenth.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();
                var n10 = _tenth.Queue.Peek();
                var n11 = _eleventh.Queue.Peek();
                var n12 = _twelfth.Queue.Peek();
                var n13 = _thirteenth.Queue.Peek();
                var n14 = _fourteenth.Queue.Peek();
                var n15 = _fifteenth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    || n10.Kind == NotificationKind.OnCompleted
                    || n11.Kind == NotificationKind.OnCompleted
                    || n12.Kind == NotificationKind.OnCompleted
                    || n13.Kind == NotificationKind.OnCompleted
                    || n14.Kind == NotificationKind.OnCompleted
                    || n15.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value,
                           n10.Value,
                           n11.Value,
                           n12.Value,
                           n13.Value,
                           n14.Value,
                           n15.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> _onNext;
        private readonly JoinObserver<T1> _first;
        private readonly JoinObserver<T2> _second;
        private readonly JoinObserver<T3> _third;
        private readonly JoinObserver<T4> _fourth;
        private readonly JoinObserver<T5> _fifth;
        private readonly JoinObserver<T6> _sixth;
        private readonly JoinObserver<T7> _seventh;
        private readonly JoinObserver<T8> _eighth;
        private readonly JoinObserver<T9> _ninth;
        private readonly JoinObserver<T10> _tenth;
        private readonly JoinObserver<T11> _eleventh;
        private readonly JoinObserver<T12> _twelfth;
        private readonly JoinObserver<T13> _thirteenth;
        private readonly JoinObserver<T14> _fourteenth;
        private readonly JoinObserver<T15> _fifteenth;
        private readonly JoinObserver<T16> _sixteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth, JoinObserver<T14> fourteenth,
            JoinObserver<T15> fifteenth, JoinObserver<T16> sixteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> onNext, Action onCompleted) : base(onCompleted)
        {
            _onNext = onNext;
            _first = first;
            _second = second;
            _third = third;
            _fourth = fourth;
            _fifth = fifth;
            _sixth = sixth;
            _seventh = seventh;
            _eighth = eighth;
            _ninth = ninth;
            _tenth = tenth;
            _eleventh = eleventh;
            _twelfth = twelfth;
            _thirteenth = thirteenth;
            _fourteenth = fourteenth;
            _fifteenth = fifteenth;
            _sixteenth = sixteenth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
            AddJoinObserver(seventh);
            AddJoinObserver(eighth);
            AddJoinObserver(ninth);
            AddJoinObserver(tenth);
            AddJoinObserver(eleventh);
            AddJoinObserver(twelfth);
            AddJoinObserver(thirteenth);
            AddJoinObserver(fourteenth);
            AddJoinObserver(fifteenth);
            AddJoinObserver(sixteenth);
        }

        internal override void Match()
        {
            if (_first.Queue.Count > 0
             && _second.Queue.Count > 0
             && _third.Queue.Count > 0
             && _fourth.Queue.Count > 0
             && _fifth.Queue.Count > 0
             && _sixth.Queue.Count > 0
             && _seventh.Queue.Count > 0
             && _eighth.Queue.Count > 0
             && _ninth.Queue.Count > 0
             && _tenth.Queue.Count > 0
             && _eleventh.Queue.Count > 0
             && _twelfth.Queue.Count > 0
             && _thirteenth.Queue.Count > 0
             && _fourteenth.Queue.Count > 0
             && _fifteenth.Queue.Count > 0
             && _sixteenth.Queue.Count > 0
                )
            {
                var n1 = _first.Queue.Peek();
                var n2 = _second.Queue.Peek();
                var n3 = _third.Queue.Peek();
                var n4 = _fourth.Queue.Peek();
                var n5 = _fifth.Queue.Peek();
                var n6 = _sixth.Queue.Peek();
                var n7 = _seventh.Queue.Peek();
                var n8 = _eighth.Queue.Peek();
                var n9 = _ninth.Queue.Peek();
                var n10 = _tenth.Queue.Peek();
                var n11 = _eleventh.Queue.Peek();
                var n12 = _twelfth.Queue.Peek();
                var n13 = _thirteenth.Queue.Peek();
                var n14 = _fourteenth.Queue.Peek();
                var n15 = _fifteenth.Queue.Peek();
                var n16 = _sixteenth.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    || n10.Kind == NotificationKind.OnCompleted
                    || n11.Kind == NotificationKind.OnCompleted
                    || n12.Kind == NotificationKind.OnCompleted
                    || n13.Kind == NotificationKind.OnCompleted
                    || n14.Kind == NotificationKind.OnCompleted
                    || n15.Kind == NotificationKind.OnCompleted
                    || n16.Kind == NotificationKind.OnCompleted
                    )
                {
                    _onCompleted();
                }
                else
                {
                    Dequeue();
                    _onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value,
                           n5.Value,
                           n6.Value,
                           n7.Value,
                           n8.Value,
                           n9.Value,
                           n10.Value,
                           n11.Value,
                           n12.Value,
                           n13.Value,
                           n14.Value,
                           n15.Value,
                           n16.Value
                           );
                }
            }
        }
    }
}
