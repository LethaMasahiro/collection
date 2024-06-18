using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InWallCamera : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject WallOne;
    public GameObject WallTwo;

    void Start()
    {
        MainCamera = Camera.main;
    }

    void Update()
    {
        var mat = WallTwo.transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0)) * WallOne.transform.worldToLocalMatrix;

        var pos = MainCamera.transform.position;
        var forward = MainCamera.transform.forward;
        var up = MainCamera.transform.up;

        pos = mat * new Vector4(pos.x, pos.y, pos.z, 1.0f);
        forward = mat * new Vector4(forward.x, forward.y, forward.z, 0);
        up = mat * new Vector4(up.x, up.y, up.z, 0);

        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
