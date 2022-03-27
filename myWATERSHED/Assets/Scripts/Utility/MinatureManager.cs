using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinatureManager : MonoBehaviour
{
    public TileManager m_TileManager;
    GameObject minatureToSpawn;

    private Transform miniatureHolder;

    //private Vector2 tileStep = new Vector2(0.5f, 0.87f);

    public void PlaceMinatures(int rows, int columns, bool heightsOn)
    {

        // DESTROY GAMEOBJECTS
        if (null != miniatureHolder)
        { Destroy(miniatureHolder.gameObject); }

        // Create an object to hold the miniatures
        miniatureHolder = new GameObject("Miniatures").transform;

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                //Debug.Log(x + " , " + y);
                GameObject myMinature;
                Vector2 tileIndex = new Vector2(x, y);
                Vector3 position;

                float yPosition;

                if (TileManager.s_TilesDictonary.TryGetValue(tileIndex, out GameObject value))
                {
                    //Debug.Log("TileDictionary exists");

                    //Set the Position
                    position = value.transform.position;

                    //Calculate height for tile to spawn at (Only used if HeightsOn)
                    yPosition = ((value.transform.localScale.z * 0.3574679f) / 2f) - (value.transform.localScale.z * 0.006f);
                    
                    //Reference and store physical type from game object
                    PhysicalType physicalType = value.GetComponent<Tile>().m_PhysicalType;

                    //Randomize which minature variant to use 1 - 3, with a default to 1 incase of errors
                    List<GameObject> m_minatureListUsed = m_TileManager.m_minatures_1;
                    int m_ListRandomizer = Random.Range(1, 4);

                    //Assign the chosen list to be applied
                    if (m_ListRandomizer == 1)
                    {
                        m_minatureListUsed = m_TileManager.m_minatures_1;
                    } else if(m_ListRandomizer == 2)
                    {
                        m_minatureListUsed = m_TileManager.m_minatures_2;
                    } else if(m_ListRandomizer == 3)
                    {
                        m_minatureListUsed = m_TileManager.m_minatures_3;
                    }
                    
                    //Assign the correct index of the list 
                    minatureToSpawn = m_minatureListUsed[(int)physicalType];

                    //Set height based on world type (Flat or height)
                    if (heightsOn)
                    {
                        position.y = yPosition;
                    }
                    else
                    {
                        if ((int)physicalType == 17)
                        {
                            position.y = 0.75f;
                        } else if ((int)physicalType == 8)
                        {
                            position.y = 0.8f;
                        }
                        else
                        {
                            position.y = 0.7f;
                        }   
                    }

                    //assign correct minature from referenced list to the gameobject using the index
                    //minatureToSpawn = GetComponent<TileManager>().m_minature[physicalType];

                    //Check if there is something to spawn
                    if (minatureToSpawn != null)
                    {
                        //Debug.Log("hey i exist and have spawned");
                        float randomRotation = Random.Range(0f, 360f);
                        // Instantiate the tile
                        myMinature = Instantiate(minatureToSpawn, position, Quaternion.Euler(0f, randomRotation, 0f));


                        //////////////////This code only needs to be fixed if we use the visual heights on the tiles, otherwise exact positions works fine//////////////////////////
                        //Raycast to find exact position
                        //int layerMask = 1 << 8; //Only check layer 8, which is tiles
                        //RaycastHit hit;

                        //if (Physics.Raycast(myMinature.transform.position, new Vector3(0, -1, 0), out hit, Mathf.Infinity, layerMask))
                        //{
                        //    Debug.Log("Hit point: " + hit.point);
                        //    Debug.Log("Collider point: " + hit.collider.transform.position);
                        //    //myMinature.transform.position = new Vector3(myMinature.transform.position.x, hit.point.y, myMinature.transform.position.z);
                        //    myMinature.transform.position = hit.point;
                        //}
                        //myMinature.transform.parent = value.transform;
                        //myMinature.transform.position = new Vector3(value.transform.position.x, value.transform.position.y, 0.185f);
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        // Set the objects scale
                        myMinature.transform.localScale = new Vector3(12f, 12f, 12f);

                        myMinature.transform.SetParent(miniatureHolder);
                    }
                }
            }
        }      
    }
}
