using System.Collections.Generic;
using UnityEngine;

public class GatherPoint : MonoBehaviour
{
    public Dictionary<Transform, List<GameObject>> gatherSlotsDictionary = new Dictionary<Transform, List<GameObject>>();
    public Transform[] gatherSlots;
    
    private void Start()
    {
        InitializeGatherSlots();
    }

    private void InitializeGatherSlots()
    {
        if(gatherSlots != null && gatherSlots.Length > 0)
        {
            foreach (Transform slot in gatherSlots)
            {
                if(slot != null && !gatherSlotsDictionary.ContainsKey(slot))
                {
                    gatherSlotsDictionary[slot] = new List<GameObject>();
                }
            }
        }
    }
    
    public bool TryAddObjectToSlot(Transform slot, GameObject obj)
    {
        if (gatherSlotsDictionary.ContainsKey(slot))
        {
            gatherSlotsDictionary[slot].Add(obj);
            return true;
        }
        return false;
    }

    public void HandleObjectsInSlot(Transform slot)
    {
        if (gatherSlotsDictionary.TryGetValue(slot, out var objectsInSlot))
        {
            Debug.Log(objectsInSlot.Count);
            if (objectsInSlot.Count == 2)
            {
                HandleTwoObjectsInSlot(objectsInSlot);
            }
            else if (objectsInSlot.Count > 2)
            {
                HandleMultipleObjectsInSlot(objectsInSlot);
            }
        }
    }
    
    private void HandleTwoObjectsInSlot(List<GameObject> objectsInSlot)
    {
        foreach (var obj in objectsInSlot)
        {
            obj.GetComponent<Animal_SM>().Reproduce();
        }
    }
    
    private void HandleMultipleObjectsInSlot(List<GameObject> objectsInSlot)
    {
        for (int i = 1; i < objectsInSlot.Count; i++)
        {
            MoveToAnotherSlot(objectsInSlot[i]);
        }
    }
    
    private void MoveToAnotherSlot(GameObject obj)
    {
        Transform availableSlot = FindAvailableSlot();

        if (availableSlot != null)
        {
            // Update the original slot.
            foreach (var slot in gatherSlotsDictionary.Keys)
            {
                if (gatherSlotsDictionary[slot].Contains(obj))
                {
                    gatherSlotsDictionary[slot].Remove(obj);
                    break;
                }
            }

            // Update the new slot.
            obj.transform.position = availableSlot.position;
            gatherSlotsDictionary[availableSlot].Add(obj);

            // Check the new slot for having 2 objects and reproduce if necessary.
            HandleObjectsInSlot(availableSlot);
        }
        else
        {
            Debug.LogWarning("No available slot found.");
        }
    }
    
    private Transform FindAvailableSlot()
    {
        foreach (var slot in gatherSlots)
        {
            if (gatherSlotsDictionary[slot].Count < 2) 
            {
                return slot;
            }
        }
        return null;
    }
}
