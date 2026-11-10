// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Pins what <see cref="ImplCalls"/> returns for the shapes the checker relies on.</summary>
[TestClass]
public sealed class ImplCallsTests
{
    [TestMethod]
    public void Typed_selector_form_resolves_to_the_closed_library_overload()
    {
        var library = ImplCalls.Resolve(Impl("SumInt32SelectorImpl"))!;

        Assert.AreEqual(typeof(Observable), library.DeclaringType);
        Assert.AreEqual("Sum", library.Name);
        Assert.IsTrue(library.IsGenericMethodDefinition);

        var tSource = library.GetGenericArguments().Single();
        var parameters = library.GetParameters();
        Assert.HasCount(2, parameters);
        Assert.AreEqual(
            typeof(IObservable<>).MakeGenericType(tSource),
            parameters[0].ParameterType);
        Assert.AreEqual(
            typeof(Func<,>).MakeGenericType(tSource, typeof(int)),
            parameters[1].ParameterType);
        Assert.AreEqual(typeof(IObservable<int>), library.ReturnType);
    }

    [TestMethod]
    public void Generic_form_resolves_to_the_open_library_definition()
    {
        var library = ImplCalls.Resolve(Impl("WhereImpl"))!;

        Assert.AreEqual("Where", library.Name);
        Assert.IsTrue(library.IsGenericMethodDefinition);

        var tSource = library.GetGenericArguments().Single();
        Assert.AreEqual(typeof(IObservable<>).MakeGenericType(tSource), library.ReturnType);
        Assert.AreEqual(
            typeof(Func<,>).MakeGenericType(tSource, typeof(bool)),
            library.GetParameters()[1].ParameterType);
    }

    [TestMethod]
    public void Instance_method_resolves_with_its_declaring_type()
    {
        var library = ImplCalls.Resolve(Impl("ConnectImpl"))!;

        Assert.AreEqual("Connect", library.Name);
        Assert.IsFalse(library.IsStatic);
        Assert.AreEqual(typeof(IConnectableObservable<>), library.DeclaringType);
    }

    [TestMethod]
    public void An_impl_that_calls_nothing_resolves_to_null()
    {
        Assert.IsNull(ImplCalls.Resolve(Impl("SubscriptionsImpl")));
    }

    private static MethodInfo Impl(string name) =>
        typeof(RxTarget).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)!;
}
