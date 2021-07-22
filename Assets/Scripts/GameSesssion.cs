using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSesssion : MonoBehaviour
{

    // Configs
    [Header("Health System")] 
    [SerializeField] int playerHealth = 3;
    [SerializeField] int maxNumOfHealth = 3; // when this is equal to 3, I should have only 3 heart containers visible in my scene view
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    
    [Header("Score System")]
    [SerializeField] TextMeshProUGUI scoreText;
    private int startingScore;
    
    [Header("Scene Load Configs")]
    [SerializeField] float resetSceneDelay = 2f;
   

    private void Awake()
    {
        SetUpSingleton();
    }

    private void Start()
    {
        scoreText.text = startingScore.ToString();
        // playerLivesText.text = playerLives.ToString();
    }

    private void Update()
    {
        HeartSystem();
    }

    // Singeleton methods apply for children gameObject as well.
    // By placing the "Canvas" gameObject under the "GameSesssion" gameObject (making it its child), 
    // the score does not get reset or the canvas doesnt get destroyed when reload the scene. 
    // Instead of adding a new Singleton method script onto the canvas.
    private void SetUpSingleton()
    {
        int numOfGameSessions = FindObjectsOfType(GetType()).Length;
        
        // If the gameObject this script is attached to is greater than one, OR instantiated (when we laod another scene) 
        if (numOfGameSessions > 1)// "GetType()" gets the gameObject that this script/class is attached to, instead of identifying the name of the gameobject in between <...>
        {
            // then destroy the new instantiated gameObject 
            gameObject.SetActive(false); // This is just a safe guard.
            Destroy(gameObject);
        }
        else
        {
            // Do not destroy the original gameObject when we load a new scene
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerHealth > 1)
        {
            // Reduces playerLives by one, and reloads the current scene.
            StartCoroutine(TakeLife());
        }
        else
        {
            /* Loads Main Menu and resets the current gameSession gameObject (by destroying it, which will destroy
               the "SetUpSingleton()" method and thus lets us keep the newly instantiated gameSession gameObject in the Main Menu scene) */
            ResetGameSession();
        }
    }
    
    private void HeartSystem()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth) // controls which type of heart is displayed
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            
            if (i < maxNumOfHealth) // controls how many hearts should be displayed in relation to
                                    // the max allowed health that the player can have
            {                       
                hearts[i].enabled = true;  
            }
            else
            {
                hearts[i].enabled = false; // This hides any extra hearts in the array that we have, that is more than maxNumOfHealth.
            }
        }
    }

    IEnumerator TakeLife()
    {
        playerHealth--;
        // playerLivesText.text = playerLives.ToString();
        yield return new WaitForSecondsRealtime(resetSceneDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    // When player picks up extra life
    public void HealthUpdate()
    {
        if (playerHealth < maxNumOfHealth)
        {
            playerHealth++;
        }
        else if (playerHealth == maxNumOfHealth)
        {
            maxNumOfHealth++;
            playerHealth++;
        }
        
    }
    
    /* Loads the "Main Menu" scene (or send the player to the Main menu) & destroys the currect gameSession 
      gameObject so that all the states in it is reset when we load to the main menu. */
    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    
    public void ScoreUpdate(int coinValue)
    {
        startingScore += coinValue;
        scoreText.text = startingScore.ToString();
    }
}
