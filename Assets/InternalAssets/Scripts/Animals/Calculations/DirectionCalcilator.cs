using Unity.Collections;
using UnityEngine;

public static class DirectionCalcilator
{
    public static Vector3 CalculateDesiredDirection(int index,Vector3 separationSum, Vector3 alignmentSum, Vector3 cohesionSum, int separationCount, int alignmentCount, int cohesionCount, NativeArray<Vector3> positions, BehaviourParams behaviourParams)
    {
        Vector3 desiredDirection = Vector3.zero;
        if(separationCount > 0) 
        {
            separationSum /= separationCount;
            desiredDirection += behaviourParams.separationWeight * separationSum.normalized;
        }

        if(alignmentCount > 0) 
        {
            alignmentSum /= alignmentCount;
            desiredDirection += behaviourParams.alignmentWeight * alignmentSum.normalized;
        }

        if(cohesionCount > 0) 
        {
            cohesionSum /= cohesionCount;
            desiredDirection += behaviourParams.cohesionWeight * (cohesionSum - positions[index]).normalized;
        }

        return desiredDirection.normalized;
    }
}
