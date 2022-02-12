using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedPickerPanel : MonoBehaviour
{
    public static int seed=231;

    [SerializeField] private TMP_InputField input;
    void Start()
    {
        randomizeSeed();
    }


    public void randomizeSeed()
    {
        seed = Random.Range(0, 1000);
        DisplaySeed();

    }
    private void DisplaySeed()
    { 
        if (seed < 10)
        {
            input.text = "00"+seed;
        }
        else if (seed < 100)
        {
            input.text = "0" + seed;
        }
        else
        {
            input.text = seed.ToString();
        }
    }

    public void SetSeed()
    {
        seed = int.Parse(input.text);
        DisplaySeed();
    }
}
