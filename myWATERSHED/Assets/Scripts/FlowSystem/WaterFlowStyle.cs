using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This script contains flow logic for all abiotic components pertaining to water tiles

public class WaterFlowStyle : FlowStyle
{
    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {

        Tile senderTileScript = senderTile.GetComponent<Tile>();
        Tile receiverTileScript = receiverTile.GetComponent<Tile>();

        // Check if the sender tile is able to run this flow in the first place
        if (senderTileScript.m_Basetype != BaseType.Water)
        {
            return false;
        }

        // Check if any of the neighbouring tiles are able to recieve data
        if (receiverTileScript.m_PhysicalType == PhysicalType.Highway)     
        {
            return true;
        }
        else if (receiverTileScript.m_Basetype == BaseType.Water)
        {
            return true;
        }
        else if (receiverTileScript.m_Basetype == BaseType.Land)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        // DEBUGS BE DEFENSIVE 
        Debug.Assert(senderTile != null, $"senderTile is null at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile != null, $"receiverTile is null at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<InsectPopulation>() != null, $"senderTile is missing an InsectPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<InsectPopulation>() != null, $"receiverTile is missing an InsectPopulation component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<PollutionLevel>() != null, $"senderTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<PollutionLevel>() != null, $"receiverTile is missing a PoullutionLevel component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<SewageLevel>() != null, $"senderTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<SewageLevel>() != null, $"receiverTile is missing a SewageLevel component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<Turbidity>() != null, $"senderTile is missing a Turbidity component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<Turbidity>() != null, $"receiverTile is missing a Turbidity component at index {tileIndexForDebugging}");
        Debug.Assert(senderTile.GetComponent<WaterTemperature>() != null, $"senderTile is missing a WaterTemperature component at index {tileIndexForDebugging}");
        Debug.Assert(receiverTile.GetComponent<WaterTemperature>() != null, $"receiverTile is missing a WaterTemperature component at index {tileIndexForDebugging}");
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        // Reset the list then scatter by adding sender tile current value to the recievers' list

        // INSECT POPULATION
        senderTile.GetComponent<InsectPopulation>().m_GatheredInsectPopulationValues.Clear();
        receiverTile.GetComponent<InsectPopulation>().m_GatheredInsectPopulationValues.Add((int)(senderTile.GetComponent<InsectPopulation>().value));
        // POLLUTION LEVEL
        senderTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Clear();
        receiverTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Add(senderTile.GetComponent<PollutionLevel>().value);
        // SEWAGE LEVEL
        senderTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Clear();
        receiverTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Add(senderTile.GetComponent<SewageLevel>().value);
        // TURBIDITY         
        senderTile.GetComponent<Turbidity>().m_GatheredTurbidityValues.Clear();
        receiverTile.GetComponent<Turbidity>().m_GatheredTurbidityValues.Add(senderTile.GetComponent<Turbidity>().value);
        // WATER TEMPERATURE
        senderTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Clear();
        receiverTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Add(senderTile.GetComponent<WaterTemperature>().value);
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        // Take the average of the gathered values and update own value

        // INSECT POPULATION
        if (senderTile.GetComponent<InsectPopulation>().m_GatheredInsectPopulationValues.Count != 0)
        {
            senderTile.GetComponent<InsectPopulation>().value = Mathf.RoundToInt((float)senderTile.GetComponent<InsectPopulation>().m_GatheredInsectPopulationValues.Average());
        }
        // POLLUTION LEVEL
        if (senderTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Count != 0)
        {
            senderTile.GetComponent<PollutionLevel>().value = senderTile.GetComponent<PollutionLevel>().m_GatheredPolutionValues.Average();
        }
        // SEWAGE LEVEL
        if (senderTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Count != 0)
        {
            senderTile.GetComponent<SewageLevel>().value = senderTile.GetComponent<SewageLevel>().m_GatheredSewageValues.Average();
        }
        // TURBIDITY
        if (senderTile.GetComponent<Turbidity>().m_GatheredTurbidityValues.Count != 0)
        {
            senderTile.GetComponent<Turbidity>().value = senderTile.GetComponent<Turbidity>().m_GatheredTurbidityValues.Average();
        }
        // WATER TEMPERATURE\
        if (senderTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Count != 0)
        {
            senderTile.GetComponent<WaterTemperature>().value = senderTile.GetComponent<WaterTemperature>().m_GatheredWaterTemperatureValues.Average();
        }
        
    }
}
