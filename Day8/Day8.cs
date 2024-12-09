namespace Day8
{
    internal class Day8
    {
        static void Main()
        {
            string[] input = File.ReadAllLines("input.txt");
            char[,] map = new char[input[0].Length, input.Length];

            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Length; y++) {
                    map[x, y] = input[y][x];
                }
            }

            Console.WriteLine(PartOne(map));
            Console.WriteLine(PartTwo(map));
        }

        private static int PartOne(char[,] map)
        {
            var distinctFrequencies = GetDistinctFrequencies(map);

            List<Vector2> antinodeLocations = [];
            foreach (char frequency in distinctFrequencies) {
                List<Vector2> locations = FindAllLocationsForFrequency(frequency, map);

                for (int i = 0; i < locations.Count; i++) {
                    for (int j = i + 1; j < locations.Count; j++) {
                        // Loop over each pair.
                        Vector2 distance = locations[i] - locations[j];

                        antinodeLocations.Add(locations[i] + distance);
                        antinodeLocations.Add(locations[j] - distance);
                    }
                }
            }

            return CalculateDistinctAntinodes(antinodeLocations, map);
        }

        private static int PartTwo(char[,] map)
        {
            var distinctFrequencies = GetDistinctFrequencies(map);

            List<Vector2> antinodeLocations = [];
            foreach (char frequency in distinctFrequencies) {
                List<Vector2> locations = FindAllLocationsForFrequency(frequency, map);

                for (int i = 0; i < locations.Count; i++) {
                    for (int j = i + 1; j < locations.Count; j++) {
                        // Loop over each pair.
                        Vector2 distance = locations[i] - locations[j];

                        antinodeLocations.AddRange(Enumerable.Range(0, 100)
                            .Select(index => {
                                return new Vector2[] { locations[i] + distance * index, locations[i] - distance * index };
                            })
                            .SelectMany(x => x));
                    }
                }
            }

            return CalculateDistinctAntinodes(antinodeLocations, map);
        }

        private static char[] GetDistinctFrequencies(char[,] map) => map
                .Cast<char>()
                .Distinct()
                .Where(frequency => frequency != '.')
                .ToArray();

        private static int CalculateDistinctAntinodes(List<Vector2> antinodeLocations, char[,] map) => antinodeLocations
                .Distinct()
                .Where(location => location.X >= 0 && location.Y >= 0 && location.X < map.GetLength(0) && location.Y < map.GetLength(1))
                .Count();

        private static List<Vector2> FindAllLocationsForFrequency(char frequency, char[,] map)
        {
            List<Vector2> locations = [];
            for (int x = 0; x < map.GetLength(0); x++) {
                for (int y = 0; y < map.GetLength(1); y++) {
                    if (map[x, y] == frequency) {
                        locations.Add(new Vector2(x, y));
                    }
                }
            }

            return locations;
        }

        private record Vector2(int X, int Y)
        {
            public static Vector2 operator -(Vector2 value1, Vector2 value2) => new(value1.X - value2.X, value1.Y - value2.Y);

            public static Vector2 operator +(Vector2 value1, Vector2 value2) => new(value1.X + value2.X, value1.Y + value2.Y);

            public static Vector2 operator *(Vector2 value1, int multiplier) => new(value1.X * multiplier, value1.Y * multiplier);
        }
    }
}
