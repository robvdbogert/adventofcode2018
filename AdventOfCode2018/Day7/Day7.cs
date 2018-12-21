using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Day7
{
    public class Day7 : PuzzleBase
    {
        public Day7() : base(7)
        {
        }

        protected override void DoExecutePart1()
        {
            var steps = GetSteps(GetInstructions(Input));
            
            var result = new StringBuilder();
            var done = false;
            while (!done)
            {
                // Find the first step with no dependencies
                var readyStep = steps.First(s => s.Value.IsReady);
                
                // Remove this step from any other steps' dependencies
                foreach (var step in steps.Values)
                {
                    step.RemoveDependency(readyStep.Key);
                }
                
                result.Append(readyStep.Key);
                steps.Remove(readyStep.Key);

                done = !steps.Any();
            }
            
            Console.WriteLine($"The order is {result}");
        }

        protected override void DoExecutePart2()
        {
            var steps = GetSteps(GetInstructions(Input));
            const int workerCount = 5;

            var done = false;
            var seconds = 0;
            var workers = Enumerable.Range(0, workerCount).Select(i => new Worker(i + 1)).ToList();
            while (!done)
            {
                // Get the first available worker
                var availableWorker = workers.FirstOrDefault(w => w.RemainingTime.GetValueOrDefault(0) == 0);
                var readyStep = steps.FirstOrDefault(s => s.Value.IsReady);
                while (availableWorker != null && readyStep.Key != null)
                {
                    // Assign work
                    availableWorker.CurrentStep = readyStep.Value;
                    availableWorker.RemainingTime = readyStep.Value.Duration;
                    steps.Remove(readyStep.Key);
                    
                    availableWorker = workers.FirstOrDefault(w => w.RemainingTime.GetValueOrDefault(0) == 0);
                    readyStep = steps.FirstOrDefault(s => s.Value.IsReady);
                }
                
                workers.ForEach(w =>
                {
                    if (w.CurrentStep == null)
                    {
                        return;
                    }
                    
                    w.RemainingTime--;
                    // When a step finishes, remove it from all other steps' dependencies
                    if (w.RemainingTime == 0)
                    {
                        foreach (var step in steps.Values)
                        {
                            step.RemoveDependency(w.CurrentStep.Name);
                        }

                        w.RemainingTime = null;
                        w.CurrentStep = null;
                    }
                });


                if (!steps.Any() && workers.All(w => w.CurrentStep == null))
                {
                    done = true;
                }
                seconds++;
            }
            
            Console.WriteLine($"Completing all steps took {seconds} seconds");
        }

        private List<Instruction> GetInstructions(string input)
        {
            return input
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => new Instruction
                {
                    Step = l[36].ToString(),
                    DependsOn = l[5].ToString()
                })
                .ToList();
        }

        private SortedDictionary<string, Step> GetSteps(List<Instruction> instructions)
        {
            var steps = new SortedDictionary<string, Step>();
            instructions
                .Select(i => i.Step)
                .Union(instructions.Select(i => i.DependsOn))
                .Distinct()
                .ToList()
                .ForEach(stepName =>
                {
                    steps.Add(stepName, new Step
                    {
                        Name = stepName
                    });
                });

            instructions.ForEach(instruction =>
            {
                steps[instruction.Step].AddDependency(instruction.DependsOn);
            });

            return steps;
        }
    }

    public class Instruction
    {
        public string Step { get; set; }

        public string DependsOn { get; set; }
    }

    public class Step
    {
        private readonly List<string> _dependencies = new List<string>();

        public string Name { get; set; }

        public int Duration => Name[0] - 4;

        public bool IsReady { get; private set; } = true;

        public void AddDependency(string name)
        {
            _dependencies.Add(name);
            IsReady = false;
        }

        public void RemoveDependency(string name)
        {
            if (!_dependencies.Contains(name))
            {
                return;
            }
            
            _dependencies.Remove(name);
            if (_dependencies.Count == 0)
            {
                IsReady = true;
            }
        }
    }

    public class Worker
    {
        public int Number { get; }
        
        public Step CurrentStep { get; set; }
        
        public int? RemainingTime { get; set; }

        public Worker(int number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return $"Worker {Number}";
        }
    }
}