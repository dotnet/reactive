// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;

namespace Playground
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class DemoAttribute : Attribute
    {
        public DemoAttribute(int index, string title)
        {
            Index = index;
            Title = title;
        }

        public int Index { get; }
        public string Title { get; }
    }
}
