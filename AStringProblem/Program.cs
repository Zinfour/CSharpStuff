using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace AStringProblem
{
    internal class Program
    {
        // A solution for https://open.kattis.com/problems/stringproblem.
        static void Main(string[] args)
        {
            int number_of_strings = int.Parse(Console.ReadLine()!);

            List<int[]> all_strings = new List<int[]>();

            for (int i = 0; i < number_of_strings; i++)
            {
                string[] split_line = Console.ReadLine()!.Split(' ');

                all_strings.Add(new int[] { int.Parse(split_line[0]), int.Parse(split_line[1]) });
            }

            int N = all_strings.Count;
            // Count how many lines we have in each direction. The direction of
            // a line from pin x to pin y is (x + y) % 2N. A direction is defined so
            // that a line in direction d from pin 0 will point to pin d. You can find
            // the pin in a given direction d starting from pin x using (d - x) % 2N.
            int[] line_direction_count = new int[2 * N];
            foreach (int[] line in all_strings)
            {
                line_direction_count[(line[0] + line[1]) % (2 * N)]++;
            }
            // Find the direction with the least number of lines we have to fix.
            int best_direction = -1;
            int minimum_faulty_lines = int.MaxValue;
            for (int d = 1; d < 2 * N; d += 2)
            {
                if (N - line_direction_count[d] < minimum_faulty_lines)
                {
                    minimum_faulty_lines = N - line_direction_count[d];
                    best_direction = d;
                }
            }

            Console.WriteLine(minimum_faulty_lines);
            GenerateInstructions(best_direction, all_strings);
        }

        // Given a direction and a list of strings, print instructions for how to
        // rearrange the string to all point in the given direction. The basic idea
        // is that we have an outer loop fixing every line to point in the correct
        // direction. To avoid the situation where we block both pins of a line
        // so that it would have to move both of its pins, we make sure that when we fix
        // one line and it touches another line, we fix it and then repeat until there are
        // no lines touching before we continue. That way each line will only have to move
        // at most one pin.
        private static void GenerateInstructions(int direction, List<int[]> all_strings)
        {
            int N = all_strings.Count;
            Debug.Assert(direction % 2 == 1, "the direction must be odd");

            // We store a lookup table from pins to lines for O(1) lookup.
            int[] pin_to_line = new int[2 * N];
            for (int i = 0; i < 2 * N; i++)
            {
                pin_to_line[i] = -1;
            }
            for (int i = 0; i < N; i++)
            {
                pin_to_line[all_strings[i][0]] = i;
                pin_to_line[all_strings[i][1]] = i;
            }

            for (int i = 0; i < N; i++)
            {
                int current_line = i;
                int pin_to_move = all_strings[current_line][0];

                while (all_strings[current_line][0] != (direction - all_strings[current_line][1] + (2 * N)) % (2 * N))
                {
                    int new_pin;
                    int old_pin = pin_to_move;

                    if (pin_to_move == all_strings[current_line][0])
                    {
                        new_pin = (direction - all_strings[current_line][1] + (2 * N)) % (2 * N);
                        all_strings[current_line] = new int[] { new_pin, all_strings[current_line][1] };
                    }
                    else
                    {
                        Debug.Assert(pin_to_move == all_strings[current_line][1]);
                        new_pin = (direction - all_strings[current_line][0] + (2 * N)) % (2 * N);
                        all_strings[current_line] = new int[] { all_strings[current_line][0], new_pin };
                    }

                    pin_to_line[old_pin] = -1;
                    int next_line = pin_to_line[new_pin];
                    pin_to_line[new_pin] = current_line;

                    Console.WriteLine("{0} {1} {2}", current_line, old_pin, new_pin);

                    if (next_line != -1 && next_line != current_line)
                    {
                        current_line = next_line;
                        pin_to_move = new_pin;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

}