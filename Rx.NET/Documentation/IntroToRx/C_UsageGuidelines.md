# Appendix C: Usage guidelines

This is a list of quick guidelines intended to help you when writing Rx queries.

- Members that return a sequence should never return null. This applies to `IEnumerable<T>` and `IObservable<T>` sequences. Return an empty sequence instead.
- Dispose of subscriptions only if you need to unsubscribe from them early.
- Always provide an `OnError` handler.
- Avoid blocking operators such as `First`, `FirstOrDefault`, `Last`, `LastOrDefault`, `Single`, `SingleOrDefault` and `ForEach`.; use the non-blocking alternative such as `FirstAsync`.
- Avoid switching back and forth between `IObservable<T>` and `IEnumerable<T>`
- Favour lazy evaluation over eager evaluation.
- Break large queries up into parts. Key indicators of a large query:
    1. nesting
    2. over 10 lines of query expression syntax
    3. using the `into` keyword
- Name your observables well, i.e. avoid using variable names like `query`, `q`, `xs`, `ys`, `subject` etc.
- Avoid creating side effects. If you really can't avoid it, don't bury the side effects in callbacks for operators designed to be use functionally such as `Select` or `Where`. Be explicit by using the `Do` operator.
- Where possible, prefer `Observable.Create` to subjects as a means of defining new Rx sources.
- Avoid creating your own implementations of the `IObservable<T>` interface. Use `Observable.Create` (or subjects if you really need to).
- Avoid creating your own implementations of the `IObserver<T>` interface. Favour using the `Subscribe` extension method overloads instead.
- The application should define the concurrency model.
    - If you need to schedule deferred work, use schedulers
    - The `SubscribeOn` and `ObserveOn` operators should always be right before a `Subscribe` method. (So don't sandwich it, e.g. `source.SubscribeOn(s).Where(x => x.Foo)`.)