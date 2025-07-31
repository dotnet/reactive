// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using RxGauntlet.Cli;

using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<RxGauntletAllPublishedRxCommand>("test-all-published-rx");
    config.AddCommand<RxGauntletCandidateCommand>("test-candidate");
});

return await app.RunAsync(args);
