using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public GameObject enemy;
    [SerializeField] public int numEnemies = 10;

    [SerializeField] public GameObject waypoint;
    [SerializeField] public Sprite[] waypointSprites;
    
    [SerializeField] public TextMeshProUGUI enemiesDestroyedText;
    [SerializeField] public TextMeshProUGUI sequenceText;

    public bool sequential = true;

    private List<Waypoint> _waypoints;
    private List<Enemy> _enemies;
    private Vector2 _bounds;
    private int _enemiesDestroyed = 0;
    
    private void Start()
    {
        _enemies = new List<Enemy>();
        _waypoints = new List<Waypoint>();
        
        _bounds  = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
        _bounds.x *= 0.9f;
        _bounds.y *= 0.9f;

        for (int i = 0; i < numEnemies; i++)
            SpawnEnemy(false);
        
        SpawnWaypoints();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            for (int i = 0; i < _waypoints.Count; i++)
            {
               _waypoints[i].toggleVisibility();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            
            sequential = !sequential;

            sequenceText.text = "Plane Mode: " + (sequential ? "Sequential" : "Random");
        }
    }

    public void SpawnEnemy(bool playerDestroyed)
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(-_bounds.x, _bounds.x);
        pos.y = Random.Range(-_bounds.y, _bounds.y);
        GameObject spawnedEnemy = Instantiate(enemy);
        spawnedEnemy.transform.position = pos;

        Enemy newEnemy = spawnedEnemy.GetComponent<Enemy>();
        newEnemy.manager = this;
        _enemies.Add(newEnemy);
        

        if (playerDestroyed)
        {
            _enemiesDestroyed++;
            enemiesDestroyedText.text = "Enemies Destroyed: " + _enemiesDestroyed;
        }
    }

    public List<Waypoint> GetWaypoints()
    {
        return _waypoints;
    }

    public void SpawnWaypoints()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos;

            switch (i)
            {
                case 0:
                    pos = new Vector3(-_bounds.x / 2, _bounds.y / 1.2f);
                    break;
                case 1:
                    pos = new Vector3(_bounds.x / 2, -_bounds.y / 1.2f);
                    break;
                case 2:
                    pos = new Vector3(_bounds.x / 2, _bounds.y / 1.2f);
                    break;
                case 3:
                    pos = new Vector3(-_bounds.x / 2, -_bounds.y / 1.2f);
                    break;
                case 4:
                    pos = new Vector3(- _bounds.x / 5, 0);
                    break;
                case 5:
                    pos = new Vector3(_bounds.x / 5, 0);
                    break;
                default:
                    pos = new Vector3(0, 0);
                    break;
            }

            GameObject spawnedWaypoint = Instantiate(waypoint);
            spawnedWaypoint.transform.position = pos;
            spawnedWaypoint.GetComponent<SpriteRenderer>().sprite = waypointSprites[i];
            
            Waypoint newWaypoint = spawnedWaypoint.GetComponent<Waypoint>();
            newWaypoint.manager = this;
            _waypoints.Add(newWaypoint);
        }
    }

    public void WaypointDestroyed(Waypoint destroyedWaypoint)
    {
        int index = _waypoints.IndexOf(destroyedWaypoint);
        _waypoints.Remove(destroyedWaypoint);
        Vector3 destroyedPos = destroyedWaypoint.gameObject.transform.position;
        Vector3 newPos = destroyedPos;
        Sprite waypointSprite = destroyedWaypoint.gameObject.GetComponent<SpriteRenderer>().sprite;
        
        GameObject spawnedWaypoint = Instantiate(waypoint);
        newPos.x = Random.Range(destroyedPos.x - 15, destroyedPos.x + 15);
        newPos.y = Random.Range(destroyedPos.y - 15, destroyedPos.y + 15);
        spawnedWaypoint.transform.position = newPos;
        spawnedWaypoint.GetComponent<SpriteRenderer>().sprite = waypointSprite;

        Waypoint newWaypoint = spawnedWaypoint.GetComponent<Waypoint>();
        newWaypoint.manager = this;
        _waypoints.Insert(index, newWaypoint);

        foreach (Enemy e in _enemies)
        {
            e.WaypointDestroyed(index);
        }
    }
}
