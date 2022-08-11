using System.Collections;
using UnityEngine;


// TODO: These should probably be in the special Editor folder so they are automatically excluded from builds
#if UNITY_EDITOR
public class FaunaPopulationDebugDrawFlow : FlowStyle
{
    private GUIStyle m_style;

    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        if(m_style == null)
        {
            m_style = new GUIStyle();
            m_style.fontSize = 15;
            m_style.alignment = TextAnchor.MiddleCenter;
        }

        string label = "";

        label += senderTile.GetComponent<RedDacePopulation>().value + "/";
        label += senderTile.GetComponent<CreekChubPopulation>().value + "/";
        label += senderTile.GetComponent<BrownTroutPopulation>().value;

        Vector3 position = senderTile.transform.position;

        // TODO: These were magic numbers that were pulled from the UI code for placing miniatures on top of the tiles. Not sure where it was calculated from
        position.y = ((senderTile.transform.localScale.z * 0.3574679f) / 2f) - (senderTile.transform.localScale.z * 0.006f);
        UnityEditor.Handles.Label(position, label, m_style);
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }
}
#endif // #if UNITY_EDITOR