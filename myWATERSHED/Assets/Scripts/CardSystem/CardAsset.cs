using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides an object type for creating cards. Includes all the base information that can be on any card.
/// </summary>

[CreateAssetMenu(fileName = "New CardAsset", menuName = "CardAsset")]
public class CardAsset : ScriptableObject
{
    // ATTRIBUTE LIST

    // CARD INFO // [13 TOTAL]
    public int m_id;
    public string m_name;
    public string m_description;
    public int m_quantity;
    public int m_effectDuration;
    public int m_timeBeforeEffect;
    public int m_initialVolunteerCost;
    public int m_volunteersGainedPerYear;
    public string m_tileTypesAffected;
    public int m_amountOfTilesTargeted;
    public int m_tileModificationScreen;
    public string m_activity;
    public int m_unlockableCArdID;

    // BIOTIC VARIABLE DATA // [6 TOTAL]
    public int m_brownTroutPopulation;
    public int m_creekChubPopulation;
    public int m_insectPopulation;
    public int m_redDacePopulation;
    public float m_riparianQuality;
    public float m_riverbedHealth;

    // ABIOTIC VARIABLE DATA // [11 TOTAL]
    public float m_asphaltDensity;
    public float m_erosionRate;
    public float m_landHeight;
    public float m_pollutionLevel;
    public float m_flowRate;
    public float m_sewageLevel;
    public float m_sinuosity;
    public float m_shadeCoverage;
    public float m_turbidity;
    public float m_waterDepth;
    public float m_waterTemperature;

    // FOR GOOGLE SHEET REFERENCE
    // COLUMN LETTER | Parameter
    // CARD INFO // [13 TOTAL]
    // A  | Id
    // B  | Name
    // C  | Description
    // D  | Quantity
    // E  | Effect Duration
    // F  | Time Before Effect
    // G  | Initial Volunteer Cost
    // H  | Volunteers Gained Per Year
    // I  | Tile Types Affected
    // J  | Amount of Tiles Targeted
    // K  | Tile Modification Screen
    // L  | Activity
    // M  | Unlockable Card ID

    // BIOTIC VARIABLE DATA // [6 TOTAL]
    // N  | Brown Trout Population
    // O  | Creek Chub Population 
    // P  | Insect Population
    // Q  | Red Dace Population 
    // R  | Riparian Quality
    // S  | Riverbed Health

    // ABIOTIC VARIABLE DATA // [11 TOTAL]
    // T  | Asphalt Density
    // U  | Erosion Rate
    // V  | Land Height
    // W  | Pollution Level
    // X  | Flow Rate
    // Y  | Sewage Level
    // Z  | Sinuosity
    // AA | Shade Coverage
    // AB | Turbidity
    // AC | Water Depth
    // AD | Water Temperature
}
