using UnityEngine;

public abstract class Platform : MonoBehaviour // base class for all platform parts.
{
    public abstract PlatformType platformType { get; }
    public enum PlatformType
    {
        collectArea, // The area which is given for the player to collect objects.
        checkPoint, // Area to check that a sufficient number of objects have been collected.
        endGame // Area to indicate game finished.
    }
}
