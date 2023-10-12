using Unity.Collections;
using UnityEngine;

public static class AligmentCalculator
{
    public static Vector3 CalculateAlignment(int index, NativeArray<Vector3> positions, NativeArray<Vector3> directions, float alignmentRadius, out int alignmentCount)
    {
        Vector3 alignmentSum = Vector3.zero;
        alignmentCount = 0;

        for (int j = 0; j < positions.Length; j++)
        {
            if (j == index) continue;

            float distance = Vector3.Distance(positions[j], positions[index]);

            if (distance < alignmentRadius)
            {
                alignmentSum += directions[j];
                alignmentCount++;
            }
        }

        return alignmentSum;
    }
}