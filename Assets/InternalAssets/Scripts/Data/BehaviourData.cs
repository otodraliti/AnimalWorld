using UnityEngine;

[CreateAssetMenu(fileName = "New BehaviourData", menuName = "Custom/New Behaviour Data")]
public class BehaviourData : ScriptableObject
{
    public float separationRadius;
    public float alignmentRadius;
    public float cohesionRadius;
    
    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;
    public float turnSmoothness;

    public float speed;
}