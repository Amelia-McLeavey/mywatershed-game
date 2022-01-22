using System.Collections.Generic;
using UnityEngine;

public class FishSimulator : MonoBehaviour
{
    public World m_worldScript;

    [SerializeField]
    private int m_dailyRequiredInsectConsumptionPerFish;
    [SerializeField]
    private int m_maximumRedDacePopulation;
    [SerializeField]
    private int m_maximumRedDaceGrowthPerUpdate;
    [SerializeField]
    private float m_ctMin;
    [SerializeField]
    private float m_ctMax;

    [SerializeField]
    private bool m_insectFactor;
    [SerializeField]
    private bool m_turbidityFactor;
    [SerializeField]
    private bool m_waterTemperatureFactor;

    private int m_rows;
    private int m_columns;

    private List<Tile> m_waterTiles = new List<Tile>();

    private void Awake()
    {
        Debug.Assert(m_dailyRequiredInsectConsumptionPerFish > 0, "DailyRequiredInsectConsumptionPerFish value must be positive");
        Debug.Assert(m_maximumRedDacePopulation > 0, "MaximumRedDacePopulation value must be positive");
        Debug.Assert(m_maximumRedDaceGrowthPerUpdate > 0, "m_maximumRedDaceGrowthPerUpdate value must be positive");
    }

    private void OnEnable()
    {
        SystemGenerator.OnSystemGenerationComplete += CacheWaterTiles;
    }

    private void OnDisable()
    {
        SystemGenerator.OnSystemGenerationComplete -= CacheWaterTiles;
    }

    private void CacheWaterTiles(int rows, int columns)
    {
        m_rows = rows;
        m_columns = columns;

        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                Vector2 index = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(index, out GameObject value))
                {
                    if (value.GetComponent<Tile>().m_Basetype == BaseType.Water)
                    {
                        m_waterTiles.Add(value.GetComponent<Tile>());
                    }
                }
            }
        }
    }

    public void UpdateFishPopulations()
    {
        UpdateRedDace();
    }

    private void UpdateRedDace()
    {
        // If there are fish in the tile
        foreach (Tile waterTile in m_waterTiles)
        {
            RedDacePopulation redDaceComponent = waterTile.gameObject.GetComponent<RedDacePopulation>();
            InsectPopulation insectComponent = waterTile.gameObject.GetComponent<InsectPopulation>();
            Turbidity turbidityComponent = waterTile.gameObject.GetComponent<Turbidity>();
            WaterTemperature waterTemperatureComponent = waterTile.gameObject.GetComponent<WaterTemperature>();

            if (redDaceComponent.m_RedDacePopulation > 0)
            {
                if (m_insectFactor)
                {
                    int prevRedDacePopulation = redDaceComponent.m_RedDacePopulation;

                    int insectPopulationRequiredToSustain = prevRedDacePopulation * m_dailyRequiredInsectConsumptionPerFish;

                    if (insectComponent.m_InsectPopulation >= insectPopulationRequiredToSustain)
                    {
                        int populationDifference = insectComponent.m_InsectPopulation - insectPopulationRequiredToSustain;
                        int newFishies = Mathf.Clamp(populationDifference / m_dailyRequiredInsectConsumptionPerFish, 0, m_maximumRedDaceGrowthPerUpdate);
                        redDaceComponent.m_RedDacePopulation += newFishies;
                    }
                    else
                    {
                        redDaceComponent.m_RedDacePopulation = insectComponent.m_InsectPopulation / m_dailyRequiredInsectConsumptionPerFish;
                    }

                    redDaceComponent.m_RedDacePopulation = Mathf.Clamp(redDaceComponent.m_RedDacePopulation, 0, m_maximumRedDacePopulation);
                }

                if (m_turbidityFactor)
                {
                    redDaceComponent.m_RedDacePopulation = Mathf.RoundToInt(redDaceComponent.m_RedDacePopulation * (1 - turbidityComponent.m_Turbidity));
                }
                
                if (m_waterTemperatureFactor)
                {
                    if (waterTemperatureComponent.m_waterTemperature < m_ctMin || waterTemperatureComponent.m_waterTemperature > m_ctMax)
                    {
                        redDaceComponent.m_RedDacePopulation = 0;
                    }
                }

            }
        }
        m_worldScript.UpdateTotalDacePopulation();
    }

}
