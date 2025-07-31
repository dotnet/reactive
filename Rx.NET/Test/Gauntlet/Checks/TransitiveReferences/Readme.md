# Problems with Transitive References to Rx.NET

The problems that the projects in this check for folder don't relate directly to any single issue in Rx.NET today. Instead, the goal is to check whether potential solutions to the [bloat](../Bloat/) problem introduce new problems in certain scenarios.

The current version of Rx.NET (6.0.1) has major problems with sometimes-unwanted `Microsoft.WindowsDesktop.App` framework dependencies. It needs to be possible for applications to use Rx.NET either directly, or indirectly through a transitive reference, without being forced into taking an unwanted framework dependency. (With Rx 6.0.1, this happens to any application with a Windows-specific TFM specifying `windows10.0.19041` or later, and the only available workarounds don't work in all situations.)

Some of the simpler proposals for fixing this fall short when transitive dependencies come into the picture, because the application author can't control the way in which Rx.NET is used. Some proposals do not prevent the unwanted framework dependencies. Some create new problems; in some cases these new problems can prevent application developers from upgrading to the latest version of the components they use. In the worst case, developers who had no idea that their application had an implicit Rx.NET dependency can find that they are unable to upgrade a component they're using, or that they're unable to start using a new component.

The goal of the checks performed by the projects in this folder is to ensure that the chosen design for Rx.NET 7.0 doesn't cause these problems in scenarios where Rx.NET is used indirectly.

For example, a candidate future Rx.NET release could relegated `System.Reactive` to being a legacy facade, introducing a new main package (e.g. `System.Reactive.Net`). With this design it becomes possible for an application to end up with references to two versions of Rx.NET simultaneously. This wouldn't typically happen with direct references. (It could, but if an application developer deliberately adds references to `System.Reactive` v6 and `System.Reactive.Net` v7, then they can't expect too much sympathy.)

Where we need to be very careful is when the application has acquired dependencies indirectly. An application may well end up using multiple libraries with references to various versions of Rx.NET. That can happen with the developer being unaware they were using Rx.NET at all. The checks in this folder all inspect scenarios involving transitive dependencies, or a mixture of direct and indirect (transitive) references.


## Problems Addressed

In all cases, we want to examine what happens when something changes, so each scenario we check compares two projects. There's a _before_ version of the project, intended to reflect the state some application might be in today. And the _after_ version makes some change (either to the application, or to some component the application depends on) that changes its Rx.NET references in some way.

The exact problems we want to check for fall into two distinct sets, which are described in the following sections.


### Application with Transitive Rx.NET v6 Reference

For an application with a transitive dependency on `System.Reactive` 6.0.1, there are three questions we want to ask about any candidate packaging approach for Rx.NET 7 in relation to this issue:

1. If the application uses self-contained deployment (possibly via AoT), and isn't using any UI-framework-specific Rx.NET features in practice, and is consequently suffering from the [bloat](../Bloat/) problem, can the application developer fix that by upgrading to a newer version of Rx.NET?
2. Suppose the application is **not** using self-contained deployment (including AoT), and thus wasn't encountering the bloat problem. Also suppose that it isn't using any UI-framework-specific Rx.NET features in practice. If the developer upgrades to the latest version of Rx.NET, do packaging changes cause new problems for that scenario?
3. Suppose the application **is** using either WPF or Windows Forms Rx.NET features. In this case the 'bloat' isn't bloat: the application does actually need WPF and Windows Forms assemblies. (WPF and Windows Forms have interop features enabling each to host instances of the other framework's UI elements, so a dependency on either effectively implies a dependency on both.) This is the scenario that `System.Reactive` v6's `net6.0-windows10.0.19041` target was designed for: the UI-framework-specific features you need are automatically available, and your app will acquire the necessary `Microsoft.WindowsDesktop.App` framework without needing an explicit `<FrameworkReference>`. Will upgrading to the latest Rx.NET package or packages break applications that rely on this, and if so, how easy will it be to make things work like they did before? (Will developers have a hard time discovering what they need to do to unbreak their app when they upgrade Rx.NET, or when they get an unexpected Rx.NET upgrade as a side effect of updating some other component?)

