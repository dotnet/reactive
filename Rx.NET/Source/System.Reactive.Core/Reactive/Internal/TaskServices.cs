// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tcs"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool TrySetCanceled<T>(TaskCompletionSource<T> tcs, CancellationToken token);
    }
}
