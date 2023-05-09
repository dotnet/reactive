// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "System.Reactive.Concurrency.LocalScheduler+WorkItem.#CompareTo(System.Reactive.Concurrency.LocalScheduler+WorkItem)", Justification = "Checked all enqueue operations against null reference insertions.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Disposables", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Linq", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Subjects", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Concurrency", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Linq", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Threading.Tasks", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "System.Reactive.Either`2+Left.#Switch(System.Action`1<!0>,System.Action`1<!1>)", Justification = "Producer cannot pass null to setSink parameter.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "System.Reactive.Either`2+Left.#Switch`1(System.Func`2<!0,!!0>,System.Func`2<!1,!!0>)", Justification = "Producer cannot pass null to setSink parameter.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Scope = "member", Target = "System.Reactive.Either`2+Right.#Switch(System.Action`1<!0>,System.Action`1<!1>)", Justification = "Producer cannot pass null to setSink parameter.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Scope = "member", Target = "System.Reactive.Either`2+Right.#Switch`1(System.Func`2<!0,!!0>,System.Func`2<!1,!!0>)", Justification = "Producer cannot pass null to setSink parameter.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope = "member", Target = "System.Reactive.Linq.ObservableImpl.ElementAt`1+_.#OnCompleted()", Justification = "Asynchronous behavior; no more index parameter in scope.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Concurrency", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.PlatformServices", Justification = "By design.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Linq", Justification = "By design")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.IQbservable", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.IQbservable`1", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.IQbservableProvider", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.Qbservable", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#Amb`1(System.Reactive.Linq.IQbservable`1<!!0>,System.IObservable`1<!!0>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#Amb`1(System.Reactive.Linq.IQbservableProvider,System.Collections.Generic.IEnumerable`1<System.IObservable`1<!!0>>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#Amb`1(System.Reactive.Linq.IQbservableProvider,System.IObservable`1<!!0>[])", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#AsQbservable`1(System.IObservable`1<!!0>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#ToQbservable`1(System.Linq.IQueryable`1<!!0>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "resource", Target = "System.Reactive.Strings_Providers.resources", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#ToQbservable`1(System.Linq.IQueryable`1<!!0>,System.Reactive.Concurrency.IScheduler)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "System.Reactive.ObservableQuery`1+ObservableRewriter.#VisitConstant(System.Linq.Expressions.ConstantExpression)", Justification = "Expression visitor should not pass in null references.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "System.Reactive.ObservableQuery`1+ObservableRewriter.#VisitMethodCall(System.Linq.Expressions.MethodCallExpression)", Justification = "Expression visitor should not pass in null references.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion", Justification = "Taken care of by lab build.")]
