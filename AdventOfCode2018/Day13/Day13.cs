using System;
using System.Collections.Generic;

namespace AdventOfCode2018.Day13
{
    public class Day13 : PuzzleBase
    {
        public Day13() : base(13)
        {
        }

        protected override void DoExecutePart1()
        {
            var (grid, carts) = GetGrid(ExampleInput);
            SetConnectedTrackPoints(grid);
            
            
        }

        protected override void DoExecutePart2()
        {
            
        }


        private Tuple<TrackPoint[,], List<Cart>> GetGrid(string input)
        {
            // Figure out the size of the grid
            var lines = input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            var width = lines[0].Length;
            var height = lines.Length;
            
            var grid = new TrackPoint[width, height];
            var carts = new List<Cart>();
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    switch (c)
                    {
                        case '/':
                            if (x == 0 || line[x + 1] == '-' || line[x + 1] == '+')
                            {
                                grid[x, y] = new TrackPoint { Type = TrackType.BendTopLeft };
                            }
                            else
                            {
                                grid[x, y] = new TrackPoint { Type = TrackType.BendBottomRight };
                            }
                            break;
                        case '\\':
                            if (x == 0 || line[x + 1] == '-' || line[x + 1] == '+')
                            {
                                grid[x, y] = new TrackPoint {Type = TrackType.BendBottomLeft};
                            }
                            else
                            {
                                grid[x, y] = new TrackPoint { Type = TrackType.BendTopRight };
                            }
                            break;
                        case '-':
                            grid[x, y] = new TrackPoint { Type = TrackType.Horizontal };
                            break;
                        case '|':
                            grid[x, y] = new TrackPoint { Type = TrackType.Vertical };
                            break;
                        case '+':
                            grid[x, y] = new TrackPoint { Type = TrackType.Intersection };
                            break;
                        case '>':
                            grid[x, y] = new TrackPoint { Type = TrackType.Horizontal };
                            carts.Add(new Cart
                            {
                                X = x, Y = y, 
                                Direction = Direction.Right
                            });
                            break;
                        case '<':
                            grid[x, y] = new TrackPoint { Type = TrackType.Horizontal };
                            carts.Add(new Cart
                            {
                                X = x, Y = y, 
                                Direction = Direction.Left
                            });
                            break;
                        case '^':
                            grid[x, y] = new TrackPoint { Type = TrackType.Vertical };
                            carts.Add(new Cart
                            {
                                X = x, Y = y, 
                                Direction = Direction.Up
                            });
                            break;
                        case 'v':
                            grid[x, y] = new TrackPoint { Type = TrackType.Vertical };
                            carts.Add(new Cart
                            {
                                X = x, Y = y, 
                                Direction = Direction.Down
                            });
                            break;
                        default:
                            break;
                    }
                }
            }

            return new Tuple<TrackPoint[,], List<Cart>>(grid, carts);
        }

        private void SetConnectedTrackPoints(TrackPoint[,] grid)
        {
            for (var x = 0; x < grid.GetLength(0); x++)
            {
                for (var y = 0; y < grid.GetLength(1); y++)
                {
                    var trackPoint = grid[x, y];
                    if (trackPoint != null)
                    {
                        switch (trackPoint.Type)
                        {
                            case TrackType.Vertical:
                                trackPoint.Top = grid[x, y - 1];
                                trackPoint.Bottom = grid[x, y + 1];
                                break;
                            case TrackType.Horizontal:
                                trackPoint.Left = grid[x - 1, y];
                                trackPoint.Right = grid[x + 1, y];
                                break;
                            case TrackType.Intersection:
                                trackPoint.Top = grid[x, y - 1];
                                trackPoint.Bottom = grid[x, y + 1];
                                trackPoint.Left = grid[x - 1, y];
                                trackPoint.Right = grid[x + 1, y];
                                break;
                            case TrackType.BendTopLeft:
                                trackPoint.Bottom = grid[x, y + 1];
                                trackPoint.Right = grid[x + 1, y];
                                break;
                            case TrackType.BendTopRight:
                                trackPoint.Left = grid[x - 1, y];
                                trackPoint.Bottom = grid[x, y + 1];
                                break;
                            case TrackType.BendBottomRight:
                                trackPoint.Top = grid[x, y - 1];
                                trackPoint.Left = grid[x - 1, y];
                                break;
                            case TrackType.BendBottomLeft:
                                trackPoint.Right = grid[x + 1, y];
                                trackPoint.Top = grid[x, y - 1];
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }
       
    }

    public class TrackPoint
    {
        public TrackType Type { get; set; }
        
        public TrackPoint Top { get; set; }
        
        public TrackPoint Bottom { get; set; }
        
        public TrackPoint Left { get; set; }
        
        public TrackPoint Right{ get; set; }
    }

    public class Cart
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public enum TrackType
    {
        Vertical,
        Horizontal,
        Intersection,
        BendTopLeft,
        BendTopRight,
        BendBottomRight,
        BendBottomLeft
    }
}