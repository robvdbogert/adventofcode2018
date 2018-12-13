using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day3
{
    public class Day3 : PuzzleBase
    {
        public Day3(): base(3)
        {
        }

        protected override void DoExecutePart1()
        {
            var squares = ParseInput();
            var overlaps = CalculateOverlaps(squares);

            int overlappingInches = 0;
            for (int x = 0; x < overlaps.GetLength(0); x++) {
                for (int y = 0; y < overlaps.GetLength(1); y++) {
                    if (overlaps[x, y] > 1) {
                        overlappingInches++;
                    }
                }
            }

            Console.WriteLine($"There are {overlappingInches} square inches with overlap");
        }

        protected override void DoExecutePart2()
        {
            var squares = ParseInput();
            var overlaps = CalculateOverlaps(squares);

            foreach (var square in squares)
            {
                bool hasOverlap = false;
                for (int x = square.Left; x < square.Right; x++)
                {
                    for (int y = square.Top; y < square.Bottom; y++)
                    {
                        if (overlaps[x, y] > 1)
                        {
                            hasOverlap = true;
                            break;
                        }
                    }

                    if (hasOverlap)
                    {
                        break;
                    }
                }

                if (!hasOverlap) {
                    Console.WriteLine($"Square {square.Id} has no overlap");
                    break;
                }
            }
        }

        private int[,] CalculateOverlaps(List<Square> squares) 
        {
            var fabricWidth = squares.Max(s => s.Right);
            var fabricHeight = squares.Max(s => s.Bottom);
            var inches = new int[fabricWidth, fabricHeight];

            foreach (var square in squares)
            {
                for (int x = square.Left; x < square.Right; x++)
                {
                    for (int y = square.Top; y < square.Bottom; y++)
                    {
                        inches[x, y]++;
                    }
                }
            }

            return inches;
        }

        private List<Square> ParseInput() 
        {
            var regex = new Regex(@"#(?<id>\d+) @ (?<left>\d+),(?<top>\d+): (?<width>\d+)x(?<height>\d+)", 
                                  RegexOptions.Compiled | RegexOptions.Multiline);
            var result = new List<Square>();

            var matches = regex.Matches(Input);
            foreach (Match match in matches)
            {
                result.Add(new Square
                {
                    Id = int.Parse(match.Groups["id"].Value),
                    Left = int.Parse(match.Groups["left"].Value),
                    Top = int.Parse(match.Groups["top"].Value),
                    Width = int.Parse(match.Groups["width"].Value),
                    Height = int.Parse(match.Groups["height"].Value)
                });
            }

            return result;
        }
    }

    public class Square
    {
        public int Id { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Bottom
        {
            get
            {
                return Top + Height;
            }
        }

        public int Right
        {
            get
            {
                return Left + Width;
            }
        }
    }
}
