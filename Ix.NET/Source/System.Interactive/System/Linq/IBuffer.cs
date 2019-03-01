// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// Represents a buffer exposing a shared view over an underlying enumerable sequence.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface IBuffer<out T> : IEnumerable<T>, IDisposable
    {
    }
}
