// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.PlatformServices;

namespace System.Reactive
{
    internal static class ExceptionHelpers
    {
        private static readonly Lazy<IExceptionServices> Services = new(Initialize);

        [DoesNotReturn]
        public static void Throw(this Exception exception) => Services.Value.Rethrow(exception);

        private static IExceptionServices Initialize()
        {
            return PlatformEnlightenmentProvider.Current.GetService<IExceptionServices>() ?? new DefaultExceptionServices();
        }
    }
}

namespace System.Reactive.PlatformServices
{
    /// <summary>
    /// (Infrastructure) Services to rethrow exceptions.
    /// </summary>
    /// <remarks>
    /// This type is used by the Rx infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IExceptionServices
    {
        /// <summary>
        /// Rethrows the specified exception.
        /// </summary>
        /// <param name="exception">Exception to rethrow.</param>
        [DoesNotReturn]
        void Rethrow(Exception exception);
    }
}
