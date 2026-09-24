// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    object Take<T>(TakeSeq<T> seq);

    object TakeScheduled<T>(TakeScheduledSeq<T> seq);

    object TakeTime<T>(TakeTimeSeq<T> seq);
}
