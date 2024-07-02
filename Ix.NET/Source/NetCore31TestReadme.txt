The tests all include a .NET Core 3.1 target, even though that has long since been out of support.

The reason for this is that Ix.NET 6.0 offers a .NET Standard 2.1 target, and currently, the only way we have of testing this is through .NET Core 3.1.

Although current versions of .NET (but not .NET Framework) support .NET Standard 2.1, they are all going to prefer the net6.0 target. We do test on currently supported versions of .NET, but we also want to test the .NET Standard 2.1 target.

It might be possible to test via Mono instead, or possibly even Unity, but this would likely involve additional CI/CD work, so for now we're sticking with the .NET Core 3.1 target framework. It's unsupported, but it's better than doing no testing at all.