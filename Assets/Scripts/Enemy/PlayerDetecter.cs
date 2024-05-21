using UnityEngine;

public class PlayerDetecter : MonoBehaviour
{
    private Health _healthEnemy;

    public Health PlayerHealth { get; private set; }

    private void Awake()
    {
        _healthEnemy = GetComponentInParent<Health>();
    }

    private void OnEnable()
    {
        _healthEnemy.onDeath += DisableComponentAtDeath;
    }

    private void OnDisable()
    {
        _healthEnemy.onDeath -= DisableComponentAtDeath;
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        collider.TryGetComponent(out Health playerHealth);

        if (playerHealth != null)
        {
            PlayerHealth = playerHealth;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Health>())
        {
            PlayerHealth = null;
        }
    }

    private void DisableComponentAtDeath()
    {
        this.enabled = false;
    }
}