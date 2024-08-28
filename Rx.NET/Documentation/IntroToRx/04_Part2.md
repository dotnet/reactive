# PART 2 - From Events to Insights 

We live in an age where data is being created, stored, and distributed at a phenomenal rate. Consuming this data can be overwhelming, like trying to drink directly from a fire hose. We need the ability to identify the important data, meaning we need ways to determine what is and is not relevant. We need to take groups of data and process them collectively to discover patterns or other information that might not be apparent from any individual raw input. Users, customers and managers need to do this with more data than ever before, while still delivering higher performance and more useful outputs.

Rx provides some powerful mechanisms for extracting meaningful insights from raw data streams. This is one of the main reasons for representing information as `IObservable<T>` streams in the first place. The preceding chapter showed how to create an observable sequence, so now we will look at how to exploit the power this has unlocked using the the various Rx methods that can process and transform an observable sequence. 

Rx supports most of the standard LINQ operators. It also defines numerous additional operators. These fall broadly into categories, and each of the following chapters tackles one category:

* [Filtering](05_Filtering.md)
* [Transformation](06_Transformation.md)
* [Aggregation](07_Aggregation.md)
* [Partitioning](08_Partitioning.md)
* [Combination](09_CombiningSequences.md)