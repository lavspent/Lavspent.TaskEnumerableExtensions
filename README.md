# Lavspent.TaskEnumerableExtensions

Adds async Linq extensions to `Task<IEnumerable<T>>` so 

instead of this:

```c#
var first = (await GetListAsync()).First();
```

we can :

```c#
var first = await GetListAsync().First();
```
