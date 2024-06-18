using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PortalMaster : MonoBehaviour
{
    [Range(0, 5)]
    public float dist = 1;

    public Transform target;

    void Update()
    {
        var v1 = target.transform.localPosition;
        v1.z = -dist;
        target.transform.localPosition = v1;
    }
}
