using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Tesseract;

namespace AdventOfCode2018.Day10
{
    public class Day10 : PuzzleBase
    {
        private const int MinX = 100;
        private const int MaxX = 300;
        private const int MinY = 100;
        private const int MaxY = 150;
        private const string FileName = "./Output/Day10_output.jpg";
        
        public Day10() : base(10)
        {
        }

        protected override void DoExecutePart1()
        {
            var points = GetPoints(Input);

            var time = 0;
            
            // First move until until the first point is in view
            var firstPointInView = false;
            while (!firstPointInView)
            {
                MovePoints(points);
                time++;
                firstPointInView = points[0].X > MinX && points[0].X < MaxX && points[0].Y > MinY && points[0].Y < MaxY;
            }
            
            // Now move until the points are closest together
            var shortestDistance = int.MaxValue;
            while (true)
            {
                MovePoints(points);
                time++;
                
                var xDistance = points.Max(p => p.X) - points.Min(p => p.X);
                var yDistance = points.Max(p => p.Y) - points.Min(p => p.Y);
                
                if (xDistance + yDistance < shortestDistance)
                {
                    // Points are still moving closer
                    shortestDistance = xDistance + yDistance;
                }
                else
                {
                    // We're 1 second past the point that all points we're closest together
                    break;
                }
            }
            
            // Draw the image from the previous list of points
            MovePointsBack(points);
            time--;
            DrawPointsToBitmap(points, FileName);
            
            Console.WriteLine($"The message was visible after {time} seconds");
            
            // Detect text... too bad it does not work on either mac / .net core or the combination thereof
            //var text = DetectText();
            //Console.WriteLine($"Detected text is: {text}");
        }

        protected override void DoExecutePart2()
        {
            // Is inside Part 1...
        }
        
        private bool DrawPointsToBitmap(List<Point> points, string fileName)
        {
            var xMin = 100;
            var xMax = 300;
            var yMin = 75;
            var yMax = 150;
            
            var pointInView = false;
            using (var image = new Image<Rgba32>(xMax - xMin, yMax - yMin))
            {
                foreach (var p in points)
                {
                    if (p.X <= xMin || p.X >= xMax || p.Y <= yMin || p.Y >= yMax)
                    {
                        continue;
                    }

                    image[p.X - xMin, p.Y - yMin] = Rgba32.White;
                    pointInView = true;
                }

                if (pointInView)
                {
                    using (var fs = new FileStream(fileName, FileMode.Create))
                    {
                        image.SaveAsJpeg(fs);
                    }
                }

                return pointInView;
            }
        }

        private List<Point> GetPoints(string input)
        {
            var regex = new Regex(
                @"position=<\s?(?<X>-?\d+),\s+(?<Y>-?\d+)>\svelocity=<\s?(?<vX>-?\d+),\s+(?<vY>-?\d+)>", 
                RegexOptions.Compiled | RegexOptions.Multiline);
            
            var result = new List<Point>();
            var matches = regex.Matches(input);
            foreach (Match match in matches)
            {
                result.Add(new Point
                {
                    X = int.Parse(match.Groups["X"].Value),
                    Y = int.Parse(match.Groups["Y"].Value),
                    VelocityX = int.Parse(match.Groups["vX"].Value),
                    VelocityY = int.Parse(match.Groups["vY"].Value)
                });
            }

            return result;
        }

        private void MovePoints(List<Point> points)
        {
            points.ForEach(p =>
            {
                p.X += p.VelocityX;
                p.Y += p.VelocityY;
            });
        }

        private void MovePointsBack(List<Point> points)
        {
            points.ForEach(p =>
            {
                p.X -= p.VelocityX;
                p.Y -= p.VelocityY;
            });
        }

        private string DetectText()
        {
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(FileName))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        return text;
                    }
                }
            }
        }
    }

    public class Point
    {
        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int VelocityX { get; set; }
        
        public int VelocityY { get; set; }
    }
}