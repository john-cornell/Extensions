using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class ObjectExtensions
{
    public static T[] AsSingleItemArray<T>(this T item)
    {
        return item.Yield().ToArray();
    }

    public static List<T> AsSingleItemList<T>(this T item)
    {
        return item.Yield().ToList();
    }

    public static IEnumerable<T> Yield<T>(this T item)
    {
        yield return item;
    }

    [DebuggerStepThrough()]
    public static bool In<T>(this T me, params T[] items)
    {
        return items.Contains(me);
    }

    [DebuggerStepThrough()]
    public static bool In<T>(this T me, IEqualityComparer<T> comparer, IEnumerable<T> items)
    {
        return items.Contains(me, comparer);
    }

    [DebuggerStepThrough()]
    public static bool NotIn<T>(this T me, params T[] items)
    {
        return !items.Contains(me);
    }

    [DebuggerStepThrough()]
    public static bool NotIn<T>(this T me, IEqualityComparer<T> comparer, IEnumerable<T> items)
    {
        return !items.Contains(me, comparer);
    }

    [DebuggerStepThrough()]
    public static bool Is<T>(this object item) where T : class
    {
        return item is T;
    }

    [DebuggerStepThrough()]
    public static bool IsNot<T>(this object item) where T : class
    {
        return !(item.Is<T>());
    }

        public static bool IfIs<T>(this object item, Action<T> action) where T : class
        {
            if (item.Is<T>())
            {
                action(item.As<T>());

                return true;
            }

            return false;
        }

    [DebuggerStepThrough()]
    public static T As<T>(this object item) where T : class
    {
        return item as T;
    }

    [DebuggerStepThrough()]
    public static T To<T>(this object item)
    {
        return (T)item;
    }

    public static bool True<T>(this T me, Func<T, bool> predicate)
    {
        return predicate(me);
    }

    public static bool False<T>(this T me, Func<T, bool> predicate)
    {
        return !predicate(me);
    }

    //source
    //http://stackoverflow.com/questions/271398/what-are-your-favorite-extension-methods-for-c-codeplex-com-extensionoverflow
    /// <summary>
    /// Mimics VB With statement
    /// </summary>        
    [DebuggerStepThrough()]
    public static void With<T>(this T obj, Action<T> act) { act(obj); }

    public static bool IsNull<T>(this T me) where T : class
    {
        return me == null;
    }

    public static bool IsNullOrDefault<T>(this T me)// where T : struct
    {
        return EqualityComparer<T>.Default.Equals(me, default(T));
    }

    public static T IfNull<T>(this T me, T item)
    {
        if (me == null || me.Equals(default(T))) return item;

        return me;
    }

    public static T If<T>(this T me, T value, T replacement)
    {
        return EqualityComparer<T>.Default.Equals(me, value) ? replacement : me;
    }

    [DebuggerStepThrough]
    public static TOutputType ConvertType<TOutputType>(this object me)
    {
        Type conversionType = typeof(TOutputType);

        if (conversionType.IsGenericType &&
            conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (me == null) { return default(TOutputType); }

            conversionType = Nullable.GetUnderlyingType(conversionType);
        }

        return (TOutputType)Convert.ChangeType(me, conversionType);
    }


    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    public static T Clone<T>(this T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", "source");
        }

        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new MemoryStream();
        using (stream)
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }

    /// <summary>
    /// Perform a deep Copy of the object, using Json as a serialisation method.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    public static T CloneJson<T>(this T source)
    {
        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
    }

    public static IEnumerable<IEnumerable<T>> Chunk<T>(
                this IEnumerable<T> values,
                Int32 chunkSize)
    {
        using (var enumerator = values.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                yield return GetChunk(enumerator, chunkSize).ToList();
            }
        }
    }
    private static IEnumerable<T> GetChunk<T>(
                     IEnumerator<T> enumerator,
                     int chunkSize)
    {
        do
        {
            yield return enumerator.Current;
        } while (--chunkSize > 0 && enumerator.MoveNext());
    }
}
