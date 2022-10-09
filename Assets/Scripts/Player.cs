using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject projectile;
    
    [SerializeField] public float speed = 20.0f;
    [SerializeField] public float rotateSpeed = 45.0f;
    [SerializeField] public float eggSpeed = 40.0f;
    
    private bool _keyMode = false;
    private Rigidbody2D _rigidbody;
    private Vector3 _bounds;
    private bool _allowFire = true;

    void Start()
    {
        _bounds  = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Space))
        {
            if (_allowFire)
                StartCoroutine(FireProjectile());
        }
        
        if (Input.GetKeyDown(KeyCode.M)) _keyMode = !_keyMode;
    }

    private IEnumerator FireProjectile()
    {
        _allowFire = false;
        GameObject spawnedProjectile = Instantiate(projectile);
        spawnedProjectile.transform.up = transform.up;
        spawnedProjectile.transform.position = transform.position + (transform.up * 4);
        spawnedProjectile.GetComponent<Rigidbody2D>().velocity = (Vector2)(transform.up * (eggSpeed + _rigidbody.velocity.magnitude)); 
        yield return new WaitForSeconds(.2f);
        _allowFire = true;
    }
    
    private void Move()
    {
        transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * 
                                          (rotateSpeed * Time.smoothDeltaTime));
        if (_keyMode)
        {
            Vector3 newPos = transform.position;
            speed += Input.GetAxis("Vertical");
            newPos += transform.up * (Time.deltaTime * speed);

            float wrapX = _bounds.x + 4f;
            float wrapY = _bounds.y + 4f;
            
            
            newPos.x = (((newPos.x + wrapX) % (wrapX * 2)) + (wrapX * 2)) % (wrapX * 2) - wrapX;
            newPos.y = (((newPos.y + wrapY) % (wrapY * 2)) + (wrapY * 2)) % (wrapY * 2) - wrapY;
            
            _rigidbody.position = newPos;
        }
        else
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;

            pos.x = Math.Clamp(pos.x, -_bounds.x, _bounds.x);
            pos.y = Math.Clamp(pos.y, -_bounds.y, _bounds.y);
            
            _rigidbody.position = pos;
        }
    }
}
