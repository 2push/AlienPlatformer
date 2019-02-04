using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    List<GameObject> coins;
    FinishMachineBehavior finishMachine;
    int _collectedCoins;

    int CollectedCoins { get => _collectedCoins;
        set
        {
            _collectedCoins = value; 
            scoreText.text = _collectedCoins.ToString();
        }
    }

    void Awake()
    {
        finishMachine = FindObjectOfType<FinishMachineBehavior>();
    }
    void Start()
    {
        CollectedCoins = 0;
        coins = new List<GameObject>();
        foreach (Transform coin in transform)
        {
            if(coin.CompareTag("Coin"))
                coins.Add(coin.gameObject);
            else
                Debug.LogWarning("There is an object in Coins of other type");
        }
    }

    public void CollectCoin()
    {
        if (++CollectedCoins >= coins.Count) //incrementing collected coins number
        {
            finishMachine.SetToBeReady();
        }
    }
    public void RespawnCoins()
    {
        CollectedCoins = 0;
        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
        }
    }
}
