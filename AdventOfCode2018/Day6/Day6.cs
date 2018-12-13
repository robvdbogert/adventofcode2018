using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day6
{
    public class Day6 : PuzzleBase
    {
        public Day6() : base(6)
        {
        }

        protected override void DoExecutePart1()
        {
            var input = Input;

            var points = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(entry =>
                {
                    var data = entry.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    return new Point(int.Parse(data[0]), int.Parse(data[1]));
                })
                .ToList();

            var pointNumber = 0;
            foreach (var point in points)
            {
                point.Number = pointNumber;
                pointNumber++;
            }

            var xMin = points.Min(p => p.X);
            var xMax = points.Max(p => p.X);
            var yMin = points.Min(p => p.Y);
            var yMax = points.Max(p => p.Y);

            var grid = new int?[xMax - xMin + 1, yMax - yMin + 1];

            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    Point closestPoint = null;
                    var closestPointDistance = int.MaxValue;
                    foreach (var point in points)
                    {
                        var distanceToPoint = GetDistanceToPoint(x, y, point);
                        if (distanceToPoint < closestPointDistance)
                        {
                            closestPoint = point;
                            closestPointDistance = distanceToPoint;
                        }
                        else if (distanceToPoint == closestPointDistance)
                        {
                            closestPoint = null;
                        }
                    }

                    grid[x - xMin, y - yMin] = closestPoint?.Number;
                }
            }
            
            // Ignore any point number that is on the edge of the grid
            var pointsOnEdge =
                GetPointsOnRow(grid, 0)
                    .Union(GetPointsOnRow(grid, grid.GetLength(1) - 1))
                    .Union(GetPointsOnColumn(grid, 0))
                    .Union(GetPointsOnColumn(grid, grid.GetLength(0) - 1))
                    .Distinct();

            var isolatedPoints = points.Select(x => x.Number).Except(pointsOnEdge);
            var areas = isolatedPoints.Select(point => new
            {
                Number = point,
                Count = Count(grid, point)
            });
            
            Console.WriteLine($"The largest area is {areas.Max(a => a.Count)}");
        }

        protected override void DoExecutePart2()
        {
            throw new System.NotImplementedException();
        }

        private int Count(int?[,] grid, int pointNumber)
        {
            var result = 0;
            for (var x = 0; x < grid.GetLength(0); x++)
            {
                for (var y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y] == pointNumber)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private int GetDistanceToPoint(int x, int y, Point p)
        {
            var dY = Math.Abs(p.Y - y);
            var dX = Math.Abs(p.X - x);

            return dY + dX;
        }

        private List<int> GetPointsOnRow(int?[,] grid, int rowIndex)
        {
            var result = new List<int>();
            for (var x = 0; x < grid.GetLength(0); x++)
            {
                var value = grid[x, rowIndex];
                if (value.HasValue)
                {
                    result.Add(value.Value);
                }
            }

            return result;
        }
        
        private List<int> GetPointsOnColumn(int?[,] grid, int columnIndex)
        {
            var result = new List<int>();
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var value = grid[columnIndex, y];
                if (value.HasValue)
                {
                    result.Add(value.Value);
                }
            }

            return result;
        }
    }

    public class Point
    {
        public int Number { get; set; }
        
        public int X { get; set; }
        
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}