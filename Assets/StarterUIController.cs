using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarterUIController : MonoBehaviour
{
    [SerializeField] Transform player;
    [Header("Starter text parts")]
    [SerializeField, TextArea] string starterQuote1;
    [SerializeField, TextArea] string starterQuote2;
    [SerializeField, TextArea] string starterQuote3;

    TextMeshProUGUI textComponent;
    Queue<string> textParts;

    private void Awake()
    {
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        textParts = new Queue<string>();
        textParts.Enqueue(starterQuote1);
        textParts.Enqueue(starterQuote2);
        textParts.Enqueue(starterQuote3);
    }

    private void Update()
    {       
        if (Input.anyKeyDown)
        {
            if (textParts.Count > 0)
                textComponent.text = textParts.Dequeue();
            else
                GameplayManager.instance.Begin();
        }
    }

    public void ShowText()
    {
        textComponent.text = textParts.Dequeue();
    } 
}
