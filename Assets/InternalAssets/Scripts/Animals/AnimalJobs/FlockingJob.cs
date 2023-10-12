using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public struct FlockingJob : IJobParallelFor, IAnimalJob
{
    public NativeArray<Vector3> positions;
    public NativeArray<Vector3> directions;
    public NativeArray<float> speeds;
    public Vector3 target;
    public float deltaTime;

    public BehaviourParams behaviourParams;
    
    public void Execute(int index)
    {
        Vector3 separationSum = SeparationCalculator.CalculateSeparation(index, positions, behaviourParams.separationRadius, out int separationCount);
        Vector3 alignmentSum = AligmentCalculator.CalculateAlignment(index, positions, directions, behaviourParams.alignmentRadius, out int alignmentCount);
        Vector3 cohesionSum = FlockingCalculator.CalculateCohesion(index, positions, behaviourParams.cohesionRadius, out int cohesionCount);
        Vector3 desiredDirection = DirectionCalcilator.CalculateDesiredDirection(index,separationSum, alignmentSum, cohesionSum, separationCount, alignmentCount, cohesionCount, positions, behaviourParams);
        
        UpdateAgentDirectionAndPosition(index, desiredDirection);
    }
    
    private void UpdateAgentDirectionAndPosition(int index, Vector3 desiredDirection)
    {
        Vector3 toTarget = target - positions[index];
        desiredDirection = Vector3.Lerp(desiredDirection, toTarget.normalized, 0.1f); 

        Vector3 newDirection = Vector3.Slerp(directions[index], desiredDirection, behaviourParams.turnSmoothness * deltaTime);
        positions[index] += newDirection * speeds[index] * deltaTime;
        directions[index] = newDirection;
    }
    
    public JobHandle Execute(
        NativeArray<Vector3> positions,
        NativeArray<Vector3> directions,
        NativeArray<float> speeds,
        Vector3 targetPosition,
        float deltaTime,
        JobHandle inputDeps)
    {
        this.positions = positions;
        this.directions = directions;
        this.speeds = speeds;
        this.target = targetPosition;
        this.deltaTime = deltaTime;

        return this.Schedule(positions.Length, 64, inputDeps);
    }
}
