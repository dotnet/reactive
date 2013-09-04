// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive
{
    class BinaryObserver<TLeft, TRight> : IObserver<Either<Notification<TLeft>, Notification<TRight>>>
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

        public IObserver<TLeft> LeftObserver { get; private set; }
        public IObserver<TRight> RightObserver { get; private set; }

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
