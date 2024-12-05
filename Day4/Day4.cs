namespace Day4
{
    internal class Day4
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            char[,] wordSearchBoard = new char[input[0].Length, input.Length];
            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Length; y++) {
                    wordSearchBoard[x, y] = input[y][x];
                }
            }

            Console.WriteLine(PartOne(wordSearchBoard));
            Console.WriteLine(PartTwo(wordSearchBoard));
        }

        private static int PartOne(char[,] wordSearchBoard)
        {
            List<Vector2> xPositions = GetPositions(wordSearchBoard, 'X');
            List<Vector2> directions = [
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
            ];

            return xPositions
                .Select(xPosition => {
                    return directions.Select(direction => SearchForWord(wordSearchBoard, xPosition, direction, "XMAS"))
                    .Count(result => result);
                })
                .Sum();
        }

        private static int PartTwo(char[,] wordSearchBoard)
        {
            List<Vector2> aPositions = GetPositions(wordSearchBoard, 'A');
            List<Vector2> directions = [
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-1, 1),
            ];

            return aPositions
                .Select(aPosition => {
                    return directions.Select(direction => SearchForWord(wordSearchBoard, aPosition - direction, direction, "MAS"))
                    .Count(result => result) == 2; // Definition of X-MAS is MAS in two diagonal directions, so an aPosition is only valid if it finds 2 MAS words.
                })
                .Count(result => result);
        }

        private static bool SearchForWord(char[,] wordSearchBoard, Vector2 position, Vector2 direction, string word)
        {
            for (int charIndex = 0; charIndex < word.Length; charIndex++) {
                Vector2 currentPosition = position + direction * charIndex;

                if (currentPosition.X < 0 || currentPosition.Y < 0 || currentPosition.X >= wordSearchBoard.GetLength(0) || currentPosition.Y >= wordSearchBoard.GetLength(1)) {
                    // Out of bounds.
                    return false;
                }

                if (wordSearchBoard[currentPosition.X, currentPosition.Y] != word[charIndex]) {
                    return false;
                }
            }

            return true;
        }

        private static List<Vector2> GetPositions(char[,] wordSearchBoard, char charToFind)
        {
            List<Vector2> positions = [];
            for (int y = 0; y < wordSearchBoard.GetLength(1); y++) {
                for (int x = 0; x < wordSearchBoard.GetLength(0); x++) {
                    if (wordSearchBoard[x, y] == charToFind) {
                        positions.Add(new Vector2(x, y));
                    }
                }
            }

            return positions;
        }

        private record Vector2(int X, int Y)
        {
            public static Vector2 operator +(Vector2 value1, Vector2 value2)
            {
                return new Vector2(value1.X + value2.X, value1.Y + value2.Y);
            }

            public static Vector2 operator -(Vector2 value1, Vector2 value2)
            {
                return new Vector2(value1.X - value2.X, value1.Y - value2.Y);
            }

            public static Vector2 operator *(Vector2 value1, int multiplier)
            {
                return new Vector2(value1.X * multiplier, value1.Y * multiplier);
            }

            public int XSize => Math.Abs(X);
            public int YSize => Math.Abs(Y);
        }
    }
}
