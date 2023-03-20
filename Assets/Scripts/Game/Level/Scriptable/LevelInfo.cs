using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "LevelInfo", menuName = "Create Level", order = 1)]
public class LevelInfo : ScriptableObject
{
    public enum PlatformType
    {
        collectArea, // The area which is given for the player to collect objects
        checkPoint, // Area to check that a sufficient number of objects have been collected
        endGame // Area to indicate game finished
    }
    public enum BallType
    {
        shapeTriangle,
        shapeCircle,
        shapeSquare,
        shapeT, // Similar to the letter 'T'
        shapeY, // Similar to the letter 'Y'
        shapeS, // Similar to the letter 'S'
    }
    public int LevelNumber; // The number of level 
    public int PlatformLenght; // How many parts does the platform consist of
    public List<PlatformType> AvailablePlatformTypes; // platform types that we can choose when creating a level
    public List<Tile> Tiles;
}