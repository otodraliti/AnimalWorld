using Unity.Collections;
using UnityEngine;

public static class FlockingCalculator
{
    public static Vector3 CalculateCohesion(int index, NativeArray<Vector3> positions, float cohesionRadius, out int cohesionCount)
    {
        Vector3 cohesionSum = Vector3.zero;
        cohesionCount = 0;

        for (int j = 0; j < positions.Length; j++)
        {
            if (j == index) continue;

            float distance = Vector3.Distance(positions[j], positions[index]);

            if (distance < cohesionRadius)
            {
                cohesionSum += positions[j];
                cohesionCount++;
            }
        }

        return cohesionSum;
    }
}
