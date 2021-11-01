using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class prototypeSheetParser : MonoBehaviour
{
    IList<IList<object>> cardSheet;
    //struct with definitions for every card attribute as strings
    struct CardData
    {
        public string id;
        public string name;
        public string description;
        public string duration;
        public string quantity;
        public string activity;
        public string initialCost;
        public string tileType;
        public string tilesTargetted;
        public string turbidity;
        public string ctMax;
        public string volunteers;
        public string waterTemp;
        public string landTemp;
        public string dace;
        public string trout;
        public string chub;
        public string asphaltDensity;
        public string pollution;
        public string sewage;
        public string waterDepth;
        public string sinuosity;
        public string landHeight;
        public string erosionRate;
        public string delay;
        public string tileModifier;
        public string unlockableCardID;

        public CardData(string ID, string Name, string Description, string Duration, string Quantity, string Activity, string InitialCost, string TileType, string TilesTargetted, string Turbidity, string CtMax, string Volunteers, string WaterTemp, string LandTemp, string Dace, string Trout, string Chub, string AsphaltDensity, string Pollution, string Sewage, string WaterDepth, string Sinuosity, string LandHeight, string ErosionRate, string Delay, string TileModifier, string UnlockableCardID) {
            id = ID;
            name = Name;
            description = Description;
            duration = Duration;
            quantity = Quantity;
            activity = Activity;
            initialCost = InitialCost;
            tileType = TileType;
            tilesTargetted = TilesTargetted;
            turbidity = Turbidity;
            ctMax = CtMax;
            volunteers = Volunteers;
            waterTemp = WaterTemp;
            landTemp = LandTemp;
            dace = Dace;
            trout = Trout;
            chub = Chub;
            asphaltDensity = AsphaltDensity;
            pollution = Pollution;
            sewage = Sewage;
            waterDepth = WaterDepth;
            sinuosity = Sinuosity;
            landHeight = LandHeight;
            erosionRate = ErosionRate;
            delay = Delay;
            tileModifier = TileModifier;
            unlockableCardID = UnlockableCardID;
        }
    }
    List<CardData> cardDeck = new List<CardData>();
    public enum cardInfo {ID,NAME,DESCRIPTION,QUANTITY,ACTIVITY,INITIALCOST,TILETYPE,TILESTARGETTED,TURBIDITY,CTMAX,VOLUNTEERS,WATERTEMP,LANDTEMP,DACE,TROUT,CHUB,ASPHALT,POLLUTION,SEWAGE,WATERDEPTH,SINUOSITY,LANDHEIGHT,EROSIONRATE,DELAY,TILEMODIFIER,UNLOCKABLECARDID};
    private void Start()
    {
        LoadCardDeck();
    }

    public void LoadCardDeck()
    {
        //
        cardSheet = GoogleSheetReader.getSheetRange("A2:AA40");
        string tempString = "";
        foreach (IList<object> list in cardSheet) {
            bool isFirstItem = true;
            foreach (object listItem in list) {
                string itemTempString = ("" + listItem);
                if (isFirstItem){
                    tempString = itemTempString;
                    isFirstItem = false;
                }
                else
                tempString = tempString + ";" + itemTempString;
            }
            string[] itemData = tempString.Split(';');
            //Debug.Log(itemData.Length);
            CardData cardEntry = new CardData(itemData[0], itemData[1], itemData[2], itemData[3], itemData[4], itemData[5], itemData[6], itemData[7], itemData[8], itemData[9], itemData[10], itemData[11], itemData[12], itemData[13], itemData[14], itemData[15], itemData[16], itemData[17], itemData[18], itemData[19], itemData[20], itemData[21], itemData[22], itemData[23], itemData[24], itemData[25], itemData[26]);
            //Debug.Log(""+ cardEntry.id + " " + cardEntry.name + " " + cardEntry.unlockableCardID + ", deck at " + cardDeck.Count);
            cardDeck.Add(cardEntry);
            tempString = "";
        }
    }

    public string GetCardInfo(int cardNumber, cardInfo selectedInfo)
    {
        if (cardNumber < cardDeck.Count)
        {
            if(selectedInfo == cardInfo.ID)
                return cardDeck[cardNumber].id;
            else if (selectedInfo == cardInfo.NAME)
                return cardDeck[cardNumber].name;
            else if (selectedInfo == cardInfo.DESCRIPTION)
                return cardDeck[cardNumber].description;
            else if (selectedInfo == cardInfo.QUANTITY)
                return cardDeck[cardNumber].quantity;
            else if (selectedInfo == cardInfo.ACTIVITY)
                return cardDeck[cardNumber].activity;
            else if (selectedInfo == cardInfo.INITIALCOST)
                return cardDeck[cardNumber].initialCost;
            else if (selectedInfo == cardInfo.TILETYPE)
                return cardDeck[cardNumber].tileType;
            else if (selectedInfo == cardInfo.TILESTARGETTED)
                return cardDeck[cardNumber].tilesTargetted;
            else if (selectedInfo == cardInfo.TURBIDITY)
                return cardDeck[cardNumber].turbidity;
        }
        return "";
    }
}
