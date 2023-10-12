using UnityEngine;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct GatherJob : IJobParallelFor
{
    public void Execute(int index)
    {
        Debug.Log(index + "gathering");
    }
}
