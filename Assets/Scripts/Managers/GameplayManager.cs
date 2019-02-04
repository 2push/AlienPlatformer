using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] int livesCapacity;
    [SerializeField] TextMeshProUGUI livesLeftText;
    [SerializeField] StarterUIController starterUI;
    [SerializeField] GameObject finishScreen;
    public static GameplayManager instance;
    int livesLeft;
    AlienController[] aliens;
    PlayerController player;
    CoinManager coinManager; //call to respawn coins after the player death
    int hitDamage = 1;

    public int LivesLeft { get => livesLeft;
        set
        {
            livesLeft = value;
            livesLeftText.text = livesLeft.ToString();
        }
    }

    void Awake()
    {
        if (instance == null)
        instance = this;
        aliens = FindObjectsOfType<AlienController>();
        player = FindObjectOfType<PlayerController>();
        coinManager = FindObjectOfType<CoinManager>();
    }
    
    void Start()
    {
        player.enabled = false;
        finishScreen.SetActive(false);
        Time.timeScale = 0;
        starterUI.ShowText();
    }

    public void Begin()
    {
        player.enabled = true;
        starterUI.gameObject.SetActive(false);
        Time.timeScale = 1;
        LivesLeft = livesCapacity;
    }

    void Restart()
    {
        ResetLives();
        RespawnAliens();
        RespawnPlayer();
        coinManager.RespawnCoins();
    }

    void ResetLives() => 
        LivesLeft = livesCapacity;

    void RespawnPlayer() => 
        player.Respawn();

    void RespawnAliens()
    {
        for(int i = 0; i < aliens.Length; i++)
        {
            aliens[i].Respawn();
        }
    }
    
    public void Win()
    {
        finishScreen.SetActive(true);
    }

    public void PlayerDamaged(bool isFallen)
    {
        LivesLeft -= isFallen ? LivesLeft : hitDamage;
        if (LivesLeft <= 0)
            Restart();
    }
}