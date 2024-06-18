using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ModifyFootprintTexture : MonoBehaviour
{

    RenderTexture footprintTexture1;
    RenderTexture footprintTexture2;

    public Material floorMat;
    public Material wallMat;

    public Material setFootprintMat;
    public Material increaseSnowLevelMat;

    public Texture white;

    // Start is called before the first frame update
    void Start()
    {
        footprintTexture1 = new RenderTexture(500, 500, 0, RenderTextureFormat.ARGB32);
        footprintTexture2 = new RenderTexture(500, 500, 0, RenderTextureFormat.ARGB32);
        Color fillColor  = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        // Graphics.Blit(white, footprintTexture1);
        // var fillColorArray = footprintTexture.GetPixels();

        // for (var i = 0; i < fillColorArray.Length; ++i)
        // {
        //     fillColorArray[i] = fillColor;
        // }

        // footprintTexture.SetPixels(fillColorArray);

        // footprintTexture.Apply();

        StartCoroutine(nameof(PutFootprint));
        StartCoroutine(nameof(AddSnow));
    }

    void Update()
    {
        wallMat.SetVector("_PlayerPos", transform.position);
    }

    private void Swap() {
        var tmp = footprintTexture1;
        footprintTexture1 = footprintTexture2;
        footprintTexture2 = tmp;
    }

    IEnumerator AddSnow() {
        while (true) {
            Graphics.Blit(footprintTexture1, footprintTexture2, increaseSnowLevelMat);
            Swap();
            floorMat.SetTexture("_FootprintTexture", footprintTexture1);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator PutFootprint() {
        while (true) {
            Vector2 position = new Vector2(transform.position.x, transform.position.z);
            Vector2 pixelPosition = new Vector2(position.x * 5, position.y * 5);

            setFootprintMat.SetVector("_PlayerPos", position * 0.01f);
            Graphics.Blit(footprintTexture1, footprintTexture2, setFootprintMat);
            Swap();

            floorMat.SetTexture("_FootprintTexture", footprintTexture1);
            // yield return new WaitForSeconds(0.05f);
            yield return null;
        }
    }
}
