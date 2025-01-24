using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCreator : MonoBehaviour
{
    [SerializeField] Bubble bubblePrefab;
    [SerializeField] Transform spawnLocation;
    Bubble current;
    float size = 0;


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        CreateBubble();
    //    }
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        ExpandBubble();
    //    }
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        LaunchBubble();
    //    }
    //}

    public void CreateBubble()
    {
        current = Instantiate(bubblePrefab, spawnLocation);
    }

    public void ExpandBubble()
    {
        size += Time.deltaTime;
        current.Expand(Time.deltaTime);
    }

    public void LaunchBubble()
    {
        Destroy(current.gameObject,size);
        current.Release(size);
        size = 0;
    }
}
