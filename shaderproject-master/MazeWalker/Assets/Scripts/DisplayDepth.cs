using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDepth : MonoBehaviour
{
    public int count = 10;
    public float distance = 1;

    public float offset = 1;
    public GameObject prefab;

    public float fogStrengthStart;
    public float fogStrengthRange;

    void Start() {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
        for (int i = 0; i < count; i++) {
            var instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            instance.transform.localPosition = new Vector3(0, 0, offset + distance * (i + 1));
            instance.transform.localRotation = Quaternion.identity;

            var rend = instance.GetComponent<FogRender>();
            rend.fogStrength = (fogStrengthRange - fogStrengthStart) * ((float)i / (count - 1)) + fogStrengthStart;
        }
    }
}
