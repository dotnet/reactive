// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

// The ObserveOn overload accepting a SynchronizationContext is available in all versions of Rx.NET,
// so this should never have a problem compiling. However, we know that in scenarios where Rx.NET
// offers WPF and Windows Forms overloads of ObserveOn, we get compiler errors if the
// Microsoft.Desktop.App framework is unavailable. (This is what stops DisableTransitiveFrameworkReferences
// from being a good workaround for bloat in Rx 6.0.1.)
//
// Even though this code doesn't try to use those overloads, it fails to compile because the C#
// compiler can see that other overloads are available, but because it can't find the types they use,
// it doesn't know whether they might be candidates here, and so it reports an error.

SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

var numbers = Observable.Range(1, 10);
var numbersViaSyncContext = numbers.ObserveOn(SynchronizationContext.Current!);
numbers.Subscribe(x => Console.WriteLine($"Number: {x}"));
