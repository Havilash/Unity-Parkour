using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shooting : MonoBehaviour
{
    new Camera camera;
    [SerializeField] float impactForce = 50f;
    [SerializeField] LayerMask player;
    [SerializeField] ParticleSystem shootEffect;
    [SerializeField] GameObject impactEffect;

    [SerializeField] NavMeshAgent navMeshAgent;
    
    LayerMask notPlayer;
    static public int crntWeapon = 1; //Default, Pistol, Bot, Grappling Hook

    [SerializeField] GameObject[] weapons;


    private void Start()
    {
        camera = Camera.main;
        notPlayer =~ player;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && crntWeapon == 1)
        {
            Shoot();
        }
        if (Input.GetButtonDown("Fire1") && crntWeapon == 2)
        {
            NavMeshShoot();
        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0) 
        {
            if (crntWeapon < weapons.Length-1) crntWeapon++;
            else crntWeapon = weapons.Length - 1;

            Debug.Log(weapons[crntWeapon]);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (crntWeapon > 0) crntWeapon--;
            else crntWeapon = 0;

            Debug.Log(weapons[crntWeapon]);
        }
    }
    private void Shoot()
    {
        shootEffect.Play();

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 1000, notPlayer))
        {
            //Debug.Log(hit.transform.name);

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            impactGo.transform.parent = hit.transform;
            Destroy(impactGo, 8f);
        }

        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * impactForce);
        }

    }

    private void NavMeshShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 1000, notPlayer))
        {
            navMeshAgent.SetDestination(hit.point);
        }
    }
}
