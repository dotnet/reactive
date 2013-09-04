// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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