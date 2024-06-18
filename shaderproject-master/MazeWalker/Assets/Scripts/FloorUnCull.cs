using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUnCull : MonoBehaviour
{
    public void OnPreCull()
    {
        var cam = Camera.current;
        cam.ResetCullingMatrix();
        Matrix4x4 m = cam.cullingMatrix;
        m.SetRow(0, m.GetRow(0)*0.5f);
        m.SetRow(1, m.GetRow(1)*0.5f);
        cam.cullingMatrix = m;
    }
}
