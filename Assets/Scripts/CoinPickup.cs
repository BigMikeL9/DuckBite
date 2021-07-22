using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinPickup : MonoBehaviour
{
    // Configs
    [SerializeField] AudioClip coinSFX;
    [SerializeField] [Range(0, 1)] float coinSFXVolume;
    [SerializeField] int coinValue = 100;

    // Cache
    GameSesssion gameSesssion;

    private void Start()
    {
        gameSesssion = FindObjectOfType<GameSesssion>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameSesssion.ScoreUpdate(coinValue);
        AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position, coinSFXVolume);
        Destroy(gameObject);
    }
}
