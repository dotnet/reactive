// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using PlugIn.Api;

using System.Reactive.Linq;

namespace PlugInTest
{
    // Note: this file is compiled into all of the plug-ins:
    //  * the .NET 4.5 and .NET 4.6 plug-ins
    //  * all versions of Rx.NET
    public partial class PlugInEntryPoint
    {
        public RxCancellationFlowBehaviour GetRxCancellationFlowBehaviour()
        {
            // There appears to be exactly one difference between the net45 and net46 builds of Rx 3.0:
            // The net46 one uses the overload of TaskCompletionSource.TrySetCanceled that takes a
            // CancellationToken as an argument. That overload was not available before .NET 4.6, so in
            // the net45 build, it calls the overload that takes no arguments.
            //
            // Rx calls TrySetCanceled in these circumstances:
            //  1. on the TCS for the Task returned by ForEachAsync when the CancellationToken also passed
            //      to ForEachAsync is canceled
            //  2. on the TCS for the Task returned by ToTask when the CancellationToken also passed
            //      to ToTask is canceled
            //
            // But what is the significance of this? The TCS just forwards the call to the Task's internal
            // TrySetCanceled, which in turn passes it to RecordInternalCancellationRequest. It appears
            // to use this for certain internal consistency checks in debug builds, but the main (only?)
            // externally observable impact of this when using a normal build of .NET is that if we
            // set the cancellation token into a cancelled state, and then Wait the task, it will
            // throw an OperationCanceledException (wrapped in an AggregateException) where the
            // CancellationToken property is associated with the CancellationToken passed to the TCS.
            // In .NET 4.5, which doesn't have this TrySetCanceled overload, the OperationCanceledException's
            // CancellationToken property is not associated with the cancellation token that we used
            // to trigger the cancellation, which is incorrect.
            // It's not clear why this might matter in practice, but it is possible to detect the
            // difference because CancellationToken.Equals lets you determine whether two CancellationToken
            // values are associated with the same underlying CancellationTokenSource.
            // Perhaps this is there so you can tell whether an operation was cancelled because you asked
            // it to be or whether something else got in there first.

            var cancel = new CancellationTokenSource();
            var t = Observable.Timer(TimeSpan.FromSeconds(0.2))
                .ForEachAsync(_ =>
                {
                }, cancel.Token);

            cancel.Cancel();

            try
            {
                t.Wait();
            }
            catch (AggregateException ae)
            {
                var ocx = (OperationCanceledException) ae.InnerException!;
                var cancelledExceptionIsAssociatedWithTokenPassedToRx = ocx.CancellationToken.Equals(cancel.Token);
                return cancelledExceptionIsAssociatedWithTokenPassedToRx
                    ? RxCancellationFlowBehaviour.FlowedToOperationCanceledException
                    : RxCancellationFlowBehaviour.NotFlowedToOperationCanceledException;
            }
            throw new InvalidOperationException("Expected Wait to throw an exception");
        }
    }
}
