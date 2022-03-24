using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RedDaceUpdateFlow : FlowStyle
{
    [SerializeField]
    [Min(1)]
    private int m_dailyRequiredInsectConsumptionPerFish;
    [SerializeField]
    [Min(1)]
    private int m_maximumRedDacePopulationPerTile;
    [SerializeField]
    [Min(1)]
    private int m_maximumRedDaceGrowthPerUpdate;
    [SerializeField]
    private float m_ctMin;
    [SerializeField]
    private float m_ctMax;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("The chance that a brown trout will eat a red dace on each update (0.0 to 1.0)")]
    private float m_brownTroutChanceToEat = 0.025f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("The chance that a red dace will spawn from a creek chub nest on each update (0.0 to 1.0)")]
    private float m_creekChubChanceToSpawn = 0.025f;

    // Bool variables that can be true or false (turned off or on) in the inspector to help test factors in isolation.
    // These should all be checked as true in inspector in the final build.
    [SerializeField]
    private bool m_brownTroutFactor;
    [SerializeField]
    private bool m_creekChubFactor;
    [SerializeField]
    private bool m_insectFactor;
    [SerializeField]
    private bool m_turbidityFactor;
    [SerializeField]
    private bool m_waterTemperatureFactor;



    public override bool CanFlow(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    public override void DistrubuteData(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }

    private void ClampLimitPerTile(RedDacePopulation redDaceComponent)
    {
        redDaceComponent.value = Mathf.Clamp(redDaceComponent.value, 0, m_maximumRedDacePopulationPerTile);
    }

    public override void ProcessData(GameObject senderTile, Vector2 tileIndexForDebugging)
    {
        RedDacePopulation redDaceComponent = senderTile.GetComponent<RedDacePopulation>();

        // If there are red dace fish in the tile
        if (redDaceComponent.value > 0)
        {
            // Get necessary component data
            BrownTroutPopulation brownTroutComponent = senderTile.GetComponent<BrownTroutPopulation>();
            CreekChubPopulation creekChubComponent = senderTile.GetComponent<CreekChubPopulation>();
            InsectPopulation insectComponent = senderTile.GetComponent<InsectPopulation>();
            Turbidity turbidityComponent = senderTile.GetComponent<Turbidity>();
            WaterTemperature waterTemperatureComponent = senderTile.GetComponent<WaterTemperature>();

            if (m_insectFactor)
            {
                int insectPopulationRequiredToSustain = Mathf.CeilToInt(redDaceComponent.value * m_dailyRequiredInsectConsumptionPerFish);

                if (insectComponent.value < insectPopulationRequiredToSustain)
                {
                    redDaceComponent.value = Mathf.RoundToInt(insectComponent.value / m_dailyRequiredInsectConsumptionPerFish);
                }

                ClampLimitPerTile(redDaceComponent);
            }

            // Brown trout eat dace
            if (m_brownTroutFactor)
            {
                float randomRoll = Random.Range(0.0f, 1.0f);

                float chanceLimit = m_brownTroutChanceToEat * brownTroutComponent.value;

                // Make sure that we are within the valid chance of any fish being eaten
                if(randomRoll < chanceLimit)
                {
                    int numberEaten = Mathf.CeilToInt(randomRoll / m_brownTroutChanceToEat);
                    redDaceComponent.value = Mathf.Max(0, redDaceComponent.value - numberEaten);
                }

                ClampLimitPerTile(redDaceComponent);
            }

            if (m_creekChubFactor)
            {
                float randomRoll = Random.Range(0.0f, 1.0f);

                float chanceLimit = m_creekChubChanceToSpawn * creekChubComponent.value;

                // Make sure that we are within the valid chance of any fish being spawned
                if (randomRoll < chanceLimit)
                {
                    int numberSpawned = Mathf.CeilToInt(randomRoll / m_creekChubChanceToSpawn);
                    redDaceComponent.value = redDaceComponent.value + numberSpawned;
                }

                ClampLimitPerTile(redDaceComponent);
            }

            if (m_turbidityFactor)
            {
                redDaceComponent.value = Mathf.RoundToInt(redDaceComponent.value * (1 - turbidityComponent.value));
                ClampLimitPerTile(redDaceComponent);
            }

            if (m_waterTemperatureFactor)
            {
                if (waterTemperatureComponent.value < m_ctMin || waterTemperatureComponent.value > m_ctMax)
                {
                    redDaceComponent.value = 0;
                }
            }
        }
    }

    public override void VerifyTiles(GameObject senderTile, GameObject receiverTile, Vector2 tileIndexForDebugging)
    {
        throw new System.NotImplementedException();
    }
}
