namespace Icer.Commons.Extensions
{
    /// <summary>
    /// Extensions class for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Check if provided <see cref="IEnumerable{T}"/> is empty or null
        /// </summary>
        /// <typeparam name="T">Type of the instance in <paramref name="src"/></typeparam>
        /// <param name="src"><see cref="IEnumerable{T}"/> to check</param>
        /// <returns>
        /// True if <paramref name="src"/> is null or does not contain any element, false otherwise
        /// </returns>
        public static bool IsEmptyOrNull<T>(this IEnumerable<T>? src)
            => src == null || !src.Any();

        /// <summary>
        /// Prepend <paramref name="element"/> to <paramref name="src"/> if <paramref
        /// name="doPrepend"/> is true
        /// </summary>
        /// <typeparam name="T">Type of the elements in <paramref name="src"/></typeparam>
        /// <param name="src">Source <see cref="IEnumerable{T}"/></param>
        /// <param name="element">The <typeparamref name="T"/> to prepend to <paramref name="src"/></param>
        /// <param name="doPrepend">True to do the prepend, false to do nothing</param>
        /// <returns>
        /// A new <see cref="IEnumerable{T}"/> that begins with <paramref name="element"/> if
        /// <paramref name="doPrepend"/> is true, <paramref name="src"/> otherwise
        /// </returns>
        public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> src, T element, bool doPrepend)
        {
            return doPrepend
                   ? src.Prepend(element)
                   : src;
        }

        /// <summary>
        /// Stop processing an <see cref="IEnumerable{T}"/> when a cancellation token is triggered
        /// </summary>
        /// <typeparam name="T">Type of elements in the <paramref name="src"/></typeparam>
        /// <param name="src"><see cref="IEnumerable{T}"/> source to enumerate</param>
        /// <param name="token"><see cref="CancellationToken"/> to watch out for</param>
        /// <returns>A cancellable iterator on <paramref name="src"/></returns>
        /// <example>myList.WithCancellation(token).Select(....)</example>
        public static IEnumerable<T> WithCancellation<T>(this IEnumerable<T> src, CancellationToken token)
        {
            foreach (var item in src)
            {
                token.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }
}
