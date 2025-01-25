using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField] Bubble parentBubble;

    public Bubble GetParentBubble()
    {
        return parentBubble;
    }
}
