using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchHand : MonoBehaviour
{
    // Start is called before the first frame update
    private float targetRot;
    public Image seasonImage;
    public Sprite[] seasonSprites;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(transform.eulerAngles.z, targetRot, Time.deltaTime));

        if (transform.eulerAngles.z >180)
        {
            if (transform.eulerAngles.z > 270)
            {
                seasonImage.sprite = seasonSprites[0];
            }
            else
            {
                seasonImage.sprite = seasonSprites[1];
            }
        }
        else
        {
            if (transform.eulerAngles.z > 90)
            {
                seasonImage.sprite = seasonSprites[2];
            }
            else
            {
                seasonImage.sprite = seasonSprites[3];
            }
        }
    }

    public void UpdateHandPos(float rot)
    {
        targetRot =  rot * -360f;
    }
}
