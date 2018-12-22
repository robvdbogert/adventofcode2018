using System;
using System.Diagnostics;
using System.IO;
using AdventOfCode2018.Interfaces;

namespace AdventOfCode2018
{
    public abstract class PuzzleBase : IPuzzle
    {
        public int Day { get; private set; }

        public string Input { get; protected set; }

        public string ExampleInput { get; protected set; }
        
        public string Example2Input { get; protected set; }

        protected PuzzleBase(int day)
        {
            Day = day;

            Input = GetInputFromFile();

            ExampleInput = GetInputFromFile("example");

            Example2Input = GetInputFromFile("example2");
        }

        protected string GetInputFromFile(string postFix = null)
        {
            var fileName = postFix != null ? $"./Input/Day{Day}_{postFix}.txt" : $"./Input/Day{Day}.txt";
            if (File.Exists(fileName)) 
            {
                return File.ReadAllText(fileName);
            }

            return null;
        }

        protected string[] GetInputLines() 
        {
            return Input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }

        public void ExecutePart1() {
            var sw = Stopwatch.StartNew();
            DoExecutePart1();
            sw.Stop();
            Console.WriteLine($"Part 1 took {sw.ElapsedMilliseconds} ms");
        }

        public void ExecutePart2()
        {
            var sw = Stopwatch.StartNew();
            DoExecutePart2();
            sw.Stop();
            Console.WriteLine($"Part 2 took {sw.ElapsedMilliseconds} ms");
        }


        protected abstract void DoExecutePart1();

        protected abstract void DoExecutePart2();
    }
}
