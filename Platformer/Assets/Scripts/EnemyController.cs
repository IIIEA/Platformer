using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple controller for enemies. Provides movement control over a patrol path.
/// </summary>
[RequireComponent(typeof(AnimationController), typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private PatrolPath _path;
    [SerializeField] private Vector2 _move;

    private PatrolPath.Mover _mover;
    private AnimationController _control;
    private Collider2D _collider;

    public Vector2 Move => _move;

    void Awake()
    {
        _control = GetComponent<AnimationController>();
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_path != null)
        {
            if (_mover == null) _mover = _path.CreateMover(_control.MaxSpeed * 0.5f);
            _move.x = Mathf.Clamp(_mover.Position.x - transform.position.x, -1, 1);
        }
    }

}