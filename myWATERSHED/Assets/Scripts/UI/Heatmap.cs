using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heatmap : MonoBehaviour
{
    private GameManager m_gameManager;
    private WorldGenerator m_worldGenerator;
    private RawImage rawImage;

    [SerializeField] private bool moreIsBad = true;
    private float colValue;
    void Start()
    {
        m_gameManager = GameManager.Instance;
        m_worldGenerator = FindObjectOfType<WorldGenerator>();
        Texture2D texture = new Texture2D(m_worldGenerator.m_rows, m_worldGenerator.m_columns);
        texture.filterMode = FilterMode.Point;
        rawImage = GetComponent<RawImage>();
        rawImage.texture = texture;
        rawImage.SetNativeSize();
        
        for (int y =0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color color=new Color(0.1f, 0.3f, 0.8f);
                Vector2 tileIndex = new Vector2(x, y);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    Debug.Log("Getting Data");
                    if (value.GetComponent<AsphaltDensity>() != null)
                    {
                        if (moreIsBad)
                        {
                            colValue = (1f-value.GetComponent<AsphaltDensity>().value) * 0.3f;
                        }
                        else
                        {
                            colValue = value.GetComponent<AsphaltDensity>().value * 0.3f;
                        }
                        color = Color.HSVToRGB(colValue, 0.8f,1f);           
                    }
                }

                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
    }
}
