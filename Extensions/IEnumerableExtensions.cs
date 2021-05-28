using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public static class IEnumerableExtensions
{
    static Random _rand;
    static IEnumerableExtensions()
    {
        _rand = new Random((int)DateTime.Now.Ticks);
    }

    public static T RandomElement<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.RandomElementUsing<T>(_rand);
    }

    public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
    {
        int count = enumerable.Count();

        if (count == 0) return default(T);

        int index = rand.Next(0, enumerable.Count());

        return enumerable.ElementAt(index);
    }

    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> sequence, int size)
    {
        List<T> partition = new List<T>(size);

        foreach (var item in sequence)
        {
            partition.Add(item);

            if (partition.Count == size)
            {
                yield return partition;
                partition = new List<T>(size);
            }
        }

        if (partition.Count > 0)
            yield return partition;
    }

    public static void ForEach(this IEnumerable source, Action<object> action)
    {
        foreach (object item in source)
        {
            action(item);
        }
    }

    public static IEnumerable<T> Type<T>(this IEnumerable source)
    {
        foreach (T item in source) yield return item;
    }

    [DebuggerStepThrough]
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action, bool guardAgainstModifiedItemException = false)
    {
        if (guardAgainstModifiedItemException) ForEachAsArray(source, action);
        else
        {
            foreach (TSource item in source)
            {
                action(item);
            }
        }
    }

    private static void ForEachAsArray<TSource>(IEnumerable<TSource> source, Action<TSource> action)
    {
        TSource[] array = source.ToArray();

        for (int i = array.Length - 1; i >= 0; i--)
        {
            action(array[i]);
        }
    }

    [DebuggerStepThrough]
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate, Action<TSource> action)
    {
        foreach (TSource item in source)
        {
            if (predicate(item)) action(item);
        }
    }

    [DebuggerStepThrough]
    public static bool Or<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (predicate(item)) return true;
        }

        return false;
    }

    [DebuggerStepThrough]
    public static bool And<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        if (!(source != null && source.First() != null)) return false;

        foreach (TSource item in source)
        {
            if (!predicate(item)) return false;
        }

        return true;
    }

    //source
    //http://stackoverflow.com/questions/271398/what-are-your-favorite-extension-methods-for-c-codeplex-com-extensionoverflow
    /// <summary>
    /// removes is null check
    /// </summary>  
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> pSeq)
    {
        return pSeq ?? Enumerable.Empty<T>();
    }

    /*
     * Distinct Extension Methods taken from the MoreLinq project:
     * 
     * MoreLinq, Version=1.0.11522.0
     * 
     * Available at https://code.google.com/p/morelinq/
     * 
     */

    /// <summary>
    /// Returns all distinct elements of the given source, where "distinctness"
    ///             is determined via a projection and the default eqaulity comparer for the projected type.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// This operator uses deferred execution and streams the results, although
    ///             a set of already-seen keys is retained. If a key is seen multiple times,
    ///             only the first element with that key is returned.
    /// 
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam><typeparam name="TKey">Type of the projected element</typeparam><param name="source">Source sequence</param><param name="keySelector">Projection for determining "distinctness"</param>
    /// <returns>
    /// A sequence consisting of distinct elements from the source sequence,
    ///             comparing them by the specified key projection.
    /// </returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return DistinctBy<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>)null);
    }

    /// <summary>
    /// Returns all distinct elements of the given source, where "distinctness"
    ///             is determined via a projection and the specified comparer for the projected type.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// This operator uses deferred execution and streams the results, although
    ///             a set of already-seen keys is retained. If a key is seen multiple times,
    ///             only the first element with that key is returned.
    /// 
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam><typeparam name="TKey">Type of the projected element</typeparam><param name="source">Source sequence</param><param name="keySelector">Projection for determining "distinctness"</param><param name="comparer">The equality comparer to use to determine whether or not keys are equal.
    ///             If null, the default equality comparer for <c>TSource</c> is used.</param>
    /// <returns>
    /// A sequence consisting of distinct elements from the source sequence,
    ///             comparing them by the specified key projection.
    /// </returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (keySelector == null) throw new ArgumentNullException("keySelector");
        return DistinctByImpl<TSource, TKey>(source, keySelector, comparer);
    }

    private static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
        foreach (TSource source1 in source)
        {
            if (knownKeys.Add(keySelector(source1)))
                yield return source1;
        }
    }

    public static IEnumerable<T> ForceLength<T>(this IEnumerable<T> me, int length, T padValue)
    {
        int count = me.Count();
        T[] items = me.ToArray();

        for (int i = 0; i < length; i++)
        {
            if (i < count)
            {
                yield return items[i];
            }
            else
            {
                yield return padValue;
            }
        }
    }

    /// <summary>
    /// Seperate supplied values into two lists, bassed on predicate
    /// </summary>
    /// <returns>A tuple of 2 lists, the first of all items in IEnumerable that pass a supplied predicate, the second a list of all items in IEnumerable that fail supplied predicate</returns>
    public static Tuple<List<T>, List<T>> Separate<T>(this IEnumerable<T> me, Predicate<T> predicate)
    {
        List<T> inclusive = new List<T>();
        List<T> exclusive = new List<T>();

        me.ForEach(t =>
        {
            if (predicate(t))
            {
                inclusive.Add(t);
            }
            else
            {
                exclusive.Add(t);
            }
        });

        return new Tuple<List<T>, List<T>>(inclusive, exclusive);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.Shuffle(new Random());
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (rng == null) throw new ArgumentNullException("rng");

        return source.ShuffleIterator(rng);
    }

    private static IEnumerable<T> ShuffleIterator<T>(
        this IEnumerable<T> source, Random rng)
    {
        var buffer = source.ToList();
        for (int i = 0; i < buffer.Count; i++)
        {
            int j = rng.Next(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
        }
    }
}