# How Rx.NET Uses Nerdbank.GitVersioning

Rx.NET uses [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning) (`nbgv`) to generate the version numbers used for NuGet packages, various file version numbers and attributes, Azure DevOps build and release numbers, and GitHub release names.

We use `nbgv` in a slightly non-standard way. This document explains the pressures that led to this, and it explains what we do and why.

## Status

_Fait accompli_. (This ADR is a retrospective explainer of what has been going on for years.)

## Authors

@idg10 ([Ian Griffiths](https://endjin.com/who-we-are/our-people/ian-griffiths/)) wrote this document, but the practices were essentially in place before endjin took over maintenance of this project. Since there doesn't seem to have been any documentation before, it's hard to know exactly who to credit, so we can't be more specific about prior authorship than: the former maintainers of Rx.NET.


## Context

To use a library safely, developers need to know where it came from. Supply chain attacks, in which malicious actors build modified versions of widely used libraries and trick developers into using them, have proven devastatingly effective. One vital defence against such attacks is to make it easy for a developer using a software component to understand the origin of that component.

### How nbgv helps

This statement from https://github.com/dotnet/Nerdbank.GitVersioning/blob/main/README.md is at the heart of `nbgv`'s philosophy:

> Every single commit can be built and produce a unique version.

Since there is a 1:1 relationship between git commits and version numbers, then for any particular copy of the library, its version is associated with one specific git commit. There should never be any doubt about what code you are running.

Of course, malicious actors are free to set version numbers to whatever they want. But since we publish exactly one set of binaries for any single version (and we sign the code) it is possible to look at any file claiming to be an Rx.NET release and check whether it really is. First check that it is signed with the `Reactive Extensions for .NET (.NET Foundation)` certificate (which in turn is certified by the .NET Foundation Projects Code Signing certificate authority). Then look at its version number. Find the release for that version in GitHub. You now know exactly which commit the library you're using was built from. (If you want to cross check—perhaps you suspect supply chain attacks have been attempted and want to and want to leave absolutely no doubt—you could also check that the binary you have is byte for byte identical to the file we published with that version. Also, you can find the commit that any Rx.NET DLL was built against by looking in the `Product version`. You can find this by right-clicking on the DLL in Windows File Explorer, selecting Properties and going to the Details tab. For `System.Reactive.dll` this will be `6.1.0+f4da16f15a`. That is consistent with the commit reported for [release 6.1.0 on GitHub](https://github.com/dotnet/reactive/releases/tag/rxnet-v6.1.0). You can also see the full SHA for any Rx.NET NuGet package in the [NuGet Package Explorer](https://nuget.info/packages/System.Reactive/6.1.0). The current version of the Package Explorer shows it on the left in the Misc section.)

One of the ways `nbgv` achieves this strict commit:version mapping is to avoid relying on tags or branch names in the version generation algorithm. Both of these can change over time. (Indeed, the normal practice for a branch is that the tip changes all the time; tags are not normally expected to change, but they can, and this makes them an unreliable way to establish provenance.) The `nbgv` command line tool can take branch names into account with some of the utilities it provides, but that is done as a convenience to help you modify the `version.json`, and does not affect the core version generation algorithm: that takes into account only the changes you've chosen to commit.

In any case, we don't use these convenience features of `nbgv`. They don't actually work for us because they make an assumption that is not true for this repo.

### The unwanted nbgv assumption

There's one problem with `nbgv` when it comes to Rx.NET: it makes an assumption about how the 3rd part of the version number (what [Semantic Versioning](https://semver.org/) calls the _patch_, and what .NET's [`Version`](https://learn.microsoft.com/en-us/dotnet/api/system.version) type calls the _build_). It assumes that you're happy for it to reflect accidents of history, leading to version numbers that look nothing like anything we've historically done in Rx.NET.

For example, the first ever Rx.NET release with a `major.minor` version of `5.0` had a NuGet version of `5.0.0`, and the .NET assemblies all had version `5.0.0.0`. Similarly, the next major release had NuGet and .NET assembly versions of `6.0.0` and `6.0.0.0`. We did two bugfix `6.0` releases with NuGet versions of `6.0.1` and `6.0.2`. (The .NET assembly version remained at `6.0.0.0` for these because you typically don't want that to change with a bugfix release.)

Our view is that these kinds of version numbers communicate intent clearly. It's very obvious that `6.0.0` is the first of the `6.0` releases. It's very obvious that `6.0.1` was the first bugfix release in the `6.0` series.

But compare that with a project that uses `nbgv` in the way `nbgv` is meant to be used. The obvious choice here is [`nbgv` itself](https://www.nuget.org/packages/Nerdbank.GitVersioning). Here are the most recent release numbers (at the time of writing this):

* `3.7.112`
* `3.7.115`
* `3.8.118`
* `3.9.50`
* `3.10.70`
* `3.10.85`

That's every public release since December 2024 up to June 2026. There's nothing technically wrong with these, but I find the third part of these versions distracting. When I first saw `3.9.50` I thought: wow, they must release a lot of versions; I had better go and check that there isn't a more recent `3.9.x`. In fact that was the only ever `3.9.x` release. (In my preferred versioning aesthetic, it would have been called `3.9.0`.) And in cases where `nbgv` has shipped more than one patch release in a particular `major.minor`, you can't infer much from these numbers: the `3.7.112` and `3.7.115` releases (a difference of 3) were about a month apart, but `3.10.70` and `3.10.85` (a difference of 15) were released on the same day!

Clearly the author of `nbgv` isn't attempting to tell consumers anything with that patch/build version component other than the fact that one version is newer than another version.

There's a reason for this: `nbgv` tries to provide a version number that increases even as you move from preview to release. For example, you might see `7.0.1-preview`, then `7.0.2-beta` then `7.0.3`. From the perspective of the developer working on a project day to day, you get a completely sensible-looking sequence of version numbers, so when a public release with `50` in the patch/build position appears, it won't look at all out of place—you've been in the codebase every day, so you know about the 49 preceding commits that got you to where you are.

But to a consumer of the library, this part of the version number is mostly noise. The only useful information it provides is that if you do have more than one release for a particular `major.minor` version, then the higher-numbered version is the latest. But versions like `3.7.112` and `3.7.115` are less informative for a library consumer than `3.7.0` and `3.7.1` (in which it's totally clear that these are the first `3.7` release and then the first bugfix `3.7` release respectively).

I also find that the noise in this third position makes it harder to see the important information: I have to look at that list of version numbers for longer to understand it than if it had been this:

* `3.7.0`
* `3.7.1`
* `3.8.0`
* `3.9.0`
* `3.10.0`
* `3.10.1`

None of which is to say that `nbgv` is wrong. It's not. It makes a logically justifiable choice. It's just not how we want our version numbers to look. We want to put in the extra effort to make the version numbers less noisy and slightly more informative.


### Controlling versions in nbgv

`nbgv` provides an escape hatch: there are various ways you can force it to emit the version numbers you want. However, since this is not how the tool's author uses the tool, you are swimming against the flow. Some of the `nbgv` command line tool's convenience features become unusable. (We can't use its `prepare-release` command, for example.) It's also very easy to get things wrong.

For example, the `6.0.0` release of Rx.NET got tagged in GitHub as [6.0.0.1](https://github.com/dotnet/reactive/releases/tag/rxnet-v6.0.0.1) because `nbgv` behaved in a way I had not anticipated. It turns out that it really wants to add a _height_-based version element somewhere, so if you tell it the version is explicitly `6.0.0`, `nbgv` goes: oh great—you left the 4th part of the version unused, so I can put the height-based part in there.

(In `nbgv`, _height_ refers to the number of commits since you last explicitly set the version number in the configuration. This generates automatically increasing version numbers. We rely on this for preview numbering. E.g., the `20` in [7.0.0-preview.20](https://www.nuget.org/packages/System.Reactive/7.0.0-preview.20) was generated by `nbgv`'s height-based mechanism. We don't mind gaps in preview numbering because they are actually informative—you get some idea of how much changed between previews. That's not very interesting to most developers, but if you've chosen to live on the bleeding edge by consuming preview builds, you probably do want to know how much change to expect. The height is relevant in previews because normal semantic versioning assurances do not apply with previews, so you get less information from the main part of the version than you would if you weren't using previews.)

If you fully specify the version number to 4 places, you've left `nbgv` with nowhere to go, and it will then not emit a height-based version. However, if you want your release to have a 3-part version number, that doesn't help you. I want the release that shows up as `6.1.0` in NuGet to be tagged as `6.1.0` in GitHub, not `6.1.0.0` (or, as `nbgv` would choose, `6.1.0.1`.)

To avoid this, you essentially have to pander to `nbgv`: you need to let it think it has expressed the height. If you look at the Rx.NET 6.1 release, and in particular [this change to `version.json`](https://github.com/dotnet/reactive/commit/f4da16f15a3cde97f178396ea6e3489cc893651f), you'll see we actually set the version number to have just two parts:

```json
"version": "6.1",
```

that way, `nbgv` can think it is getting to emit the 3rd part of the version as a height-based commit, and then it's happy. But since we actually want this to be `6.1.0`, we use the slightly awkward escape hatch later in that file:

```json
"versionHeightOffset": -1
```

This tells `nbgv` that we want to adjust the height-based number. Since this commit makes a significant change to the `"version"`, `nbgv` considers this to be a _version base_, meaning that it becomes the starting point for height-based counting for `6.1.x` releases. But `nbgv` starts counting from `1`, a strange habit normally only exhibited by humans. Since we want the first version to be `6.1.0`, we set the height offset to `-1` (as otherwise, `nbgv` would choose `6.1.1`). We're happy because we get the version number we want; `nbgv` is happy because it thinks it got to determine one part of the version based on height.

Well, we're almost happy.

### Other File Versions

In addition to the NuGet and .NET Assembly versions, there are a couple of additional versions that `nbgv` sets:

* The `AssemblyFileVersion` (which also goes into the PE header, and is the version visible in File Properties in Windows File Explorer)
* The `AssemblyInformationalVersion`

In Rx 6.1.0, the latter was set to `6.1.0+f4da16f15a`. That's the first 10 digits of the SHA of the [commit for the `6.1.0` release](https://github.com/dotnet/reactive/commit/f4da16f15a3cde97f178396ea6e3489cc893651f). As mentioned earlier, that makes it easy to see which actual commit was used to build this version of the component. We are happy with this.

But the `AssemblyFileVersion` value came as a surprise when I was researching this issue in order finally to document properly how we do this. (This is the number that Windows File Explorer reports as the **File version**.) It is set to `6.1.0.62682` in the `6.1.0` release. I had never previously noticed that the file version had what appeared to be a random number in the final place. This was true as far back as v4.0.0 (although oddly, v5.0 was an exception: it has `5.0.0.1` here).

I was unable to find any documentation about this, but single-stepping through the source code for `nbgv` revealed the answer. If `nbgv` believes that it put the height in the 3rd part of the version, it invariably sets the 4th part of `AssemblyFileVersion` to a number derived from the commit hash. There does not seem to be any direct way to configure this behaviour; you can prevent it by stopping `nbgv` from putting the height in the 3rd part of the version, but if you do that, you then end up with the nominal version number for non-preview releases having 4 parts, leading to anomalies like when the `6.0.0` release got tagged as `6.0.0.1`.

I think this is to deal with situations like the one we get with our preview releases. Because we're not using `nbgv`'s preferred approach of giving every release a different patch/build number, our previews get versions like `7.0.0-preview.1`, `7.0.0-preview.15`, and so on. (We put the height after the prerelease tag. If we'd done what `nbgv` prefers these would be called `7.0.1-preview`, `7.0.15-preview` etc.) If it weren't for the commit-hash-derived final part of the `AssemblyFileVersion`, every single `7.0.0-preview.X` release would have a file version of `7.0.0.0`, and so would the final `7.0.0` release. But by putting a number based on the commit, each of these gets a version number that Win32 tooling can see is different. (The PE file version format requires four integers, so there's no way to express anything equivalent to a semver prerelease tag.) It's slightly sketchy because there aren't enough bits available in this version record to guarantee uniqueness; doubtless the author of `nbgv` would say that this is exactly why I should just let `nbgv` do what it wants, incrementing the `patch`/`build` version for every distinct build, but I've already discussed why we don't want to do that. Ideally I'd have that hash-based version for the previews, but fix it to be `7.0.0.0` in the final release, but the `nbgv` tool doesn't provide a way to do that.

## Decision

Regarding the commit-derived final portion of the `AssemblyFileVersion`, we just accept that while it looks odd, we can't change it, because any `nbgv` configuration change that would fix this would then causes other, worse problems.

Having got that out of the way, we move onto the main part of the decision in this ADR. We have different procedures for ongoing dev work vs release preparation.

### Day to day development (preview releases)

For day to day development, our `version.json` in `main` looks something like this:

```json
{
  "version": "7.0.0-preview.{height}",
  "publicReleaseRefSpec": [
    "^refs/heads/main$", // we release out of main
    "^refs/heads/rel/v\\d+\\.\\d+", // we also release branches starting with rel/vN.N
    "^refs/heads/rel/rx-v\\d+\\.\\d+" // we also release branches starting with rel/vN.N
  ],
  "nugetPackageVersion":{
    "semVer": 2
  }
}
```

The `version` spec there means that `nbgv`'s automatic height-based versioning gives us a stream of continuously incrementing version numbers without us needing to change the `version.json`. We've explicitly told it to put the height right at the end (and this is enough to satisfy `nbgv`'s conviction that it has to emit the height _somewhere_).

The release ref specs reflect our branch naming, so that `nbgv` knows whether this is intended to be a public release. For non-public releases, `nbgv` always adds a hash into the version to ensure uniqueness in the face of concurrent development across multiple branches. (You can see examples in the [Rx.NET preview package feed](https://dev.azure.com/dotnet/Rx.NET/_artifacts/feed/RxNet/NuGet/System.Reactive/versions/7.0.0-rc.1), such as `7.0.0-preview.20.g9ca7a5f26b`.) Without that, concurrent work in progress on multiple branches can defeat the unique version guarantee that commit counting is meant to provide. `nbgv` needs to know which branches are intended to integrate the changes from such concurrent development. A purely height-based version number relies on an assumption of linear progress, which is only really true for integration branches, not topic branches, so we have to tell `nbgv` which branches those are. It will consider any other branch to be non-public, and so it will incorporate the first 10 digits of the SHA in the version to ensure uniqueness. (For example, the builds produced for an open PR always include this SHA-based component.)

The effect is that when we merge to `main`, this produces a version with a name like `7.0.0-preview.20` (without the hash), which is suitable for publishing to NuGet.org should we decide to do so.

(This file also reflects the fact that going back a bit, we used not to put `rx` in the branch names. We could probably delete that now since we aren't ever going to go back and do bugfix for any version that old.)

The `"semVer": 2` tells `nbgv` that it doesn't need to support ancient tooling that wouldn't understand a version like `7.0.0-preview.15`.

### New major or minor releases

When we want to produce some `major.minor` for the first time (e.g., if `7.0.0` is out there and we intend to produce a `7.1.0`) we create a new branch where the name includes the major and minor version, e.g. `rel/rx-v7.1`.

**Note**: if you use `nbgv` as intended, you can use its `nbgv prepare-release` to do this. We don't, because it appears to presume that you will be able to push to `main`. `prepare-release` does four things: 1) it creates the new release branch, 2) it updates the `version.json` on that release branch, 3) it modifies the `version.json` on `main` to produce a higher version number, 4) it synthesizes a merge commit in a way that means if you do any hot fixes on your release branch and subsequently merge those back into `main`, that merge will not have the side effect of trying to apply the `version.json` changes made on the release branch to `main`. We have branch protection on `main`, so steps 3 and 4 don't really work for us, and it turns out we would also need to modify the changes it makes in step 2. So it's just easier for us not to use `prepare-release`.

We may decide not to go straight from `preview` to a final release. We avoid using `beta` because the NuGet package manager sorts prerelease tags alphabetically before looking at the number after the tag, meaning it thinks that `7.1.0-preview.20` is a newer release than `7.1.0-beta.21`. So we use `rc` instead. (`7.1.0-rc.1` sorts as a newer release than `7.1.0-preview.20`.) So we would have:

```json
  "version": "7.1.0-rc.{height}"
```

This produces the version `7.1.0-rc.1`. (Although we like our patch/build number to start from 0, as in `7.1.0`, we do actually prefer our pre-release tags to start from `1`, so we don't set any `versionHeightOffset` if using an `rc` prefix. The default gives us the initial version we want.)

If, later, an `rc.2` is needed (and even `rc.3` or more) each commit to this branch will produce a higher version than the last (but all with the same `7.0.0-rc.` prefix). No further updates to the `version.json` would be required at this stage.

Once we are ready for a non-preview (final) release, we make these changes to the `version.json`:

```json
  "version": "7.1",
  ...
  "versionHeightOffset": -1
```

This causes `nbgv` to put the height-based version in as the patch/build, and that `versionHeightOffset` causes that height to come out as 0, giving us the `7.1.0` version number we want.

Back on `main`, the `version.json` should be set to a higher `major.minor` than the newly-created release branch, e.g.:

```json
  "version": "7.2.0-preview.{height}",
```

or even:

```json
  "version": "8.0.0-preview.{height}",
```

What we don't want is for `main` to be producing either `7.1.0-preview.X` or `7.1.1-preview.X` releases, because once we create the `rel/rx-v7.1` release branch, any `7.1.X-preview.Y` release should come from there, not `main` (The reason for that is that if both branches end up producing versions of the same overall format, there's a possibility that two different commits on different branches could produce a release with the same version number, which is exactly what we're trying to avoid by using `nbgv`.)

Note that since `main` has branch protection, we update the `version.json` in a new feature branch created for that purpose and then merge it to `main` with a PR.

### New patch releases

If we have some existing `major.minor` and want a new version with the same `major.minor`, our goal is to set the 3rd version part (what semver calls the patch) to be one higher in the NuGet package. E.g., after `7.0.0`, we will want `7.0.1`. (This would typically only be done to fix a bug—semantic versioning requires a change to either the minor or major version.)

All we need to do is create a new commit containing the bugfix on the release branch (e.g. `rel/rx-v7.0`). (Our release branches have branch protection policies, so we can't do this directly. In practice you would create a new bugfix branch off the release branch, make the fix in there (possibly by cherry picking the commits that fix the bug from `main`) and then create a PR that you squash-commit into the release branch.) `nbgv` will increment the patch version because there's now an additional commit on the release branch, so if we had been on `7.0.0`, this will become `7.0.1`.

If, after squashing the PR to the release branch, we discover that the fix didn't work as intended, and the resulting libraries are unusable, we essentially have to burn that version number. (Hopefully you will have detected the problem while the PR from the bugfix branch to the release branch was still open. But mistakes happen.) Our build and release processes initially release packages to a dedicated Rx.NET NuGet feed, so things don't appear on NuGet.org until we've tested them, and have decided to publish them. However that Rx.NET feed is public, and anyone could download packages from there, so at that point a `7.0.1` release would be 'out there' even though it was never on NuGet. In this case we just have to live with our mistake and we go straight from `7.0.0` to `7.0.2` on NuGet.org. So we should do our best to make absolutely certain that the fix works as intended before merging the PR to the feature branch.

If the bugfix is intended to address a problem reported by a user, and we're not confident that we'll get it right in one shot, we might want to make a preview release available on NuGet.org. This provides a way for the developer who reported the problem to test the fix before we push it out. In this case we would modify the `version.json` on the release branch to reinstate a preview tag. At this point we would use:

```json
  "version": "7.0.1-preview.{height}",
```

We shouldn't need to set `versionHeightOffset` here, because as discussed earlier, we should never be producing `7.0.1-preview.X` releases from `main`. (`main`'s `version.json` should now be on `7.1` or `8.0`, or later.) But if for some reason that mistake has been made, then we'd need to first fix the `version.json` on `main`, and then pick a `versionHeightOffset` that gives us a high enough starting point to guarantee unique versions.

Once the fix has been validated, we will want to produce a non-preview `7.0.1` release. This will mean modifying the `version.json` again. We will revert to `"version": "7.0"`, and will set `versionHeightOffset` to whatever value is required to get the version number we want. (If we want `7.0.1`, then we want `"versionHeightOffset": 0`. For `7.0.2` we'd want `"versionHeightOffset": 1`, and so on. Remember, `nbgv` considers a commit that changes `"version"` to be a _version base_ and considers the commit height to be `1` at that point, so `versionHeightOffset` needs to be set to make up the difference.)

## Consequences

During day to day development, every time a PR merges to `main` we automatically get a new `major.minor.0-preview.X` release which has a unique version that is higher than the previous preview. (This automatically goes to the Rx.NET NuGet feed, and we can optionally choose to push to NuGet.org.) So we don't have to think about version numbers in the preview phase.

For non-preview releases, we are able to control the version numbers, keeping in line with historical practices. (I.e., we can have `7.0.0`, `7.0.1`, `7.0.2`, `7.1.0`, `7.1.1`, `8.0.0` and so on.) In cases where a patch release contains a simple bugfix where we don't need to make a preview available on NuGet.org first, we can simply squash-commit a PR containing the fix into the release branch, and the patch/build version number will automatically increment.

A downside is that we are not using `nbgv` quite as its author intended. This creates a risk that we might accidentally produce two different releases that have the same version number. (E.g. if, after release `7.0.2`, we produce a `7.0.3-preview.1` to enable a user to validate that our change fixes their problem, there's a risk at the point where we're ready to do the final release. The procedure at that point is to set the `versionHeightOffset` to force `nbgv` to produce the `7.0.3` version that we want, but if we get that manual edit wrong, we could accidentally produce another `7.0.2` release that is different from the existing `7.0.2` release. It would be impossible to publish this to either the private Rx.NET feed or to NuGet.org: both would recognize the mistake and block publication. So it's not a huge problem, but we would have created and signed this bogus set of `7.0.2` packages, and they'd be visible in the build output. This is annoyingly messy, but since the NuGet feeds would reject any attempt to publish, there are no serious consequences.) The `nbgv` tool is trying to protect us from that, but our decision to overrule its versioning strategy means the onus is now on us not to mess up.

There's also a risk that because we're doing non-standard things, future update to `nbgv` could disrupt us, and we might need to change our practices. (It has already created some friction—it took a lot of experimentation to establish the practices described in this document. And prior to writing them up in this ADR, it was hard to discover exactly what we did and why for previous releases, causing the occasional mishap like the unwanted 4th release number component in GitHub release for `6.0.0`. However, by documenting what we do and why, the hope is that that particular friction is now in the past.)