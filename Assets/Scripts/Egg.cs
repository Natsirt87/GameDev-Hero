using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI eggText;

    void Start()
    {
        eggText = GameObject.Find("/Canvas/Eggs").GetComponent<TextMeshProUGUI>();
        int eggCount = Int32.Parse(eggText.text.Substring(6)) + 1;
        eggText.text = "Eggs: " + eggCount;
    }

    void OnBecameInvisible()
    {
        int eggCount = Int32.Parse(eggText.text.Substring(6)) - 1;
        eggText.text = "Eggs: " + eggCount;
        Destroy(this.gameObject);
    }
}
