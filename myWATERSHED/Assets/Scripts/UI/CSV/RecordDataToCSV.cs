using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordDataToCSV : MonoBehaviour
{
    private WorldGenerator m_worldGenerator;

    [SerializeField] private TMP_Text filePathText;
    

    private void Start()
    {
        m_worldGenerator = FindObjectOfType<WorldGenerator>();
    }

    public void CallAddRecord()
    {
        if (filePathText.text.Contains(".txt"))
        {
            addRecord(filePathText.text);
        }
        else
        {
            Debug.Log("CHECK FILE PATH");
        }
    }

    public void addRecord(string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new StreamWriter(@filepath, false))
            {
                for (int y = 0; y < m_worldGenerator.m_rows; y++)
                {
                    for (int x = 0; x < m_worldGenerator.m_columns; x++)
                    {
                        file.WriteLine(GetData(new Vector2(y,x)));
                    }
                }               
            }
        }
        catch (UnityException ex)
        {
            Debug.LogError("Error: " + ex);
        }
    }

    private string GetData(Vector2 tileIndex)
    {
        string dataList = tileIndex.x+","+ tileIndex.y + ",";
        dataList = dataList + GetTileType(tileIndex) + ",";

        dataList = dataList + GetVariable(tileIndex, "AsphaltDensity") + ",";
        dataList = dataList + GetVariable(tileIndex, "BrownTroutPopulation") + ",";
        dataList = dataList + GetVariable(tileIndex, "CreekChubPopulation") + ",";
        dataList = dataList + GetVariable(tileIndex, "ErosionRate") + ",";
        dataList = dataList + GetVariable(tileIndex, "InsectPopulation") + ",";
        dataList = dataList + GetVariable(tileIndex, "LandHeight") + ",";
        dataList = dataList + GetVariable(tileIndex, "PollutionLevel") + ",";
        dataList = dataList + GetVariable(tileIndex, "RateOfFlow") + ",";
        dataList = dataList + GetVariable(tileIndex, "RedDacePopulation") + ",";
        dataList = dataList + GetVariable(tileIndex, "RiparianLevel") + ",";
        dataList = dataList + GetVariable(tileIndex, "RiverbedHealth") + ",";
        dataList = dataList + GetVariable(tileIndex, "SewageLevel") + ",";
        dataList = dataList + GetVariable(tileIndex, "ShadeCoverage") + ",";
        dataList = dataList + GetVariable(tileIndex, "Sinuosity") + ",";
        dataList = dataList + GetVariable(tileIndex, "Turbidity") + ",";
        dataList = dataList + GetVariable(tileIndex, "WaterDepth") + ",";
        dataList = dataList + GetVariable(tileIndex, "WaterTemperature");

        return dataList;
    }

    private float GetVariable(Vector2 tileIndex, string varName)
    {
        if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
        {
            //if it can find a variableClass component with the name provided
            if (value.GetComponent(varName) as VariableClass)
            {
                return (value.GetComponent(varName) as VariableClass).value;
            }
        }

        return 0f;
    }

    private string GetTileType(Vector2 tileIndex)
    {
        if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
        {
            //if it can find a variableClass component with the name provided
            if (value.GetComponent<Tile>())
            {
                return (value.GetComponent<Tile>().m_PhysicalType.ToString());
            }
        }

        return "Unknown";
    }
}
