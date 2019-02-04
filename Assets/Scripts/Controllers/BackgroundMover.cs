using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    MeshRenderer meshRenderer;
    Vector2 savedOffset; //contains default offset of mesh render, which is typicaly (0,0)

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        savedOffset = meshRenderer.sharedMaterial.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        float newX = Time.time * scrollSpeed;
        Vector2 offset = new Vector2(newX, 0);
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    void OnDisable()
    {
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}