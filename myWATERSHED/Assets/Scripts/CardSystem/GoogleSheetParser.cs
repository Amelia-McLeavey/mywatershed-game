using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GoogleSheetParser : MonoBehaviour
{
    public static List<CardAsset> s_cardAssets;

    // Main parser function to read and sort google sheet
    public static void LoadCardDeck()
    {
        // Read through the google sheet, convert each row into a single string

        if (Application.isEditor)
        {
            //if (AssetDatabase.IsValidFolder("Assets/Resources/Cards"))
            //{
            //    List<string> failures = new List<string>();
            //    //AssetDatabase.DeleteAssets(new string[] { "Assets/Resources/Cards" }, failures);
            //    Debug.Assert(failures.Count == 0, "Your assets did not delete");          
            //}

            //AssetDatabase.CreateFolder("Assets/Resources", "Cards");

            IList <IList<object>> cardSheetRows = GoogleSheetReader.getSheetRange("A2:AD61");
            foreach (IList<object> currentRow in cardSheetRows)
            {
                List<string> itemData = new List<string>();
                foreach (object currentColumn in currentRow)
                {
                    itemData.Add(currentColumn.ToString());
                }

                CardAsset cardAsset = ScriptableObject.CreateInstance<CardAsset>();

                // CARDASSET ATTRIBUTE LIST

                // CARD INFO // [13 TOTAL (0-12)]
                cardAsset.m_id = Convert.ToInt32(itemData[0]);
                cardAsset.m_name = itemData[1];
                cardAsset.m_description = itemData[2];
                cardAsset.m_quantity = Convert.ToInt32(itemData[3]);
                cardAsset.m_effectDuration = Convert.ToInt32(itemData[4]);
                cardAsset.m_timeBeforeEffect = Convert.ToInt32(itemData[5]);
                cardAsset.m_initialVolunteerCost = Convert.ToInt32(itemData[6]);
                cardAsset.m_volunteersGainedPerYear = Convert.ToInt32(itemData[7]);
                cardAsset.m_tileTypesAffected = itemData[8];
                cardAsset.m_amountOfTilesTargeted = Convert.ToInt32(itemData[9]);
                cardAsset.m_tileModificationScreen = Convert.ToInt32(itemData[10]);
                cardAsset.m_activity = itemData[11];
                cardAsset.m_unlockableCArdID = Convert.ToInt32(itemData[12]);

                // BIOTIC VARIABLE DATA // [6 TOTAL (13-18)]
                cardAsset.m_brownTroutPopulation = Convert.ToInt32(itemData[13]);
                cardAsset.m_creekChubPopulation = Convert.ToInt32(itemData[14]);
                cardAsset.m_insectPopulation = Convert.ToInt32(itemData[15]);
                cardAsset.m_redDacePopulation = Convert.ToInt32(itemData[16]);
                cardAsset.m_riparianQuality = Convert.ToSingle(itemData[17]);
                cardAsset.m_riverbedHealth = Convert.ToSingle(itemData[18]);

                // ABIOTIC VARIABLE DATA // [11 TOTAL (19-29)]
                cardAsset.m_asphaltDensity = Convert.ToSingle(itemData[19]);
                cardAsset.m_erosionRate = Convert.ToSingle(itemData[20]);
                cardAsset.m_landHeight = Convert.ToSingle(itemData[21]);
                cardAsset.m_pollutionLevel = Convert.ToSingle(itemData[22]);
                cardAsset.m_flowRate = Convert.ToSingle(itemData[23]);
                cardAsset.m_sewageLevel = Convert.ToSingle(itemData[24]);
                cardAsset.m_sinuosity = Convert.ToSingle(itemData[25]);
                cardAsset.m_shadeCoverage = Convert.ToSingle(itemData[26]);
                cardAsset.m_turbidity = Convert.ToSingle(itemData[27]);
                cardAsset.m_waterDepth = Convert.ToSingle(itemData[28]);
                cardAsset.m_waterTemperature = Convert.ToSingle(itemData[29]);

                //AssetDatabase.CreateAsset(cardAsset, $"Assets/Resources/Cards/CardDataAsset_{cardAsset.m_id}.asset");
            }

            //AssetDatabase.SaveAssets();
        }
    }
}

