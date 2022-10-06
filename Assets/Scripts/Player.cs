using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float maxX;
    private float maxY;

    // Start is called before the first frame update
    void Start()
    {
        maxX = Camera.main.orthographicSize;
        maxY = maxX * Camera.main.aspect;
        Debug.Log("X: " + maxX + ", Y: " + maxY);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        pos.x = Math.Clamp(pos.x, -maxX, maxX);
        pos.y = Math.Clamp(pos.y, -maxY, maxY);
        transform.position = pos;
    }
}
