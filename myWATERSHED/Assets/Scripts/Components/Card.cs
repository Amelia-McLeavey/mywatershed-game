using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    //public Sprite art;
    public float quantityInDeck;
    //public float cost //will be kinda weird because the cost often has changing values depending on card
    //need a way to handle the tile types affected by card?
    public float turbidity;
    public float ctMax;
    public float volunteers;
    public float waterTemp;
    public float landTemp;
    public float redsideDace;
    public float brownTrout;
    public float creekChub;
    public float asphaltDensity;
    public float pollution;
    public float sewage;
    public float waterDepth;
    public float sinuosity;
    public float landHeight;
    public float erosionRate;
    public float timeBeforeEffect;
    public float effectDuration;


}
