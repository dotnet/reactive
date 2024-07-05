// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;

namespace Playground
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class DemoAttribute(int index, string title) : Attribute
    {
        public int Index { get; } = index;
        public string Title { get; } = title;
    }
}
