using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
