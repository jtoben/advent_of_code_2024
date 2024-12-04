namespace Day1
{
    internal class Day1
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var firstList = input
                .Select(line => line.Split("   ")[0])
                .Select(int.Parse)
                .ToArray();
            var secondList = input
                .Select(line => line.Split("   ")[1])
                .Select(int.Parse)
                .ToArray();


            Console.WriteLine(PartOne(firstList, secondList));
            Console.WriteLine(PartTwo(firstList, secondList));
        }

        private static int PartOne(int[] leftList, int[] rightList)
        {
            leftList = [.. leftList.Order()];
            rightList = [.. rightList.Order()];

            int differenceSum = 0;
            for (int locationIndex = 0; locationIndex < leftList.Length; locationIndex++) {
                differenceSum += Math.Abs(leftList[locationIndex] - rightList[locationIndex]);
            }
            return differenceSum;
        }

        private static int PartTwo(int[] leftList, int[] rightList)
        {
            int similarityScore = 0;
            for (int locationIndex = 0; locationIndex < leftList.Length; locationIndex++) {
                similarityScore += leftList[locationIndex] * rightList.Count(number => number == leftList[locationIndex]);
            }

            return similarityScore;
        }
    }
}
