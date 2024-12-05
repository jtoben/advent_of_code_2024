namespace Day2
{
    internal class Day2
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt")
                .Select(line => line.Split(" "))
                .Select(line => line.Select(int.Parse).ToArray())
                .ToArray();

            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        private static int PartOne(int[][] input)
        {
            return input
                .Where(CheckReport)
                .Count();
        }

        private static int PartTwo(int[][] input)
        {
            return input
                .Where(CheckReportWithTolerance)
                .Count();
        }

        private static bool CheckReportWithTolerance(int[] report)
        {
            bool correct = CheckReport(report);

            if (!correct) {
                // Incorrect report, remove 1st number, then 2nd etc, until report = correct
                for (int levelIndex = 0; levelIndex <= report.Length - 1; levelIndex++) {
                    var adaptedReport = report.Where((_, index) => index != levelIndex).ToArray();

                    if (CheckReport(adaptedReport)) {
                        correct = true;
                        break;
                    }
                }
            }

            return correct;
        }

        private static bool CheckReport(int[] report)
        {
            bool correct = true;
            bool decreasing = report[0] > report[^1];

            for (int levelIndex = 0; levelIndex < report.Length - 1; levelIndex++) {
                if (!CheckTwoLevels(report[levelIndex], report[levelIndex + 1], decreasing)) {
                    correct = false;
                    break;
                }
            }

            return correct;
        }

        private const int _maxDifference = 3;
        private static bool CheckTwoLevels(int levelA, int levelB, bool decreasing)
        {
            if (Math.Abs(levelA - levelB) > _maxDifference) {
                return false;
            }

            if (decreasing != levelA > levelB) {
                return false;
            }

            return levelA != levelB;
        }
    }
}
