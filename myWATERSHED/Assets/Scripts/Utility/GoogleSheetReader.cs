using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

class GoogleSheetReader : MonoBehaviour
{
    //variables to reference google spreadsheet and authentication key
    static private string spreadsheetId = "1O3UbAxylUPJallWONjJOvmxG6wuV95gh4YkUo1lSQzk";
    static private string jsonPath = "/StreamingAssets/Credentials/mywatershed-be9a6dd88852.json";
    static private SheetsService service;

    static GoogleSheetReader()
    {
        //organize and set up the authentication key
        string fullJsonPath = Application.dataPath + jsonPath;
        Stream jsonCred = (Stream)File.Open(fullJsonPath, FileMode.Open);
        ServiceAccountCredential cred = ServiceAccountCredential.FromServiceAccountData(jsonCred);
        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = cred,
        });
    }

    public static IList<IList<object>> getSheetRange(string sheetRange)
    {
        //access the google spreadsheet with the authentication key, using the sheet's id
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetRange);
        request.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;

        //return the spreadsheet in an ilist, each item in the ilist is a list with the values of each cell in a row
        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;
        if (values != null && values.Count > 0)
        {
            return values;
        }
        else
        {
            Debug.Log("No data found.");
            return null;
        }
    }
}


