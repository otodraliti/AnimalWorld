using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [Header("Agents")]
    public GameObject[] agents;
    public Transform target;
    
    [Header("Radius")]
    public float separationRadius = 5f;
    public float alignmentRadius = 2f;
    public float cohesionRadius = 2f;
    public float turnSmoothness = 1f;
    
    [Header("Weights")]
    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;
    
    [Header("Gathering Settings")]
    public float gatheringDistanceThreshold = 10f;
    
    
    private NativeArray<Vector3> agentPositions;
    private NativeArray<Vector3> agentDirections;
    private NativeArray<float> agentSpeeds;
    
    
    
    private void Start()
    {
        InitializeAgentData();
    }

    private void Update()
    {
        ExecuteAppropriateJob();
        UpdateAgentsInScene();
    }

    private void OnDestroy()
    {
        DisposeNativeArrays();
    }

    private void InitializeAgentData()
    {
        int numAgents = agents.Length;

        agentPositions = new NativeArray<Vector3>(numAgents, Allocator.Persistent);
        agentDirections = new NativeArray<Vector3>(numAgents, Allocator.Persistent);
        agentSpeeds = new NativeArray<float>(numAgents, Allocator.Persistent);

        for (int i = 0; i < numAgents; i++)
        {
            agentPositions[i] = agents[i].transform.position;
            agentDirections[i] = agents[i].transform.forward;
            
            agentSpeeds[i] = agents[i].GetComponent<AnimalReference>().preset.speed;
        }
    }

    private void ExecuteFlockingJob()
    {
        FlockingJob flockingJob = new FlockingJob
        {
            positions = agentPositions,
            directions = agentDirections,
            speeds = agentSpeeds,
            target = target.position,
            deltaTime = Time.deltaTime,
            separationRadius = separationRadius,
            alignmentRadius = alignmentRadius,
            cohesionRadius = cohesionRadius,
            turnSmoothness = turnSmoothness,
            separationWeight = separationWeight,
            alignmentWeight = alignmentWeight,
            cohesionWeight = cohesionWeight
        };

        JobHandle jobHandle = flockingJob.Schedule(agentPositions.Length, 64);
        jobHandle.Complete();
    }

    private void UpdateAgentsInScene()
    {
        for (int i = 0; i < agents.Length; i++)
        {
            AnimalReference agentBehavior = agents[i].GetComponent<AnimalReference>();
            agentBehavior.UpdateAgent(agentPositions[i], agentDirections[i]);
        }
    }

    private void DisposeNativeArrays()
    {
        agentPositions.Dispose();
        agentDirections.Dispose();
        agentSpeeds.Dispose();
    }
    
    private void ExecuteAppropriateJob()
    {
        bool isGathering = AnyAgentNearTarget();

        if (isGathering)
        {
            ExecuteGatherJob();
        }
        else
        {
            ExecuteFlockingJob();
        }
    }

    private bool AnyAgentNearTarget()
    {
        for (int i = 0; i < agents.Length; i++)
        {
            if (Vector3.Distance(agentPositions[i], target.position) <= gatheringDistanceThreshold)
            {
                return true;
            }
        }
        return false;
    }

    private void ExecuteGatherJob()
    {
        GatherJob gatherJob = new GatherJob
        {
           
        };
        JobHandle gatherHandle = gatherJob.Schedule(agentPositions.Length, 64);
        gatherHandle.Complete();
    }
}
