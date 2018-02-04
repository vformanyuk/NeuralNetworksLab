using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworksLab.App.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> MostCommon<T>(this IEnumerable<T> set)
        {
            return (from s in set
                let t = s.GetType()
                group s by t
                into g
                orderby g.Count() descending
                select g).FirstOrDefault();
        }
    }
}
