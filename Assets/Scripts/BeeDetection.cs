using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDetection : MonoBehaviour
{
    [SerializeField] float maxSize;
    [SerializeField] PlayerController player;

    private void Update()
    {
        float currArea = Mathf.PI * maxSize * maxSize * (player.getCurrentSnotAmount() / player.getMaxSnotAmount());
        float currRadius = Mathf.Sqrt(currArea / Mathf.PI);

        transform.position = player.transform.position;
        transform.localScale = new Vector3(currRadius, currRadius, currRadius);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bee"))
        {
            if (collision.gameObject.TryGetComponent<Bee>(out Bee bee))
            {
                bee.NoticePlayerEnteredRange();
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bee"))
        {
            if (collision.gameObject.TryGetComponent<Bee>(out Bee bee))
            {
                bee.NoticePlayerExitedRange();
            }
        }
    }
}
