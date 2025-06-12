# How to publish a `System.Reactive` release

This document describes how to publish a new release of `System.Reactive` and associated NuGet packages.

There are two procedures, one for when either the major or minor version number changes, and a different process to publish a new patch version for an existing major.minor version.

## New Major.Minor release

If we are bumping either the major or minor version number (e.g., when the latest version is 6.0.1, and we want to publish 6.1, or 7.0), we do the following:

### Create a new release branch

Create a new branch from `main` named `rel/rx-v<major>.<minor>` e.g.:

```
git checkout -b rel/rx-v6.0`
```

### Update `version.json`

The `Rx.NET/Source/version.json` file always specifies a preview tag on `main`. We remove this on release branches. E.g., we change something like this:

```json
  "version": "6.1.0-preview.{height}",
```

To this:

```json
  "version": "6.1",
```

**Note**: although `main` specifies all 3 digits of the semantic version, the way `nbgv` works for releases requires us to specify just two. It will add the third based on counting the number of commits since the version number was last change. If we were to set this to `6.1.0`, it would set the version to `6.1.0.1`. This doesn't affect the NuGet package version, but it is how we accidentally ended up with the [6.0.0 release](https://github.com/dotnet/reactive/releases/tag/rxnet-v6.0.0.1) being accidentally mislabelled as 6.0.0.1 in GitHub.

Commit this change. You can check that you get the correct version by running nbgv from the `Rx.NET/Source` folder:

```
nbgv get-version
```

**Note**: commits are significant to nbgv, meaning:

* you need to commit the change to `version.json` for `nbgv get-version` to report the right version
* any subsequent commits on the release branch will result in higher version numbers

That second point means that the update to `Rx.NET/Source/version.json` should be the _last_ commit on the release branch. If it isn't that implies you weren't quite ready to create the release branch, but it also means you'll get, say, 6.1.1 instead of 6.1.0. (If for some reason you really can't avoid an additional commit, you can use the technique shown later for creating a new Patch release to adjust the version number down, but normally you shouldn't need to do this for a new Major.Minor release.)

### Publish the branch to GitHub

Push your new branch up to GitHub. This should trigger a new CI build. Watch the [Rx.NET-CI](https://dev.azure.com/dotnet/Rx.NET/_build?definitionId=9&_a=summary) pipeline on Azure DevOps.

Occasionally, these builds fail for no good reason. E.g., if you look at the [Rx.NET 6.0.1 build](https://dev.azure.com/dotnet/Rx.NET/_build/results?buildId=105869&view=logs&j=d19fa873-01b0-5cbd-94ee-08d1f56df169) you'll see that the `IntegrationTests` step took two attempts for both Linux and Windows. In both cases, the first attempt failed in the step that installs the .NET 8.0 SDK on the build agent. There seemed to be no reason for this, and just rerunning the failed step a bit later worked.

You should check that the build number reported by Azure DevOps matches your expectations.


### Create a Release in Azure DevOps

Once your build completes, go to the [Rx.NET Deploy](https://dev.azure.com/dotnet/Rx.NET/_release?view=all&_a=releases&definitionId=1) release, and create a new release to publish that build.

Note that for non-preview builds, this release's title will grow an extra part of the version number e.g. v6.1.0.1. Don't worry about that. It won't appear in any of the artifacts this creates. It's just a side effect of how this release is set up to incorporate the preview tag when present.

### Check the NuGet packages in the Azure DevOps package feed

The release will automatically publish packages to the [RxNet Azure DevOps package feed](https://dev.azure.com/dotnet/Rx.NET/_artifacts/feed/RxNet) (but not yet to NuGet.org). You should create a project in Visual Studio that uses that feed to test the newly published packages.

Note that at this stage, these are the real packages. They should have a non-preview version number, and they will have been signed. When you use them in Visual Studio they should look exactly as you would expect them to appear to the general public once you put them on `NuGet.org`. The only difference is that they're coming from an Azure DevOps feed.

This is our last chance to decide it's all gone wrong. (Note that if you do decide to abandon the release, you should consider the version number to have been "burned". Once you've fixed the problem you will need a new number, e.g., 6.1.1. This is unsatisfying, but the Azure DevOps RxNet feed is publicly visible, so it's conceivable that someone somewhere managed to get hold of the 6.1.0 package you put in that feed. It is a bad idea to create multiple different publicly visible versions of a NuGet package with the same version number.)

### Publish to NuGet.org

If your package passes the smoke test, you can approve the final stage of the release. It is labelled "NuGet.org", but it also creates a GitHub release.

Once this completes, the only thing left to do is wait while NuGet indexes your package and makes it available to all.


## New Patch Release

When some existing Major.Minor version (e.g. 6.1.0) already exists, it will sometimes be appropriate to create a patch release, in which only the third part of the version number changes. With semantic versioning, this would mean that there is no change to the intended public surface area. There may be a bug fix (which might entail fixing an unintended deviation from the intended public API behaviour) or a performance enhancement. But there are no new or modified APIs, and no changes to behaviour other than in cases where the current behaviour is not what was intended.

### Cherry Pick Changes

We use the existing `rel/rx-v<major>.<minor>` branch. The change (e.g. bug fix, or performance enhancement) will already exist as one or more commits on another branch, usually `main`. Cherry pick as many commits as are required to make the required change. If necessary make any modifications required to get this commit to work in context. (It might be that the relevant change depends on other changes made on the other branch that we don't wish to incorporate in this patch release. Patch releases should change as little as possible. These release branches are essentially stuck in time, except for whatever bugfixes or other changes we choose to bring in, so cherry picking won't always work without some additional changes.)

Obviously you should test at this point.

### Consider adjusting `version.json` to avoid skipped version numbers

`nbgv` automatically generates new patch version numbers by counting the number of commits that have occurred since the last commit that cause a specific version number to be used. Since we're on a `rel/rx-v<major>.<minor>` branch, there will be a change to `version.json` because the step [described earlier](#update-versionjson) will have been followed. If you are making the first patch release for this major.minor version, that will normally be the last commit on the release branch. So if you cherry pick a single commit onto the release branch, `nbgv` will see that there is now one commit after the one that modified `version.json`, and so it will choose `.1` as the patch version.

However, if you want to incorporate multiple changes, or if you needed to cherry pick multiple commits, or you had to make further changes after cherry picking to get things working, you may find the version number is higher than you want. Instead of 6.1.0 being followed by 6.1.1, you may find that `nbgv` chooses 6.1.3 because it sees 3 commits after the last change to `version.json`.

We can avoid confusing gaps in version numbering by editing `version.json` to adjust for this. If you look at the [`rel/rx-v6.0`](https://github.com/dotnet/reactive/commits/rel/rx-v6.0) branch, you'll see commit [`e29c7a5`](https://github.com/dotnet/reactive/commit/e29c7a50db88513a87651c366088da8b7f40b1f0) in which the preview tag was removed from the `version.json`. (You can also see we failed to remove the patch version number, which is why the GitHub release for Rx.NET 6.0.0 is anomalously labelled 6.0.0.1.) That is the commit that fixes the semantic version at 6.0.0.

The 6.0.1 release is some 9 commits later. That's because we now support .NET 8.0 testing, so we needed to bring some build changes onto the branch, in addition to the two changes we actually wanted. To avoid a big gap in version numbers, [commit `1cfc646`](https://github.com/dotnet/reactive/commit/1cfc6465d1c9c6144d5b4e6420240f2767c8f85c) adds a `versionHeightOffset`, setting it to -8 to get us back to the version 6.0.1 as required.

You don't have to do this, and historically there have been a few Rx.NET releases that skipped a version. But it can be less confusing to keep it sequential, particularly in cases like this where the change would have been fairly large.

### Publish

You can now follow the earlier steps starting from [creating a release in Azure DevOps](#create-a-release-in-azure-devops) to make this new version available.