using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDetection : MonoBehaviour
{
    [SerializeField] float maxSize;
    [SerializeField] float minSize;
    [SerializeField] PlayerController player;

    private void Update()
    {
        transform.localScale = new Vector3(maxSize, maxSize, maxSize) * (player.getCurrentSnotAmount()/player.getMaxSnotAmount());
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("dsa");
        //if (collision.gameObject.CompareTag("Bee")) 
        //{
        //    Debug.Log("ewq");
        //    if (collision.gameObject.TryGetComponent<Bee>(out Bee bee))
        //    {
        //        Debug.Log("zxc");
        //        bee.NoticePlayerEnteredRange();
        //    }
        //}
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bee"))
        {
            if (collision.gameObject.TryGetComponent<Bee>(out Bee bee))
            {
                bee.NoticePlayerEnteredRange();
            }
        }
    }
}
