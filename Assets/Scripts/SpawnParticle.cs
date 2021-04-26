using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    [SerializeField]
    private GameObject ParticlePrefab;

    [SerializeField]
    private float timeElapsed;

    [SerializeField]
    private float frequency;

    [SerializeField]
    private float force;

    [SerializeField]
    private float destroySec;

    private void Start()
    {
        StartCoroutine(enumerator());
        StartCoroutine(rotate());
    }

    IEnumerator enumerator()
    {
        while (timeElapsed <= frequency)
        {
            GameObject Particle = Instantiate(ParticlePrefab, Vector3.zero, Quaternion.identity);

            Particle.transform.parent = transform;
            Particle.transform.localPosition = Vector3.down * 0.05f;

            Force(Particle);

            Destroy(Particle, destroySec);
            
            timeElapsed ++;
            
            yield return null;
        }

        
    }

    IEnumerator rotate()
    {
        float timer = 0f;
        while (InputObject.test)
        {
            float angle = Mathf.Sin(timer) * 70;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Force(GameObject particle)
    {
        Rigidbody rb = particle.GetComponent<Rigidbody>();

        rb.AddForce(Vector3.up * force, ForceMode.Force);
    }
}
