using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shooting : MonoBehaviour
{
    private new Camera camera;
    public float impactForce = 30f;
    public ParticleSystem shootEffect;
    public GameObject impactEffect;
    public Animator shootAnim;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        shootAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        shootEffect.Play();
        shootAnim.SetTrigger("ShootingTrigger");
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
        }

        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * impactForce);
        }


        GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGo, 1f);
    }
}

