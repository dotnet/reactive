// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System
{
    internal static class Error
    {
        public static Exception ArgumentNull(string paramName) => new ArgumentNullException(paramName);
        public static Exception ArgumentOutOfRange(string paramName) => new ArgumentOutOfRangeException(paramName);
        public static Exception NoElements() => new InvalidOperationException(Strings.NO_ELEMENTS);
        public static Exception MoreThanOneElement() => new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
        public static Exception NotSupported() => new NotSupportedException(Strings.NOT_SUPPORTED);
    }
}
