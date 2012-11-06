// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Reactive.Linq", Justification = "By design")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.IQbservable", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.IQbservable`1", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.IQbservableProvider", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "type", Target = "System.Reactive.Linq.Qbservable", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#Amb`1(System.Reactive.Linq.IQbservable`1<!!0>,System.IObservable`1<!!0>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#Amb`1(System.Reactive.Linq.IQbservableProvider,System.Collections.Generic.IEnumerable`1<System.IObservable`1<!!0>>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Amb", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#Amb`1(System.Reactive.Linq.IQbservableProvider,System.IObservable`1<!!0>[])", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#AsQbservable`1(System.IObservable`1<!!0>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Remotable", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#Remotable`1(System.Reactive.Linq.IQbservable`1<!!0>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#ToQbservable`1(System.Linq.IQueryable`1<!!0>)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "resource", Target = "System.Reactive.Strings_Providers.resources", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qbservable", Scope = "member", Target = "System.Reactive.Linq.Qbservable.#ToQbservable`1(System.Linq.IQueryable`1<!!0>,System.Reactive.Concurrency.IScheduler)", Justification = "It's homoiconic, dude!")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "System.Reactive.ObservableQuery`1+ObservableRewriter.#VisitConstant(System.Linq.Expressions.ConstantExpression)", Justification = "Expression visitor should not pass in null references.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "System.Reactive.ObservableQuery`1+ObservableRewriter.#VisitMethodCall(System.Linq.Expressions.MethodCallExpression)", Justification = "Expression visitor should not pass in null references.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Taken care of by lab build.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion", Justification = "Taken care of by lab build.")]