It's important to note that although 1 has caused significant pain (leading to some projects to drop Rx.NET completely) it only affects applications using self-contained deployment (including AoT compilation), and which are built with a Windows-only TFM, but which aren't using WPF or Windows Forms. Non-OS-specific apps don't have any problems, framework-dependent deployment doesn't suffer from the bloat problems, and applications that are using WPF or Windows Forms really do need that framework reference. Scenarios 2 and 3 above work just fine today. So we need to be very careful to ensure that in enabling 1, we don't accidentally create a lot of new problems for scenarios 2 or 3. (We don't want a repeat of how the fix for the [plug-in problems](../PlugIns/) caused new problems that were worse than the problem being fixed.)

Framework-dependent deployment is the default, and if we break that, we're likely to cause pain for many more people in scenario 2 than were having problems in scenario 1. Solutions that address 1 have the potential to resemble the attempts to solve the original plug-in problem in Rx 3.1: the fix worked as intended but created new problems for people who weren't writing plug-ins. And since the majority of people using Rx.NET weren't writing plug-ins, the amount of new pain caused was much greated than the amount of existing pain solved.

This kind of net-more-pain outcome happens when a very common scenario is _currently_ fine, but becomes problematic as a result of a fix for a more specialized scenario. The fact that the common scenario was fine before a change makes it all too easy to miss the fact that a change targeting a narrow problem will make things worse in the common scenario. The new problem isn't even on the radar because it didn't exist until after the fix was created.

For the purposes of the bloat issue, we need to keep in mind that the most common scenario is framework-dependent deployment, and the specialized scenario is self-contained deployment. Ideally, any changes we make to solve the bloat problem for self-contained deployment would not create new problems for framework-dependent deployment, or for applications that actually want that framework dependency. It might not be possible to achieve this, in which case we need to consider the balance of gains and losses. We do need to be able to solve 1. The choice of solution there may be influenced by how well it deals with 2 and 3.


### Transitively Acquired Rx.NET Upgrade

Another important scenario to consider is when an application developer hasn't chosen to upgrade to the latest version of Rx.NET (e.g., because the 'bloat' issue didn't cause any problems for their scenario, and they are content with how things work right now), but ends up getting that update anyway because of a transitive reference. If the application has just a single transitive dependendency on Rx.NET, and doesn't directly use Rx.NET itself, the application developer typically won't even notice the change. But in cases where there are two dependency chains referring to Rx.NET, we can end up with two different Rx.NET versions. Some solutions to the bloat problem can have problems with this.

For example, consider an application which currently has this tree of package dependencies:

* `MyApplication`
  * `LibA` v1.0.0
    * `System.Reactive` v6.0.1
  * `LibB` v1.0.0
    * `System.Reactive` v6.0.1

Now suppose that a new version of `LibA` is released, let's say `LibA` v2.0.0, and that it now uses the latest version of Rx. Now we have:

* `MyApplication`
  * `LibA` v1.0.0
    * Latest Rx.NET
  * `LibB` v1.0.0
    * `System.Reactive` v6.0.1

Some proposed designs for solving the bloat problem retain `System.Reactive` as the primary Rx.NET package, in which case, that "Latest Rx.NET" would be a reference to `System.Reactive` v7.0.0. The application build would determine that although `LibB` asked for an older version, the `LibA`'s newer version requirement 'wins', and so both will get 7.0.0. However, we are also considering a new packaging design in which `System.Reactive` becomes a legacy facade. In that case, this 'after' version would in practice look like this:

* `MyApplication`
  * `LibA` v1.0.0
    * `System.Reactive.Net` v7.0.0
  * `LibB` v1.0.0
    * `System.Reactive` v6.0.1

This creates the interesting situation in which we've got two different versions of Rx.NET available simultaneously. If the application doesn't use Rx.NET itself, this is likely not to be a problem. However if it does, it will now get compiler errors complaining that Rx.NET's extension methods (e.g. the `Select` extension methods available for `IObservable<T>`) are all ambiguous because they are all defined in each of two different assemblies.

So for an application that had previously happily been using Rx.NET through a transitive reference, but which now finds itself with two transitive references to Rx.NET where one is v6, and one is newer, we need to ask the following questions:

1. Will the application build without error if it does not use Rx.NET directly?
2. Will the application build without error if it does use Rx.NET directly?
3. Will the components that depend on Rx.NET find the types they need at runtime?

