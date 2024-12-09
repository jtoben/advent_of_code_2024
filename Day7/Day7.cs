namespace Day7
{
    internal class Day7
    {
        static void Main()
        {
            Equation[] input = File.ReadAllLines("input.txt")
                .Select(line => new Equation(line.Split(": ")[1].Split(' ').Select(long.Parse).ToArray(), long.Parse(line.Split(": ")[0])))
                .ToArray();

            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        private static long PartOne(Equation[] input)
        {
            return input
                .Select(equation => GetAllPermutations(equation.Numbers.Length - 1, ['*', '+'])
                    .Select(permutation => SolveEquation(equation.Numbers, permutation))
                    .Where(solution => solution == equation.Result)
                    .FirstOrDefault())
                .Sum();
        }

        private static long PartTwo(Equation[] input)
        {
            return input
                .Select(equation => GetAllPermutations(equation.Numbers.Length - 1, ['*', '+', '|'])
                    .Select(permutation => SolveEquation(equation.Numbers, permutation))
                    .Where(solution => solution == equation.Result)
                    .FirstOrDefault())
                .Sum();
        }

        private static List<string> GetAllPermutations(int length, char[] options, List<string>? permutations = null)
        {
            List<string> newPermutations = [];
            if (permutations == null) {
                foreach (char option in options) {
                    newPermutations.Add("" + option);
                }
            } else {
                foreach (string permutation in permutations) {
                    foreach (char option in options) {

                        newPermutations.Add(permutation + option);
                    }
                }
            }

            if (length > 1) {
                return GetAllPermutations(length - 1, options, newPermutations);
            } else {
                return newPermutations;
            }
        }

        private static long SolveEquation(long[] numbers, string permutation)
        {
            long solution = numbers[0];
            for (int permutationIndex = 0; permutationIndex < permutation.Length; permutationIndex++) {
                solution = permutation[permutationIndex] switch {
                    '*' => solution * numbers[permutationIndex + 1],
                    '+' => solution + numbers[permutationIndex + 1],
                    '|' => long.Parse("" + solution + numbers[permutationIndex + 1]),
                    _ => throw new NotImplementedException(),
                };
            }

            return solution;
        }

        private record Equation(long[] Numbers, long Result);
    }
}
