// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using CheckTransitiveFrameworkReference;

using Spectre.Console.Cli;


var app = new CommandApp<CheckTransitiveFrameworkReferenceCommand>();

return await app.RunAsync(args);
