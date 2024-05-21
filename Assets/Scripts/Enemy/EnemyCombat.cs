using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _playerLayer;

    private float _attackRate = 1f;
    private float _nextAttackTime = 0;
    private float _damage = 15f;
    private Health _health;

    public event Action onAttacked;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.onDeath += DisableComponentAtDeath;
    }

    private void OnDisable()
    {
        _health.onDeath -= DisableComponentAtDeath;
    }

    private void Update()
    {
        float oneSecondTime = 1f;

        if (Time.time >= _nextAttackTime)
        {
            Attack();

            _nextAttackTime = Time.time + oneSecondTime / _attackRate;
        }
    }

    public void Attack()
    {
        Collider2D _playerHit = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);

        if (_playerHit != null)
        {
            if(_damage > 0)
            {
                onAttacked?.Invoke();
                _playerHit.GetComponent<Health>().TakeDamage(_damage);
            }
        }
    }

    private void DisableComponentAtDeath()
    {
        this.enabled = false;
    }
}