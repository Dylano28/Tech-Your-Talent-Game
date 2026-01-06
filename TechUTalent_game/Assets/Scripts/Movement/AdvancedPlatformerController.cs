using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Input = UnityEngine.Input;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class AdvancedPlatformerController : MonoBehaviour
{
    private Vector3 _velocity;
    public Vector3 Velocity => _velocity;
    private int _lastXInput;
    public bool lockMovement;
    
    private bool _isGrounded;
    private bool _isOnWall;

    private const float GROUND_RAY_DIVISION = 1.95f;
    private const float WALL_RAY_DIVISION = 1.8f;
    private const int WALL_RAY_AMOUNT = 3;
    private readonly Vector3 GROUND_RAY_MARGIN = Vector3.right * 0.1f;
    
    [Header("Layers")] 
    [Tooltip("Solid ground & walls")] 
    [SerializeField] private LayerMask groundLayerMask = 0;

    [Tooltip("One-way platforms (use with one-way physics)")] 
    [SerializeField] private LayerMask oneWayLayerMask = 0;

    [Tooltip("Optional layer mask for platform effects (bounce pads etc.)")] 
    [SerializeField] private LayerMask platformEffectLayerMask = 0;

    private ContactFilter2D _groundFilter;
    private ContactFilter2D _oneWayFilter;
    
    private float _currentXSpeed = 0f;
    [Header("Movement")] [SerializeField] private float xSpeed = 10f;
    [SerializeField] private float xAccel = 1.25f;
    [SerializeField] private float airSpeedMod = 1.15f;
    
    [Header("Gravity & Jump")] [SerializeField]
    private float gravity = 1.2f;

    [SerializeField] private float gravityMax = 32f;
    [SerializeField] private bool gravityOnJump = true;

    private bool _isJumping;
    private bool _hasJumped;
    private float _currentCoyoteTime;
    private float _currentJumpPower;

    [SerializeField] private float jumpSpeed = 2048f;
    [SerializeField] private float jumpPower = 18f;
    [SerializeField] private float initialJumpPower = 6f;
    [SerializeField] private float coyoteTime = 0.04f;
    
    private bool _hasLanded;
    [SerializeField] public UnityEvent onJump;
    [SerializeField] public UnityEvent onLand;
    [SerializeField] public UnityEvent onStartMove;
    [SerializeField] public UnityEvent onStopMove;
    [SerializeField][HideInInspector] public UnityEvent onMoving;
    [SerializeField][HideInInspector] public UnityEvent onGrounded;
    
    private Rigidbody2D _rb;
    private Collider2D _coll;
    
    [Header("Runtime Layers")] [Tooltip("Name of the Player layer")] [SerializeField]
    private string playerLayerName = "Player";

    private int _playerLayerIndex;
    private int _oneWayLayerIndex;
    
    [Header("Drop-through")] [SerializeField]
    private float dropThroughTime = 0.25f;

    private bool _dropThrough;
    private float _dropThroughTimer;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;

        _coll = GetComponent<Collider2D>();
        
        _groundFilter = new ContactFilter2D();
        _groundFilter.SetLayerMask(groundLayerMask);
        _groundFilter.useTriggers = false;

        _oneWayFilter = new ContactFilter2D();
        _oneWayFilter.SetLayerMask(oneWayLayerMask);
        _oneWayFilter.useTriggers = false;

        _playerLayerIndex = LayerMask.NameToLayer(playerLayerName);
        _oneWayLayerIndex = LayerMaskToFirstLayerIndex(oneWayLayerMask);
    }
    
    private void FixedUpdate()
    {
        HandleInputAndMovement();
        ApplyGravity();
        
        HandleOneWayCollision();
        
        if (_dropThrough)
        {
            _dropThroughTimer -= Time.deltaTime;
            if (_dropThroughTimer <= 0f) _dropThrough = false;
        }

        if (!lockMovement)
            _rb.MovePosition(transform.position + _velocity * Time.deltaTime);
    }
    
    private void HandleInputAndMovement()
    {
        HorizontalMovement();
        VerticalMovement();
    }
    
    private void HorizontalMovement()
    {
        var input = Input.GetAxisRaw("Horizontal");
        var wallBlocked = _isOnWall && input == _lastXInput;
        if (input == 0 || wallBlocked)
        {
            onStopMove.Invoke();
            _velocity.x = 0f;
            _currentXSpeed = 0f;
            return;
        }

        onMoving.Invoke();
        if (_currentXSpeed == 0f && _isGrounded) onStartMove.Invoke();

        var newAirSpeedMod = _isGrounded ? 1f : airSpeedMod;
        _lastXInput = (int)input;
        _currentXSpeed = _currentXSpeed < xSpeed ? _currentXSpeed + xAccel : xSpeed;
        _velocity.x = input * (_currentXSpeed * newAirSpeedMod);
    }
    
    private void VerticalMovement()
    {
        if (Input.GetButton("Jump") == false) _hasJumped = false;

        if (_hasJumped || _currentCoyoteTime <= 0f) return;
        if (Input.GetButton("Jump"))
        {
            if (!_isJumping) onJump.Invoke();
            _isJumping = true;

            var newJump = _currentJumpPower + (1f / (jumpPower - _currentJumpPower) * jumpSpeed * Time.deltaTime);
            _currentJumpPower = Mathf.Clamp(newJump, initialJumpPower, jumpPower);
            _velocity.y = _currentJumpPower;
        }

        if (!_isJumping) return;
        if (Input.GetButton("Jump") && !(_currentJumpPower >= jumpPower)) return;
        _isJumping = false;
        _hasJumped = true;

        _currentCoyoteTime = 0f;
        _currentJumpPower = 0f;
        if (!gravityOnJump) _velocity.y = 0f;
    }
    
    private void ApplyGravity()
    {
        if (_isJumping) return;

        if (_isGrounded)
        {
            onGrounded.Invoke();
            
            _velocity.y = 0f;
            _currentCoyoteTime = coyoteTime;
            return;
        }

        var newVelocity = _velocity.y - gravity;
        if (_velocity.y < -gravityMax)
        {
            newVelocity = -gravityMax;
            _hasLanded = false;
        }

        _velocity.y = newVelocity;
        _currentCoyoteTime = Mathf.Clamp(_currentCoyoteTime - Time.deltaTime, 0f, coyoteTime);
    }
    
    private void Update()
    {
        var newGrounded = DetectGround();
        if (newGrounded && !_hasLanded) onLand.Invoke();

        _isGrounded = newGrounded;
        _isOnWall = DetectWalls();

        if (_isGrounded) _hasLanded = true;

        if (!_isGrounded || _isJumping || !(Input.GetAxisRaw("Vertical") < 0f)) return;
        _dropThrough = true;
        _dropThroughTimer = dropThroughTime;
        _velocity.y = -1f;
    }
    
    private bool DetectGround()
    {
        if (_dropThrough) return false;

        var rayLength = _coll.bounds.size.y / GROUND_RAY_DIVISION;
        var origin = transform.position;
        var halfWidth = Vector3.right * (_coll.bounds.size.x / 2f - 0.05f);
        
        if (RaycastDown(origin, _groundFilter, rayLength)) return true;
        if (RaycastDown(origin + halfWidth, _groundFilter, rayLength)) return true;
        if (RaycastDown(origin - halfWidth, _groundFilter, rayLength)) return true;

        if (!(_velocity.y <= 0f) || _dropThrough) return false;
        RaycastHit2D[] results = new RaycastHit2D[4];
        if (Physics2D.Raycast(origin, Vector2.down, _oneWayFilter, results, rayLength) <= 0) return false;
        var platformY = results[0].point.y;
        var feetY = transform.position.y - (_coll.bounds.size.y / 2f);
        return feetY >= platformY - 0.01f;
    }
    
    private bool RaycastDown(Vector3 origin, ContactFilter2D filter, float length)
    {
        var results = new RaycastHit2D[4];
        return Physics2D.Raycast(origin, Vector2.down, filter, results, length) > 0;
    }
    
    private bool DetectWalls()
    {
        var rayLength = _coll.bounds.size.x / WALL_RAY_DIVISION;
        var halfHeight = _coll.bounds.size.y / 2f;
        var dir = new Vector2(Mathf.Sign(_lastXInput), 0f);

        if (dir.x == 0f) return false;

        for (var i = 0; i < WALL_RAY_AMOUNT; i++)
        {
            var heightStep = (halfHeight * 2f) / (WALL_RAY_AMOUNT - 1);
            var yOffset = -halfHeight + (heightStep * i);
            var origin = new Vector2(transform.position.x, transform.position.y + yOffset);

            var results = new RaycastHit2D[4];
            if (Physics2D.Raycast(origin, dir, _groundFilter, results, rayLength) > 0)
                return true;
        }

        return false;
    }
    
    private void HandleOneWayCollision()
    {
        if (_playerLayerIndex < 0 || _oneWayLayerIndex < 0) return;

        var ignore = _velocity.y > 0f || _dropThrough;
        Physics2D.IgnoreLayerCollision(_playerLayerIndex, _oneWayLayerIndex, ignore);
    }
    
    private static int LayerMaskToFirstLayerIndex(LayerMask mask)
    {
        var maskVal = mask.value;
        if (maskVal == 0) return -1;
        for (var i = 0; i < 32; i++)
            if ((maskVal & (1 << i)) != 0)
                return i;
        return -1;
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (_coll == null) return;

        Gizmos.color = Color.green;
        var length = _coll.bounds.size.y / GROUND_RAY_DIVISION;
        var sideVector = new Vector3(_coll.bounds.size.x / 2, 0);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * length);
        Gizmos.DrawLine(transform.position + sideVector - GROUND_RAY_MARGIN,
            transform.position + sideVector - GROUND_RAY_MARGIN + Vector3.down * length);
        Gizmos.DrawLine(transform.position - sideVector + GROUND_RAY_MARGIN,
            transform.position - sideVector + GROUND_RAY_MARGIN + Vector3.down * length);

        Gizmos.color = Color.red;
        var wallLength = _coll.bounds.size.x / WALL_RAY_DIVISION;
        for (var rays = 0; rays < WALL_RAY_AMOUNT; rays++)
        {
            var halfHeight = _coll.bounds.size.y / 2;
            var pos = new Vector3(transform.position.x, transform.position.y - halfHeight + (halfHeight * rays));
            Gizmos.DrawLine(pos, pos + (new Vector3(Mathf.Floor(_lastXInput), 0) * wallLength));
        }
    }

    private void LateUpdate()
    {
        if (_isGrounded && !_hasLanded)
        {
            _hasLanded = true;
        }
    }
}