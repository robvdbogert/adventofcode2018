using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Day2
{
    public class Day2 : PuzzleBase
    {
        public Day2() : base(2)
        {
        }

        protected override void DoExecutePart1()
        {
            var entries = Input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var boxIdsWithSameLetterTwice = 0;
            var boxIdsWithSameLetterThreeTimes = 0;
            foreach (var entry in entries)
            {
                var characterCounts = new Dictionary<char, int>();
                foreach (var c in entry)
                {
                    if (!characterCounts.ContainsKey(c))
                    {
                        characterCounts.Add(c, 1);
                    }
                    else
                    {
                        characterCounts[c]++;
                    }
                }

                bool hasLetterTwice = characterCounts.Values.Any(v => v == 2);
                bool hasLetterThreeTimes = characterCounts.Values.Any(v => v == 3);
                if (hasLetterTwice) {
                    boxIdsWithSameLetterTwice++;
                }
                if (hasLetterThreeTimes) {
                    boxIdsWithSameLetterThreeTimes++;
                }
            }

            var result = boxIdsWithSameLetterTwice * boxIdsWithSameLetterThreeTimes;
            Console.WriteLine($"Result = {boxIdsWithSameLetterTwice} * {boxIdsWithSameLetterThreeTimes} = {result}");
        }

        protected override void DoExecutePart2()
        {
            var input = GetInputFromFile();
            //var entries = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

//            var input = @"
//abcde
//fghij
//klmno
//pqrst
//fguij
//axcye
//wvxyz";

            var entries = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int highestSimilarity = 0;
            var mostSimilarEntries = new string[2];
            for (int i = 0; i < entries.Length / 2; i++)
            {
                for (int j = i + 1; j < entries.Length; j++)
                {
                    var entry1 = entries[i];
                    var entry2 = entries[j];
                    int similarity = 0;
                    for (int c = 0; c < entry1.Length; c++) {
                        if (entry1[c] == entry2[c]) {
                            similarity++;
                        }
                    }

                    if (similarity > highestSimilarity) {
                        highestSimilarity = similarity;
                        mostSimilarEntries[0] = entry1;
                        mostSimilarEntries[1] = entry2;
                    }
                }
            }

            var result = new StringBuilder();
            for (int i = 0; i < mostSimilarEntries[0].Length; i++)
            {
                if (mostSimilarEntries[0][i] == mostSimilarEntries[1][i]) {
                    result.Append(mostSimilarEntries[0][i]);
                }
            }

            Console.WriteLine($"Most similar are {mostSimilarEntries[0]} and {mostSimilarEntries[1]} with common letters {result}");
        }
    }
}
