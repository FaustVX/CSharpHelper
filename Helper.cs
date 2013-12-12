using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpHelper
{
	public static class Helper
	{
		public static TResult IfNotNull<TSource, TResult>(this TSource source, Func<TSource, TResult> action)
		{
			return Equals(source, default(TSource)) ? default(TResult) : action(source);
		}

		public static void IfNotNull<TSource>(this TSource source, Action<TSource> action)
		{
			if (Equals(source, default(TSource)))
				return;
			action(source);
		}

		public static bool IsInArray<T>(this T[] array, int value)
		{
			return value >= 0 && value < array.Length;
		}

		public static bool IsInArray<T>(this T[][] array, int x, int y)
		{
			if (x < 0 || x >= array.GetUpperBound(0))
				return false;
			bool result = true;
			for (int i = 0; result && i < array.GetLength(0); i++)
				result &= array[i].IsInArray(y);
			return result;
		}

		public static bool IsInArray<T>(this T[,] array, int x, int y)
		{
			bool result = x >= 0 && y >= 0 && x < array.GetLength(0) && y < array.GetLength(1);
			return result;
		}

		public static IList<Tresult> ToList<T, Tresult>(this IEnumerable<T> list, Func<T, Tresult> convert)
		{
			return list.Select(convert).ToList();
		}

		public static void ForEach<T>(this IEnumerable<IEnumerable<T>> array, Action<int, int, T> action)
		{
			int i = 0;

			foreach (var items in array)
			{
				int i1 = i;
				items.ForEach((j, item) => action(i1, j, item));
				i++;
			}

			//for (int i = 0; i < array.Count; i++)
			//{
			//	int i1 = i;
			//	array[i].ForEach((j, item) => action(i1, j, item));
			//}
		}

		public static void ForEach<T>(this IEnumerable<T> array, Action<int, T> action)
		{
			int i = 0;
			foreach (T item in array)
				action(i++, item);

			//for (int i = 0; i < array.Count; i++)
			//	action(i, array[i]);
		}

		public static void ForEach<T>(this IEnumerable<IEnumerable<T>> array, Action<T> action)
		{
			array.ForEach<IEnumerable<T>>(list => list.ForEach(action));
		}

		public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
		{
			if(action != null)
				foreach (T item in array)
				{
					action(item);
					//yield return item;
				}
		}

		public static string Join<T>(this ICollection<T> array, string separator)
		{
			return array.Join(separator, item => item.ToString());
		}

		public static string Join<T>(this ICollection<T> array, string separator, Func<T, string> toString)
		{
			string result = "";

			if (!array.Any())
				return result;

			result = toString(array.ElementAt(0));
			for (int i = 1; i < array.Count; i++)
				result += separator + toString(array.ElementAt(i));

			return result;
		}
	}
}