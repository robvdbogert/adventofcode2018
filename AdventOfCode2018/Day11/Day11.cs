using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day11
{
    public class Day11 : PuzzleBase
    {
        private const int Serial = 6548;

        private const int GridSize = 300;

        private const int SquareSize = 3;

        public Day11() : base(11)
        {
        }

        protected override void DoExecutePart1()
        {
            var grid = GetGrid();

            // Find the square with the largest total power
            var powerGrid = CalculatePowerSquares(SquareSize, grid);

            // Find the square with the largest total power
            var findResult = FindSquareWithLargestPower(powerGrid);

            Console.WriteLine(
                $"The grid with top left {findResult.X}, {findResult.Y} " +
                $"has the largest total power of {findResult.Power}");
        }

        protected override void DoExecutePart2()
        {
            var grid = GetGrid();

            var results = new Dictionary<int, GridFindResult>();

            var largestPowerZeroCount = 0;
            for (var squareSize = 1; squareSize <= GridSize; squareSize++)
            {
                // Find the square with the largest total power
                var powerGrid = CalculatePowerSquares(squareSize, grid);

                // Find the square with the largest total power
                var findResult = FindSquareWithLargestPower(powerGrid);

                // Add result to dictionary
                results.Add(squareSize, findResult);
                
                // To make the loop stop when power is zero more than a number times:
                if (findResult.Power == 0)
                {
                    largestPowerZeroCount++;
                    if (largestPowerZeroCount > 3)
                    {
                        break;
                    }
                }
                else
                {
                    largestPowerZeroCount = 0;
                }
                
                Console.WriteLine(
                    $"Results for square size {squareSize}: " +
                    $"X = {findResult.X}, " +
                    $"Y = {findResult.Y}, " +
                    $"Power = {findResult.Power}");
            }

            var largestPowerFindResult = results.OrderByDescending(r => r.Value.Power).First();
            
            Console.WriteLine(
                $"The grid with top left {largestPowerFindResult.Value.X}, {largestPowerFindResult.Value.Y} " +
                $"and square size {largestPowerFindResult.Key} " +
                $"has the largest total power of {largestPowerFindResult.Value.Power}");
        }

        private int[,] GetGrid()
        {
            var grid = new int[GridSize, GridSize];

            for (var x = 1; x <= GridSize; x++)
            {
                for (var y = 1; y <= GridSize; y++)
                {
                    var power = ((x + 10) * y + Serial) * (x + 10);
                    var powerText = power.ToString("D3");
                    var hundredsDigit = int.Parse(powerText[powerText.Length - 3].ToString());
                    power = hundredsDigit - 5;
                    grid[x - 1, y - 1] = power;
                }
            }

            return grid;
        }

        
        private int[,] CalculatePowerSquares(int squareSize, int[,] grid)
        {
            // Find the square with the largest total power
            var powerGrid = new int[GridSize, GridSize];
            for (var x = 0; x < GridSize - squareSize - 1; x++)
            {
                for (var y = 0; y < GridSize - squareSize - 1; y++)
                {
                    // Sum all powers from the fuel cells in the square
                    for (var xSquare = x; xSquare < x + squareSize; xSquare++)
                    {
                        for (var ySquare = y; ySquare < y + squareSize; ySquare++)
                        {
                            powerGrid[x, y] += grid[xSquare, ySquare];
                        }
                    }
                }
            }

            return powerGrid;
        }

        private GridFindResult FindSquareWithLargestPower(int[,] powerGrid)
        {
            var largestTotalPower = int.MinValue;
            var largestTotalPowerTopLeft = new[] {-1, -1};
            for (var x = 0; x < GridSize; x++)
            {
                for (var y = 0; y < GridSize; y++)
                {
                    if (powerGrid[x, y] > largestTotalPower)
                    {
                        largestTotalPower = powerGrid[x, y];
                        largestTotalPowerTopLeft[0] = x + 1;
                        largestTotalPowerTopLeft[1] = y + 1;
                    }
                }
            }

            return new GridFindResult
            {
                Power = largestTotalPower,
                X = largestTotalPowerTopLeft[0],
                Y = largestTotalPowerTopLeft[1]
            };
        }
    }

    public class GridFindResult
    {
        public int Power { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}