# ~~Hyperyx.Core.Store~~ Redux.NET: Reactive libraries for .NET Core

`dotnet add package ~~Hyperyx.Core.Store --version 1.0.2~~`

This is a simple framework allowing you to easily manage a reactive state in your dotnet app. The above invocation installs the necessary reactive libraries. This framework is inspired by ngrx, a client-side framework providing reactive libraries for Angular. So just like in ngrx you can use reducers, effects and selectors to manage your state store.

This framework has many benefits:
- Promotes the SOLID development principles.
- Introduces a modular way to build new features.
- Removes unnecessary dependencies to other code.

## Getting Started

It's easy, just follow a few steps:
- Define an interface for your state (like: [IAppState](https://github.com/sujenk/Hyperyx-Core-Store.Demo/blob/main/Core/Store/AppState.Interface.cs))
- Create an object for your state (like: [AppState](https://github.com/sujenk/Hyperyx-Core-Store.Demo/blob/main/Core/Store/AppState.cs))
- Create actions to dispatch your payload (like: [SearchResultsActions](https://github.com/sujenk/Hyperyx-Core-Store.Demo/blob/main/Core/Store/Search/SearchResults/SearchResults.Actions.cs))
- Create a reducer to manage your state (like: [AppStateReducer](https://github.com/sujenk/Hyperyx-Core-Store.Demo/blob/main/Core/Store/AppState.Reducer.cs))
- Optionally add your side effects for server operations (like: [SearchResultsEffect](https://github.com/sujenk/Hyperyx-Core-Store.Demo/blob/main/Core/Store/Search/SearchResults/SearchResults.Effect.cs))
- Optionally add selectors for cleaner retrieval of state slices (like: [SelectSearch](https://github.com/sujenk/Hyperyx-Core-Store.Demo/blob/main/Core/Store/Search/SearchState.Selectors.cs))
- Now you are good to go! You can dispatch actions and select state slices (like: [HomeController](https://github.com/sujenk/Hyperyx-Core-Store.Demo/blob/main/Controllers/HomeController.cs))

## Demo

The [Hyperyx-Core-Store.Demo](https://github.com/sujenk/Hyperyx-Core-Store.Demo) is a showcase on how this framework can be implemented.
