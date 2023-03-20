using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPack : MonoBehaviour
{
    public BallPackType type;
    public enum BallPackType
    {
        Horizontal,
        LetterMixed,
        StickMans
    }
}
