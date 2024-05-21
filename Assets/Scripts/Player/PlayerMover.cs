using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMover : MonoBehaviour
{
    public const string Horizontal = nameof(Horizontal);

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private float _currentRotation = 0;
    private bool _isGrounded = false;
    private bool _isJump = false;
    private Health _playerHealth;
    private CapsuleCollider2D _colliderPlayer;
    private Rigidbody2D _rigidbody;

    public event Action onRun;
    public event Action onStopRun;
    public event Action onJump;
    public event Action onStopJump;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _colliderPlayer = GetComponent<CapsuleCollider2D>();
    }

    private void OnEnable()
    {
        _playerHealth.onDeath += DisableComponentsAtDeath;
    }

    private void OnDisable()
    {
        _playerHealth.onDeath -= DisableComponentsAtDeath;
    }

    private void FixedUpdate()
    {
        if (_isJump)
        {
            Jump();
            _isGrounded = false;
            _isJump = false;
        }
    }

    private void Update()
    {
        if (Input.GetButton(Horizontal))
        {
            Move();
            onRun?.Invoke();
        }
        else
        {
            onStopRun?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isJump = true;
            onJump?.Invoke();
        }
        else
        {
            onStopJump?.Invoke();
        }
    }

    private void Move()
    {
        Vector3 direction = Vector3.right * Input.GetAxis(Horizontal);

        transform.position += direction * _speed * Time.deltaTime;

        if (direction.x != 0)
        {
            _currentRotation = direction.x < 0.0f ? 180 : 0;
            transform.rotation = Quaternion.Euler(0, _currentRotation, 0);
        }
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground))
        {
            _isGrounded = true;
        }
    }

    private void DisableComponentsAtDeath()
    {
        _rigidbody.simulated = false;
        _colliderPlayer.enabled = false;
        this.enabled = false;
    }
}