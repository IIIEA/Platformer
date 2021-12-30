using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class AnimationController : KinematicObject
{
    [SerializeField] private float _maxSpeed = 7;
    [SerializeField] private EnemyController _enemy;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public float MaxSpeed => _maxSpeed;

    protected virtual void Awake()
    {
        _enemy = GetComponent<EnemyController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        if (_enemy.Move.x > 0.01f)
            _spriteRenderer.flipX = true;
        else if (_enemy.Move.x < -0.01f)
            _spriteRenderer.flipX = false;

        _animator.SetFloat("velocityX", Mathf.Abs(_velocity.x) / _maxSpeed);

        _targetVelocity = _enemy.Move * _maxSpeed;
    }
}