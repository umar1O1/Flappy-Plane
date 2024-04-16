using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Vector2 moveDir;
    public float speedMultiplier;

    Material material;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        material.mainTextureOffset += speedMultiplier * Time.deltaTime * moveDir;
    }
}
