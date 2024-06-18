using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{
    public RenderTexture depthTex;

    void OnPreRender() {
        Shader.SetGlobalInt("_UseDepth", 1);
        Shader.SetGlobalTexture("_PortalDepthTex", depthTex);
    }
}
