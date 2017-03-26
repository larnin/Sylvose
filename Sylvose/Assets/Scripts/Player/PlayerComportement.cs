using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerComportement : MonoBehaviour
{
    public float groundDistanceDetection = 1.0f;
    public float maxSpeed = 1.0f;
    public float acceleration = 1.0f;
    public float jumpVelocity = 1.0f;
    public LayerMask groundLayer;
    public float attackOffset = 1.0f;
    public GameObject attackPrefab;
    public GameObject contaminer;
    public GameObject plantPrefab;
    public float plantRange = 2.0f;
    public float plantRadius = 2.0f;
    public float growSpeed = 1.0f;

    Rigidbody2D _rigidbody;
    BoxCollider2D _collider;
    SpriteRenderer _renderer;
    bool _grounded = false;
    float _currentSpeed = 0;
    bool _moveLeft = false;

    void Awake()
    {
        G.Sys.playerComportement = this;
    }

    void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        float value = Input.GetAxisRaw("Horizontal");
        if (value != 0)
            _moveLeft = value < 0;
        float currentMaxSpeed = maxSpeed * Mathf.Abs(value);
        float currentAcceleration = value * acceleration * Time.deltaTime;

        if (Mathf.Abs(_currentSpeed) > currentMaxSpeed)
        {
            float sign = Mathf.Sign(_currentSpeed);
            _currentSpeed -= sign * acceleration * Time.deltaTime;
            if (Mathf.Abs(_currentSpeed) < currentMaxSpeed || Mathf.Sign(_currentSpeed) != sign)
                _currentSpeed = sign * currentMaxSpeed;
        }
        else
        {
            _currentSpeed += currentAcceleration;
            if (Mathf.Abs(_currentSpeed) > currentMaxSpeed)
                _currentSpeed = Mathf.Sign(_currentSpeed) * currentMaxSpeed;
        }

        _renderer.flipX = _moveLeft;
        
        _rigidbody.velocity = new Vector2(_currentSpeed, _rigidbody.velocity.y);

        _grounded = Physics2D.BoxCast(transform.position, _collider.size, 0, new Vector2(0, -1), groundDistanceDetection, groundLayer).collider != null;

        if (_grounded && Input.GetButtonDown("Jump"))
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.y, jumpVelocity);

        if (Input.GetButtonDown("Fire1"))
            Attack();

        if (Input.GetButtonDown("Fire2"))
            Contamine();

        if (Input.GetButton("Fire3"))
            GrowPlant();

    }

    void Attack()
    {
        var attackObj = Instantiate(attackPrefab);
        attackObj.transform.position = transform.position + new Vector3(_moveLeft ? -attackOffset : attackOffset, 0, 0);
    }

    void Contamine()
    {
        var contamineObj = Instantiate(contaminer);
        contamineObj.transform.position = transform.position;
    }

    void GrowPlant()
    {
        var objects = Physics2D.OverlapCircleAll(transform.position + new Vector3(_moveLeft ? -plantRange : plantRange, 0, 0), plantRadius);
        bool foundPlant = false;
        foreach(var o in objects)
        {
            var comp = o.GetComponent<Plant>();
            if (comp == null)
                continue;
            foundPlant = true;
            comp.Grow(Time.deltaTime * growSpeed);
            break;
        }
        if (foundPlant)
            return;
        var grounds = Physics2D.RaycastAll(transform.position + new Vector3(_moveLeft ? -plantRange : plantRange, 0, 0), new Vector2(0, -plantRadius), groundLayer);
        if (grounds.Length == 0)
            return;
        Vector3 pos = transform.position;
        float dist = plantRadius;
        bool ok = false;
        foreach(var g in grounds)
        {
            if (g.distance < dist)
            {
                pos = g.point;
                ok = true;
            }
        }
        if (!ok)
            return;
        var plant = Instantiate(plantPrefab);
        plant.transform.position = pos;
    }
}
