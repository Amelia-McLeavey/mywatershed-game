using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinatureManager : MonoBehaviour
{
    [SerializeField]
    public int m_rows = 0;
    [SerializeField]
    public int m_columns = 0;

    public TileManager m_TileManager;
    GameObject minatureToSpawn;

    private Vector2 tileStep = new Vector2(0.5f, 0.87f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceMinatures(int rows, int columns)
    {
        m_rows = rows;
        m_columns = columns;
        for (int x = 0; x < m_rows; x++)
        {
            for (int y = 0; y < m_columns; y++)
            {
                GameObject myMinature;
                Vector2 tileIndex = new Vector2(x, y);
                Vector3 position;
                // Adjust the position of the cloneTile to compliment the hex tiles
                if (y % 2 == 0)
                { position = new Vector3(x, 0f, y * tileStep.y); }
                else
                { position = new Vector3(x + tileStep.x, 0f, y * tileStep.y); }
                //Debug.Log("Im going through the index:" + tileIndex);

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    Debug.Log("TileDictionary exists");
                    
                    //Reference and store physical type from game object
                    PhysicalType physicalType = value.GetComponent<Tile>().m_PhysicalType;
                    minatureToSpawn = m_TileManager.m_minatures[(int)physicalType];
                    //assign correct minature from referenced list to the gameobject using the index
                    //minatureToSpawn = GetComponent<TileManager>().m_minature[physicalType];
                    //Check if there is something to spawn
                    if (minatureToSpawn != null)
                    {
                        Debug.Log("hey i exist and should ahve spawned");
                        // Instantiate the tile
                        TileManager.s_TilesDictonary.Add(new Vector2(x, y), myMinature = Instantiate(minatureToSpawn, position, Quaternion.Euler(0f, 0f, 0f)));
                        // Set the tile height.. Z because of the orientation of the asset
                        myMinature.transform.localScale = new Vector3(12f, 12f, 12f);
                    }
                }
            }
        }
        
    }
}
