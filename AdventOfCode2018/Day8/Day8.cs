using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day8
{
    public class Day8 : PuzzleBase
    {
        public Day8() : base(8)
        {
        }

        protected override void DoExecutePart1()
        {
            var numbers = Input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var offset = 0;
            var root = GetNode(numbers, ref offset);

            var allNodes = root.Flatten();
            var metadataEntriesSum = allNodes.Sum(x => x.MetadataEntries.Sum());
            
            Console.WriteLine($"Sum of all metadata entries is {metadataEntriesSum}");
            
        }

        private Node GetNode(int[] numbers, ref int offset)
        {
            var result = new Node();
            
            var nrChildNodes = numbers[offset];
            var nrMetadataEntries = numbers[offset + 1];
            offset += 2;

            var remainingChildren = nrChildNodes;
            while (remainingChildren > 0)
            {
                var childNode = GetNode(numbers, ref offset);
                result.Nodes.Add(childNode);
                remainingChildren--;
            }


            var remainingMetadataEntries = nrMetadataEntries;
            while (remainingMetadataEntries > 0)
            {
                result.MetadataEntries.Add(numbers[offset]);
                offset += 1;
                remainingMetadataEntries--;
            }

            return result;
        }

        protected override void DoExecutePart2()
        {
            var numbers = Input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var offset = 0;
            var root = GetNode(numbers, ref offset);
            var result = GetNodeValue(root);

            Console.WriteLine($"The value of the root node is {result}");

        }

        private int GetNodeValue(Node node)
        {
            var result = 0;
            if (node.Nodes.Any())
            {
                node.MetadataEntries.ForEach(metadataEntry =>
                {
                    if (metadataEntry <= node.Nodes.Count)
                    {
                        result += GetNodeValue(node.Nodes[metadataEntry - 1]);
                    }
                });
            }
            else
            {
                result = node.MetadataEntries.Sum();
            }

            return result;
        }
    }

    public class Node
    {
        public List<Node> Nodes { get; }
        
        public List<int> MetadataEntries { get; }

        public Node()
        {
            Nodes = new List<Node>();
            MetadataEntries = new List<int>();
        }

        public List<Node> Flatten()
        {
            var result = new List<Node>();
            foreach (var childNode in Nodes)
            {
                result.AddRange(childNode.Flatten());
            }
            result.Add(this);

            return result;
        }
    }
}