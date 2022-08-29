using System.Text;

namespace MTGCC.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                return null;

            foreach (T item in enumerable)
                action(item);

            return enumerable;
        }

        public static IEnumerable<TFirst> SupersectBy<TFirst, TSecond>(this IEnumerable<TFirst> first,
    IEnumerable<TSecond> second, Func<TFirst, TSecond> keySelector)
        {
            HashSet<TSecond> intersection = new HashSet<TSecond>(first.IntersectBy(second, keySelector).Select(x => keySelector(x)));

            IEnumerable<TFirst> supersection = first.Where(x => intersection.Contains(keySelector(x)));

            return supersection;
        }
    }
}
