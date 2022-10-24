using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
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
                manager.WaypointDestroyed(this);
                Destroy(this.gameObject);
            }
        }
    }

    public void toggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
