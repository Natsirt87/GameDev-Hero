using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public int numEnemies = 10;

    private Vector2 _bounds;
    
    private void Start()
    {
        _bounds  = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
        _bounds.x *= 0.9f;
        _bounds.y *= 0.9f;

        for (int i = 0; i < numEnemies; i++)
            SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        Vector2 pos;
        pos.x = Random.Range(-_bounds.x, _bounds.x);
        pos.y = Random.Range(-_bounds.y, _bounds.y);

        GameObject spawnedEnemy = Instantiate(enemy);
        spawnedEnemy.transform.position = pos;
        spawnedEnemy.GetComponent<Enemy>().manager = this;
    }
}
