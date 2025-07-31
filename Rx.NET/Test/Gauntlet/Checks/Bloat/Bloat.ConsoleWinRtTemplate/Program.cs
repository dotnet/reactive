// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

// This illustrates a plausible use of WinRT APIs in combination with Rx in a .NET 8 console application.
//
// This is useful because it's about the most lightweight form of dependency on a Windows-specific TFM: the WinRT APIs
// would be unavailable with a normal TFM, but we've not got a heavyweight dependency on a UI framework. Nor have we had
// to adapt the application structure in any way to accommodate the WinRT APIs - we just invoke them.
//
// We're using the WinRT network connectivity API.

using System.Reactive.Linq;

using Windows.Networking.Connectivity;

var p = NetworkInformation.GetConnectionProfiles().ToList();
var ip = NetworkInformation.GetInternetConnectionProfile();
Console.WriteLine(ip.ProfileName);

var networkStatusChanges = Observable.FromEvent<NetworkStatusChangedEventHandler, object>(
    h => NetworkInformation.NetworkStatusChanged += h,
    h => NetworkInformation.NetworkStatusChanged -= h);

var connectivity = Observable
    .Concat(Observable.Return(default(object)), networkStatusChanges)
    .Select(_ => NetworkInformation.GetInternetConnectionProfile()?.GetNetworkConnectivityLevel() ?? NetworkConnectivityLevel.None)
    .DistinctUntilChanged();

Console.WriteLine("Listening for network connectivity changes...");
connectivity.Subscribe(level => Console.WriteLine($"Network connectivity level: {level}"));

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
