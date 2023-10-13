using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    
    #region Manual Settings
    [Header("Agents")] 
    public List<GameObject> agents;
   

    [Header("Flocking Data")] 
    public BehaviourData behaviourData;

    [Header("Gathering Settings")]
    public Transform target;
    public float gatheringDistanceThreshold = 10f;
    
    #endregion

    #region Private Settings
    

    private NativeArray<Vector3> agentPositions;
    private NativeArray<Vector3> agentDirections;
    private NativeArray<float> agentSpeeds;
    private IAnimalJob _currentBehaviour;
    private BehaviourParams behaviourParams;

    #endregion
    
    #region Unity Methods

    private void Start()
    {
        SetBehaviourParams(behaviourData);
        InitializeAgentData();
        InitializeFlockingBehaviour();
    }
    
    private void Update()
    {
        if (agents == null)
        {
            DisposeNativeArrays();
            return;
        }
        if (AnyAgentNearTarget(gatheringDistanceThreshold))
        {
            SetBehaviour(null);
        }
        ExecuteCurrentBehaviour();
        UpdateAgentsInScene();
    }
    
    private void OnDestroy() => DisposeNativeArrays();

    #endregion
    
    #region Behaviour Updating 
    private void InitializeFlockingBehaviour()
    {
        FlockingJob flockingJob = new FlockingJob
        {
            positions = agentPositions,
            directions = agentDirections,
            speeds = agentSpeeds,
            target = target.position,
            deltaTime = Time.deltaTime,
            behaviourParams = behaviourParams
        };

        SetBehaviour(flockingJob); 
    }
    
    private void ExecuteFlockingJob(BehaviourParams behaviourParams)
    {
        FlockingJob flockingJob = new FlockingJob
        {
            positions = agentPositions,
            directions = agentDirections,
            speeds = agentSpeeds,
            target = target.position,
            deltaTime = Time.deltaTime,
            behaviourParams = behaviourParams
        };
        
        JobHandle handle = flockingJob.Schedule(agentPositions.Length, 64, new JobHandle());
        handle.Complete();
    }
    
    
    private void UpdateAgentsInScene()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            AnimalReference agentBehavior = agents[i].GetComponent<AnimalReference>();
            agentBehavior.UpdateAgent(agentPositions[i], agentDirections[i]);
        }
    }

    #endregion
    
    #region Behaviour Utility

    private void SetBehaviour(IAnimalJob newBehaviour)
    {
        _currentBehaviour = newBehaviour;
    }

    private void ExecuteCurrentBehaviour()
    {
        if (_currentBehaviour == null) return;
        
        JobHandle jobHandle = _currentBehaviour.Execute(
            agentPositions,
            agentDirections,
            agentSpeeds,
            target.position,
            Time.deltaTime,
            new JobHandle());

        jobHandle.Complete();
    }

    #endregion

    #region Initialization
   
    private void InitializeAgentData()
    {
        int numAgents = agents.Count;

        agentPositions = new NativeArray<Vector3>(numAgents, Allocator.Persistent);
        agentDirections = new NativeArray<Vector3>(numAgents, Allocator.Persistent);
        agentSpeeds = new NativeArray<float>(numAgents, Allocator.Persistent);

        for (int i = 0; i < numAgents; i++)
        {
            agentPositions[i] = agents[i].transform.position;
            agentDirections[i] = agents[i].transform.forward;
            agentSpeeds[i] = agents[i].GetComponent<AnimalReference>().preset.speed;
        }
        ExecuteFlockingJob(behaviourParams);
    }

    

    #endregion
    
    #region Utilities
    private bool AnyAgentNearTarget(float Threshold)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            if (Vector3.Distance(agentPositions[i], target.position) <= Threshold)
            {
                agents[i].GetComponent<Animal_SM>().GatherState(target.GetComponent<GatherPoint>());
                RemoveFromList(agents[i]);
                return false;
            }
        }
        return false;
    }

    private void RemoveFromList(GameObject agent)
    {
        agents.Remove(agent);
    }

    private void AddToList(GameObject agent)
    {
        agents.Add(agent);
    }
    
    private void SetBehaviourParams(BehaviourData behaviourData)
    {
        behaviourParams = new BehaviourParams
        {
            separationRadius = behaviourData.separationRadius,
            alignmentRadius = behaviourData.alignmentRadius,
            cohesionRadius = behaviourData.cohesionRadius,
            separationWeight = behaviourData.separationWeight,
            alignmentWeight = behaviourData.alignmentWeight,
            cohesionWeight = behaviourData.cohesionWeight,
            turnSmoothness = behaviourData.turnSmoothness
        };
    }

    #endregion

    #region Dispose

    private void DisposeNativeArrays()
    {
        agentPositions.Dispose();
        agentDirections.Dispose();
        agentSpeeds.Dispose();
    }

    #endregion
    
}