The most likely reason for 1 to fail would be if the NuGet package selection step decided it couldn't resolve conflicting package version requests.

Designs that relegate `System.Reactive` to a facade make 2 possible because if you have a reference to the new component, and also a reference to an old version of `System.Reactive`, you end up with two complete Rx.NET implementations available. (It should be possible to resolve this by adding a `System.Reactive` v7.0 package reference to the application. This means the application will now use the facade version of `System.Reactive` which effectively unifies it all back to a single implementation because it has type forwarders saying that all the types that used to be in `System.Reactive` now live in `System.Reactive.Net`.)

 In cases where some of the code involved is actually using a UI framework, we get some new challenges. Depending on the exact design approach taken, the following might occur:

 * The application might previously have been relying on a transitive `Microsoft.WindowsDesktop.App` framework reference, resulting in build errors arising from basic WPF and/or Windows Forms types no longer being available (which the app can fix by adding `<UseWpf>` and/or `<UseWindowsForms>` in the `csproj`)
 * The application might have been using UI-specific Rx.NET types, and these might no longer be publicly visible through `System.Reactive` (which the app can fix by adding a package reference to, say, `System.Reactive.For.Wpf`)
 * `LibB` might have been using a UI-framework-specific Rx.NET type, and we might get a runtime `MissingMethodException` or `MissingTypeException`

The first two problems are undesirable but aren't necessarily showstoppers. The third one isn't something an application can work around, so if that occurs then we effectively have a bug in Rx.NET. (Applications can find themselves forced into this two-transient-dependencies-with-different-versions situation. If it causes an unfixable problem, the only options left are to stop depending on at least one of these components, or to wait until both support the latest Rx.NET.) So designs causing that final problem are unacceptable.


## Problematic Scenarios

The key aspect of this check is that we have an application using a library using some version of Rx.NET that is known to cause bloat, e.g.:

* `TheApp`
  * `SomeLibUsingRx`
    * `System.Reactive` 6.0.1

However, there are some variations on this theme.

### Consuming Application Dimensions

Before we get to the way any particular future version of Rx.NET might be packaged, there are two dimensions in this scenario creating 6 variations.

#### Direct Reference to Rx.NET, or Only Transitive?

In the _before_ case, Does the application also use Rx.NET itself, or does only the library use it? That is, in examples where the application itself adds a reference to the new Rx.NET, do we have this scenario:

<table>
<thead>
<td><b>Before</b></td><td><b>After</b></td>
</thead>
<tbody>
<tr>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li><code>SomeLibUsingRx</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li>New Rx.NET</li>
      <li><code>SomeLibUsingRx</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
</tr>
</table>

Or it could even look like this, in which the app initially is using the new Rx.NET, but later acquires a transitive dependency on the old one:

<table>
<thead>
<td><b>Before</b></td><td><b>After</b></td>
</thead>
<tbody>
<tr>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li><code>System.Reactive</code> 6.0.1</li>
      <li><code>SomeLibUsingRx</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li>New Rx.NET</li>
      <li><code>SomeLibUsingRx</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
</tr>
</table>



or perhaps the app was already using the latest Rx.NET, but then later acquires a dependency on a library with a dependency on an older one, in which case we'd have this scenario:

or this one:

<table>
<thead>
<td><b>Before</b></td><td><b>After</b></td>
</thead>
<tbody>
<tr>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li>New Rx.NET</li>
    </ul>
  </li>
</ul>
</td>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li>New Rx.NET</li>
      <li><code>SomeLibUsingRx</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
</tr>
</table>


#### Use of UI-framework-specific features in `System.Reactive`?

Does the library use any UI-framework-specific features in `System.Reactive`? For example, do we have this scenario:

<table>
<thead>
<td><b>Before</b></td><td><b>After</b></td>
</thead>
<tbody>
<tr>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li><code>LibUsingOnlyNonUiFrameworkRxFeatures</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li>New Rx.NET</li>
      <li><code>LibUsingOnlyNonUiFrameworkRxFeatures</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
</tr>
</table>

or might it be this (because the application developer knows they need the UI-framework-specific Rx.NET features, and knows that these live in separate packages in the new Rx.NET):

