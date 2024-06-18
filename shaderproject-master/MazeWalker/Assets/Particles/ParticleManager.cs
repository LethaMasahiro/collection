using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 targetVelocity;
        public Vector3 targetOffset;
    }

    [Range(0, 1)]
    public float drag = 1;

    public float radius = 1;
    public float steering = 1;
    public float speedMultiplier = 5;

    public int ParticleCount;
    public Material ParticleMaterial;
    public ComputeShader ComputeShader;

    ComputeBuffer buffer;
    private int kernelID;

    float[] targetPosition = new float[3];

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        const int B_FLOAT = 4;
        const int B_VEC3 = 3 * B_FLOAT;
        buffer = new ComputeBuffer(ParticleCount, B_VEC3 * 4);
        SetParticles();
        
        //Find the ID of the kernel
        kernelID = ComputeShader.FindKernel("CSMain");

        //Set the particle buffer in the compute and particle shader
        ComputeShader.SetBuffer(kernelID, "particleBuffer", buffer);
        ParticleMaterial.SetBuffer("particleBuffer", buffer);
    }

    bool moveParticles = true;

    public Vector3 wind = new Vector3(1, 0, 0);

    private bool wasTriggered = false;
    
    void SetParticles() {
        Particle[] particles = new Particle[ParticleCount];
        for (int i = 0; i < ParticleCount; ++i)
        {
            particles[i].position.x = (Random.value * 2 - 1.0f) * radius * 0.5f + transform.position.x;
            particles[i].position.y = (Random.value * 2 - 1.0f) * radius * 0.5f + transform.position.y;
            particles[i].position.z = (Random.value * 2 - 1.0f) * radius * 0.5f + transform.position.z;

            particles[i].velocity.x = (Random.value * 2 - 1.0f) * radius * 0.5f;
            particles[i].velocity.y = (Random.value * 2 - 1.0f) * radius * 0.5f;
            particles[i].velocity.z = (Random.value * 2 - 1.0f) * radius * 0.5f;

            particles[i].targetVelocity.x = 0;
            particles[i].targetVelocity.y = 0;
            particles[i].targetVelocity.z = 0;


            particles[i].targetOffset = Random.insideUnitSphere;
        }
        buffer.SetData(particles);  
    }

    void Update()
    {
        Vector3 mousePosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
            mousePosition = hit.point;
        
        var pos = transform.position;
        targetPosition[0] = pos.x;
        targetPosition[1] = pos.y;
        targetPosition[2] = pos.z;

        wind = Quaternion.AngleAxis(360 * Time.deltaTime, Vector3.up) * wind;

        //Update mouse position and delta time
        ComputeShader.SetFloat("drag", drag);
        ComputeShader.SetFloat("deltaTime", Time.deltaTime);
        ComputeShader.SetFloat("radius", radius);
        ComputeShader.SetFloat("steering", steering);
        ComputeShader.SetFloat("speedMultiplier", speedMultiplier);
        ComputeShader.SetFloats("targetPosition", targetPosition);
        ComputeShader.SetVector("wind", wind);


        if (Input.GetKeyDown(KeyCode.F5)) {
            SetParticles();
        }
        
        //Run the compute shader
        if (Input.GetKeyDown(KeyCode.F1)) moveParticles = !moveParticles;
        // moveParticles = true;
        if (moveParticles) {
            ComputeShader.Dispatch(kernelID, Mathf.CeilToInt((float)ParticleCount / 256), 1, 1);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!wasTriggered && other.CompareTag("Player")) {
            // animator.SetTrigger("Move");
            // wasTriggered = true;
            moveParticles = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!wasTriggered && other.CompareTag("Player")) {
            // animator.SetTrigger("Move");
            // wasTriggered = true;
            moveParticles = false;
        }
    }

    void OnRenderObject()
    {
        ParticleMaterial.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, 1, ParticleCount);
    }

    private void OnApplicationQuit()
    {
        buffer.Release();
    }
}
