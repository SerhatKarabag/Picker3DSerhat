using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectArea : Platform // Collectarea object which is platform part.
{
    public override PlatformType platformType
    {
        get { return PlatformType.collectArea; }
    }
}
