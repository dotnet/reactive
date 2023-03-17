// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Attribute to decorate a task-like type to specify a compatible asynchronous method builder.
    /// </summary>
    internal sealed class AsyncMethodBuilderAttribute : Attribute
    {
        /// <summary>
        /// Creates a new instance of the attribute using the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type implementing the asynchronous method builder.</param>
#pragma warning disable IDE0060 // Remove unused parameter - this attribute is understood intrinsically by the compiler, so it doesn't need to use its argument.
        public AsyncMethodBuilderAttribute(Type type)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }
    }
}
