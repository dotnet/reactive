using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        internal static async IAsyncEnumerable<TSource> Wrap<TSource>(this IAsyncEnumerable<TSource> source)
        {
            await foreach (var e in source.ConfigureAwait(false))
                yield return e;
        }
    }
}
