using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day4
{
    public class Day4 : PuzzleBase
    {
        public Day4(): base(4)
        {
        }

        protected override void DoExecutePart1()
        {
            var logEntries = GetLogEntries(Input);
            logEntries = logEntries.OrderBy(l => l.DateTime).ToList();

            var guardSleepPeriods = GetGuardSleepPeriods(logEntries);

            var guardWithMostSleep = guardSleepPeriods
                .OrderByDescending(g => g.Value.Sum(p => p.MinutesAsleep))
                .First();
            
            Console.WriteLine(
                $"Guard {guardWithMostSleep.Key} slept most ({guardWithMostSleep.Value.Sum(p => p.MinutesAsleep)} minutes)");

            var minutes = GetSleepCountsPerMinute(guardWithMostSleep.Value);

            var minuteWithHighestSleepCount = GetMinuteWithHighestSleepCount(minutes);
           
            Console.WriteLine($"Minute with most sleep = {minuteWithHighestSleepCount}");
            Console.WriteLine($"Answer is {guardWithMostSleep.Key * minuteWithHighestSleepCount}");
        }

        protected override void DoExecutePart2()
        {
            var logEntries = GetLogEntries(Input);
            logEntries = logEntries.OrderBy(l => l.DateTime).ToList();

            var guardSleepPeriods = GetGuardSleepPeriods(logEntries);

            var highestSleepCount = -1;
            var minute = -1;
            var guardId = -1;
            foreach (var guardSleepData in guardSleepPeriods)
            {
                var sleepCountPerMinute = GetSleepCountsPerMinute(guardSleepData.Value);
                var minuteWithHighestSleepCount = GetMinuteWithHighestSleepCount(sleepCountPerMinute);

                if (sleepCountPerMinute[minuteWithHighestSleepCount] > highestSleepCount)
                {
                    guardId = guardSleepData.Key;
                    highestSleepCount = sleepCountPerMinute[minuteWithHighestSleepCount];
                    minute = minuteWithHighestSleepCount;
                }
            }
            
            Console.WriteLine($"Guard {guardId} was most asleep on minute {minute} ({highestSleepCount} times)");
            Console.WriteLine($"Answer is {guardId * minute}");
        }
        
        private Dictionary<int, List<Period>> GetGuardSleepPeriods(List<LogEntry> logEntries)
        {
            var currentGuardId = logEntries.First().GuardId;
            var startSleep = DateTime.MinValue;
            var guardSleepPeriods = new Dictionary<int, List<Period>>();
            logEntries.ForEach(logEntry =>
            {
                switch (logEntry.Event)
                {
                    case "falls asleep":
                        startSleep = logEntry.DateTime;
                        break;
                    case "wakes up":
                        guardSleepPeriods[currentGuardId].Add(new Period
                        {
                            Start = startSleep,
                            End = logEntry.DateTime
                        });
                        break;
                    case "begins shift":
                        currentGuardId = logEntry.GuardId;
                        if (!guardSleepPeriods.ContainsKey(currentGuardId))
                        {
                            guardSleepPeriods[currentGuardId] = new List<Period>();
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            return guardSleepPeriods;
        }

        private int[] GetSleepCountsPerMinute(List<Period> sleepPeriods)
        {
            var minutes = new int[60];
            sleepPeriods.ForEach(period =>
            {
                var time = period.Start < period.End.Date ? period.End.Date : period.Start;
                while (time < period.End)
                {
                    minutes[time.Minute]++;
                    time = time.AddMinutes(1);
                }
            });
            return minutes;
        }
        
        private int GetMinuteWithHighestSleepCount(int[] sleepCountPerMinute)
        {
            var maxValue = 0;
            var iMax = 0;
            for (var i = 0; i < sleepCountPerMinute.Length; i++)
            {
                if (sleepCountPerMinute[i] > maxValue)
                {
                    maxValue = sleepCountPerMinute[i];
                    iMax = i;
                }
            }

            return iMax;
        }

        private List<LogEntry> GetLogEntries(string input) 
        {
            var regex = new Regex(
                @"\[(?<date>\d{4}-\d{2}-\d{2} \d{2}:\d{2})\]\s(Guard #(?<id>[\d]+))*\s*(?<event>.*)", 
                RegexOptions.Compiled | RegexOptions.Multiline);

            var result = new List<LogEntry>();

            foreach(Match match in regex.Matches(input)) 
            {
                result.Add(new LogEntry
                {
                    DateTime = DateTime.Parse(match.Groups["date"].Value),
                    Event = match.Groups["event"].Value,
                    GuardId = match.Groups["id"].Success ? int.Parse(match.Groups["id"].Value) : 0
                });
            }

            return result;
        }
    }

    public class LogEntry 
    {
        public DateTime DateTime { get; set; }
        public int GuardId { get; set; }
        public string Event { get; set; }
    }

    public class Period
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int MinutesAsleep => (int)(End - Start).TotalMinutes;
    }
}
