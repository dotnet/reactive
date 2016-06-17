// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using Xunit;
using System;

namespace Tests
{
    internal class AssertEx
    {
        internal static void Throws<T>(Action action)
            where T : Exception
        {
            Throws<T>(action, _ => true);
        }

        internal static void Throws<T>(Action action, Func<T, bool> assert)
            where T : Exception
        {
            var failed = false;

            try
            {
                action();
            }
            catch (T ex)
            {
                Assert.True(assert(ex));

                failed = true;
            }

            Assert.True(failed);
        }

        internal static void SucceedOrFailProper(Action action)
        {
            try
            {
                action();   
            }
            catch (AggregateException ex)
            {
                var inner = ex.Flatten().InnerException;

                // TODO: proper assert; unfortunately there's not always a good call stack
            }
        }
    }
}