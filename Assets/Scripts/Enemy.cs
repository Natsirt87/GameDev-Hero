using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyManager manager;
    
    private int health = 100;

    void Start()
    {
        Debug.Log(manager);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            health -= 25;
            Color col = GetComponent<SpriteRenderer>().color;
            col.a = health * 0.01f;
            GetComponent<SpriteRenderer>().color = col;
            if (health <= 0)
            {
                Destroy(this.gameObject);
                manager.SpawnEnemy();
            }
                
        }
    }
}
