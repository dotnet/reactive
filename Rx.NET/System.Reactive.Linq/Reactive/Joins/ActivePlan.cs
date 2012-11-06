// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace System.Reactive.Joins
{
    internal abstract class ActivePlan
    {
        Dictionary<IJoinObserver, IJoinObserver> joinObservers = new Dictionary<IJoinObserver, IJoinObserver>();

        internal abstract void Match();

        protected void AddJoinObserver(IJoinObserver joinObserver)
        {
            joinObservers.Add(joinObserver, joinObserver);
        }

        protected void Dequeue()
        {
            foreach (var joinObserver in joinObservers.Values)
                joinObserver.Dequeue();
        }
    }

    internal class ActivePlan<T1> : ActivePlan
    {
        private readonly Action<T1> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;

        internal ActivePlan(JoinObserver<T1> first, Action<T1> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            AddJoinObserver(first);
        }

        internal override void Match()
        {
            if (first.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();

                if (n1.Kind == NotificationKind.OnCompleted)
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2> : ActivePlan
    {
        private readonly Action<T1, T2> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, Action<T1, T2> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            AddJoinObserver(first);
            AddJoinObserver(second);
        }

        internal override void Match()
        {
            if (first.Queue.Count > 0
             && second.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted)
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
                           n2.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3> : ActivePlan
    {
        private readonly Action<T1, T2, T3> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third, Action<T1, T2, T3> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
        }

        internal override void Match()
        {
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted)
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
                           n2.Value,
                           n3.Value
                           );
                }
            }
        }
    }

    internal class ActivePlan<T1, T2, T3, T4> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third, JoinObserver<T4> fourth, Action<T1, T2, T3, T4> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
        }

        internal override void Match()
        {
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted)
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
                           n2.Value,
                           n3.Value,
                           n4.Value
                           );
                }
            }
        }
    }
