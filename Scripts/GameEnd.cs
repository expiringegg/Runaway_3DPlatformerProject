using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    public PlayableDirector endScene;
    public GameObject player;
    public GameObject sceneChanger;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (sceneChanger.activeInHierarchy == true)
        {
            SceneManager.LoadScene("Game End");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            endScene.gameObject.SetActive(true);
            player.SetActive(false);
        }
    }
}
