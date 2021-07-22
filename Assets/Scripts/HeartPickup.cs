using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    // Configs
    [SerializeField] AudioClip heartPickupSFX;
    [SerializeField] [Range(0, 1)] float heartPickupSFXVolume = 0.1f;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<GameSesssion>().HealthUpdate();
        AudioSource.PlayClipAtPoint(heartPickupSFX, Camera.main.transform.position, heartPickupSFXVolume);
        Destroy(gameObject);
    }
}
