using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyManager manager;
    
    [SerializeField] public int health = 100;
    [SerializeField] public float speed = 15f;
    [SerializeField] public float rotationSpeed = 8f;
    
    private List<Waypoint> _waypoints;
    private Vector3 _curWaypoint;
    private int _curWaypointIndex;
    
    private Rigidbody2D _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        // Set the current waypoint to a random one
        _waypoints = manager.GetWaypoints();
        _curWaypointIndex = Random.Range(0, _waypoints.Count - 1);
        _curWaypoint = _waypoints[_curWaypointIndex].gameObject.transform.position;
    }

    private void Update()
    {
        // Move forward at a set speed
        Vector3 newPos = transform.position;
        newPos += transform.up * (Time.deltaTime * speed);
        _rigidbody.position = newPos;

        // Always rotate to look at the next waypoint
        Vector3 relativePos = _curWaypoint - newPos;
        Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, relativePos);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);

        if (relativePos.magnitude < 15)
        {
            NextWaypoint();
        }
    }

    private void NextWaypoint()
    {
        if (manager.sequential)
            _curWaypointIndex = (_curWaypointIndex < _waypoints.Count - 1 ? _curWaypointIndex + 1 : 0);
        else
            _curWaypointIndex = Random.Range(0, _waypoints.Count - 1);

        _curWaypoint = _waypoints[_curWaypointIndex].gameObject.transform.position;
    }

    public void WaypointDestroyed(int destroyedIndex)
    {
        Debug.Log("Waypoint destroyed at index " + destroyedIndex);
        _waypoints = manager.GetWaypoints();

        if (_curWaypointIndex == destroyedIndex)
        {
            _curWaypoint = _waypoints[_curWaypointIndex].gameObject.transform.position;
        }
    }
    
    

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
                manager.SpawnEnemy(true);
            }
        }
        else if (!other.gameObject.CompareTag("Waypoint") && !other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
            manager.SpawnEnemy(other.gameObject.CompareTag("Player"));
        }
    }
}