<table>
<thead>
<td><b>Before</b></td><td><b>After</b></td>
</thead>
<tbody>
<tr>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li><code>LibUsingUiFrameworkRxFeatures</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li>New Rx.NET</li>
      <li>New UI-framework-specific Rx package reference(s)</li>
      <li><code>LibUsingUiFrameworkRxFeatures</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
</tr>
</table>

There's also a variation in which the library does use UI-framework-specific features, but the application happens not to exercise any of the code paths in `LibUsingUiFrameworkRxFeatures` that do so. In this case we are also interested in what happens with this:

<table>
<thead>
<td><b>Before</b></td><td><b>After</b></td>
</thead>
<tbody>
<tr>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li><code>LibUsingUiFrameworkRxFeatures</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
<td>
<ul>
  <li><code>TheApp</code>
    <ul>
      <li>New Rx.NET</li>
      <li><code>LibUsingUiFrameworkRxFeatures</code>
        <ul>
          <li><code>System.Reactive</code> 6.0.1</li>
        </ul>
      </li>
    </ul>
  </li>
</ul>
</td>
</tr>
</table>



#### New Rx.NET Reference Transitive or Direct?

The preceding scenarios assume that the upgrade to Rx.NET is done by a change to the application's `csproj`. However, we're also interested in the behaviour when the upgrade occurs due to the addition of, or a change to another package reference. So instead of this:


| Before | After |
|--|--|
| <ul><li>`TheApp`<ul><li>`SomeLibUsingRx`<ul><li>`System.Reactive` 6.0.1</li></ul></li></ul></li></ul> | <ul><li>`TheApp`<ul><li>New Rx.NET</li><li>`SomeLibUsingRx`<ul><li>`System.Reactive` 6.0.1</li></ul></li></ul></li></ul> |

we might have this:

| Before | After |
|--|--|
| <ul><li>`TheApp`<ul><li>`SomeLibUsingRx`<ul><li>`System.Reactive` 6.0.1</li></ul></li></ul></li><li>`AnotherLibUsingRx` 1.0.0<ul><li>`System.Reactive` 6.0.1</li></ul></li></ul> | <ul><li>`TheApp`<ul><li>`SomeLibUsingRx`<ul><li>`System.Reactive` 6.0.1</li></ul></li></ul></li><li>`AnotherLibUsingRx` 2.0.0<ul>New Rx.NET</ul></li></ul> |

or even this:

| Before | After |
|--|--|
| <ul><li>`TheApp`<ul><li>`SomeLibUsingRx`<ul><li>`System.Reactive` 6.0.1</li></ul></li></ul></li></ul> | <ul><li>`TheApp`<ul><li>`SomeLibUsingRx`<ul><li>`System.Reactive` 6.0.1</li></ul></li></ul></li><li>`AnotherLibUsingRx`<ul>New Rx.NET</ul></li></ul> |

### Future Rx.NET Packaging Choices

At the time of writing this we are still trying to decide on the packaging changes in Rx.NET 7. Consequently, as well as the dimensions created by the possible application scenario variations just described, there are more dimensions that arise from the possible choices there:

* Is `System.Reactive` still the main component, or is it a legacy facade?
* Does `System.Reactive` continue to make the UI-framework-specific features visible at compile time, or does it relegate them to being available only at runtime in assemblies in the `lib` folder while removing them from compile-time visibility by supplying reference assemblies under `ref` with a reduced API surface area?
* Does `System.Reactive` still automatically bring in the `Microsoft.WindowsDesktop.App` framework on Windows-specific TFMs?

The next sections describe the implications of these Rx.NET packaging choices for the application scenarios described above.

#### Is `System.Reactive` still the main component or a legacy facade?

If `System.Reactive` continues to be the main component, then an application that has acquired a transitive dependency on an older version of `System.Reactive` can upgrade thus:

* `TheApp`
  * `SomeLibUsingRx`
    * `System.Reactive` 6.0.1
  * `System.Reactive` 7.0.0
  * If `SomeLibUsingRx` uses UI-framework-specific features, it's possible that one or more of the following will also be needed:
    * `UseWPF` or `UseWindowsForms` in the project file
    * References to one or more UI-framework-specific Rx.NET packages

If `System.Reactive` continues to be the main component, the application scenario where the application also uses Rx.NET directly looks exactly the same.

If `System.Reactive` is relegated to being a legacy facade, then the application scenario where Rx.NET is used only transitively, but the application upgrades to avoid bloat, looks like this:

