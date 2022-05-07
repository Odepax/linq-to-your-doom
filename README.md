Linq to Your Doom
====

![NuGet](https://img.shields.io/nuget/v/LinqToYourDoom?label=NuGet&logo=nuget)
![Tests](./tests-badge.svg)

Installation
----

Install [LinqToYourDoom](https://www.nuget.org/packages/LinqToYourDoom/) from NuGet.

Non-Exhaustive Overview
----

### `IAssignable`

`IAssignable` was thought of as a deep counterpart to JavaScript's [`Object.assign()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/assign). An _assignable_ data object can be _assigned_ from another object, which has for effect to copy over the values of its fields to the receiver.

The extension comes in with a bunch of extension methods to help assigning strings, equatable values, and 1-depth dictionaries.

Example:

```cs
sealed class Cat : IAssignable<Cat, Cat> {
   public int Id { get; init; }
   public string Name { get; set; } = string.Empty;
   public double PurrPower { get; set; }
   public List<Cat> Friends { get; } = new();

   public Cat Assign(Cat other, ConflictHandling conflictHandling = default) {
      Name = Name.Assign(other.Name, StringComparison.InvariantCulture, conflictHandling, string.Concat, nameof(Name));
      PurrPower = PurrPower.Assign(other.PurrPower, conflictHandling, MathD.Avg, nameof(PurrPower));

      Friends
         .ToDictionary(it => it.Id)
         .Assign(other.Friends.SelectKeyed(it => it.Id), conflictHandling, nameof(Friends))
         .Values
         .SetTo(Friends);

      return this;
   }
}

var a = new Cat { Id = 1, Name = "A", PurrPower = 0.10, Friends = {
   new Cat { Id = 3, Name = "C" }
} };

var b = new Cat { Id = 2, Name = "B", PurrPower = 0.20, Friends = {
   new Cat { Id = 3, PurrPower = 0.30, Friends = {
      new Cat { Id = 4, Name = "D", PurrPower = 0.40 }
   } }
} };

a.Assign(b);
// a is now Cat { Id = 1, Name = "B", PurrPower = 0.20, Friends = [
//    Cat { Id = 3, Name = "C", PurrPower = 0.30, Friends = [
//       Cat { Id = 4, Name = "D", PurrPower = 0.40 }
//    ] }
// ] };
```

### `TypedDictionary`

`TypeDictionary` is a proxy to a standard dictionary, allowing to retrieve values while also specifying a narrower return type.

```cs
var dictionary = new Dictionary<TKey, object>();

dictionary.AsTyped().Get<int>(key); // Get or throw.
dictionary.AsTyped().TryGet<string>(key, out var value);
dictionary.AsTyped().GetOrDefault<float>(key, defaultValue);
dictionary.AsTyped().GetOrSet<Cat>(key, defaultValue);
```

### Cloneable Interfaces

Typed cloning, with explicit _deep_ or _shallow_ sematics lacked by `ICloneable`:

```cs
interface IShallowCloneable<out TSelf> {
   TSelf ShallowClone();
}

interface IDeepCloneable<out TSelf> {
   TSelf DeepClone(uint depth);
}
```

### `IEqualityComparer<T>` from Delegates

```cs
EqualityComparer.From<T>(Func<T?, T?, bool> equals);
EqualityComparer.From<T>(Func<T?, T?, bool> equals, Func<T, int> getHashCode);
EqualityComparer.From<T>(                           Func<T, int> getHashCode);
```

### Useless Extensions on `IEnumerable<T>`

```cs
enumerableOfEnumerables.SelectMany(); // IEnumerable<IEnumerable<T>> => IEnumerable<T>
enumerable.SelectIndexed(); // IEnumerable<T> => IEnumerable<(int Index, T Value)>
enumerable.SelectKeyed(it => it.Key); // IEnumerable<T> => IEnumerable<KeyValuePair<TKey, T>>
enumerable.SelectMap(it => it.Value); // IEnumerable<T> => IEnumerable<(T Item, TMapped MappedValue)>
enumerable.SelectDefault(i => new(i)); // IEnumerable<T?> => IEnumerable<T>

enumerable.Each(it => LazyAction(it));
enumerable.TrySelect((T it, out float value) => it.TryGetValue("B", out value));
enumerable.TrySelect(it => (it.HasValue, it.GetValueOrThrow()));

enumerable.DefaultIfEmpty(fallbackEnumerable) {

enumerable.WhereNotNull();
enumerableOfString.WhereNotNullNorEmpty();
enumerableOfString.WhereNotNullNorWhiteSpace();

enumerable.Order();
enumerable.OrderDescending();

enumerable.HasDuplicates();
enumerable.HasDuplicatesBy(it => it.UniqueValue);

enumerable.TakeExact(42);
enumerable.TakeAtLeast(42);

enumerable.AddRangeTo(destinationCollection);
enumerable.RemoveRangeFrom(destinationCollection);

enumerable.MinBy(it => it.ComparableValue, defaultValueIfEmpty);
enumerable.MaxBy(it => it.ComparableValue, defaultValueIfEmpty);

enumerable.Recurse(it => it.Parent); // T => IEnumerable<T>
enumerable.RecurseMany(it => it.Children); // T => IEnumerable<T>
enumerable.RecurseManyDepthed(it => it.Children); // T => IEnumerable<(int Depth, T Item)>
enumerable.SelectRecurse(it => it.Children); // IEnumerable<T> => IEnumerable<T>
enumerable.SelectRecurseDepthed(it => it.Children, int startDepth = 0); // IEnumerable<T> => IEnumerable<(int Depth, T Item)>

enumerableOfStrings.JoinToStringBuilder(separator: ", ", prefix = "[ ", suffix = " ]", fallback: = "null"); // => StringBuilder
enumerableOfStrings.JoinToString(separator: ", ", prefix = "[ ", suffix = " ]", fallback: = "null"); // => string
```

Special extensions for dictionaries:

```cs
dictionary.WhereValueNotNull();
dictionaryOfStrings.WhereValueNotNullNorEmpty();
dictionaryOfStrings.WhereValueNotNullNorWhiteSpace();

dictionary.GetValueOrDefault(key, key => new(key));
dictionary.GetValueOrSet(key, defaultValue);
```

### Useless Extensions on Objects

```cs
any.IsContainedIn(collection);
any.AddTo(destinationCollection);
any.RemoveFrom(sourceCollection);

any.GetDeclaredType();

any
   .As<Other>()
   .Also(it => {
      if (it.CanDoSomething)
         it.DoSomething();
   })
   .To(it => it.Children)
   .Tee(out List<Other> children);
```

There's also an underdeveloped `Lazy` counterpart:

```cs
lazy
   .As<Other>() // Lazy<T> => Lazy<Other>
   .Also(it => {
      if (it.CanDoSomething)
         it.DoSomething();
   })
   .To(it => it.Children); // Still lazy: no invocation is performed until .Value is accessed.
```

### Useless Extension on `IComparable<T>`

```cs

42.CoerceAtLeast(100); // => 100
42.CoerceAtMost(100); // => 42
42.CoerceIn(0, 10); // => 10
```

### `IndentedStringBuilder`

```cs
value.ToBuilder(); // string => StringBuilder

value.ToIndentedBuilder(); // string => IndentedStringBuilder
builder.ToIndentedBuilder(); // StringBuilder => IndentedStringBuilder
```

Source Code Structure
----

<!-- 41DA3A82-0B89-4CB9-AF10-8E4D00FF60E1 -->

Unlike what is the default in the C# world, directories in the source code of the main assembly are not supposed to provide sub-namespaces. The assembly is flattened into a single `LinqToYourDoom` namespace, while preserving a nice organization in the solution explorer IDE's side bar.

This decision is enforced by `LinqToYourDoom.Tests.SingleNamespaceTests.SingleNamespace()`.
