using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicObject : MonoBehaviour
{
    [SerializeField] private float minGroundNormalY = .65f;
    [SerializeField] private float gravityModifier = 1f;

    [SerializeField] protected Vector2 _velocity;

    protected Vector2 _targetVelocity;

    private Vector2 _groundNormal;
    private Rigidbody2D _rigidBodu;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    public bool IsGrounded { get; private set; }

    protected virtual void OnEnable()
    {
        _rigidBodu = GetComponent<Rigidbody2D>();
        _rigidBodu.isKinematic = true;
    }

    protected virtual void OnDisable()
    {
        _rigidBodu.isKinematic = false;
    }

    protected virtual void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        _targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (_velocity.y < 0)
            _velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        else
            _velocity += Physics2D.gravity * Time.deltaTime;

        _velocity.x = _targetVelocity.x;

        IsGrounded = false;

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

        if (distance > minMoveDistance)
        {
            int count = _rigidBodu.Cast(move, _contactFilter, _hitBuffer, distance + shellRadius);

            _hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;

                if (currentNormal.y > minGroundNormalY)
                {
                    IsGrounded = true;

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

                var modifiedDistance = _hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        _rigidBodu.position = _rigidBodu.position + move.normalized * distance;
    }

}
