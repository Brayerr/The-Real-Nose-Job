using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] public SpringJoint[] joints;
    [SerializeField] int testint;


    public void Expand(float amount)
    {
        foreach (var joint in joints)
        {
            joint.transform.position += joint.transform.up * amount;
        }
    }

    public void Release(float size)
    {
        foreach (var joint in joints)
        {
            joint.connectedAnchor = new(joint.transform.localPosition.x, joint.transform.localPosition.y, joint.transform.localPosition.z);
            joint.GetComponent<Rigidbody>().isKinematic = false;
            joint.gameObject.GetComponentAtIndex<SpringJoint>(3).connectedAnchor = new Vector3(-size, -size, 0);
        }
    }
}
