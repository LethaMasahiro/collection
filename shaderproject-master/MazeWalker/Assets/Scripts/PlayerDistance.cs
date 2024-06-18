using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistance : MonoBehaviour
{

    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 heading = transform.position - camera.transform.position;
        float cameraDistance = heading.magnitude;
        Shader.SetGlobalFloat("_PlayerPos", cameraDistance);
    }
}
