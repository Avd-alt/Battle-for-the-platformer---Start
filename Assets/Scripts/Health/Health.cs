using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float _health = 100;
    private float _currentHealth;
    private Coroutine _destroyCoroutine;

    public event Action onDeath;
    public event Action onHurt;

    private void Start()
    {
        _currentHealth = _health;
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

            onHurt?.Invoke();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void RestoreHealth(float amountHealthRestore)
    {
        if (amountHealthRestore > 0)
        {
            _currentHealth += amountHealthRestore;

            if (_currentHealth >= _health)
            {
                _currentHealth = _health;
            }
        }
    }

    private void Die()
    {
        onDeath?.Invoke();
        _destroyCoroutine = StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        float delay = 5f;

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}