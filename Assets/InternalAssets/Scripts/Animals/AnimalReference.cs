using UnityEngine;

public class AnimalReference : MonoBehaviour
{
    public SAnimal preset;
    
    public void UpdateAgent(Vector3 newPosition, Vector3 newDirection)
    {
        transform.position = newPosition;
        transform.forward = newDirection;
    }
}
