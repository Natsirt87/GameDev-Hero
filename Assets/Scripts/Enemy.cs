using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyManager manager;
    
    [SerializeField] public int health = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
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
        else
        {
            Destroy(this.gameObject);
            manager.SpawnEnemy();
        }
    }
}
