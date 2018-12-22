using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day13
{
    public class Day13 : PuzzleBase
    {
        public Day13() : base(13)
        {
        }

        protected override void DoExecutePart1()
        {
            var lines = Input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

            var carts = GetCarts(lines);
            var collided = false;
            while (!collided)
            {
                foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X))
                {
                    MoveCart(cart, lines);
                    collided = CheckForCollisions(carts);
                    if (collided)
                    {
                        break;
                    }
                }

                if (!collided)
                {
                    ShowCartPositions(carts);
                }
            }
        }
        
        protected override void DoExecutePart2()
        {
            var lines = Input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

            var carts = GetCarts(lines);
            while (carts.Count > 1)
            {
                var cartsByPosition = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
                foreach (var cart in cartsByPosition)
                {
                    MoveCart(cart, lines);
                    RemoveCollidedCarts(carts);
                }
            }

            var remainingCart = carts.Single();
            Console.WriteLine($"Cart {remainingCart.Number} remains at location {remainingCart.X},{remainingCart.Y}");
        }

        private static bool CheckForCollisions(List<Cart> carts)
        {
            foreach (var cart in carts)
            {
                if (carts.Any(c => c != cart && c.X == cart.X && c.Y == cart.Y))
                {
                    Console.WriteLine($"Collision at {cart.X},{cart.Y}");
                    return true;
                }
            }

            return false;
        }

        private static void MoveCart(Cart cart, string[] lines)
        {
            // Move the cart forward in its current direction
            MoveCartInCurrentDirection(cart);

            switch (lines[cart.Y][cart.X])
            {
                case '\\':
                    switch (cart.Direction)
                    {
                        case Direction.North:
                            cart.Direction = Direction.West;
                            break;
                        case Direction.East:
                            cart.Direction = Direction.South;
                            break;
                        case Direction.West:
                            cart.Direction = Direction.North;
                            break;
                        case Direction.South:
                            cart.Direction = Direction.East;
                            break;
                    }
                    break;
                case '/':
                    switch (cart.Direction)
                    {
                        case Direction.North:
                            cart.Direction = Direction.East;
                            break;
                        case Direction.East:
                            cart.Direction = Direction.North;
                            break;
                        case Direction.West:
                            cart.Direction = Direction.South;
                            break;
                        case Direction.South:
                            cart.Direction = Direction.West;
                            break;
                    }   
                    break;
                case '+':
                    ProcessIntersection(cart);
                    break;
            }
        }

        private static void ProcessIntersection(Cart cart)
        {
            switch (cart.Direction)
            {
                case Direction.North:
                    cart.Direction = cart.NextTurnDirection == TurnDirection.Left ? Direction.West :
                        cart.NextTurnDirection == TurnDirection.Right ? Direction.East :
                        cart.Direction;
                    break;
                case Direction.East:
                    cart.Direction = cart.NextTurnDirection == TurnDirection.Left ? Direction.North :
                        cart.NextTurnDirection == TurnDirection.Right ? Direction.South :
                        cart.Direction;
                    break;
                case Direction.South:
                    cart.Direction = cart.NextTurnDirection == TurnDirection.Left ? Direction.East :
                        cart.NextTurnDirection == TurnDirection.Right ? Direction.West :
                        cart.Direction;
                    break;
                case Direction.West:
                    cart.Direction = cart.NextTurnDirection == TurnDirection.Left ? Direction.South :
                        cart.NextTurnDirection == TurnDirection.Right ? Direction.North :
                        cart.Direction;
                    break;
            }

            cart.NextTurnDirection = (TurnDirection) (((int) cart.NextTurnDirection + 1) % 3);
        }

        private static void MoveCartInCurrentDirection(Cart cart)
        {
            switch (cart.Direction)
            {
                case Direction.North:
                    cart.Y -= 1;
                    break;
                case Direction.East:
                    cart.X += 1;
                    break;
                case Direction.South:
                    cart.Y += 1;
                    break;
                case Direction.West:
                    cart.X -= 1;
                    break;
            }
        }

        private static List<Cart> GetCarts(string[] lines)
        {
            var carts = new List<Cart>();
            var cartNumber = 1;
            // Find the cart positions
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++)
                {
                    Direction? cartDirection = null;
                    switch (line[x])
                    {
                        case '>':
                            cartDirection = Direction.East;
                            break;
                        case '<':
                            cartDirection = Direction.West;
                            break;
                        case '^':
                            cartDirection = Direction.North;
                            break;
                        case 'v':
                            cartDirection = Direction.South;
                            break;
                    }

                    if (cartDirection.HasValue)
                    {
                        carts.Add(new Cart
                        {
                            Number = cartNumber,
                            X = x,
                            Y = y,
                            Direction = cartDirection.Value,
                            NextTurnDirection = TurnDirection.Left
                        });
                        cartNumber++;
                    }
                }
            }

            return carts;
        }
        
        private static void RemoveCollidedCarts(List<Cart> carts)
        {
            var crashedCarts = carts
                .Where(c => carts.Any(other => other != c && other.X == c.X && other.Y == c.Y))
                .ToList();
            
            crashedCarts.ForEach(c => carts.Remove(c));
        }
        
        private void ShowCartPositions(List<Cart> carts)
        {
            carts.ForEach(c => Console.WriteLine(
                $"Cart {c.Number} is at {c.X},{c.Y} going {c.Direction} " +
                $"and will turn {c.NextTurnDirection} at the next intersection"));
        }

        public class Cart
        {
            public int Number { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public Direction Direction { get; set; }
            public TurnDirection NextTurnDirection { get; set; }
        }

        public enum TurnDirection
        {
            Left = 0,
            Straight = 1,
            Right = 2
        }

        public enum Direction
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }
    }
}