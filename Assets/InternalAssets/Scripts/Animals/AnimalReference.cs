using UnityEngine;

public class AnimalReference : MonoBehaviour
{
    public AnimalData preset;
    
    public void UpdateAgent(Vector3 newPosition, Vector3 newDirection)
    {
        transform.position = newPosition;
        transform.forward = newDirection;
    }

    public void ReachedTarget()
    {
        Debug.Log(gameObject.name + " Reached");
    }
}
