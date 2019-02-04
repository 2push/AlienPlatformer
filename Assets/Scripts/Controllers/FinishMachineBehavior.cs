using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMachineBehavior : MonoBehaviour
{
    [SerializeField] Sprite readySprite;
    SpriteRenderer spriteRenderer;
    ParticleSystem portalEffect;
    bool isReady;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        portalEffect = GetComponentInChildren<ParticleSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isReady)
            GameplayManager.instance.Win();
    }

    public void SetToBeReady()
    {
        isReady = true;
        portalEffect.Play();
        spriteRenderer.sprite = readySprite;
    }
}
