using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] protected Vector2 _velocity;

    private bool _isGrounded;
    private Vector2 _groundNormal;
    private Rigidbody2D _rigidBody;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);

    private const float _minMoveDistance = 0.001f;
    private const float _shellRadius = 0.01f;

    protected Vector2 TargetVelocity;
    public bool IsGrounded => _isGrounded;

    protected virtual void OnEnable()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.isKinematic = true;
    }

    protected virtual void OnDisable()
    {
        _rigidBody.isKinematic = false;
    }

    protected virtual void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    protected virtual void FixedUpdate()
    {
        if (_velocity.y < 0)
            _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        else
            _velocity += Physics2D.gravity * Time.deltaTime;

        _velocity.x = TargetVelocity.x;

        _isGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);

    }

    private void Movement(Vector2 move, bool yMovement)
    {
        var distance = move.magnitude;

        if (distance > _minMoveDistance)
        {
            int count = _rigidBody.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);

            _hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    _isGrounded = true;

                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }
                else
                {
                    _velocity.x *= 0;
                    _velocity.y = Mathf.Min(_velocity.y, 0);
                }

                var modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        _rigidBody.position = _rigidBody.position + move.normalized * distance;
    }
}
