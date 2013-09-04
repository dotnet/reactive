// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

//
// NOTE: Identical copies of this file are kept in System.Reactive.Linq and System.Reactive.Providers.
//

namespace System.Reactive
{
    // We can't make those based on the Strings_*.resx file, because the ObsoleteAttribute needs a compile-time constant.

    class Constants_Linq
    {
#if PREFER_ASYNC
        public const string USE_ASYNC = "This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
        public const string USE_TASK_FROMASYNCPATTERN = "This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
#endif
    }
}