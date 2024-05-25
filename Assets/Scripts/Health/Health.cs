using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _maxHealth = 100;
    private float _minHealth = 0;
    private float _currentHealth;
    private Coroutine _destroyCoroutine;

    public event Action Died;
    public event Action Hurt;

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
            _currentHealth = Mathf.Clamp(_currentHealth - damage, _minHealth, _maxHealth);

            Hurt?.Invoke();

            if(_currentHealth == _minHealth)
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
        Died?.Invoke();
        _destroyCoroutine = StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        float delay = 5f;

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}