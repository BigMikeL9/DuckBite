using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCoinPickup : MonoBehaviour
{
    // Configs
    [SerializeField] AudioClip coinSFX;
    [SerializeField] [Range(0, 1)] float coinSFXVolume;

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position, coinSFXVolume);
        Destroy(gameObject);
    }
}
