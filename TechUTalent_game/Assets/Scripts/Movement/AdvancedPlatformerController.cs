using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Input = UnityEngine.Input;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class AdvancedPlatformerController : MonoBehaviour
{
    private Vector3 _velocity;
    private int _lastXInput;
    public bool lockMovement;
    
    private bool _isGrounded;
    private bool _isOnWall;
    private Vector3 GROUND_RAY_MARGIN = Vector3.right * 0.1f;
    private const float GROUND_RAY_DIVISION = 1.95f;
    private const float WALL_RAY_DIVISION = 1.8f;
    private const int WALL_RAY_AMOUNT = 3;
    
    [SerializeField] private string groundLayer = "Ground";
    [SerializeField] private string oneWayPlatformLayer = "OneWayPlatform";

    private ContactFilter2D _groundFilter;
    private ContactFilter2D _oneWayFilter;

    private float _currentXSpeed = 0f;
    [SerializeField] private float xSpeed = 10f;
    [SerializeField] private float xAccel = 1.25f;
    [SerializeField] private float airSpeedMod = 1.15f;

    [SerializeField] private float gravity = 1.2f;
    [SerializeField] private float gravityMax = 32f;

    private bool _isJumping;
    private bool _hasJumped;
    private float _currentCoyoteTime;
    private float _currentJumpPower;
    [SerializeField] private bool gravityOnJump = true;
    [SerializeField] private float jumpSpeed = 2048f;
    [SerializeField] private float jumpPower = 18f;
    [SerializeField] private float InitialJumpPower = 6f;
    [SerializeField] private float coyoteTime = 0.04f;

    private bool _hasLanded; // For landing event
    [SerializeField] private UnityEvent onJump;
    [SerializeField] private UnityEvent onLand;
    [SerializeField] private UnityEvent onStartMove;

    Rigidbody2D _rb;
    Collider2D _coll;
    ContactFilter2D _collisionFilter;
    
    private int _playerLayer;
    private int _oneWayLayer;
    
    [SerializeField] private float dropThroughTime = 0.25f;

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
        _groundFilter.SetLayerMask(LayerMask.GetMask(groundLayer));

        _oneWayFilter = new ContactFilter2D();
        _oneWayFilter.SetLayerMask(LayerMask.GetMask(oneWayPlatformLayer));
        
        _playerLayer = LayerMask.NameToLayer("Player");
        _oneWayLayer = LayerMask.NameToLayer(oneWayPlatformLayer);
    }


    // Frame based operations
    private void FixedUpdate()
    {
        HorizontalMovement();
        VerticalMovement();
        ApplyGravity();
        
        HandleOneWayCollision();
        
        if (_dropThrough)
        {
            _dropThroughTimer -= Time.deltaTime;
            if (_dropThroughTimer <= 0f)
                _dropThrough = false;
        }

        if (lockMovement) return;
        _rb.MovePosition(transform.position + _velocity * Time.deltaTime);
    }


    private void HorizontalMovement()
    {
        var input = Input.GetAxisRaw("Horizontal");
        var wallBlocked = _isOnWall == true && input == _lastXInput;
        if (input == 0 || wallBlocked)
        {
            _velocity.x = 0f;
            _currentXSpeed = 0f;
            return;  
        }

        if (_currentXSpeed == 0f && _isGrounded) onStartMove.Invoke(); 

        var newAirSpeedMod = 1f;
        if (_isGrounded == false) newAirSpeedMod = airSpeedMod;

        _lastXInput = (int)input;
        _currentXSpeed = _currentXSpeed < xSpeed ? _currentXSpeed + xAccel : xSpeed;
        _velocity.x = input * (_currentXSpeed * newAirSpeedMod);
    }

    private void VerticalMovement()
    {
        if (Input.GetButton("Jump") == false) _hasJumped = false; // Reset jump

        if (_hasJumped || _currentCoyoteTime <= 0) return;
        if (Input.GetButton("Jump"))
        {
            if (_isJumping == false) onJump.Invoke();
            _isJumping = true;

            var newJump = _currentJumpPower + (1 / (jumpPower - _currentJumpPower) * jumpSpeed * Time.deltaTime);
            _currentJumpPower = Mathf.Clamp(newJump, InitialJumpPower, jumpPower);
            _velocity.y = _currentJumpPower;
        }

        if (_isJumping == false) return;
        if (Input.GetButton("Jump") == false || _currentJumpPower >= jumpPower)
        {
            _isJumping = false;
            _hasJumped = true;

            _currentCoyoteTime = 0f;
            _currentJumpPower = 0f;
            if (gravityOnJump == false) _velocity.y = 0f;
        }
    }

    private void ApplyGravity()
    {
        if (_isJumping) return;
        if (_isGrounded)
        {
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
        _currentCoyoteTime = Mathf.Clamp(_currentCoyoteTime - Time.deltaTime, 0, coyoteTime);
    }



    // Always updating
    private void Update()
    {
        var newGrounded = DetectGround();
        if (newGrounded && _hasLanded == false) onLand.Invoke();

        _isGrounded = newGrounded;
        _isOnWall = DetectWalls();
        if (_isGrounded == true) _hasLanded = true;
        
        if (_isGrounded && !_isJumping && Input.GetAxisRaw("Vertical") < 0)
        {
            _dropThrough = true;
            _dropThroughTimer = dropThroughTime;

            // Force separation
            _velocity.y = -1f;
        }
    }

    
    private bool DetectGround()
    {
        if (_dropThrough)
            return false;
        RaycastHit2D[] results = new RaycastHit2D[10];
        float rayLength = _coll.bounds.size.y / GROUND_RAY_DIVISION;

        Vector3 origin = transform.position;
        Vector3 halfWidth = Vector3.right * (_coll.bounds.size.x / 2f - 0.05f);

        // ---- SOLID GROUND ----
        if (Physics2D.Raycast(origin, Vector2.down, _groundFilter, results, rayLength) > 0)
            return true;

        if (Physics2D.Raycast(origin + halfWidth, Vector2.down, _groundFilter, results, rayLength) > 0)
            return true;

        if (Physics2D.Raycast(origin - halfWidth, Vector2.down, _groundFilter, results, rayLength) > 0)
            return true;

        // ---- ONE-WAY PLATFORM (only when falling) ----
        if (_velocity.y <= 0f)
        {
            if (Physics2D.Raycast(origin, Vector2.down, _oneWayFilter, results, rayLength) > 0)
            {
                float platformY = results[0].point.y;
                float feetY = transform.position.y - (_coll.bounds.size.y / 2f);

                if (feetY >= platformY - 0.01f)
                    return true;
            }
        }

        return false;
    }

    private bool DetectWalls()
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        float rayLength = _coll.bounds.size.x / WALL_RAY_DIVISION;

        float halfHeight = _coll.bounds.size.y / 2f;
        Vector2 dir = new Vector2(Mathf.Sign(_lastXInput), 0f);

        if (dir.x == 0f)
            return false;

        // Cast 3 rays vertically along the body
        for (int i = 0; i < WALL_RAY_AMOUNT; i++)
        {
            float heightStep = (halfHeight * 2f) / (WALL_RAY_AMOUNT - 1);
            float yOffset = -halfHeight + (heightStep * i);

            Vector2 origin = new Vector2(transform.position.x, transform.position.y + yOffset);

            if (Physics2D.Raycast(origin, dir, _groundFilter, results, rayLength) > 0)
                return true;
        }

        return false;
    }
    
    private void HandleOneWayCollision()
    {
        bool ignore = _velocity.y > 0f || _dropThrough;
        Physics2D.IgnoreLayerCollision(_playerLayer, _oneWayLayer, ignore);
    }

    
    // Debugging
    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode == false) return;

        // Draw visual raycasts
        Gizmos.color = Color.green;
        var length = _coll.bounds.size.y / GROUND_RAY_DIVISION;
        var sideVector = new Vector3(_coll.bounds.size.x / 2, 0);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * length);
        Gizmos.DrawLine(transform.position + sideVector - GROUND_RAY_MARGIN, transform.position + sideVector - GROUND_RAY_MARGIN + Vector3.down * length);
        Gizmos.DrawLine(transform.position - sideVector + GROUND_RAY_MARGIN, transform.position - sideVector + GROUND_RAY_MARGIN + Vector3.down * length);

        Gizmos.color = Color.red;
        var wallLength = _coll.bounds.size.x / WALL_RAY_DIVISION;
        for (int rays = 0; rays < WALL_RAY_AMOUNT; rays++)
        {
            var halfHeight = _coll.bounds.size.y / 2;
            var pos = new Vector3(transform.position.x, transform.position.y - halfHeight + (halfHeight * rays));
            Gizmos.DrawLine(pos, pos + (new Vector3(Mathf.Floor(_lastXInput), 0) * wallLength));
        }

        // Draw Coyote time
        if (_currentCoyoteTime == coyoteTime || _currentCoyoteTime == 0) return;
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _coll.bounds.size.y);
    }
}