* `TheApp`
  * `SomeLibUsingRx`
    * `System.Reactive` 6.0.1
  * `System.Reactive` 7.0.0 (which will add a transitive reference to `System.Reactive.Net` 7.0.0)
  * If `SomeLibUsingRx` uses UI-framework-specific features, it's possible that one or more of the following will also be needed:
    * `UseWPF` or `UseWindowsForms` in the project file
    * References to one or more UI-framework-specific Rx.NET packages


The application scenario where the application also uses Rx.NET directly would typically look like this:

* `TheApp`
  * `SomeLibUsingRx`
    * `System.Reactive` 6.0.1
  * `System.Reactive.Net` 7.0.0
  * `System.Reactive` 7.0.0
  * If `SomeLibUsingRx` uses UI-framework-specific features, it's possible that one or more of the following will also be needed (and both _will_ be required if the app uses Rx.NET UI framework features directly):
    * `UseWPF` or `UseWindowsForms` in the project file
    * References to one or more UI-framework-specific Rx.NET packages

(The difference here is that the app references `System.Reactive.Net` directly. It wouldn't strictly have to, because that is acquired transitively through `System.Reactive` 7.0.0, but an application author may wish to express the intent to use the latest Rx.NET directly with a package reference to the new main Rx package.)

The second scenario looks different depending on whether Rx.NET 7 continues making `System.Reactive` the main component, or relegates that to a legacy facade. In the former case, it's:

* `TheApp`
  * `LibUsingRx`
    * `System.Reactive` 6.0.1
  * `System.Reactive` 7.0.0

but in the latter case, an application might end up in this state:

* `TheApp`
  * `System.Reactive.Net` 7.0.0
  * `LibUsingRx`
    * `System.Reactive` 6.0.1

This is particularly likely if they initially didn't use `LibUsingRx` and were only using Rx.NET 7 directly. When they add `LibUsingRx`, they will find themselves in this situation where they get the older version of `System.Reactive` as a transitive dependency, and now they have references to two different Rx.NET versions. They will need to do this to unify things:

* `TheApp`
  * `System.Reactive.Net` 7.0.0
  * `System.Reactive` 7.0.0
  * `LibUsingRx`
    * `System.Reactive` 6.0.1



Issues:

* Discoverability: how does the app author know they need to upgrade to `System.Reactive` v7 as well as continuing to use `System.Reactive.Net` v7?
* Long-run: if the `LibUsingRx` does eventually upgrade to Rx.NET 7, is the app now at a disadvantage because of its ongoing `System.Reactive` dependency, and can it discover when that's no longer needed?
* 

## Command Line Behaviour

This is the first Rx Gauntlet check that needs to deal with two versions of Rx.NET at a time. Checks that use the common `RxSourceSettings` all inspect behaviour when an application is using exactly one version of Rx.NET. (This is true even for the plug-in tests: we load two plug-ins, but the failure scenarios for that were always when two plug-ins used the same Rx.NET version but different TFMs.) In those cases, we were asking whether a particular version of Rx.NET exhibits a particular behaviour.

This check is slightly different. We're not trying to establish whether given versions of Rx.NET exhibit bloat—we already have a separate check for that. Instead we want to know how potential new versions of Rx.NET when an application has a transitive dependency on an older version of Rx.NET that is already known to cause bloat. So the 'old' version in these checks is fixed to be `System.Reactive` 6.0.1 (actually the current version at the time of writing this).


## Detecting Problems

This folder contains a [`Transitive.App`](Transitive.App/) project that gets built in numerous (hundreds of) different ways. The [`CheckTransitiveFrameworkReference`](CheckTransitiveFrameworkReference/) project creates all of these variations, and it does so in pairs: there is a _before_ configuration representing a state some application might already be in in which it is not encountering problems, and an _after_ configuration in which we introduce some change in the references.

Things we check:

* did we get any compilation errors?
* did we get any compilation warnings?
* do we get runtime failures (e.g., we know some designs result in exceptions due to types or methods not being found)?
* are we expecting Rx.NET itself to emit diagnostics (e.g., hints telling you what to do), and if so did we see them?
* did we get WPF and/or Windows Forms assemblies in the output (which we call 'bloat' in cases where the application developer does not want this)?
