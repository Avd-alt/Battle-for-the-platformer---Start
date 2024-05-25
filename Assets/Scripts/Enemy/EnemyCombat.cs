using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _playerLayer;

    private float _oneSecondTime = 1f;
    private float _attackRate = 1f;
    private float _nextAttackTime = 0;
    private float _damage = 15f;
    private Health _health;
    private Coroutine _coroutineAttack;

    public event Action Attacked;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        if(_coroutineAttack != null)
        {
            StopCoroutine(_coroutineAttack);
        }

        _coroutineAttack = StartCoroutine(AttackCoroutine());
    }

    private void OnEnable()
    {
        _health.Died += DisableComponentAtDeath;
    }

    private void OnDisable()
    {
        _health.Died -= DisableComponentAtDeath;
    }

    private void Attack()
    {
        Collider2D _playerHit = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);

        if (_playerHit != null)
        {
            if(_damage > 0)
            {
                Attacked?.Invoke();
                _playerHit.GetComponent<Health>().TakeDamage(_damage);
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        _nextAttackTime = Time.time + _oneSecondTime / _attackRate;

        var delay = new WaitForSeconds(_nextAttackTime);

        while (true)
        {
            if (Time.time >= _nextAttackTime)
            {
                Attack();
            }
            yield return delay;
        }
    }

    private void DisableComponentAtDeath()
    {
        StopCoroutine(_coroutineAttack);
        this.enabled = false;
    }
}