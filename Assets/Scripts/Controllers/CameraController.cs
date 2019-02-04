using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform background;
    [SerializeField] float smooth = 1f;
    Vector3 previousCamPos;
    Transform cachedTransform;


    void Awake()
    {
        cachedTransform = transform;
    }
    void Start()
    {
        previousCamPos = cachedTransform.position;
    }

    void LateUpdate()
    {
        cachedTransform.position = new Vector3(cachedTransform.position.x, player.position.y, cachedTransform.position.z);

        float parallax = previousCamPos.y - cachedTransform.position.y;
        float backgroundTargetY = background.position.y + parallax;
        Vector3 backgroundTargetPos = new Vector3(background.position.x, backgroundTargetY, background.position.z);
        background.position = Vector3.Lerp(background.position, backgroundTargetPos, smooth * Time.deltaTime);
        previousCamPos = cachedTransform.position;
    }
}