using UnityEngine;

public static class RandomNumGenerator
{
    public static int Randomize(int max)
    {
        var randomNumber = Random.Range(0, max);
        return randomNumber;
    }
}
