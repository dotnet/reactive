// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// TODO: should we:
//  1. Move this type back out into this assembly, reinstating this as a proper package, not a facade
//  2. Add a new System.Reactive.Integration.Remoting for this.
//  3. Leave it in the V6 System.Reactive facade, since that's where existing dependents will already expect it to be.
// (Currently on 3.)
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Reactive.Linq.RemotingObservable))]
