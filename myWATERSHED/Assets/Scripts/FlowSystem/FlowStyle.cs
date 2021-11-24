using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This abstract class is only defining the methods that we want to be able to call on different flow styles

public abstract class FlowStyle
{
    public abstract bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging);
    public abstract void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging);
    public abstract void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging);
}
