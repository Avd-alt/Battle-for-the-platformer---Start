using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyPatrolGround))]
[RequireComponent (typeof(EnemyCombat))]
public class EnemyAnimator : MonoBehaviour
{
    private string _hurt = "Hurt";
    private string _death = "IsDead";
    private string _attack = "Attack";
    private string _isStay = "IsStay";
    private Animator _animatorEnemy;
    private Health _enemyHealth;
    private EnemyPatrolGround _enemyPatrolGround;
    private EnemyCombat _enemyCombat;

    private void Awake()
    {
        _animatorEnemy = GetComponent<Animator>();
        _enemyHealth = GetComponent<Health>();
        _enemyPatrolGround = GetComponent<EnemyPatrolGround>();
        _enemyCombat = GetComponent<EnemyCombat>();
    }

    private void OnEnable()
    {
        _enemyHealth.died += OnAnimationDeath;
        _enemyHealth.hurt += OnHurtAnimation;
        _enemyPatrolGround.stayed += OnAnimationStay;
        _enemyPatrolGround.run += OnAnimationRun;
        _enemyCombat.attacked += OnAnimationAttack;
    }

    private void OnDisable()
    {
        _enemyHealth.died -= OnAnimationDeath;
        _enemyHealth.hurt -= OnHurtAnimation;
        _enemyPatrolGround.stayed -= OnAnimationStay;
        _enemyPatrolGround.run -= OnAnimationRun;
        _enemyCombat.attacked -= OnAnimationAttack;
    }

    public void OnHurtAnimation()
    {
        _animatorEnemy.SetTrigger(_hurt);
    }

    public void OnAnimationDeath()
    {
        _animatorEnemy.SetBool(_death, true);
    }

    public void OnAnimationAttack()
    {
        _animatorEnemy.SetTrigger(_attack);
    }

    public void OnAnimationStay()
    {
        _animatorEnemy.SetBool(_isStay, true);
    }

    public void OnAnimationRun()
    {
        _animatorEnemy?.SetBool(_isStay, false);
    }
}