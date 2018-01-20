using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworksLab.App.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> MostCommon<T>(this IEnumerable<T> set)
        {
            return (from a in (from s in set
                    let t = s.GetType()
                    select new
                    {
                        Entity = s,
                        EType = t
                    })
                group a.Entity by a.EType
                into g
                orderby g.Count()
                select g).First();
        }
    }
}
