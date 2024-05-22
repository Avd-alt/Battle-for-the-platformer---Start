using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float _maxHealth = 100;
    private float _minHealth = 0;
    private float _currentHealth;
    private Coroutine _destroyCoroutine;

    public event Action died;
    public event Action hurt;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void OnDestroy()
    {
        if (_destroyCoroutine != null)
        {
            StopCoroutine(_destroyCoroutine);
        }
    }

    public void TakeDamage(float damage)
    {
        if(damage > 0)
        {
            _currentHealth -= damage;

            hurt?.Invoke();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(float amountHealthRestore)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amountHealthRestore, _minHealth, _maxHealth);
    }

    private void Die()
    {
        died?.Invoke();
        _destroyCoroutine = StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        float delay = 5f;

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}