// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    object WindowClosings<T, TWindowClosing>(WindowClosingsSeq<T, TWindowClosing> seq);

    object WindowOpenings<T, TWindowOpening, TWindowClosing>(WindowOpeningsSeq<T, TWindowOpening, TWindowClosing> seq);

    object WindowBoundaries<T, TWindowBoundary>(WindowBoundariesSeq<T, TWindowBoundary> seq);

    object WindowCount<T>(WindowCountSeq<T> seq);

    object WindowTime<T>(WindowTimeSeq<T> seq);

    object WindowTimeShift<T>(WindowTimeShiftSeq<T> seq);

    object WindowTimeOrCount<T>(WindowTimeOrCountSeq<T> seq);
}
