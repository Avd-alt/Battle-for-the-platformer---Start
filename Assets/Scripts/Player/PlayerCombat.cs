using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemyLayers;

    private int _attackDamage = 20;
    private float _attackRate = 1f;
    private float _nextAttackTime = 0;
    private Health _playerHealth;

    public event Action attacked;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _playerHealth.died += DisableComponentAtDeath;
    }

    private void OnDisable()
    {
        _playerHealth.died -= DisableComponentAtDeath;
    }

    private void Update()
    {
        float oneSecondTime = 1f;

        if(Time.time >= _nextAttackTime)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Attack();

                _nextAttackTime = Time.time + oneSecondTime/ _attackRate;
            }
        }
    }

    private void Attack()
    {
        attacked?.Invoke();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy != null)
            {
                if(_attackDamage > 0)
                {
                    enemy.GetComponent<Health>().TakeDamage(_attackDamage);
                }
            }
        }
    }

    private void DisableComponentAtDeath()
    {
        this.enabled = false;
    }
}