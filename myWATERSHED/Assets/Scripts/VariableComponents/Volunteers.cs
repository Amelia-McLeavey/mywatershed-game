using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volunteers : VariableClass
{
    public List<GameObject> affectedTiles = new List<GameObject>();

    private int shift;

    private Vector2 xOffset = new Vector2(1,0);
    private Vector2 yOffset = new Vector2(0,1);

    private VolunteerManager volManager;
    void Awake()
    {
        variableName = "Volunteers";
        value = 0;
        moreIsBad = false;

    }

    public void SetSurroundingTiles(Vector2 tileIndex)
    {
        if (tileIndex.y % 2 == 1)
        {
            shift = 1;
        }
        else
        {
            shift = -1;
        }

        if (affectedTiles.Count == 0)
        {
            AddTile(tileIndex);
            AddTile(tileIndex - xOffset);
            AddTile(tileIndex + xOffset);
            AddTile(tileIndex - yOffset);
            AddTile(tileIndex - yOffset + (xOffset*shift));
            AddTile(tileIndex + yOffset); 
            AddTile(tileIndex + yOffset + (xOffset * shift));
        }
    }

    private void AddTile(Vector2 index)
    {
        if (TileManager.s_TilesDictonary.TryGetValue(index, out GameObject value))
        {
            affectedTiles.Add(value);
        }
    }

    //TAKE THIS OUT!!!!
    //DEBUGGING ONLY
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && affectedTiles.Count!=0)
        {
            volManager = GameObject.FindObjectOfType<VolunteerManager>();

            UpdateValues();
        }
    }

    public void UpdateValues()
    {
        for(int i = 0; i< affectedTiles.Count; i++)
        {
            foreach (VariablesAffected varAffected in volManager.variablesAffected)
            {
                if (affectedTiles[i].GetComponent(varAffected.variableName) as VariableClass)
                {                
                    VariableClass varClass = affectedTiles[i].GetComponent(varAffected.variableName) as VariableClass;

                    varClass.value = varClass.value + (varAffected.influencePerVol * value * CheckDropoff(varAffected.dropOffRate, i));

                    if ((varAffected.influencePerVol < 0 && varClass.value < varAffected.targetValue) || (varAffected.influencePerVol > 0 && varClass.value > varAffected.targetValue))
                    {
                        varClass.value = varAffected.targetValue;
                    }
                }
            }
        } 
    }


    private float CheckDropoff(float dropoff, int index)
    {
        if (index > 0)
        {
            return 1 - dropoff;
        }
        else
        {
            return 1;
        }
    }
}
