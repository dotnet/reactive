---
name: Issue report for Ix
about: Creates an issue report regarding a bug, question or feature request for Ix.NET
title: ''
labels: '[area] Ix'
assignees: ''
---
Hello and thank you for using dotnet/reactive. Please select a category and detail your issue by answering the questions there:

#### Bug

Despite our best efforts, bugs can slip into releases or corner cases forgotten about. We will try our best to remedy the situation
and/or provide workarounds. Note that certain (odd) behaviors are by design and as such are not considered bugs.

> Which subcomponent library (Ix, Async.Ix)?

> Which library version?

> What are the platform(s), environment(s) and related component version(s)?

> What is the use case or problem?

> What is the expected outcome?

> What is the actual outcome?

> What is the stacktrace of the exception(s) if any?

> Do you have a code snippet or project that reproduces the problem?

#### Question

Before you ask us a question, please note that dotnet/reactive is maintained by a handful of dedicated people voluntarily and in their free time.
You could help us tremendously by first searching for some keywords related to your question with your favorite search engine,
our [issue list](https://github.com/dotnet/reactive/issues) or the related [stackoverflow.com](https://stackoverflow.com) keywords (such as
[ienumerable](https://stackoverflow.com/questions/tagged/ienumerable) and
[iasyncenumerable](https://stackoverflow.com/questions/tagged/iasyncenumerable)
). Please also consider asking questions, such as **"How do I do X?"** or **"Where can I find Y?"**, under one of these tags on *StackOverflow* instead.

In case you have not found an answer or your question is not really suited for *StackOverflow*, you are welcome to ask it here.

> What is the context of your question or problem?

> What is the question or problem you try to solve?

> What were the (original) requirements you tried to solve?

> What have you tried so far, what code have you written so far?

#### Feature request

The dotnet/reactive hosts fundamental components and operators for `IObservable`, `IEnumerable` and `IAsyncEnumerable`, and as such, to be and
to stay as a dependable family of libraries, we have to carefully consider what new features to include. Therefore, before asking for a new component,
operator or feature, please consider the following cases and resolutions first:

a) **New source factory method.** Static factory methods creating an instance of the types mentioned above can live in any class in any library
without too much inconvenience. Please consider hosting such factory methods outside dotnet/reactive.

b) **New instance method/operator.** The .NET world features extension methods which gives the flexibility to have fluent API expansions in
your local project or any third party library. Please consider hosting such methods outside dotnet/reactive too.

c) **Support for or bridge to other 1st or 3rd party components.** These are considered on a specific case-by-case basis but generally,
please consider hosting such support/bridge code outside dotnet/reactive.

d) **New reactive/interactive base type or concept.** Components requiring changes or introduction of new protocols (for example, flow control,
item lifecycle, async) are generally better suited for their own 3rd party library hosting and interoperation should be provided, via the standard
types mentioned above, there.

e) **Behavior change on an existing operator.** Such changes involve a lot of risks for existing users, therefore, usually, it is better to introduce
a completely new component or operator, for which see points a)..d) again.

Considering the points above, please describe the feature or behavior you would like dotnet/reactive included:

> Which subcomponent library (Ix, Async.Ix)?

> Which next library version (i.e., patch, minor or major)?

> What are the platform(s), environment(s) and related component version(s)?

> How commonly is this feature needed (one project, several projects, company-wide, global)?

> Please describe the feature.