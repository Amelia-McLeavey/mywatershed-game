using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowSimulator : MonoBehaviour
{
    private int m_rows;
    private int m_columns;

    private void OnEnable()
    {
        SystemGenerator.OnSystemGenerationComplete += InitializeFlow;
    }

    private void OnDisable()
    {
        SystemGenerator.OnSystemGenerationComplete -= InitializeFlow;
    }

    // Initializes tiles by finding the nieghbours of each tile that will receive information
    private void InitializeFlow(int rows, int columns)
    {
        //Debug.Log("ITIALIZE FLOW");

        m_rows = rows;
        m_columns = columns;

        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    SetNeighbours(value, tileIndex);
                    SetStartingVariableValues(value, tileIndex);
                }
            }
        }
    }

    private void SetNeighbours(GameObject tile, Vector2 tileIndex)
    {
        Tile tileScript = tile.GetComponent<Tile>();
        List<Vector2> neighbourIndices = NeighbourUtility.FindAllNeighbours(tileIndex);

        if (tileScript.m_Basetype == BaseType.Water)
        {
            tileScript.m_receiverNeighbours = NeighbourUtility.FindLowestNeighbours(neighbourIndices);
        }

        if (tileScript.m_Basetype == BaseType.Land)
        {
            tileScript.m_receiverNeighbours = NeighbourUtility.FindLowestNeighbour(neighbourIndices, tile);
        }
    }

    private void SetStartingVariableValues(GameObject tile, Vector2 tileIndex)
    {
        SendProcessingPulse(tile, new VariableInitialization(), tileIndex);

    }

    public void UpdateFlow()
    {
        SendTwoStageFlow(new LandVariableProcessor(), BaseType.Land);
        SendTwoStageFlow(new WaterVariableProcessor(), BaseType.Water);

        SendTwoStageFlow(new LandFlowStyle(), BaseType.Land);
        SendTwoStageFlow(new WaterFlowStyle(), BaseType.Water);
    }



    private void SendTwoStageFlow(FlowStyle flowStyle, BaseType baseType)
    {
        // Reset the list then scatter by adding sender tile current value to the recievers' list
        for (int x = m_rows; x > 0; x--)
        {
            for (int y = m_columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == baseType)
                    {
                        SendDistributionPulse(value, flowStyle, tileIndex); // scatter
                    }
                }
            }
        }
        // Take the average of the gathered values and update own value
        for (int x = m_rows; x > 0; x--)
        {
            for (int y = m_columns; y > 0; y--)
            {
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == baseType)
                    {
                        SendProcessingPulse(value, flowStyle, tileIndex);
                    }
                }
            }
        }
    }

    private void SendDistributionPulse(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
    {
        List<GameObject> receiverTiles = GetRequiredNeighbours(senderTile);

        foreach (GameObject receiverTile in receiverTiles)
        {
            if (flowStyle.CanFlow(senderTile, receiverTile, indexForDebugging))
            {
                flowStyle.DistrubuteData(senderTile, receiverTile, indexForDebugging);
            }
            else
            {

            }
        }
    }

    private void SendProcessingPulse(GameObject senderTile, FlowStyle flowStyle, Vector2 indexForDebugging)
    {
        flowStyle.ProcessData(senderTile, indexForDebugging);
     
    }

    private List<GameObject> GetRequiredNeighbours(GameObject senderTile)
    {
        return senderTile.GetComponent<Tile>().m_receiverNeighbours;
    }
}
