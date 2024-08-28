// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace Tests.System.Reactive
{
    /// <summary>
    /// Verifies behavior around unobserved exceptions from tasks.
    /// </summary>
    /// <remarks>
    /// Testing whether unhandled exceptions emerge from <see cref="TaskScheduler.UnobservedTaskException"/> is not
    /// entirely straightforward. A few tests need to do this because we have some historical behaviour described in
    /// https://github.com/dotnet/reactive/issues/1256 that needs to be preserved for backwards compatibility, along
    /// with some new functionality enabling optional different behavior regarding unobserved exceptions. This provides
    /// common mechanism to enable such testing.
    /// </remarks>
    internal class TaskErrorObservation : IDisposable
    {
        private ManualResetEventSlim _exceptionReportedAsUnobserved;
        private WeakReference<Task> _taskWeakReference;

        public TaskErrorObservation()
        {
            _exceptionReportedAsUnobserved = new(false);
            TaskScheduler.UnobservedTaskException += HandleUnobservedException;
        }

        public Exception Exception { get; } = new();

        public void Dispose()
        {
            if (_exceptionReportedAsUnobserved is not null)
            {
                _exceptionReportedAsUnobserved.Dispose();
                _exceptionReportedAsUnobserved = null;
                TaskScheduler.UnobservedTaskException -= HandleUnobservedException;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public IDisposable SuscribeWithoutKeepingSourceReachable(
            Func<Func<Task, Task>, Exception, IDisposable> subscribe)
        {
            // We provide nested function because the temporary storage location that
            // holds the value returned by a call to, say, Observable.StartAsync can end up keeping it reachable
            // for GC purposes, which in turn keeps the task reachable. That stops the
            // finalization-driven unobserved exception detection from working.
            // By calling Subscribe in a method whose stack frame is then immediately torn
            // down, we ensure that we don't hang onto anything other than the IDisposable
            // it returns.

            return subscribe(
                t =>
                {
                    _taskWeakReference = new(t);
                    return t;
                },
                Exception);
        }

        public IDisposable SuscribeWithoutKeepingSourceReachable<T>(
            Func<Func<Task<T>, Task<T>>, Exception, IDisposable> subscribe)
        {
            return SuscribeWithoutKeepingSourceReachable(
                (Func<Task, Task> setTask, Exception ex) => subscribe(
                    t =>
                    {
                        setTask(t);
                        return t;
                    }, ex));
        }


        public void AssertExceptionReportedAsUnobserved()
        {
            var start = Environment.TickCount;
            var firstIteration = true;
            while (!_exceptionReportedAsUnobserved.Wait(TimeSpan.FromSeconds(firstIteration ? 0 : 0.001)) &&
                ((Environment.TickCount - start) < 5000))
            {
                firstIteration = false;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Assert.True(_exceptionReportedAsUnobserved.Wait(TimeSpan.FromSeconds(0.01)));
        }

        /// <summary>
        /// Waits for the task to become unreachable, and then verifies that this did not result in
        /// <see cref="TaskScheduler.UnobservedTaskException"/> reporting the failure.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void AssertExceptionNotReportedAsUnobserved()
        {
            if (_taskWeakReference is null)
            {
                throw new InvalidOperationException("Test did not supply task to " + nameof(TaskErrorObservation));
            }

            var start = Environment.TickCount;
            var firstIteration = true;
            do
            {
                // We try to get away without sleeping, to enable tests to run as quickly as
                // possible, but if the object remains reachable after the initial attempt to
                // force a GC and then immediately run finalizers, there's probably some deferred
                // work waiting to happen somewhere, so we are better off backing off and giving
                // that a chance to run.
                if (firstIteration)
                {
                    firstIteration = false;
                }
                else
                {
                    Thread.Sleep(1);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            } while (IsTaskStillReachable() &&
                     ((Environment.TickCount - start) < 5000));


            // The task is now unreachable, but it's possible that this happened in between our
            // last call to GC.WaitForPendingFinalizers and our test for reachability, in which
            // case it might still be awaiting finalization, so we need one more of these to ensure
            // it gets flushed through:
            GC.WaitForPendingFinalizers();

            Assert.False(_exceptionReportedAsUnobserved.IsSet);
        }

        // This needs to be done in a separate method to ensure that when the weak reference returns a task, we
        // immediately destroy the stack frame containing the temporary variable into which it was returned,
        // to avoid keeping the task reachable by accident.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private bool IsTaskStillReachable()
        {
            return _taskWeakReference.TryGetTarget(out _);
        }


        private void HandleUnobservedException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (e.Exception.InnerException == Exception)
            {
                e.SetObserved();
                _exceptionReportedAsUnobserved.Set();
            }
        }
    }
}
