using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField]
    private float agility = 1.0f;

    private Vector3 nextPosition;

    private void Start()
    {
        // These values are hardcoded to the water-cube objects current position, it it changes this will break
        nextPosition = new Vector3(Random.Range(-0.5f, 3.5f), Random.Range(0.5f, 2.5f), Random.Range(-7f, -4f));
    }

    private void Update()
    {
        Vector3 heading = nextPosition - transform.position;
        float distance = heading.magnitude;

        if (distance <= 0.5f)
        {
            // These values are hardcoded to the water-cube objects current position, it it changes this will break
            nextPosition = new Vector3(Random.Range(-0.5f, 3.5f), Random.Range(0.5f, 2.5f), Random.Range(-7f, -4f));
        }

        transform.position = Vector3.Lerp(transform.position, nextPosition, agility * Time.deltaTime);
    }
}
