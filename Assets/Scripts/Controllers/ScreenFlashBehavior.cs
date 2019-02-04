using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreenFlashBehavior : MonoBehaviour
{
    [SerializeField] float blinkSpeed;
    Image image;
    GameObject player;

    private void Awake()
    {
        image = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        StartCoroutine(ProduceFlash());
    }

    IEnumerator ProduceFlash()
    {
       do
       {
            if (image.color.a >= 1)
            {
                player.SetActive(false);
                blinkSpeed *= -1;
            }
            Color tempColor = image.color;
            tempColor.a = tempColor.a + blinkSpeed;
            image.color = tempColor;
            yield return null;          
       } while (image.color.a > 0);
    }
}