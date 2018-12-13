using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day5
{
    public class Day5 : PuzzleBase
    {
        public Day5() : base(5)
        {
        }

        protected override void DoExecutePart1()
        {
            var input = Input.Trim();
            var output = ProcessReactions(input.ToList());
            
            Console.WriteLine($"{output.Length} units remain");
        }

        protected override void DoExecutePart2()
        {
            var input = Input.Trim();
            //var input = "dabAcCaCBAcCcaDA";

            input = ProcessReactions(input.ToList());
            
            var uniqueCharacters = input.ToLower().Distinct().Select(c => c.ToString());

            string shortestPolymer = null;
            foreach (var c in uniqueCharacters)
            {
                var filteredInput = Regex.Replace(input, c, "", RegexOptions.IgnoreCase);
                var output = ProcessReactions(filteredInput.ToList());
                if (shortestPolymer == null || output.Length < shortestPolymer.Length)
                {
                    shortestPolymer = output;
                }
            }
            
            Console.WriteLine($"Shortest polymer length is {shortestPolymer.Length}");
        }
        
        private string ProcessReactions(List<char> polymer)
        {
            var i = 0;
            while (i < polymer.Count - 1)
            {
                var c1 = polymer[i];
                var c2 = polymer[i + 1];

                if (char.ToLower(c1) != char.ToLower(c2))
                {
                    i++;
                    continue;
                }
                
                if ((char.IsLower(c1) && char.IsUpper(c2)) ||
                    (char.IsUpper(c1) && char.IsLower(c2)))
                {
                    // Remove pair
                    polymer.RemoveAt(i);
                    polymer.RemoveAt(i);
                    if (i > 0)
                    {
                        i--;
                    }
                }
                else
                {
                    i++;
                }
            }

            return new string(polymer.ToArray());
        }
    }
}