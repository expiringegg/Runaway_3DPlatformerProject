using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointUpdater : MonoBehaviour
{
    public Checkpoint checkPoint;
    public Player player;
    public Material cross;
    public Material tick;
    public MeshRenderer meshrenderer;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        checkPoint = GameObject.Find("CheckPointHandle").GetComponent<Checkpoint>();

       if(checkPoint.levelTwoFirstLoad == false && SceneManager.GetActiveScene().name == "Level2")
        {
            checkPoint.levelTwoFirstLoad = true;
            checkPoint.savedTransform = new Vector3(0, 0, -2);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.name == "Player")
        {
            checkPoint.GetComponent<Checkpoint>().savedTransform = gameObject.transform.position - new Vector3(0f,0f,3f);
            //on collsion it saves all current values to the checkpoint so on death the player doesn't lose progress
        }
    }

    private void Update()
    {
        if (checkPoint.GetComponent<Checkpoint>().savedTransform == gameObject.transform.position - new Vector3(0f, 0f, 3f))
        {
            meshrenderer.material = tick;
        }
        if (checkPoint.GetComponent<Checkpoint>().savedTransform != gameObject.transform.position - new Vector3(0f, 0f, 3f))
        {
            meshrenderer.material = cross;
        }  
    }
}

