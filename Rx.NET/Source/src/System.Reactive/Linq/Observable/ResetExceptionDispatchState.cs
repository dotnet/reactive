// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq.ObservableImpl
{
    internal class ResetExceptionDispatchState<TSource> : Producer<TSource, ResetExceptionDispatchState<TSource>._>
    {
        private readonly IObservable<TSource> _source;

        public ResetExceptionDispatchState(IObservable<TSource> source)
        {
            _source = source;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new(observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            public _(IObserver<TSource> observer)
                : base(observer)
            {
            }

            public override void OnError(Exception error)
            {
                try
                {
                    // We use an ordinary throw and not the ExceptionDispatchInfo.Throw because the latter results in
                    // ever-growing stack trace messages. That is what caused this issue:
                    //  https://github.com/dotnet/reactive/issues/2187
                    // It is a basic assumption of ExceptionDispatchInfo.Throw that each call to Throw matches
                    // up with exactly one actual throw. Since Rx signals errors by passing Exceptions as arguments
                    // to OnError instead of throwing them, there typically isn't a throw. Since we do use
                    // ExceptionDispatchInfo.Throw in scenarios where we convert an OnError to the raising of an
                    // actual exception (e.g., when code uses await on an IObservable<T>), the use of a singleton
                    // exception object results in ever-growing StackTrace strings. The purpose of the
                    // ResetExceptionDispatchState is to execute the throw that ExceptionDispatchInfo.Throw
                    // expects to have happened.
                    throw error;
                }
                catch
                {
                }
                base.OnError(error);
            }
        }
    }
}
