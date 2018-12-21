using System;
using System.Linq;
using System.Reflection;
using AdventOfCode2018.Interfaces;

namespace AdventOfCode2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzleType = typeof(IPuzzle);
            var types = Assembly.GetExecutingAssembly().GetTypes().ToList();
            var puzzleTypes = types.Where(t => t.IsClass && !t.IsAbstract && puzzleType.IsAssignableFrom(t));

            var puzzles = puzzleTypes.Select(t => Activator.CreateInstance(t)).Cast<IPuzzle>();
            var puzzle = puzzles.Single(p => p.Day == 12);

            puzzle.ExecutePart1();
            puzzle.ExecutePart2();
        }
    }
}
