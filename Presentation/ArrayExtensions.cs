using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public static class ArrayExtensions
    {
        // Selectの戻り値無し　即時評価版
        public static void ForEach<T>(this IEnumerable<T> sources, Action<T> action)
        {
            if(sources == null) throw new ArgumentNullException(nameof(sources));
            if(action == null) throw new ArgumentNullException(nameof(action));

            foreach(var source in sources)
            {
                action(source);
            }
        }
    }
}
