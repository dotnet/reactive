// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                Assert.IsTrue(assert(ex));

                failed = true;
            }

            Assert.IsTrue(failed);
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