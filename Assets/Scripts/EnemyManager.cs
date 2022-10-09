using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public GameObject enemy;
    [SerializeField] public int numEnemies = 10;
    [SerializeField] public float minEnemySpread = 10f;

    private List<GameObject> _enemies;
    private Vector2 _bounds;
    
    private void Start()
    {
        _enemies = new List<GameObject>();
        
        _bounds  = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
        _bounds.x *= 0.9f;
        _bounds.y *= 0.9f;

        for (int i = 0; i < numEnemies; i++)
            SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        bool goodPosition = false;
        
        Vector3 pos= new Vector3();
        while (!goodPosition)
        {
            pos.x = Random.Range(-_bounds.x, _bounds.x);
            pos.y = Random.Range(-_bounds.y, _bounds.y);

            goodPosition = true;
            if (_enemies.Count > 0)
            {
                foreach (GameObject other in _enemies)
                {
                    if (!other)
                    {
                        _enemies.Remove(other);
                        break;
                    }
                    
                    float distance = Vector3.Distance(other.transform.position, pos);

                    if (distance < minEnemySpread)
                    {
                        goodPosition = false;
                        break;
                    }
                }
            }
        }
        
        GameObject spawnedEnemy = Instantiate(enemy);
        spawnedEnemy.transform.position = pos;
        spawnedEnemy.GetComponent<Enemy>().manager = this;
        _enemies.Add(spawnedEnemy);
    }
}
