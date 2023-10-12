using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public interface IAnimalJob
{
    JobHandle Execute(
        NativeArray<Vector3> positions,
        NativeArray<Vector3> directions,
        NativeArray<float> speeds,
        Vector3 targetPosition,
        float deltaTime,
        JobHandle inputDeps);
}
