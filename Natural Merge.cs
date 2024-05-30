using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace KP;

public static class NaturalMerge
{
    public static void NaturalMergeSort(int[] array)
    {
        if (array.Length <= 1)
        {
            return;
        }

        List<int[]> runs = FindRuns(array);
        while (runs.Count > 1)
        {
            List<int[]> newRuns = new List<int[]>();

            for (int i = 0; i < runs.Count; i += 2)
            {
                if (i + 1 < runs.Count)
                {
                    newRuns.Add(Merge(runs[i], runs[i + 1]));
                }
                else
                {
                    newRuns.Add(runs[i]);
                }
            }

            runs = newRuns;
            Console.WriteLine(runs.Count);
        }

        Array.Copy(runs[0], array, array.Length);
    }

    private static List<int[]> FindRuns(int[] array)
    {
        List<int[]> runs = new List<int[]>();
        int start = 0;

        while (start < array.Length)
        {
            int end = start + 1;

            while (end < array.Length && array[end - 1] <= array[end])
            {
                end++;
            }

            int[] run = new int[end - start];
            Array.Copy(array, start, run, 0, run.Length);
            runs.Add(run);

            start = end;
        }

        return runs;
    }

    private static int[] Merge(int[] left, int[] right)
    {
        int[] result = new int[left.Length + right.Length];
        int i = 0, j = 0, k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
            {
                result[k++] = left[i++];
            }
            else
            {
                result[k++] = right[j++];
            }
        }

        while (i < left.Length)
        {
            result[k++] = left[i++];
        }

        while (j < right.Length)
        {
            result[k++] = right[j++];
        }

        return result;
    }
}