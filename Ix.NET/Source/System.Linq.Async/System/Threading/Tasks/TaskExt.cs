// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Threading.Tasks
{
    internal static class TaskExt
    {
        public static readonly ValueTask<bool> True = new ValueTask<bool>(true);
        public static readonly ValueTask<bool> False = new ValueTask<bool>(false);
        public static readonly ValueTask CompletedTask = new ValueTask(Task.FromResult(true));

        /// <summary>
        ///  A constant TaskCompletionSource already completed with true.
        /// </summary>
        public static readonly TaskCompletionSource<bool> ResumeTrue;

        static TaskExt()
        {
            ResumeTrue = new TaskCompletionSource<bool>();
            ResumeTrue.TrySetResult(true);
        }
    }
}
