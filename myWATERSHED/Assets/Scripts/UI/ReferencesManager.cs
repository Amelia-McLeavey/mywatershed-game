using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesManager : MonoBehaviour
{
    [SerializeField] private Transform tabHolder;
    [SerializeField] private Transform topTabHolder;

    [SerializeField] private Transform[] tabs;

    [SerializeField] private GameObject[] referenceObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TabPressed(int tabNum)
    {
        for(int i=0; i<tabs.Length; i++)
        {
            if (i == tabNum)
            {
                tabs[i].SetParent(topTabHolder);
                referenceObjects[i].SetActive(true);
            }
            else
            {
                tabs[i].SetParent(tabHolder);
                referenceObjects[i].SetActive(false);
            }
        }
    }
}
