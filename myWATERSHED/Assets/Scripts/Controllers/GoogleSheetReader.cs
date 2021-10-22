using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


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

    public IList<IList<object>> getSheetRange(string sheetNameAndRange)
    {
        //access the google spreadsheet with the authentication key, using the sheet's id
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetNameAndRange);

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



    //https://sheets.googleapis.com/v4/spreadsheets/spreadsheetId?includeGridData=false
    //https://sheets.googleapis.com/v4/spreadsheets/1O3UbAxylUPJallWONjJOvmxG6wuV95gh4YkUo1lSQzk?key=AIzaSyCrui6mxEduyXuBzJrRjv7qzph7Bxs74Gk
    //https://docs.google.com/spreadsheets/d/{spreadsheetId}/gviz/tq
    //https://docs.google.com/spreadsheets/d/1O3UbAxylUPJallWONjJOvmxG6wuV95gh4YkUo1lSQzk/qviz/tq?key=upheld-quasar-329704

    //IEnumerator GetGoogleSheet()
    //{
    //    UnityWebRequest sheetAddress = UnityWebRequest.Get("https://spreadsheets.google.com/feeds/list/1O3UbAxylUPJallWONjJOvmxG6wuV95gh4YkUo1lSQzk/od6/public/values?alt=json");
    //    yield return sheetAddress.SendWebRequest();

    //    if(sheetAddress.isNetworkError || sheetAddress.isHttpError)
    //    {
    //        Debug.Log(" " + sheetAddress.error);
    //    }

    //    else
    //    {
    //        string sheetJson = sheetAddress.downloadHandler.text;
    //    }
    //}

}
