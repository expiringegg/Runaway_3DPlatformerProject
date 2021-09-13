using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint instance;

    public Vector3 savedTransform;

    public bool attackUnlocked;
    public bool dashUnlocked;
    public bool DoubleJumpUnlocked;

    public bool memory1;
    public bool memory2;
    public bool memory3;

    public bool levelTwoFirstLoad;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //allows for values to not be overwritten and only have one of the object on reload of scenes
    }
}

