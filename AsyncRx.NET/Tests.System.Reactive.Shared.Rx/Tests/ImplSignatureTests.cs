// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Pins that the real calls in <see cref="RxTarget"/> keep Rx.NET's genericity.</summary>
/// <remarks>
/// An <c>*Impl</c> written over a closed observable type compiles and passes every flat scenario,
/// and fails only when a nested one reaches it. This turns that into an immediate failure.
/// </remarks>
[TestClass]
public sealed class ImplSignatureTests
{
    [TestMethod]
    public void Impl_methods_keep_the_librarys_genericity()
    {
        var problems = ImplSignatures.Check(
            typeof(RxTarget),
            typeof(IObservable<>),
            typeof(IGroupedObservable<,>),
            typeof(ITestableObservable<>));

        Assert.IsEmpty(problems, string.Join(Environment.NewLine, problems));
    }
}
