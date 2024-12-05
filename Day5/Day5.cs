namespace Day5
{
    internal class Day5
    {
        static void Main()
        {
            var input = File.ReadAllText("input.txt")
                .Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Pairing[] rules = input[0]
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => new Pairing(int.Parse(line.Split('|')[0]), int.Parse(line.Split('|')[1])))
                    .ToArray();

            List<int[]> updates = input[1]
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Split(',').Select(int.Parse).ToArray())
                    .ToList();

            Console.WriteLine(PartOne(rules, updates));
            Console.WriteLine(PartTwo(rules, updates));
        }

        private static int PartOne(Pairing[] rules, List<int[]> updates)
        {
            return updates
                .Where(update => IsUpdateCorrect(update, rules))
                .Select(update => update[update.Length / 2])
                .Sum();
        }

        private static int PartTwo(Pairing[] rules, List<int[]> updates)
        {
            return updates
                .Where(update => !IsUpdateCorrect(update, rules))
                .Select(incorrectUpdate => {
                    bool isNowCorrect = false;

                    while (!isNowCorrect) {
                        for (int pageIndex = 1; pageIndex < incorrectUpdate.Length; pageIndex++) {
                            Pairing? brokenRule = GetBrokenRuleForPage(incorrectUpdate, pageIndex, rules);

                            if (brokenRule == null) {
                                continue;
                            }

                            int incorrectPageIndex = incorrectUpdate.ToList().IndexOf(brokenRule.After);
                            incorrectUpdate[pageIndex] = brokenRule.After;
                            incorrectUpdate[incorrectPageIndex] = brokenRule.Before;

                            if (IsUpdateCorrect(incorrectUpdate, rules)) {
                                isNowCorrect = true;
                                break;
                            }

                        }
                    }

                    return incorrectUpdate;
                })
                .Select(update => update[update.Length / 2])
                .Sum();
        }

        private static bool IsUpdateCorrect(int[] update, Pairing[] rules)
        {
            for (int pageIndex = 1; pageIndex < update.Length; pageIndex++) {
                if (GetBrokenRuleForPage(update, pageIndex, rules) != null) {
                    return false;
                }
            }

            return true;
        }

        private static Pairing? GetBrokenRuleForPage(int[] update, int pageIndex, Pairing[] rules)
        {
            Pairing[] relevantRules = rules.Where(rule => rule.Before == update[pageIndex]).ToArray();

            int[] partOfUpdateToCheck = update[0..pageIndex];

            return relevantRules
                                .Where(relevantRule => partOfUpdateToCheck.Contains(relevantRule.After))
                                .FirstOrDefault();
        }

        private record Pairing(int Before, int After);
    }
}
