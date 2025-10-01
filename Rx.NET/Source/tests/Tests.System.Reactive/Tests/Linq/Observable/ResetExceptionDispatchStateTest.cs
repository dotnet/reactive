// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;
using System.Threading.Tasks;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ResetExceptionDispatchStateTest
    {

        [TestMethod]
        public async Task ResetExceptionDispatchState_Throw_Consistent_StackTrace_On_Await()
        {
            var ts = Observable.Throw<int>(new Exception("Aaargh!")).ResetExceptionDispatchState();

            string stackTrace = null;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _ = await ts;
                }
                catch (Exception e)
                {
                    if (stackTrace is null)
                    {
                        stackTrace = e.StackTrace;
                    }
                    else
                    {
                        Assert.Equal(stackTrace, e.StackTrace);
                    }
                }
            }
        }

        [TestMethod]
        public async Task ResetExceptionDispatchState_Replay_Consistent_StackTrace_On_Await()
        {
            var cts = Observable.Throw<int>(new Exception("Aaargh!"), CurrentThreadScheduler.Instance).Replay(1);
            cts.Connect();
            var ts = cts.ResetExceptionDispatchState();

            string stackTrace = null;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _ = await ts;
                }
                catch (Exception e)
                {
                    if (stackTrace is null)
                    {
                        stackTrace = e.StackTrace;
                    }
                    else
                    {
                        Assert.Equal(stackTrace, e.StackTrace);
                    }
                }
            }
        }
    }
}
