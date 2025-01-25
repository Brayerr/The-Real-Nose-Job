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

    public void SetSpawnLocation(Transform location)
    {
        spawnLocation = location;
    }

    public void CreateBubble()
    {
        current = Instantiate(bubblePrefab, spawnLocation);
    }

    public void ExpandBubble()
    {
        size += Time.deltaTime;
        current.Expand(Time.deltaTime);
        current.transform.position += Time.deltaTime / 2 * Vector3.up;
    }

    public void AbortBubble()
    {
        if (current != null)
        {
            Bubble b = current;
            current = null;
            b.Explode();
        }
    }

    public Transform LaunchBubble()
    {
        //Destroy(current.gameObject, size);
        current.Release(size);
        size = 0;
        return current.transform;
    }
}
