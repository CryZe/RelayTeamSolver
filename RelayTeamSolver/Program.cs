using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayTeamSolver
{

    public static class Enumerable
    {
        public static IEnumerable<IEnumerable<T>> AllPermutations<T>(this IEnumerable<T> enumerable, int takeCount = -1)
        {
            if (!enumerable.Skip(1).Any())
                yield return enumerable;
            else foreach (var firstElement in enumerable)
            {
                if (takeCount == 1)
                {
                    yield return new[] { firstElement };
                }
                else
                {
                    var allSubPermutations = enumerable.Where(x => !Object.Equals(x, firstElement)).ToList().AllPermutations(takeCount - 1);
                    foreach (var subPermutation in allSubPermutations)
                    {
                        var result = new[] { firstElement }.Concat(subPermutation);

                        yield return result;
                    }
                }
            }
        }

        public static bool Contains(this IEnumerable<Entry> enumerable, String name)
        {
            return enumerable.FirstOrDefault(x => x?.Name == name) != null;
        }
    }

    public class Entry
    {
        public String Name { get; }
        public TimeSpan Time { get; }

        public Entry(String name, TimeSpan time)
        {
            Name = name;
            Time = time;
        }

        public override string ToString() => "\{Name} (\{Time})";
    }

    class Program
    {
        static readonly Entry[] OcarinaOfTime = new Entry[]
        {
            //new Entry("Piticarus", TimeSpan.Parse("01:26:00")),
            //new Entry("Marco", TimeSpan.Parse("01:26:30")),
            //new Entry("zfg", TimeSpan.Parse("01:27:00")),
            //new Entry("elorkeloff", TimeSpan.Parse("01:27:30")),
            new Entry("Glitchymon", TimeSpan.Parse("01:30:00")),
            new Entry("moose1137", TimeSpan.Parse("01:30:00")),
            new Entry("Apasher", TimeSpan.Parse("01:35:00")),
            new Entry("silentknight115", TimeSpan.Parse("01:35:00")),
            new Entry("Sniping117", TimeSpan.Parse("01:35:00")),
        };

        static readonly Entry[] MajorasMask = new Entry[]
        {
            //new Entry("EnNopp112", TimeSpan.Parse("01:30:00")),
            //new Entry("majinphil", TimeSpan.Parse("01:32:30")),
            //new Entry("thiefbug", TimeSpan.Parse("01:34:00")),
            new Entry("VPP", TimeSpan.Parse("01:35:00")),
            //new Entry("FuryRising", TimeSpan.Parse("01:40:00")),
            new Entry("Zelkys", TimeSpan.Parse("01:51:52")),
            new Entry("Bob5858", TimeSpan.Parse("01:54:00")),
            new Entry("mtmannion", TimeSpan.Parse("02:15:00")),
        };

        static readonly Entry[] WindWaker = new Entry[]
        {
            //new Entry("Demon9", TimeSpan.Parse("04:20:00")),
            //new Entry("Ace", TimeSpan.Parse("04:20:00")),
            //new Entry("Chasetopher", TimeSpan.Parse("04:30:00")),
            //new Entry("hood_rad", TimeSpan.Parse("04:50:00")),
            new Entry("TrogWW", TimeSpan.Parse("04:50:00")),
            new Entry("wooferzfg1", TimeSpan.Parse("04:55:00")),
            new Entry("Zellpree", TimeSpan.Parse("05:00:00")),
            new Entry("Chilla", TimeSpan.Parse("05:20:00")),
            //new Entry("Goldphnx", TimeSpan.Parse("05:30:00")),
        };

        static readonly Entry[] TwilightPrincess = new Entry[]
        {
            //new Entry("Giradam", TimeSpan.Parse("03:10:00")),
            //new Entry("Zaf1re", TimeSpan.Parse("03:12:03")),
            new Entry("Rodner", TimeSpan.Parse("03:13:00")),
            //new Entry("Skyreon", TimeSpan.Parse("03:18:00")),
            //new Entry("Pheenoh", TimeSpan.Parse("03:20:00")),
            new Entry("Zayloox", TimeSpan.Parse("03:21:30")),
            new Entry("chobin50", TimeSpan.Parse("03:22:00")),
            new Entry("Domiok", TimeSpan.Parse("03:23:00")),
            new Entry("Kejsmaster", TimeSpan.Parse("03:25:00")),
            new Entry("Mofat", TimeSpan.Parse("03:35:00")),
        };

        static readonly Entry[] SkywardSword = new Entry[]
        {
            //new Entry("Tenderhearted", TimeSpan.Parse("05:20:00")),
            //new Entry("TestRunner", TimeSpan.Parse("05:30:00")),
            new Entry("Bob_Loblaw_Law", TimeSpan.Parse("05:31:00")),
            //new Entry("sva", TimeSpan.Parse("05:35:00")),
            //new Entry("tlozsr", TimeSpan.Parse("05:35:00")),
            new Entry("Bananas", TimeSpan.Parse("05:50:00")),
            new Entry("heero_fred", TimeSpan.Parse("06:15:00")),
            new Entry("Phionex", TimeSpan.Parse("06:40:00")),
        };

        static void Main(string[] args)
        {
            var teams = new Entry[4, 5];
            TimeSpan[] times = new TimeSpan[teams.GetLength(0)];
            TimeSpan[] bestTimes = times;
            var bestTeams = teams;
            var bestTimeDifference = TimeSpan.MaxValue;

            Action<int, IEnumerable<Entry>> assignTeams = (i, p) =>
            {
                var pArr = p.ToList();
                for (var teamId = 0; teamId < teams.GetLength(0); ++teamId)
                {
                    teams[teamId, i] = p.ElementAt(teamId);
                }
            };


            foreach (var oot in OcarinaOfTime.AllPermutations(4))
            {
                assignTeams(0, oot);

                foreach (var mm in MajorasMask.AllPermutations(4))
                {
                    assignTeams(1, mm);

                    foreach (var tww in WindWaker.AllPermutations(4))
                    {
                        assignTeams(2, tww);

                        foreach (var tp in TwilightPrincess.AllPermutations(4))
                        {
                            assignTeams(3, tp);

                            foreach (var ss in SkywardSword.AllPermutations(4))
                            {
                                assignTeams(4, ss);

                                if (!CheckTeams(teams))
                                    continue;

                                CalculateTimes(teams, times);

                                var timeDifference = times.Max() - times.Min();

                                if (timeDifference < bestTimeDifference)
                                {
                                    bestTeams = (Entry[,])teams.Clone();
                                    bestTimes = (TimeSpan[])times.Clone();
                                    bestTimeDifference = timeDifference;
                                    Console.WriteLine("\n\nTime Difference: \{bestTimeDifference}");

                                    PrintTeams(bestTeams, bestTimes);
                                }
                            }
                        }
                    }
                }
            }

            PrintTeams(bestTeams, bestTimes);
        }

        private static bool CheckTeams(Entry[,] teams)
        {
            for (var teamId = 0; teamId < teams.GetLength(0); ++teamId)
            {
                var team = GetTeam(teams, teamId);

                if (!CheckTeam(team))
                    return false;
            }

            return true;
        }

        private static bool CheckTeam(IEnumerable<Entry> team)
        {
            return !(team.Contains("wooferzfg1") && team.Contains("Goldphnx"));
        }

        private static IEnumerable<Entry> GetTeam(Entry[,] teams, int teamId)
        {
            for (var gameId = 0; gameId < teams.GetLength(1); ++gameId)
            {
                yield return teams[teamId, gameId];
            }
        }

        private static void PrintTeams(Entry[,] teams, TimeSpan[] times)
        {
            for (var teamId = 0; teamId < teams.GetLength(0); ++teamId)
            {
                Console.Write("Team \{teamId + 1} (\{times[teamId]}): ");
                for (var gameId = 0; gameId < teams.GetLength(1); ++gameId)
                {
                    var entrant = teams[teamId, gameId];
                    if (gameId != 0)
                        Console.Write(", ");
                    Console.Write(entrant);
                }

                Console.WriteLine();
            }
        }

        private static void CalculateTimes(Entry[,] teams, TimeSpan[] times)
        {
            for (var teamId = 0; teamId < teams.GetLength(0); ++teamId)
            {
                TimeSpan teamTime = TimeSpan.Zero;

                for (var gameId = 0; gameId < teams.GetLength(1); ++gameId)
                {
                    teamTime += teams[teamId, gameId].Time;
                }

                times[teamId] = teamTime;
            }
        }
    }
}
