using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace PhantomGrid.Extension
{
    public static class CsharpExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            var shuffledList = list.ToList();
            for (int i = 0; i < shuffledList.Count; i++)
            {
                int rand = Random.Range(i, shuffledList.Count);
                (shuffledList[i], shuffledList[rand]) = (shuffledList[rand], shuffledList[i]);
            }
            return shuffledList;
        }
    }
}