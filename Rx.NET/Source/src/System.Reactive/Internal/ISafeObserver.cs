// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    /// <summary>
    /// Base interface for observers that can dispose of a resource on a terminal notification
    /// or when disposed itself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface ISafeObserver<in T> : IObserver<T>, IDisposable
    {
        void SetResource(IDisposable resource);
    }
}
