// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Xunit.Sdk
{
    /// <summary>
    /// Marker interface required by xUnit.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>xunit.assert.source</c> package normally includes this. However, unlike all the
    /// other types that package adds to our project, this one type is declared to be
    /// unconditionally public.
    /// </para>
    /// <para>
    /// We think this might be a bug: https://github.com/xunit/xunit/issues/2703
    /// </para>
    /// <para>
    /// In any case, we do not want our library to be exporting public types in the Xunit.Sdk
    /// namespace. So the csproj file carefully excludes the Asserts/Sdk/IAssertionException.cs
    /// file supplied by the package (which defines this type as <c>public</c>), and this file
    /// supplies an equivalent but <c>internal</c> definition.
    /// </para>
    /// </remarks>
    internal interface IAssertionException
    {
    }
}
