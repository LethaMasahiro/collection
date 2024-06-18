using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogRender : MonoBehaviour
{
    private Mesh mesh;
    public Material material;
    
    public float fogStrength = 0.1f;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        // material.enableInstancing = true;
    }

    void OnRenderObject()
    {
        material.SetPass(0);
        material.SetFloat("_FogStrength", fogStrength);
        Graphics.DrawMeshNow(mesh, transform.localToWorldMatrix);
        // Graphics.DrawMeshInstanced(mesh, 1, material, new Matrix4x4[] { transform.localToWorldMatrix });
    }
}
