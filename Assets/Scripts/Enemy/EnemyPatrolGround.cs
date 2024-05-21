using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Health))]
public class EnemyPatrolGround : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private PlayerDetecter _detecterPlayer;

    private float _originalSpeed = 2f;
    private float _speed;
    private bool _isMovingLeft = true;
    private int _randomIndexPoint;
    private Coroutine _coroutineSpeed;
    private Rigidbody2D _rigidbodyEnemy;
    private CapsuleCollider2D _colliderEnemy;
    private Health _healthEnemy;

    public event Action onStayble;
    public event Action onRun;

    private void Awake()
    {
        _rigidbodyEnemy = GetComponent<Rigidbody2D>();
        _colliderEnemy = GetComponent<CapsuleCollider2D>();
        _healthEnemy = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _healthEnemy.onDeath += DisableComponentsAtDeath;
    }

    private void OnDisable()
    {
        _healthEnemy.onDeath -= DisableComponentsAtDeath;
    }

    private void Start()
    {
        _speed = _originalSpeed;
        _randomIndexPoint = GetRandomIndex();
    }

    private void Update()
    {
        if(_detecterPlayer.PlayerHealth != null)
        {
            MoveToPlayer(_detecterPlayer.PlayerHealth );
        }
        else
        {
            MoveToPoint();
        }
    }

    public void OffMoveToHurt()
    {
        if (_speed != 0)
        {
            _coroutineSpeed = StartCoroutine(StopMoveAfterTakingDamage());
        }

        StopCoroutine(StopMoveAfterTakingDamage());
    }

    private void Turn()
    {
        float scaleX = 1.5f;
        float reverseScaleX = -1.5f;
        float scaleY = 1.5f;
        float scaleZ = 1f;

        if (_isMovingLeft)
        {
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
        else
        {
            transform.localScale = new Vector3(reverseScaleX, scaleY, scaleZ);
        }
    }

    private IEnumerator StopMoveAfterTakingDamage()
    {
        var delay = 0.25f;

        _speed = 0;

        yield return new WaitForSeconds(delay);

        _speed = _originalSpeed;
    }

    private void MoveToPoint()
    {
        float distanceChangePoint = 0.2f;

        Turn();

        Vector3 direction = _points[_randomIndexPoint].position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, _points[_randomIndexPoint].position, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _points[_randomIndexPoint].position) < distanceChangePoint)
        {
            _randomIndexPoint = GetRandomIndex();
        }

        if (direction.x > 0)
        {
            _isMovingLeft = false;
        }
        else if (direction.x < 0)
        {
            _isMovingLeft = true;
        }
    }

    private void MoveToPlayer(Health playerHealth)
    {
        float distanceToAttack = 1f;
        float distanceToPlayer = Vector2.Distance(transform.position, playerHealth.transform.position);

        if (distanceToPlayer < distanceToAttack)
        {
            StopMove();
        }
        else
        {
            _speed = _originalSpeed;

            onRun?.Invoke();

            transform.position = Vector2.MoveTowards(transform.position, playerHealth.transform.position, _speed * Time.deltaTime);
        }
    }

    private void StopMove()
    {
        _speed = 0;

        onStayble?.Invoke();
    }

    private void DisableComponentsAtDeath()
    {
        _rigidbodyEnemy.simulated = false;
        _colliderEnemy.enabled = false;
        this.enabled = false;
    }

    private int GetRandomIndex() => Random.Range(0, _points.Length);
}