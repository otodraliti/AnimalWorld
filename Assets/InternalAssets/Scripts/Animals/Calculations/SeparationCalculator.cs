using Unity.Collections;
using UnityEngine;

public static class SeparationCalculator
{
    public static Vector3 CalculateSeparation(int index, NativeArray<Vector3> positions, float separationRadius, out int separationCount)
    {
        Vector3 separationSum = Vector3.zero;
        separationCount = 0;

        for (int j = 0; j < positions.Length; j++)
        {
            if (j == index) continue;

            float distance = Vector3.Distance(positions[j], positions[index]);

            if (distance < separationRadius)
            {
                Vector3 directionToOther = positions[index] - positions[j];
                directionToOther = directionToOther.normalized / distance;
                separationSum += directionToOther;
                separationCount++;
            }
        }

        return separationSum;
    }
}
