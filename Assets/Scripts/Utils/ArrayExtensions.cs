using UnityEngine;

namespace ArrayExtensions
{
    public static class ArrayExtensions
    {
        public static T Get<T>(this T[] array, Vector2Int index, int maxWidth)
        {
            return array[index.x * maxWidth + index.y];
        }

        public static void Set<T>(this T[] array, T value, Vector2Int index, int maxWidth)
        {
            array[index.x * maxWidth + index.y] = value;
        }
    }
}
