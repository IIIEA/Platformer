using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimationSetter), typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private PatrolPath _path;
    [SerializeField] private Vector2 _speed;

    private PatrolPath.Mover _mover;
    private EnemyAnimationSetter _animationSetter;
    private Collider2D _collider;

    public Vector2 Move => _speed;

    private void Awake()
    {
        _animationSetter = GetComponent<EnemyAnimationSetter>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_path != null)
        {
            if (_mover == null) _mover = _path.CreateMover(_animationSetter.MaxSpeed * 0.5f);
            _speed.x = Mathf.Clamp(_mover.Position.x - transform.position.x, -1, 1);
        }
    }

}