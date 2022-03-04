/// <summary>
/// A struct for creating an instance of a card. Provides a way to reference card data.
/// </summary>

public struct CardInstance
{
    // Int ID to reference the appropriate data as needed
    public int cardAssetID;

    public string cardName;
    public string cardDescription;
    // Variables that will change at runtime
    public int durationRemaining;

    public int delayBeforeEffect;

    public string tileType;

    public int numberOfTiles;

    public bool global;

    //Variables influenced

    public float brownTroutInfluence;
    public float creekChubInfluence;
    public float insectInfluence;
    public float redDaceInfluence;
    public float riparianInfluence;
    public float riverbedHealthInfluence;
    public float asphaltDensityInfluence;
    public float erosionInfluence;
    public float landHeightInfluence;
    public float pollutionInfluence;
    public float flowRateInfluence;
    public float sewageInfluence;
    public float sinuosityInfluence;
    public float shadeInfluence;
    public float turbidityInfluence;
    public float waterDepthInfluence;
    public float waterTempInfluence;


}