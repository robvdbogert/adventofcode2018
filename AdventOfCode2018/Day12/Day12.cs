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
            Run(20);
        }      

        protected override void DoExecutePart2()
        {
            Run(50000000000);
        }
        
        private void Run(long maxGeneration) 
        {
            var history = new Dictionary<string, int>();
            var inputLines = Input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var state = inputLines[0].Substring(inputLines[0].IndexOf('#')).Replace(".", " ");
            inputLines.RemoveAt(0);

            var patterns = inputLines.Select(l => new Pattern
            {
                RegEx = new Regex("(?=" + l.Substring(0, 5).Replace(".", " ") + ")", RegexOptions.Compiled),
                WillHavePlant = l[9] == '#'
            }).ToList();

            var indexOfPotZero = 0;
            var endGeneration = 1;
            var repeatTotalDiff = 0;

            var i = 0;
            while (i < maxGeneration)
            {
                i++;
                
                var indexFirstPotWithPlant = state.IndexOf("#");
                state = "    " + state.Trim() + "    ";
                
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

                var newIndexOfPotZero = indexOfPotZero - indexFirstPotWithPlant + 4;
                
                state = new string(newState);
                if (history.ContainsKey(state))
                {
                    var previousSum = GetSumOfPotsWithPlants(indexOfPotZero, state);
                    var currentSum = GetSumOfPotsWithPlants(newIndexOfPotZero, state);
                    repeatTotalDiff = currentSum - previousSum;
                    
                    Console.WriteLine(
                        $"Repeat detected at generation {i}, " +
                        $"same pattern was at generation {history[state]}");
                    Console.WriteLine(
                        $"Difference of sum between these generations is {repeatTotalDiff}");
                    
                    indexOfPotZero = newIndexOfPotZero;
                    break;
                }

                indexOfPotZero = newIndexOfPotZero;
                history.Add(state, i);

                endGeneration++;
            }

            var sumOfPotNumbersWithPlants = GetSumOfPotsWithPlants(indexOfPotZero, state);
            
            Console.WriteLine($"Gen {endGeneration}): {state}");
            Console.WriteLine($"Sum of all pots with a plant is {sumOfPotNumbersWithPlants}");

            var remainingGenerations = maxGeneration - endGeneration;
            var endTotal = sumOfPotNumbersWithPlants + remainingGenerations * repeatTotalDiff;
            
            Console.WriteLine($"Final total at generation {endTotal}");
        }
        
        private int GetSumOfPotsWithPlants(int indexOfPotZero, string state)
        {
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

            return sumOfPotNumbersWithPlants;
        }
    }

    public class Pattern
    {
        public Regex RegEx { get; set; }
        
        public bool WillHavePlant { get; set; }
    }
}