using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// Interprets google sheet and populates each card asset with the appropriate data.
/// </summary>
/// 

//// In this script, comments are denoted with 4 slashes "////"
//// DO NOT DELETE CODE THAT HAS BEEN COMMENTED OUT WITH 2 SLASHES "//" AND IS BOOKENDED WITH --------
//// We use this "//" code to regen cards during development if there are changes to the google sheet.
//// If changes are made externally in the google sheet and you need to update in game:
//// 1. Uncomment the "//" code (the code bookended with "//// ---------" markers)
//// 2. Save this script.
//// 3. Navigate to the "Card Manager" game object in the game scene. Find in the hierarchy.
//// 4. Click "Import Cards" GUI button in the Unity Editor Inspector window.
//// 5. Wait for import. It may take a couple minutes.
//// 6. Go back to the script and comment out the code with "//" that you uncommented in step 1.
//// 7. Save this script.
//// 8. You will see a new batch of CardAsset objects in version control. Commit and Push those.

//// Q: Why do we need to uncomment and comment this code in the first place? Why can't we just leave it uncommented?
//// A: If this code is left uncommented then the game will not build or will be errors in the build.


public class GoogleSheetParser : MonoBehaviour
{
    public static List<CardAsset> s_cardAssets;

    //// Main parser function to read and sort google sheet
    public static void LoadCardDeck()
    {
        //// Read through the google sheet, convert each row into a single string
        //// This whole script only runs when the application is the Unity Editor
        if (Application.isEditor)
        {
            //// ------------------------------------------------------------------------------------
            //if (AssetDatabase.IsValidFolder("Assets/Resources/Cards"))
            //{
            //    List<string> failures = new List<string>();
            //    AssetDatabase.DeleteAssets(new string[] { "Assets/Resources/Cards" }, failures);
            //    Debug.Assert(failures.Count == 0, "Your assets did not delete");
            //}

            //AssetDatabase.CreateFolder("Assets/Resources", "Cards");
            //// ------------------------------------------------------------------------------------

            //// SHEET RANGE STRING VALUE MUST BE UPDATED MANUALLY IF RANGE CHANGES
            IList<IList<object>> cardSheetRows = GoogleSheetReader.GetSheetRange("A2:AD66");
            foreach (IList<object> currentRow in cardSheetRows)
            {
                List<string> itemData = new List<string>();
                foreach (object currentColumn in currentRow)
                {
                    itemData.Add(currentColumn.ToString());
                }

                CardAsset cardAsset = ScriptableObject.CreateInstance<CardAsset>();

                //// CARDASSET ATTRIBUTE LIST
                //// If new attributes are added externally, or an attribute is removed, or an attribute's column in the spreadsheet changes
                //// then itemData[int] int values must be updated so that numbers are in correct sequence (i.e. 0, 1, 2, 3...etc.)

                //// CARD INFO // [13 TOTAL (0-12)]
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

                //// BIOTIC VARIABLE DATA // [6 TOTAL (13-18)]
                cardAsset.m_brownTroutPopulation = Convert.ToInt32(itemData[13]);
                cardAsset.m_creekChubPopulation = Convert.ToInt32(itemData[14]);
                cardAsset.m_insectPopulation = Convert.ToInt32(itemData[15]);
                cardAsset.m_redDacePopulation = Convert.ToInt32(itemData[16]);
                cardAsset.m_riparianQuality = Convert.ToSingle(itemData[17]);
                cardAsset.m_riverbedHealth = Convert.ToSingle(itemData[18]);

                //// ABIOTIC VARIABLE DATA // [11 TOTAL (19-29)]
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

                //// --------------------------------------------------------------------------------------------------
                //AssetDatabase.CreateAsset(cardAsset, $"Assets/Resources/Cards/CardDataAsset_{cardAsset.m_id}.asset");
                //// --------------------------------------------------------------------------------------------------
            }
            //// ------------------------
            //AssetDatabase.SaveAssets();
            //// ------------------------
        }
    }
}

