# Lavspent.TaskEnumerableExtensions

Adds async Linq extensions to `Task<IEnumerable<T>>`.

## Install

```
nuget.exe install Lavspent.TaskEnumerableExtensions
```

## Use

Instead of:


```c#
var first = (await GetListAsync()).First();
```

you can:

```c#
var first = await GetListAsync().First();
```
