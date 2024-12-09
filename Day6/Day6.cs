namespace Day6
{
    internal class Day6
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            char[,] map = new char[input[0].Length, input.Length];

            Vector2 startPosition = new(0, 0);
            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Length; y++) {
                    map[x, y] = input[y][x];

                    if (map[x, y] == '^') {
                        startPosition = new Vector2(x, y);
                    }
                }
            }

            Console.WriteLine(PartOne(map, startPosition));
            Console.WriteLine(PartTwo(map, startPosition));
        }

        private static int PartOne(char[,] map, Vector2 currentPosition)
        {
            return GetGuardPath(map, currentPosition).Count;
        }

        private static int PartTwo(char[,] map, Vector2 startPosition)
        {
            List<Vector2> guardPath = GetGuardPath(map, startPosition);

            return guardPath
                .Where(guardLocation => guardLocation != startPosition)
                .Where(guardPosition => {
                    char[,] copyOfMap = Copy(map);
                    copyOfMap[guardPosition.X, guardPosition.Y] = '#';

                    return GetGuardPath(copyOfMap, startPosition).Count == 0;
                })
                .Count();
        }

        private static List<Vector2> GetGuardPath(char[,] map, Vector2 currentPosition)
        {
            Vector2 direction = new(0, -1);
            bool finished = false;
            bool guardInALoop = false;
            Dictionary<Vector2, List<Vector2>> turnPositionsByDirections = [];

            List<Vector2> guardPositions = [currentPosition];

            while (!finished) {
                Vector2 newPosition = currentPosition + direction;

                if (map[newPosition.X, newPosition.Y] == '#') {

                    // Rotate direction by 90 degrees.
                    direction = new(-direction.Y, direction.X);

                    if (turnPositionsByDirections.TryGetValue(direction, out List<Vector2>? positions)) {
                        if (positions.Contains(newPosition)) {
                            // Bump into same spot twice for identical direction, loop detected.
                            guardInALoop = true;
                            finished = true;
                        } else {
                            positions.Add(newPosition);
                        }
                    } else {
                        turnPositionsByDirections[direction] = [];
                    }
                    continue;
                }

                currentPosition = newPosition;
                guardPositions.Add(currentPosition);

                // Do edge of map check.
                if (currentPosition.X == 0 || currentPosition.Y == 0 || currentPosition.X == map.GetLength(0) - 1 || currentPosition.Y == map.GetLength(1) - 1) {
                    finished = true;
                }
            }

            if (guardInALoop) {
                return [];
            }

            return guardPositions.Distinct().ToList();
        }

        private static char[,] Copy(char[,] original)
        {
            char[,] copy = new char[original.GetLength(0), original.GetLength(1)];
            for (int x = 0; x < original.GetLength(0); x++) {
                for (int y = 0; y < original.GetLength(1); y++) {
                    copy[x, y] = original[x, y];
                }
            }

            return copy;

        }

        private record Vector2(int X, int Y)
        {
            public static Vector2 operator +(Vector2 value1, Vector2 value2) => new(value1.X + value2.X, value1.Y + value2.Y);
        }
    }
}
