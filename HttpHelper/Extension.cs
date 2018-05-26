using System;
using System.Linq;

namespace HttpHelper
{
    public static class Extension
    {
        /// <summary>
        ///     Добавляет пути к url
        /// </summary>
        /// <param name="uri"> </param>
        /// <param name="paths"> Пути для добавления к исходному url</param>
        /// <returns> Новый урл</returns>
        public static Uri AppendUri(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri,
                (current, path) => $"{current.TrimEnd('/', ' ')}/{path.TrimStart('/', ' ')}"));
        }
    }
}