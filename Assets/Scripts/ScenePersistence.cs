using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistence : MonoBehaviour
{
    
    // cache
    private int startingSceneIndex;

    
     
    private void Start()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        SetUpSingleton();
    }

    // If the player dies, but is still in the same scene => then dont destroy
    // the pickups gameObject which is childed to this script's gameObject.
    // Else if we load to next scene, then destroy this scripts gameObject (along with its child which is the pickup gameObject)
    // inorder to load the coins and other pickups in the other scene.
    private void Update()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex != startingSceneIndex)
        {
            Destroy(gameObject);
        }
    }

    private void SetUpSingleton()
    {
        var numOfScenePersist = FindObjectsOfType(GetType()).Length;
        
        if (numOfScenePersist > 1)
        {
            gameObject.SetActive(false); // safe guard
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    
}
