using StateManager;
using UnityEngine;

public class Gather_State: State
{
    private Transform animal;
    
    private GatherPoint _gatherPoint;
    private int GatherSlot;

    public Gather_State(Transform Animal)
    {
        animal = Animal;
    }
    
    public override void Enter()
    {
        bool isAdded = _gatherPoint.TryAddObjectToSlot(_gatherPoint.gatherSlots[GatherSlot], animal.gameObject);
        if(isAdded)
        {
            animal.transform.position = _gatherPoint.gatherSlots[GatherSlot].position; 
        }
        else
        {
            Debug.LogWarning("Object was not added to slot!");
        }
    }

    public override void Exit()
    {
        _gatherPoint.gatherSlotsDictionary[_gatherPoint.gatherSlots[GatherSlot]]?.Remove(animal.gameObject);
    }

    public override void Update()
    {
        base.Update();
    }
    
    public void SetGatherPoint(GatherPoint gatherPoint)
    {
        _gatherPoint = gatherPoint;
        GatherSlot = RandomNumGenerator.Randomize(_gatherPoint.gatherSlots.Length);
        if (!_gatherPoint.TryAddObjectToSlot(_gatherPoint.gatherSlots[GatherSlot], animal.gameObject))
        {
            Debug.LogError("Failed to add object to slot");
        }
        _gatherPoint.HandleObjectsInSlot(_gatherPoint.gatherSlots[GatherSlot]);
    }
}