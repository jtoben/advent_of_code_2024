namespace Day3
{
    internal class Day3
    {
        static void Main()
        {
            var input = File.ReadAllText("input.txt");

            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        private static int PartOne(string program)
        {
            return GetInstructionIndexes(program)
                .Select(instructionIndex => CalculateMultiplicationForInstruction(program, instructionIndex))
                .Sum();
        }

        private static int PartTwo(string program)
        {
            List<(int startIndex, int endIndex)> forbiddenInstructionRanges = GetForbiddenInstructionRanges(program);

            return GetInstructionIndexes(program)
                .Where(instructionIndex => !forbiddenInstructionRanges.Any(range => instructionIndex.startIndex > range.startIndex && instructionIndex.startIndex < range.endIndex))
                .Select(instructionIndex => CalculateMultiplicationForInstruction(program, instructionIndex))
                .Sum();
        }

        private static int CalculateMultiplicationForInstruction(string program, (int startIndex, int endIndex) instructionIndex)
        {
            const int instructionLength = 4;

            string instruction = program.Substring(instructionIndex.startIndex + instructionLength, instructionIndex.endIndex - instructionIndex.startIndex - instructionLength);

            string[] parts = instruction.Split(',');
            if (parts.Length != 2) {
                return 0;
            }

            if (int.TryParse(parts[0], out int firstNumber) && int.TryParse(parts[1], out int secondNumber)) {
                return firstNumber * secondNumber;
            }

            return 0;
        }

        private static List<(int startIndex, int endIndex)> GetInstructionIndexes(string program)
        {
            const string mulInstruction = "mul(";
            const char endOfMulInstruction = ')';

            List<(int startIndex, int endIndex)> instructionIndexes = [];
            for (int instructionIndex = program.IndexOf(mulInstruction); instructionIndex > -1; instructionIndex = program.IndexOf(mulInstruction, instructionIndex + 1)) {
                int endOfInstructionIndex = program.IndexOf(endOfMulInstruction, instructionIndex);
                if (endOfInstructionIndex == -1) {
                    continue;
                }

                instructionIndexes.Add(new(instructionIndex, endOfInstructionIndex));
            }

            return instructionIndexes;
        }

        private static List<(int startIndex, int endIndex)> GetForbiddenInstructionRanges(string program)
        {
            const string dontInstruction = "don't()";
            const string doInstruction = "do()";

            List<(int startIndex, int endIndex)> forbiddenInstructionRanges = [];

            for (int forbiddenInstructionIndex = program.IndexOf(dontInstruction); forbiddenInstructionIndex > -1; forbiddenInstructionIndex = program.IndexOf(dontInstruction, forbiddenInstructionIndex + 1)) {
                int endOfForbiddenInstructionIndex = program.IndexOf(doInstruction, forbiddenInstructionIndex);

                if (endOfForbiddenInstructionIndex == -1) {
                    forbiddenInstructionRanges.Add(new(forbiddenInstructionIndex, program.Length));
                    break;
                }

                forbiddenInstructionRanges.Add(new(forbiddenInstructionIndex, endOfForbiddenInstructionIndex));
                forbiddenInstructionIndex = endOfForbiddenInstructionIndex;
            }

            return forbiddenInstructionRanges;
        }
    }
}
