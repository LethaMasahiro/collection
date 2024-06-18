using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private RenderTexture portalTex;

    [SerializeField]
    private RenderTexture depthTex;

    public Camera portalCam;
    public Camera clipCam;

    public Renderer portalRenderer;
    public Renderer targetPortalRenderer;
    
    void Start()
    {
        portalTex = new RenderTexture(1920, 1080, 0, GraphicsFormat.R8G8B8A8_SRGB);
        depthTex = new RenderTexture(1920, 1080, 0, GraphicsFormat.R8G8B8A8_SRGB);

        portalCam.targetTexture = portalTex;
        clipCam.targetTexture = depthTex;

        portalRenderer.material.SetTexture("_MainTex", portalTex);
        targetPortalRenderer.material.SetTexture("_MainTex", depthTex);
        GetComponentInChildren<PortalCam>().depthTex = depthTex;
    }
}
