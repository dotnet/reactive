// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using PlugIn.HostDriver;

namespace CheckIssue97;

internal record Scenario(string HostTfm, PlugInDescriptor FirstPlugIn, PlugInDescriptor SecondPlugIn);
