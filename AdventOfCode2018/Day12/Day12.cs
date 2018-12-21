using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Processing;

namespace AdventOfCode2018.Day12
{
    public class Day12: PuzzleBase
    {
        public Day12() : base(12)
        {
        }

        protected override void DoExecutePart1()
        {
            var history = new Dictionary<string, int>();
            var inputLines = Input.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

            var state = inputLines[0].Substring(inputLines[0].IndexOf('#')).Replace(".", " ");
            inputLines.RemoveAt(0);

            var patterns = inputLines.Select(l => new Pattern
            {
                RegEx = new Regex("(?=" + l.Substring(0, 5).Replace(".", " ") + ")", RegexOptions.Compiled),
                WillHavePlant = l[9] == '#'
            }).ToList();

            var indexOfPotZero = 0;

            for (var i = 1; i <= 200000; i++)
            {
                var indexFirstPotWithPlant = state.IndexOf("#");
                state = "    " + state.Trim() + "    ";
                
                indexOfPotZero = indexOfPotZero - indexFirstPotWithPlant + 4;
                
                var newState = new char[state.Length];
                for (var x = 0; x < newState.Length; x++)
                {
                    newState[x] = ' ';
                }

                foreach (var pattern in patterns.Where(p => p.WillHavePlant))
                {
                    foreach (Match match in pattern.RegEx.Matches(state))
                    {
                        // We found the pattern. This means at the middle position of that pattern, 
                        // the next generation will have a plant.
                        newState[match.Index + 2] = '#';
                    }
                }

                state = new string(newState);
                if (history.ContainsKey(state))
                {
                    Console.WriteLine($"Repeat detected at generation {i}, same state was at generation {history[state]}");
                    break;
                }
                else
                {
                    history.Add(state, i);
                }
            }

            var leftPotNumber = -indexOfPotZero;
            var sumOfPotNumbersWithPlants = 0;
            for (var i = 0; i < state.Length; i++)
            {
                var potNumber = leftPotNumber + i;
                if (state[i] == '#')
                {
                    sumOfPotNumbersWithPlants += potNumber;
                }
            }
            
            Console.WriteLine($"End: {state}");
            Console.WriteLine($"Sum of all pots with a plant is {sumOfPotNumbersWithPlants}");
        }

        protected override void DoExecutePart2()
        {
            // throw new System.NotImplementedException();
        }
    }

    public class Pattern
    {
        public Regex RegEx { get; set; }
        
        public bool WillHavePlant { get; set; }
    }
}