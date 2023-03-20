using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : Platform // End game object which is platform part.
{
    public override PlatformType platformType
    {
        get { return PlatformType.endGame; }
    }
}
