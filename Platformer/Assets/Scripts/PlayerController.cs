using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : KinematicObject
{
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private float _jumpTakeOffSpeed = 6;
    [SerializeField] private float _jumpModifier = 1.5f;
    [SerializeField] private float _jumpDeceleration = 0.5f;

    [SerializeField] private JumpState jumpState = JumpState.Grounded;

    [SerializeField] private bool controlEnabled = true;

    private bool _stopJump;
    private bool _jump;
    private Vector2 _move;
    private SpriteRenderer _spriteRenderer;
    internal Animator _animator;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (controlEnabled)
        {
            _move.x = Input.GetAxis("Horizontal");

            if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
            {
                jumpState = JumpState.PrepareToJump;
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
        base.Update();
    }

    void UpdateJumpState()
    {
        _jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                _jump = true;
                _stopJump = false;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }
    }

    protected override void ComputeVelocity()
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

        _animator.SetBool("grounded", IsGrounded);
        _animator.SetFloat("velocityX", Mathf.Abs(_velocity.x) / _maxSpeed);

        _targetVelocity = _move * _maxSpeed;
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }
}