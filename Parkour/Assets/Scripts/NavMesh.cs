using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMesh : MonoBehaviour
{
    private new Camera camera;
    private NavMeshAgent navMeshAgent;
    private Shooting shootingScript;
    [SerializeField] private GameObject cross;
    private GameObject crossClone = null;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject crossUI;
    private RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
        shootingScript = FindObjectOfType<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            SetDestination();
        }
        if (crossClone != null)
        {
            navMeshAgent.SetDestination(crossClone.transform.position);
        }
    }

    private void SetDestination()
    {
        shootingScript.shootAnim.SetTrigger("ShootingTrigger");

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {

            //Instatiate, Destroy
            if (crossClone != null)
            {
                crossClone.GetComponent<Animator>().SetTrigger("CrossDeathTrigger");
                Destroy(crossClone.transform.parent.gameObject, 2f);
            }

            crossClone = Instantiate(cross, hit.point, Quaternion.LookRotation(hit.normal));

            //Parenting (Avoid Scaling by Parenting)
            var emptyGameObject = new GameObject();

            crossClone.transform.parent = emptyGameObject.transform;
            emptyGameObject.transform.parent = hit.transform.gameObject.transform;


            if (crossClone.transform.root.gameObject == player)
            {
                Debug.Log("TEst");
                //Hide Cross
                crossClone.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
                crossClone.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = false;
                crossUI.GetComponent<Animator>().SetTrigger("CrossImgSpawnTrigger");
            }
            else if (crossClone.transform.root.gameObject != player)
            {
                crossUI.GetComponent<Animator>().SetTrigger("CrossImgDeathTrigger");
            }
        }
    }
}
