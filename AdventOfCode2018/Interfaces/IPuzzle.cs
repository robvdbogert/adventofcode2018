using System;
namespace AdventOfCode2018.Interfaces
{
    public interface IPuzzle
    {
        int Day { get; }

        void ExecutePart1();

        void ExecutePart2();
    }
}
