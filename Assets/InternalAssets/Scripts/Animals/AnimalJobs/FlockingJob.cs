using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public struct FlockingJob : IJobParallelFor
{
    public NativeArray<Vector3> positions;
    public NativeArray<Vector3> directions;
    public NativeArray<float> speeds;
    public Vector3 target;
    public float deltaTime;
    public float separationRadius;
    public float alignmentRadius;
    public float cohesionRadius;
    public float turnSmoothness;
    
    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;

    public void Execute(int index)
    {
        Vector3 separationSum = SeparationCalculator.CalculateSeparation(index, positions, separationRadius, out int separationCount);
        Vector3 alignmentSum = AligmentCalculator.CalculateAlignment(index, positions, directions, alignmentRadius, out int alignmentCount);
        Vector3 cohesionSum = FlockingCalculator.CalculateCohesion(index, positions, cohesionRadius, out int cohesionCount);
        
        
        Vector3 desiredDirection = Vector3.zero;
        if(separationCount > 0) 
        {
            separationSum /= separationCount;
            desiredDirection += separationWeight * separationSum.normalized;
        }

        if(alignmentCount > 0) 
        {
            alignmentSum /= alignmentCount;
            desiredDirection += alignmentWeight * alignmentSum.normalized;
        }

        if(cohesionCount > 0) 
        {
            cohesionSum /= cohesionCount;
            desiredDirection += cohesionWeight * (cohesionSum - positions[index]).normalized;
        }
        desiredDirection = desiredDirection.normalized;
        
        
        Vector3 toTarget = target - positions[index];
        desiredDirection = Vector3.Lerp(desiredDirection, toTarget.normalized, 0.1f);

        Vector3 newDirection = Vector3.Slerp(directions[index], desiredDirection, turnSmoothness * deltaTime);
        positions[index] += newDirection * speeds[index] * deltaTime;
        directions[index] = newDirection;
    }
}
