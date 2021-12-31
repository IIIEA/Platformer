using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(EnemyMovement))]
public class EnemyAnimationSetter : PhysicsMovement
{
    [SerializeField] private float _maxSpeed = 7;
    [SerializeField] private EnemyMovement _enemy;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public float MaxSpeed => _maxSpeed;

    protected virtual void Awake()
    {
        _enemy = GetComponent<EnemyMovement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_enemy.Move.x > 0.01f)
            _spriteRenderer.flipX = true;
        else if (_enemy.Move.x < -0.01f)
            _spriteRenderer.flipX = false;

        _animator.SetFloat("velocityX", Mathf.Abs(_velocity.x) / _maxSpeed);

        TargetVelocity = _enemy.Move * _maxSpeed;
    }
}