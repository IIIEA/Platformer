using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(EnemyMovement))]
public class EnemyAnimator : PhysicsMovement
{
    [SerializeField] private float _maxSpeed = 7;
    [SerializeField] private EnemyMovement _enemy;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private const string VelocityX = nameof(VelocityX);
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

        if (_maxSpeed != 0)
            _animator.SetFloat(VelocityX, Mathf.Abs(_velocity.x) / _maxSpeed);

        TargetVelocity = _enemy.Move * _maxSpeed;
    }
}