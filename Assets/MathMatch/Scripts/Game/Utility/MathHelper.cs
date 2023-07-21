using System.Collections.Generic;
using UnityEngine;

namespace MathMatch.Game.Utility
{
    public static class MathHelper
    {
        public static List<int> Digits = new()
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        };
        
        public static void Shuffle<T>(this IList<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = Random.Range(0, n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
        
        public static List<List<int>> CoinChangeProblem(List<int> numbers, int target)
        {
            var result = new List<List<int>>();
            CoinChangeProblemRecursive(result, numbers, target, new List<int>());
            return result;
        }
        
        private static void CoinChangeProblemRecursive(List<List<int>> result, List<int> numbers, int target, List<int> partial)
        {
            var s = 0;
            for (var i = 0; i < partial.Count; i++)
            {
                var x = partial[i];
                s += x;
            }

            if (s == target)
            {
                result.Add(new List<int>(partial));
            }
            
            if (s >= target)
            {
                return;
            }
            
            for (int i = 0; i < numbers.Count; i++)
            {
                var remaining = new List<int>();
                var n = numbers[i];
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    remaining.Add(numbers[j]);
                }
            
                var partialRec = new List<int>(partial) { n };
                CoinChangeProblemRecursive(result, remaining, target, partialRec);
            }
        }
    }
}