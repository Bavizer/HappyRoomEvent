using System.Collections.Generic;
using System;
using System.Linq;

namespace HappyRoomEvent.Extensions;

public static class CollectionExtensions
{
    public static T? RandomValue<T>(this T[] array) =>
        array.Length == 0 ? default : array[UnityEngine.Random.Range(0, array.Length)];

    public static T? RandomValue<T>(this IEnumerable<T> collection) =>
        collection.ToArray().RandomValue();

    public static T? RandomValue<T>(this IEnumerable<T> collection, Func<T, bool> selector) =>
        collection.Where(selector).RandomValue();

    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (T item in collection)
            action(item);
    }
}
