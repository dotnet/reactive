// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    /// <summary>
    /// Marks the program elements that are experimental. This class cannot be inherited.
    /// </summary>
    [Experimental, AttributeUsage(AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    public sealed class ExperimentalAttribute : Attribute
    {
    }
}