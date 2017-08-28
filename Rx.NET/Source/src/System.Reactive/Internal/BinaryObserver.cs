// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    internal sealed class BinaryObserver<TLeft, TRight> : IObserver<Either<Notification<TLeft>, Notification<TRight>>>
    {
        public BinaryObserver(IObserver<TLeft> leftObserver, IObserver<TRight> rightObserver)
        {
            LeftObserver = leftObserver;
            RightObserver = rightObserver;
        }

        public BinaryObserver(Action<Notification<TLeft>> left, Action<Notification<TRight>> right)
            : this(left.ToObserver(), right.ToObserver())
        {
        }

        public IObserver<TLeft> LeftObserver { get; }
        public IObserver<TRight> RightObserver { get; }

        void IObserver<Either<Notification<TLeft>, Notification<TRight>>>.OnNext(Either<Notification<TLeft>, Notification<TRight>> value)
        {
            value.Switch(left => left.Accept(LeftObserver), right => right.Accept(RightObserver));
        }

        void IObserver<Either<Notification<TLeft>, Notification<TRight>>>.OnError(Exception exception)
        {
        }

        void IObserver<Either<Notification<TLeft>, Notification<TRight>>>.OnCompleted()
        {
        }
    }
}
