using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heatmap : MonoBehaviour
{
    private GameManager m_gameManager;
    private WorldGenerator m_worldGenerator;
    [SerializeField] private RawImage asphaltDensImage;
    [SerializeField] private RawImage turbidityImage;
    [SerializeField] private RawImage redDacePopImage;
    [SerializeField] private RawImage insectImage;
    [SerializeField] private RawImage temperatureImage;
    [SerializeField] private RawImage sewageImage;

    private float colValue;
    public enum varOptions { asphaltDensity, turbidity, redDacePop, insectPop, temperatrue, sewage}

    private varOptions selectedVar = varOptions.asphaltDensity;
    
    //find things
    private void Start()
    {
        m_gameManager = GameManager.Instance;
        m_worldGenerator = FindObjectOfType<WorldGenerator>();
    }

    //Just for debuggging
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GenerateMaps();
        }
    }

    public void GenerateMaps()
    {
        //setup textures to be the right size and assign them to the Raw Images, this will have to be done for each Heatmap - not sure if theres some way to optomize this?
        Texture2D asphaltTexture = new Texture2D(m_worldGenerator.m_rows, m_worldGenerator.m_columns);
        asphaltTexture.filterMode = FilterMode.Point;
        asphaltDensImage.texture = asphaltTexture;
        asphaltDensImage.SetNativeSize();

        Texture2D turbidityTexture = new Texture2D(m_worldGenerator.m_rows, m_worldGenerator.m_columns);
        turbidityTexture.filterMode = FilterMode.Point;
        turbidityImage.texture = turbidityTexture;
        turbidityImage.SetNativeSize();

        Texture2D redDaceTexture = new Texture2D(m_worldGenerator.m_rows, m_worldGenerator.m_columns);
        redDaceTexture.filterMode = FilterMode.Point;
        redDacePopImage.texture = redDaceTexture;
        redDacePopImage.SetNativeSize();


        //setup textures to be the right size and assign them to the Raw Images, this will have to be done for each Heatmap - not sure if theres some way to optomize this?
        Texture2D insectTexture = new Texture2D(m_worldGenerator.m_rows, m_worldGenerator.m_columns);
        insectTexture.filterMode = FilterMode.Point;
        insectImage.texture = insectTexture;
        insectImage.SetNativeSize();

        Texture2D tempTexture = new Texture2D(m_worldGenerator.m_rows, m_worldGenerator.m_columns);
        tempTexture.filterMode = FilterMode.Point;
        temperatureImage.texture = tempTexture;
        temperatureImage.SetNativeSize();

        Texture2D sewageTexture = new Texture2D(m_worldGenerator.m_rows, m_worldGenerator.m_columns);
        sewageTexture.filterMode = FilterMode.Point;
        sewageImage.texture = sewageTexture;
        sewageImage.SetNativeSize();

        //loop though all tiles once and update all data
        for (int y =0; y < asphaltTexture.height; y++)
        {
            for (int x = 0; x < asphaltTexture.width; x++)
            {
                //nice function to keep everything clean - IMPORTANT: make sure the string name is spelt perfectly
                //here to change the defalut colour for the maps
                SetPixel(new Vector2(x,y), "AsphaltDensity", asphaltTexture, new Color(0.2f, 0.3f, 1f), true);

                SetPixel(new Vector2(x, y), "Turbidity", turbidityTexture, new Color(0.2f, 0.3f, 1f), true);

                SetPixel(new Vector2(x, y), "RedDacePopulation", redDaceTexture, new Color(0.2f, 0.3f, 1f), false);

                SetPixel(new Vector2(x, y), "InsectPopulation", insectTexture, new Color(0.2f, 0.3f, 1f), false);

                SetPixel(new Vector2(x, y), "WaterTemperature", tempTexture, new Color(0.2f, 0.3f, 1f), true);

                SetPixel(new Vector2(x, y), "SewageLevel", sewageTexture, new Color(0.2f, 0.3f, 1f), true);
            }
        }

        //actually display textures on the Raw Images
        asphaltTexture.Apply();
        turbidityTexture.Apply();
        redDaceTexture.Apply();
        insectTexture.Apply();
        tempTexture.Apply();
        sewageTexture.Apply();
    }

    //function for grabing data for each tile
    private void SetPixel(Vector2 tileIndex, string varName, Texture2D texture, Color defaultCol, bool moreIsBad)
    {
        Color color = defaultCol;

        if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
        {         
            //if it can find a variableClass component with the name provided
            if (value.GetComponent(varName) as VariableClass)
            {
                VariableClass variableClass = value.GetComponent(varName) as VariableClass;

                if (moreIsBad)
                {
                    colValue = (variableClass.maxValue - variableClass.value)/ variableClass.maxValue * 0.3f;
                }
                else
                {
                    colValue = variableClass.value / variableClass.maxValue * 0.3f;
                }
                //hue shifts colours intead of just using RGB
                //the 0.3f above is to limit the range of colours - change this if you want more/less variety
                color = Color.HSVToRGB(colValue, 0.8f, 1f);
            }
        }

        texture.SetPixel((int)tileIndex.x, (int)tileIndex.y, color);
    }


    //called when tab pressed
    public void ChangeHeatmap(varOptions newOption)
    {
        GetHeatmap(selectedVar).SetActive(false);
        GetHeatmap(newOption).SetActive(true);
        selectedVar = newOption;
    }

    //grabs the appropritate game object attached to the heatmap image
    private GameObject GetHeatmap(varOptions newOption)
    {
        switch (newOption)
        {
            case varOptions.turbidity:
                return turbidityImage.gameObject;

            case varOptions.redDacePop:
                return redDacePopImage.gameObject;

            case varOptions.insectPop:
                return insectImage.gameObject;

            case varOptions.temperatrue:
                return temperatureImage.gameObject;

            case varOptions.sewage:

                return sewageImage.gameObject;

            default:
                return asphaltDensImage.gameObject;
        }
    }
}
