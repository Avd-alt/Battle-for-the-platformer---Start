using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(Health))]
[RequireComponent (typeof(PlayerMover))]
[RequireComponent (typeof(PlayerCombat))]
public class PlayerAnimator : MonoBehaviour
{
    private string _run = "Run";
    private string _jump = "Jump";
    private string _attack = "Attack";
    private string _hurt = "Hurt";
    private string _isDead = "IsDead";
    private Animator _animatorPlayer;
    private Health _health;
    private PlayerMover _playerMover;
    private PlayerCombat _playerCombat;

    private void Awake()
    {
        _animatorPlayer = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _playerMover = GetComponent<PlayerMover>();
        _playerCombat = GetComponent<PlayerCombat>();
    }

    private void OnEnable()
    {
        _health.hurt += OnAnimationHurt;
        _health.died += OnDeathAnimation;
        _playerMover.run += OnRunAnimation;
        _playerMover.stoppedRun += OffRunAnimation;
        _playerMover.jumped += OnJumpAnimation;
        _playerMover.stoppedJumped += OffJumpAnimation;
        _playerCombat.attacked += AttackAnimation;
    }

    private void OnDisable()
    {
        _health.hurt -= OnAnimationHurt;
        _health.died -= OnDeathAnimation;
        _playerMover.run -= OnRunAnimation;
        _playerMover.stoppedRun -= OffRunAnimation;
        _playerMover.jumped -= OnJumpAnimation;
        _playerMover.stoppedJumped -= OnJumpAnimation;
        _playerCombat.attacked -= AttackAnimation;
    }

    private void OnRunAnimation()
    {
        _animatorPlayer.SetBool(_run, true);
    }

    private void OffRunAnimation()
    {
        _animatorPlayer.SetBool(_run, false);
    }

    private void OnJumpAnimation()
    {
        _animatorPlayer.SetBool(_jump, true);
    }

    private void OffJumpAnimation()
    {
        _animatorPlayer.SetBool(_jump, false);
    }

    private void AttackAnimation()
    {
        _animatorPlayer.SetTrigger(_attack);
    }

    private void OnAnimationHurt()
    {
        _animatorPlayer.SetTrigger(_hurt);
    }

    private void OnDeathAnimation()
    {
        _animatorPlayer.SetBool(_isDead, true);
    }
}