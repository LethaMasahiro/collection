using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPortal : MonoBehaviour
{
    // Start is called before the first frame update
    public RenderTexture renderTexture;

    public Transform targetPortalPlane;
    public Mesh mesh;

    void Start()
    {
        var rend = targetPortalPlane.GetComponent<MeshFilter>();
        mesh = rend.mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        // Graphics.DrawMeshNow(mesh, targetPortalPlane.)
    }
}
