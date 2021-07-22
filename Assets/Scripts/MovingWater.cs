using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWater : MonoBehaviour
{
    // Configs
    [Tooltip("Game units per second")]
    [SerializeField] float scrollRate = 3f;
    
    private void Update()
    {
        waterMoving();
    }

    private void waterMoving()
    {
        float yMovement = scrollRate * Time.deltaTime;
        transform.Translate(new Vector2(0, yMovement));
    }
  
}
