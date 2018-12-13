using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2018.Interfaces;

namespace AdventOfCode2018.Day1
{
    public class Day1 : PuzzleBase
    {
        public Day1() : base(1)
        {
        }

        protected override void DoExecutePart1()
        {
            var numbers = Input.Split('\n').Select(x => Convert.ToInt32(x));
            var frequency = numbers.Sum();

            Console.WriteLine(frequency);
        }

        protected override void DoExecutePart2()
        {
            var numbers = Input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => Convert.ToInt32(x)).ToList();
            var frequenciesVisited = new Dictionary<int, bool>() {
                { 0, true }
            };
            var done = false;
            var index = 0;
            var frequency = 0;
            while (!done) 
            {
                frequency += numbers[index];
                if (frequenciesVisited.ContainsKey(frequency))
                {
                    done = true;
                }
                else
                {
                    frequenciesVisited[frequency] = true;
                }

                index = (index + 1) % numbers.Count;

            }

            Console.WriteLine($"First frequency reached twice = {frequency}");
		}
	}
}
