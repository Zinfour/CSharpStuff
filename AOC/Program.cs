// See https://aka.ms/new-console-template for more information



Console.WriteLine("Day1:");
Day1();
Console.WriteLine("Day2:");
Day2();
Console.WriteLine("Day3:");
Day3();
Console.WriteLine("Day4:");
Day4();
Console.WriteLine("Day5:");
Day5();


static void Day1()
{
    var puzzleInput = File.ReadLines("./puzzleInputs/day1.txt");

    int zeroesHit = 0;
    int zeroesHitWhenever = 0;
    int pointingAt = 50;
    foreach (string line in puzzleInput)
    {
        for (int step = 0; step < int.Parse(line[1..]); step++)
        {
            if (line[0] == 'R')
            {
                pointingAt = Mod(pointingAt + 1, 100);
            }
            else
            {
                pointingAt = Mod(pointingAt - 1, 100);
            }
            
            if (pointingAt == 0)
            {
                zeroesHitWhenever++;
            }
        }
        if (pointingAt == 0)
        {
            zeroesHit++;
        }
    }

    Console.WriteLine(zeroesHit);
    Console.WriteLine(zeroesHitWhenever);
}

static int Mod(int k, int n) {  return ((k %= n) < 0) ? k+n : k;  }

static void Day2()
{
    var puzzleInput = File.ReadAllText("./puzzleInputs/day2.txt");

    long part1Sum = 0;
    long part2Sum = 0;

    foreach (string pair in puzzleInput.Split(','))
    {
        string[] numbers = pair.Split('-');
        for (long i = long.Parse(numbers[0]); i <= long.Parse(numbers[1]); i++)
        {
            if (IsInvalid1(i.ToString())) part1Sum += i;
            if (IsInvalid2(i.ToString())) part2Sum += i;
        }
    }

    Console.WriteLine(part1Sum);
    Console.WriteLine(part2Sum);
}

static bool IsInvalid1(string text)
{
    return text[..(text.Length / 2)] == text[(text.Length / 2)..];
}

static bool IsInvalid2(string text)
{
    for (int i = 1; i <= text.Length/2; i++)
    {
        if (text.Length % i == 0)
        {
            string pattern = text[..i];
            bool invalid = true;
            for (int j = 0; j + i <= text.Length; j += i)
            {
                if (text[j..(j+i)] != pattern)
                {
                    invalid = false;
                    break;
                }
            }
            if (invalid)
            {
                return true;
            }
        }
    }
    return false;
}

static void Day3()
{
    var puzzleInput = File.ReadLines("./puzzleInputs/day3.txt");

    long sum1 = 0;
    long sum2 = 0;
    foreach (string line in puzzleInput)
    {
        sum1 += long.Parse(BestJoltage(2, line));
        sum2 += long.Parse(BestJoltage(12, line));
    }

    Console.WriteLine(sum1);
    Console.WriteLine(sum2);
}

static string BestJoltage(int batteries, string line)
{
    string bestJoltage = "";
    int startFrom = 0;
    while (bestJoltage.Length < batteries)
    {
        int highestValue = -1;
        int index = -1;
        for (int i = startFrom; i <= line.Length - (batteries - bestJoltage.Length); i++)
        {
            int n = int.Parse([line[i]]);
            if (n > highestValue)
            {
                index = i;
                highestValue = n;
            }
        }
        bestJoltage += highestValue;
        startFrom = index + 1;
    }
    return bestJoltage;
}

static void Day4()
{
    var puzzleInput = File.ReadLines("./puzzleInputs/day4.txt");

    List<List<bool>> grid = new List<List<bool>>();

    foreach (string line in puzzleInput)
    {

        List<bool> row = new List<bool>();
        foreach (char character in line)
        {
            row.Add(character == '@');
        }
        grid.Add(row);
    }
    int width = grid[0].Count;
    int height = grid.Count;

    Console.WriteLine(FindAccessible(grid, width, height).Count);

    int sum = 0;
    while (true)
    {
        List<(int, int)> coordinates = FindAccessible(grid, width, height);
        if (coordinates.Count == 0) break;
        sum += coordinates.Count;
        foreach ((int, int) coordinate in coordinates)
        {
            grid[coordinate.Item1][coordinate.Item2] = false;
        }
    }

    Console.WriteLine(sum);
}

static List<(int, int)> FindAccessible(List<List<bool>> grid, int width, int height)
{
    List<(int, int)> accessible = new List<(int, int)>();

    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            int count = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (0 <= i + x && i + x < height && 0 <= j + y && j + y < width && grid[i + x][j + y])
                    {
                        count += 1;
                    }
                }
            }

            if (grid[i][j] && count <= 4)
            {
                accessible.Add((i, j));
            }
        }
    }

    return accessible;
}

static void Day5()
{
    string puzzleInput = File.ReadAllText("./puzzleInputs/day5.txt");

    string[] split = puzzleInput.Split(new [] { "\n\n" }, StringSplitOptions.None);

    List<(long, long)> freshRanges = new List<(long, long)>();
    foreach (string line in split[0].Split('\n'))
    {
        string[] range = line.Split('-');
        freshRanges.Add((long.Parse(range[0]), long.Parse(range[1])));
    }
    

    int freshCount = 0;

    foreach (string unparsedIngredient in split[1].Split('\n', StringSplitOptions.RemoveEmptyEntries))
    {
        foreach ((long, long) bound in freshRanges)
        {
            long ingredient = long.Parse(unparsedIngredient);
            if (bound.Item1 <= ingredient && ingredient <= bound.Item2)
            {
                freshCount += 1;
                break;
            }
        }
    }
    Console.WriteLine(freshCount);

    List<(long, long)> mergedFreshRanges = new List<(long, long)>();

    foreach ((long, long) bound1 in freshRanges)
    {
        List<(long, long)> newMergedFreshRanges = new List<(long, long)>();
        long lowest = bound1.Item1;
        long highest = bound1.Item2;
        foreach ((long, long) bound2 in mergedFreshRanges)
        {
            if ((bound2.Item1 < lowest && bound2.Item2 < lowest) || (bound2.Item1 > highest && bound2.Item2 > highest))
            {
                newMergedFreshRanges.Add(bound2);
                continue;
            }
            
            if (bound2.Item1 <= lowest && bound2.Item2 >= lowest)
            {
                lowest = bound2.Item1;
            }
            
            if (bound2.Item1 <= highest && bound2.Item2 >= highest)
            {
                highest = bound2.Item2;
            }
        }
        newMergedFreshRanges.Add((lowest, highest));
        mergedFreshRanges = newMergedFreshRanges;
    }
    long sum = 0;
    foreach ((long, long) bound2 in mergedFreshRanges)
    {
        sum += bound2.Item2 - bound2.Item1 + 1;
    }

    Console.WriteLine(sum);
}

