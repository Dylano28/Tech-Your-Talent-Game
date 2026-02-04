using System;
using UnityEngine;

public enum WaypointLoopMode
{
    None,
    LoopV1,
    LoopV2
}

public class MovingPlatform : MonoBehaviour
{
    [Header("Waypoint")]
    [SerializeField] private WaypointPath waypointPath;
    [SerializeField] private float speed = 2f;

    [Header("Behaviour")]
    [SerializeField] private WaypointLoopMode loopMode = WaypointLoopMode.LoopV2;
    [SerializeField] private bool moveOnlyWhenPlayerOnPlatform;

    private int _currentIndex;
    private int _direction = 1;
    private bool _isActive = true;
    private bool _playerOnPlatform;

    private void Awake()
    {
        if (waypointPath == null)
            waypointPath = GetComponent<WaypointPath>();

        if (waypointPath == null || waypointPath.Length == 0)
        {
            Debug.LogError("MovingPlatform needs a WaypointPath with points.", this);
            enabled = false;
            return;
        }

        // Start at first waypoint position
        transform.position = waypointPath.GetPoint(0);
    }

    private void Update()
    {
        if (!_isActive) return;
        if (moveOnlyWhenPlayerOnPlatform && !_playerOnPlatform) return;

        MovePlatform();
    }

    private void MovePlatform()
    {
        Vector3 target = waypointPath.GetPoint(_currentIndex);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.SqrMagnitude(transform.position - target) > 0.0001f)
            return;

        transform.position = target;
        AdvanceIndex();
    }

    private void AdvanceIndex()
    {
        _currentIndex += _direction;

        if (_currentIndex >= 0 && _currentIndex < waypointPath.Length)
            return;

        switch (loopMode)
        {
            case WaypointLoopMode.None:
                _isActive = false;
                _currentIndex = Mathf.Clamp(_currentIndex, 0, waypointPath.Length - 1);
                break;

            case WaypointLoopMode.LoopV1:
                _currentIndex = _direction > 0 ? 0 : waypointPath.Length - 1;
                break;

            case WaypointLoopMode.LoopV2:
                _direction *= -1;
                _currentIndex += _direction * 2;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region Player Collision

    private void OnCollisionEnter(Collision collision)
    {
        if (!moveOnlyWhenPlayerOnPlatform) return;
        if (!collision.collider.CompareTag("Player")) return;

        _playerOnPlatform = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!moveOnlyWhenPlayerOnPlatform) return;
        if (!collision.collider.CompareTag("Player")) return;

        _playerOnPlatform = false;
    }

    #endregion
}