#if !NO_LARGEARITY

    internal class ActivePlan<T1, T2, T3, T4, T5> : ActivePlan
    {
        private readonly Action<T1, T2, T3, T4, T5> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, Action<T1, T2, T3, T4, T5> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
        }

        internal override void Match()
        {
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted)
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, 
            Action<T1, T2, T3, T4, T5, T6> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            AddJoinObserver(first);
            AddJoinObserver(second);
            AddJoinObserver(third);
            AddJoinObserver(fourth);
            AddJoinObserver(fifth);
            AddJoinObserver(sixth);
        }

        internal override void Match()
        {
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted)
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            Action<T1, T2, T3, T4, T5, T6, T7> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted)
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0)
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    )
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
                    || n2.Kind == NotificationKind.OnCompleted
                    || n3.Kind == NotificationKind.OnCompleted
                    || n4.Kind == NotificationKind.OnCompleted
                    || n5.Kind == NotificationKind.OnCompleted
                    || n6.Kind == NotificationKind.OnCompleted
                    || n7.Kind == NotificationKind.OnCompleted
                    || n8.Kind == NotificationKind.OnCompleted
                    || n9.Kind == NotificationKind.OnCompleted
                    )
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;
        private readonly JoinObserver<T10> tenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
             && tenth.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();
                var n10 = tenth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
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
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;
        private readonly JoinObserver<T10> tenth;
        private readonly JoinObserver<T11> eleventh;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
            this.eleventh = eleventh;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
             && tenth.Queue.Count > 0
             && eleventh.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();
                var n10 = tenth.Queue.Peek();
                var n11 = eleventh.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
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
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;
        private readonly JoinObserver<T10> tenth;
        private readonly JoinObserver<T11> eleventh;
        private readonly JoinObserver<T12> twelfth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
            this.eleventh = eleventh;
            this.twelfth = twelfth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
             && tenth.Queue.Count > 0
             && eleventh.Queue.Count > 0
             && twelfth.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();
                var n10 = tenth.Queue.Peek();
                var n11 = eleventh.Queue.Peek();
                var n12 = twelfth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
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
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;
        private readonly JoinObserver<T10> tenth;
        private readonly JoinObserver<T11> eleventh;
        private readonly JoinObserver<T12> twelfth;
        private readonly JoinObserver<T13> thirteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
            this.eleventh = eleventh;
            this.twelfth = twelfth;
            this.thirteenth = thirteenth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
             && tenth.Queue.Count > 0
             && eleventh.Queue.Count > 0
             && twelfth.Queue.Count > 0
             && thirteenth.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();
                var n10 = tenth.Queue.Peek();
                var n11 = eleventh.Queue.Peek();
                var n12 = twelfth.Queue.Peek();
                var n13 = thirteenth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
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
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;
        private readonly JoinObserver<T10> tenth;
        private readonly JoinObserver<T11> eleventh;
        private readonly JoinObserver<T12> twelfth;
        private readonly JoinObserver<T13> thirteenth;
        private readonly JoinObserver<T14> fourteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth, JoinObserver<T14> fourteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
            this.eleventh = eleventh;
            this.twelfth = twelfth;
            this.thirteenth = thirteenth;
            this.fourteenth = fourteenth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
             && tenth.Queue.Count > 0
             && eleventh.Queue.Count > 0
             && twelfth.Queue.Count > 0
             && thirteenth.Queue.Count > 0
             && fourteenth.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();
                var n10 = tenth.Queue.Peek();
                var n11 = eleventh.Queue.Peek();
                var n12 = twelfth.Queue.Peek();
                var n13 = thirteenth.Queue.Peek();
                var n14 = fourteenth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
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
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;
        private readonly JoinObserver<T10> tenth;
        private readonly JoinObserver<T11> eleventh;
        private readonly JoinObserver<T12> twelfth;
        private readonly JoinObserver<T13> thirteenth;
        private readonly JoinObserver<T14> fourteenth;
        private readonly JoinObserver<T15> fifteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth, JoinObserver<T14> fourteenth,
            JoinObserver<T15> fifteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
            this.eleventh = eleventh;
            this.twelfth = twelfth;
            this.thirteenth = thirteenth;
            this.fourteenth = fourteenth;
            this.fifteenth = fifteenth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
             && tenth.Queue.Count > 0
             && eleventh.Queue.Count > 0
             && twelfth.Queue.Count > 0
             && thirteenth.Queue.Count > 0
             && fourteenth.Queue.Count > 0
             && fifteenth.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();
                var n10 = tenth.Queue.Peek();
                var n11 = eleventh.Queue.Peek();
                var n12 = twelfth.Queue.Peek();
                var n13 = thirteenth.Queue.Peek();
                var n14 = fourteenth.Queue.Peek();
                var n15 = fifteenth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
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
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
        private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> onNext;
        private readonly Action onCompleted;
        private readonly JoinObserver<T1> first;
        private readonly JoinObserver<T2> second;
        private readonly JoinObserver<T3> third;
        private readonly JoinObserver<T4> fourth;
        private readonly JoinObserver<T5> fifth;
        private readonly JoinObserver<T6> sixth;
        private readonly JoinObserver<T7> seventh;
        private readonly JoinObserver<T8> eighth;
        private readonly JoinObserver<T9> ninth;
        private readonly JoinObserver<T10> tenth;
        private readonly JoinObserver<T11> eleventh;
        private readonly JoinObserver<T12> twelfth;
        private readonly JoinObserver<T13> thirteenth;
        private readonly JoinObserver<T14> fourteenth;
        private readonly JoinObserver<T15> fifteenth;
        private readonly JoinObserver<T16> sixteenth;

        internal ActivePlan(JoinObserver<T1> first, JoinObserver<T2> second, JoinObserver<T3> third,
            JoinObserver<T4> fourth, JoinObserver<T5> fifth, JoinObserver<T6> sixth, JoinObserver<T7> seventh,
            JoinObserver<T8> eighth, JoinObserver<T9> ninth, JoinObserver<T10> tenth, JoinObserver<T11> eleventh,
            JoinObserver<T12> twelfth, JoinObserver<T13> thirteenth, JoinObserver<T14> fourteenth,
            JoinObserver<T15> fifteenth, JoinObserver<T16> sixteenth,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.first = first;
            this.second = second;
            this.third = third;
            this.fourth = fourth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.seventh = seventh;
            this.eighth = eighth;
            this.ninth = ninth;
            this.tenth = tenth;
            this.eleventh = eleventh;
            this.twelfth = twelfth;
            this.thirteenth = thirteenth;
            this.fourteenth = fourteenth;
            this.fifteenth = fifteenth;
            this.sixteenth = sixteenth;
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
            if (first.Queue.Count > 0
             && second.Queue.Count > 0
             && third.Queue.Count > 0
             && fourth.Queue.Count > 0
             && fifth.Queue.Count > 0
             && sixth.Queue.Count > 0
             && seventh.Queue.Count > 0
             && eighth.Queue.Count > 0
             && ninth.Queue.Count > 0
             && tenth.Queue.Count > 0
             && eleventh.Queue.Count > 0
             && twelfth.Queue.Count > 0
             && thirteenth.Queue.Count > 0
             && fourteenth.Queue.Count > 0
             && fifteenth.Queue.Count > 0
             && sixteenth.Queue.Count > 0
                )
            {
                var n1 = first.Queue.Peek();
                var n2 = second.Queue.Peek();
                var n3 = third.Queue.Peek();
                var n4 = fourth.Queue.Peek();
                var n5 = fifth.Queue.Peek();
                var n6 = sixth.Queue.Peek();
                var n7 = seventh.Queue.Peek();
                var n8 = eighth.Queue.Peek();
                var n9 = ninth.Queue.Peek();
                var n10 = tenth.Queue.Peek();
                var n11 = eleventh.Queue.Peek();
                var n12 = twelfth.Queue.Peek();
                var n13 = thirteenth.Queue.Peek();
                var n14 = fourteenth.Queue.Peek();
                var n15 = fifteenth.Queue.Peek();
                var n16 = sixteenth.Queue.Peek();

                if (   n1.Kind == NotificationKind.OnCompleted
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
                    onCompleted();
                else
                {
                    Dequeue();
                    onNext(n1.Value,
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
#endif
}