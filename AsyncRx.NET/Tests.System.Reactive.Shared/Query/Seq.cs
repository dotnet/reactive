// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

// The query representation: a Seq<T> is a *description* of an observable
// sequence, not an observable. Its leaves are the platform's own objects (NativeSeq<T>: a
// testable source the test created through the scheduler, or an inner window/group handed to
// a callback) and the neutral creation operators; its interior nodes are operator applications,
// one node type per overload. Nothing here runs. A platform (IPlatform, in the Harness folder) is a
// visitor that turns a tree into its own pipeline at the moment the test's Start callback is
// invoked, and the result is native from end to end: xs.Window(...).Select((w, i) => ...).Merge()
// materializes as the platform's Window, Select and Merge with nothing in between — the inner
// window reaches the projection callback as a NativeSeq<T> *value*, and the callback's result
// is materialized in place. A runtime abstraction that dispatched each operator call to the
// platform at once could not do that: it would have to wrap every inner window with a Select and
// unwrap it with another, so the pipeline under test would not be the one written. Everything
// around the query — the scheduler, the sources, Start, the assertions — is in the Harness folder.

/// <summary>A description of an observable sequence of <typeparamref name="T"/>.</summary>
public abstract class Seq<T> : ISeq
{
    public abstract object Accept(ISeqVisitor visitor);

    public abstract override string ToString();
}
