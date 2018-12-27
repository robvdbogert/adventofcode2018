using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Day14
{
    public class Day14 : PuzzleBase
    {
        public Day14() : base(14)
        {
        }

        protected override void DoExecutePart1()
        {
            var recipesAtStart = "37";
            
            var recipes = recipesAtStart.Select(c => int.Parse(c.ToString())).ToList();

            var positionElf1 = 0;
            var positionElf2 = 1;

            var recipeLimit = 380621;
            var recipeCount = recipesAtStart.Length;
            while (recipeCount < recipeLimit + 10)
            {
                var sumOfRecipes = recipes[positionElf1] + recipes[positionElf2];
                recipes.AddRange(sumOfRecipes.ToString().Select(c => int.Parse(c.ToString())));

                positionElf1 = (positionElf1 + 1 + recipes[positionElf1]) % recipes.Count;
                positionElf2 = (positionElf2 + 1 + recipes[positionElf2]) % recipes.Count;
                
                // Console.WriteLine(string.Join(' ', recipes.Select(i => i.ToString())));

                recipeCount += sumOfRecipes.ToString().Length;
            }
            
            Console.WriteLine("The score of the next ten recipes is: {0}",
                string.Join("", recipes.GetRange(recipeLimit, 10).Select(i => i.ToString())));
        }

        protected override void DoExecutePart2()
        {
            var recipesAtStart = "37";

            var recipes = new List<int>();
            recipes.AddRange(recipesAtStart.Select(c => int.Parse(c.ToString())));
            
            var positionElf1 = 0;
            var positionElf2 = 1;

            var recipesToFind = new List<int> {3, 8, 0, 6, 2, 1};
            var recipesToFindCount = recipesToFind.Count;
            while (true)
            {
                var sumOfRecipes = recipes[positionElf1] + recipes[positionElf2];

                if (sumOfRecipes >= 10)
                {
                    recipes.Add(sumOfRecipes / 10);
                    if (recipes.Count > recipesToFindCount && 
                        recipes
                        .GetRange(recipes.Count - recipesToFindCount, recipesToFindCount)
                        .SequenceEqual(recipesToFind))
                    {
                        break;
                    }
                }
                recipes.Add(sumOfRecipes % 10);
                
                if (recipes.Count > recipesToFindCount && 
                    recipes
                    .GetRange(recipes.Count - recipesToFindCount, recipesToFindCount)
                    .SequenceEqual(recipesToFind))
                {
                    break;
                }
                

                positionElf1 = (positionElf1 + 1 + recipes[positionElf1]) % recipes.Count;
                positionElf2 = (positionElf2 + 1 + recipes[positionElf2]) % recipes.Count;
            }

            Console.WriteLine("{0} was found after {1} recipes",
                new string(recipesToFind.Select(r => r.ToString()[0]).ToArray()),
                recipes.Count - recipesToFindCount
            );
        }
    }
}