using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCreator : MonoBehaviour
{
    [SerializeField] Bubble bubblePrefab;
    [SerializeField] Transform spawnLocation;
    Bubble current;
    float size = 0;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            current = Instantiate(bubblePrefab, spawnLocation.position, Quaternion.identity);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            size += Time.deltaTime;
            current.Expand(Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            current.Release(size);
            size = 0;
        }
    }
}
