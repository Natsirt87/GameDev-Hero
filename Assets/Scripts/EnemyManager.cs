using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public GameObject enemy;
    [SerializeField] public int numEnemies = 10;

    [SerializeField] public TextMeshProUGUI enemyCount;
    [SerializeField] public TextMeshProUGUI enemiesDestroyedText;
    
    private Vector2 _bounds;
    private int _enemiesDestroyed = 0;
    
    private void Start()
    {
        enemyCount.text = "Enemies: " + numEnemies;
        
        _bounds  = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
        _bounds.x *= 0.9f;
        _bounds.y *= 0.9f;

        for (int i = 0; i < numEnemies; i++)
            SpawnEnemy(false);
    }

    public void SpawnEnemy(bool playerDestroyed)
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(-_bounds.x, _bounds.x);
        pos.y = Random.Range(-_bounds.y, _bounds.y);
        GameObject spawnedEnemy = Instantiate(enemy);
        spawnedEnemy.transform.position = pos;
        spawnedEnemy.GetComponent<Enemy>().manager = this;

        if (playerDestroyed)
        {
            _enemiesDestroyed++;
            enemiesDestroyedText.text = "Enemies Destroyed: " + _enemiesDestroyed;
        }
    }
}
