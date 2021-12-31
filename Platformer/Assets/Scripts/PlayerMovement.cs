using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : PhysicsMovement
{
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private float _jumpTakeOffSpeed = 6;
    [SerializeField] private float _jumpModifier = 1.5f;
    [SerializeField] private float _jumpDeceleration = 0.5f;
    [SerializeField] private bool _controlEnabled = true;
    [SerializeField] private JumpState _jumpState = JumpState.Grounded;

    private bool _stopJump;
    private bool _jump;
    private Vector2 _move;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_controlEnabled)
        {
            _move.x = Input.GetAxis("Horizontal");

            if (_jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
            {
                _jumpState = JumpState.PrepareToJump;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                _stopJump = true;
            }
        }
        else
        {
            _move.x = 0;
        }

        UpdateJumpState();
        ComputeTargetVelocity();
    }

    void UpdateJumpState()
    {
        _jump = false;
        switch (_jumpState)
        {
            case JumpState.PrepareToJump:
                _jumpState = JumpState.Jumping;
                _jump = true;
                _stopJump = false;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    _jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    _jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                _jumpState = JumpState.Grounded;
                break;
        }
    }

    private void ComputeTargetVelocity()
    {
        if (_jump && IsGrounded)
        {
            _velocity.y = _jumpTakeOffSpeed * _jumpModifier;
            _jump = false;
        }
        else if (_stopJump)
        {
            _stopJump = false;
            if (_velocity.y > 0)
            {
                _velocity.y = _velocity.y * _jumpDeceleration;
            }
        }

        if (_move.x > 0.01f)
            _spriteRenderer.flipX = true;
        else if (_move.x < -0.01f)
            _spriteRenderer.flipX = false;

        _animator.SetBool(AnimationState.grounded.ToString(), IsGrounded);
        _animator.SetFloat(AnimationState.velocityX.ToString(), Mathf.Abs(_velocity.x) / _maxSpeed);

        TargetVelocity = _move * _maxSpeed;
    }

    private enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }

    private enum AnimationState
    {
        grounded,
        velocityX
    }
}