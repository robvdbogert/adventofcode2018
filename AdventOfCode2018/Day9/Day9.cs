using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day9
{
    public class Day9 : PuzzleBase
    {
        public Day9() : base(9)
        {
        }
       
        protected override void DoExecutePart1()
        {
            PlayMarblesFast(9, 25);
            PlayMarblesFast(10, 1618);
            PlayMarblesFast(13, 7999);
            PlayMarblesFast(17, 1104);
            PlayMarblesFast(21, 6111);
            PlayMarblesFast(30, 5807);
            
            PlayMarblesFast(424, 71144);
        }
        
        protected override void DoExecutePart2()
        {
            PlayMarblesFast(424, 71144 * 100);
        }
        
        private void PlayMarblesFast(int playerCount, int finalMarble)
        {
            var initialMarbles = new[] { 0 };
            var marbles = new LinkedList<int>(initialMarbles);
            
            var players = new Dictionary<int, long>();
            for (var i = 1; i <= playerCount; i++)
            {
                players.Add(i, 0);
            }

            var currentNode = marbles.First;
            var currentPlayer = 1;
            var currentMarble = 1;

            while (currentMarble <= finalMarble)
            {
                LinkedListNode<int> node;
                if (currentMarble % 23 == 0)
                {
                    players[currentPlayer] += currentMarble;

                    // Take away the marble 7 positions counter clockwise of the current one
                    var nodeToRemove = currentNode;
                    for (var i = 0; i < 7; i++)
                    {
                        nodeToRemove = nodeToRemove.Previous ?? marbles.Last;
                    }

                    // And add the value to the score of the current player
                    players[currentPlayer] += nodeToRemove.Value;
                    node = nodeToRemove.Next ?? marbles.First;
                    marbles.Remove(nodeToRemove);
                }
                else
                {
                    node = currentNode.Next ?? marbles.First;
                    node = marbles.AddAfter(node, currentMarble);
                }

                currentPlayer = currentPlayer + 1 > playerCount ? 1 : currentPlayer + 1;
                currentMarble++;
                currentNode = node;
            }

            var playerWithHighestScore = players.OrderByDescending(p => p.Value).First();
            
            Console.WriteLine($"Player {playerWithHighestScore.Key} scored highest: {playerWithHighestScore.Value}");
        }

        private void PlayMarbles(int playerCount, int finalMarble)
        {
            var marbles = new List<int>(finalMarble);
            marbles.Add(0);
            
            var players = new Dictionary<int, int>();
            for (var i = 1; i <= playerCount; i++)
            {
                players.Add(i, 0);
            }

            var currentPosition = 0;
            var currentPlayer = 1;
            var currentMarble = 1;

            while (currentMarble <= finalMarble)
            {
                if (currentMarble % 100000 == 0)
                {
                    Console.WriteLine($"{currentMarble * 100 / finalMarble}%");
                }
                
                var nextPosition = (currentPosition + 1) % marbles.Count + 1;

                if (currentMarble % 23 == 0)
                {
                    players[currentPlayer] += currentMarble;

                    // Take away the marble 7 positions counter clockwise of the current one
                    var positionOfMarbleToRemove = currentPosition - 7;
                    if (positionOfMarbleToRemove < 0)
                    {
                        positionOfMarbleToRemove = marbles.Count + positionOfMarbleToRemove;
                    }

                    // And add the value to the score of the current player
                    players[currentPlayer] += marbles[positionOfMarbleToRemove];
                    marbles.RemoveAt(positionOfMarbleToRemove);
                    
                    nextPosition = positionOfMarbleToRemove;
                }
                else
                {
                    marbles.Insert(nextPosition, currentMarble);
                }

                currentPlayer = currentPlayer + 1 > playerCount ? 1 : currentPlayer + 1;
                currentMarble++;
                currentPosition = nextPosition;
            }

            var playerWithHighestScore = players.OrderByDescending(p => p.Value).First();
            
            Console.WriteLine($"Player {playerWithHighestScore.Key} scored highest: {playerWithHighestScore.Value}");
        }
    }
}