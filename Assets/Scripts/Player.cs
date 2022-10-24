using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public GameObject projectile;
    
    [SerializeField] public float speed = 20.0f;
    [SerializeField] public float rotateSpeed = 45.0f;
    [SerializeField] public float eggSpeed = 40.0f;

    [SerializeField] public TextMeshProUGUI controlMode;
    [SerializeField] public TextMeshProUGUI collisionsText;

    [SerializeField] public Slider cooldownBar;

    private bool _keyMode = false;
    private Rigidbody2D _rigidbody;
    private Vector3 _bounds;
    private bool _allowFire = true;
    private int _collisions = 0;

    private float timeToCooldown = 0;
    
    void Start()
    {
        _bounds  = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
        _bounds.x += 4;
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

        if (Input.GetKeyDown(KeyCode.M))
        {
            _keyMode = !_keyMode;
            if (_keyMode)
                controlMode.text = "Mode: Keyboard";
            else
                controlMode.text = "Mode: Mouse";
        }

        if (!_allowFire)
        {
            timeToCooldown += Time.deltaTime / 0.2f;
            cooldownBar.transform.localScale =
                Vector3.Lerp(new Vector3(1, 3, 0), new Vector3(0, 3, 0), timeToCooldown);
        }
    }

    private IEnumerator FireProjectile()
    {
        _allowFire = false;
        GameObject spawnedProjectile = Instantiate(projectile);
        spawnedProjectile.transform.up = transform.up;
        spawnedProjectile.transform.position = transform.position + (transform.up * 4);
        spawnedProjectile.GetComponent<Rigidbody2D>().velocity = (Vector2)(transform.up * (eggSpeed + _rigidbody.velocity.magnitude));
        yield return new WaitForSeconds(.2f);
        timeToCooldown = 0f;
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

            float wrapX = _bounds.x + 2f;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _collisions++;
            collisionsText.text = "Collisions: " + _collisions;
        }
    }
}
