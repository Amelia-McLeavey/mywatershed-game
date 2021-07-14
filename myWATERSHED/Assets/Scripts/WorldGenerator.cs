using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType { Water, Wetland, Devland, Forest, Rock };
public enum WorldSize { XS, Small, Medium, Large, XL}

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private WorldSize worldSize;

    [SerializeField]
    private int seed = 0;
    [SerializeField]
    private float randomnessMagnitude = 1.5f;
    [SerializeField]
    private float magnitudeReductionRate = 0.80f;

    [SerializeField]
    private GameObject tile;

    private int columns = 0;
    private int rows = 0;

    private float size = 0;

    private Vector2 tileStep = new Vector2(0.5f, 0.87f);

    private Transform worldHolder;

    private Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();

    private void Start()
    {
        HeightmapGenerator.CreateHeightmap((int)worldSize, seed, randomnessMagnitude, magnitudeReductionRate);
        StartCoroutine(WorldSetup());
    }

    private void DetermineSize()
    {
        if (worldSize == WorldSize.XS)           { size = 1.0f; }
        else if (worldSize == WorldSize.Small)   { size = 3.0f; }
        else if (worldSize == WorldSize.Medium)  { size = 5.0f; }
        else if (worldSize == WorldSize.Large)   { size = 9.0f; }
    }

    public IEnumerator WorldSetup()
    {
        DetermineSize();
        columns = (int)size;
        rows = (int)size;
        
        worldHolder = new GameObject("World").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject cloneTile;
                Vector3 position; 

                if (y % 2 == 0)
                { position = new Vector3(x, 0f, y * tileStep.y); } 
                else
                { position = new Vector3(x + tileStep.x, 0f, y * tileStep.y); }

                tiles.Add(new Vector2(x, y), cloneTile = Instantiate(tile, position, Quaternion.identity));
                cloneTile.transform.localScale = new Vector3(1f, HeightmapGenerator.Heightmap[x, y], 1f);
                cloneTile.transform.position = new Vector3(position.x, HeightmapGenerator.Heightmap[x, y] / 4, position.z);

                cloneTile.transform.SetParent(worldHolder);

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
