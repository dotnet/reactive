// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_TPL

using System.ComponentModel;
using System.Reactive.PlatformServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal static class TaskHelpers
    {
        private static Lazy<ITaskServices> s_services = new Lazy<ITaskServices>(Initialize);

        private static ITaskServices Initialize()
        {
            return PlatformEnlightenmentProvider.Current.GetService<ITaskServices>() ?? new DefaultTaskServices();
        }

        public static bool TrySetCanceled<T>(this TaskCompletionSource<T> tcs, CancellationToken token)
        {
            return s_services.Value.TrySetCanceled(tcs, token);
        }
    }
}

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Services to leverage evolving TPL Task APIs.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ITaskServices
    {
        /// <summary>
        /// Attempts to transition the underlying Task{T} into the Canceled state.
        /// </summary>
        /// <typeparam name="T">Type of the result of the underlying task.</typeparam>
        /// <param name="tcs">Task completion source whose underlying task to transition into the Canceled state.</param>
        /// <param name="token">Cancellation token that triggered the cancellation.</param>
        /// <returns>True if the operation was successful; false if the operation was unsuccessful or the object has already been disposed.</returns>
        bool TrySetCanceled<T>(TaskCompletionSource<T> tcs, CancellationToken token);
    }
}

#endif