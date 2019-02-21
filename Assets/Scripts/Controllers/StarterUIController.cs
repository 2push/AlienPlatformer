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

    private void Start()
    {
        ShowIntroText();
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.touchCount > 0)
        {
            if (textParts.Count > 0)
                ShowIntroText();
            else
                GameplayManager.instance.Begin();
        }
    }

    private void ShowIntroText()
    {
        textComponent.text = textParts.Dequeue();
    }
}
