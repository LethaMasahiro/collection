using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowParticleManager : MonoBehaviour
{
    private struct SnowParticle
    {
        public Vector3 position;
        public Vector3 velocity;
    }

    public int SnowParticleCount;
    public Material SnowParticleMaterial;
    public ComputeShader ComputeShader;
    public GameObject player;

    ComputeBuffer buffer;
    private int kernelID;

    public Vector3 radius = new Vector3(60, 10, 60);
    public Vector3 offset = new Vector3(0, 5, 0);

    // Start is called before the first frame update
    void Start()
    {
        buffer = new ComputeBuffer(SnowParticleCount, 3 * 8);
        SetParticles();

        //Find the ID of the kernel
        kernelID = ComputeShader.FindKernel("CSMain2");

        //Set the particle buffer in the compute and particle shader
        ComputeShader.SetBuffer(kernelID, "particleBuffer", buffer);
        SnowParticleMaterial.SetBuffer("particleBuffer", buffer);
    }

    void SetParticles()
    {
        SnowParticle[] particles = new SnowParticle[SnowParticleCount];
        for (int i = 0; i < SnowParticleCount; ++i)
        {
            particles[i].position.x = (Random.value * 2 - 1.0f) * radius.x;
            particles[i].position.y = (Random.value * 2 - 1.0f) * radius.y;
            particles[i].position.z = (Random.value * 2 - 1.0f) * radius.z;

            particles[i].velocity.x = (Random.value * 2 - 1.0f) * 0.5f;
            particles[i].velocity.y = (Random.value * 2 - 1.0f) * 0.5f;
            particles[i].velocity.z = (Random.value * 2 - 1.0f) * 0.5f;
        }
        buffer.SetData(particles);
    }

    // Update is called once per frame
    void Update()
    {
        ComputeShader.SetFloat("deltaTime", Time.deltaTime);
        ComputeShader.SetFloat("time", Time.time);
        ComputeShader.SetFloat("gravity", 1f);
        // ComputeShader.SetVector("playerPos", player.transform.position);
        ComputeShader.SetVector("playerPos", transform.position + offset);
        ComputeShader.SetVector("snowRadius", radius);

        SnowParticleMaterial.SetVector("playerPos", transform.position);

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SetParticles();
        }

        
        ComputeShader.SetBuffer(kernelID, "particleBuffer", buffer);
        ComputeShader.Dispatch(kernelID, Mathf.CeilToInt((float)SnowParticleCount / 256), 1, 1);
        // Debug.Log("f");
    }

    void OnRenderObject()
    {
        // Debug.Log("Render snow");
        SnowParticleMaterial.SetBuffer("particleBuffer", buffer);
        SnowParticleMaterial.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, 1, SnowParticleCount);
    }

    private void OnApplicationQuit()
    {
        buffer?.Release();
    }
}
