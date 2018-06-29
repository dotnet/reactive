// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Never<TResult> : IObservable<TResult>
    {
        /// <summary>
        /// The only instance for a TResult type: this source
        /// is completely stateless and has a constant behavior.
        /// </summary>
        internal static readonly IObservable<TResult> Default = new Never<TResult>();

        /// <summary>
        /// No need for instantiating this more than once per TResult.
        /// </summary>
        private Never()
        {

        }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return Disposable.Empty;
        }
    }
}
