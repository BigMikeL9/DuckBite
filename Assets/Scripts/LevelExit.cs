using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    int currentSceneIndex;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] [Range(0f, 1f)] float levelExitSlowMoFactor = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextScene());
        
    }

    IEnumerator LoadNextScene()
    {
        Time.timeScale = levelExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        Destroy(FindObjectOfType<ScenePersistence>().gameObject);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        Time.timeScale = 1f;
    }
}

