using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComportement : MonoBehaviour
{
    public float groundDistanceDetection = 1.0f;
    public float maxSpeed = 1.0f;
    public float acceleration = 1.0f;
    public float jumpVelocity = 1.0f;
    public LayerMask groundLayer;

    Rigidbody2D _rigidbody;
    BoxCollider2D _collider;
    bool _grounded = false;
    float _currentSpeed = 0;

    void Awake()
    {
        G.Sys.playerComportement = this;
    }

    void Start ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
	}
	
	void Update ()
    {
        float value = Input.GetAxisRaw("Horizontal");
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
        
        _rigidbody.velocity = new Vector2(_currentSpeed, _rigidbody.velocity.y);

        _grounded = Physics2D.BoxCast(transform.position, _collider.size, 0, new Vector2(0, -1), groundDistanceDetection, groundLayer).collider != null;

        if (_grounded && Input.GetButtonDown("Jump"))
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.y, jumpVelocity);
	}
}
