using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CarController))]
public class CarAIHandler : MonoBehaviour
{
    private enum AIMode { followPlayer, followWaypoints, followMouse };

    private CarController _carController;

    [Header("AI Settings")]
    [SerializeField] private AIMode _mode;
    [Space(2)]

    [SerializeField] private float _maxSpeed = 13;
    [Space(2)]

    [SerializeField] private bool _isAvoiding = true;
    [Space(2)]

    [SerializeField] private WaypointNode _firstWaypoint;
    [Space(5)]

    private float _skillLevel;

    private Vector3 _targetPosition;
    private Transform _targetTransform;
    private float _originalMaxSpeed;

    private WaypointNode _currentWaypoint;

    private PolygonCollider2D _collider;

    private WaypointNode[] _allWaypoints;
    private GameObject _player;

    private void Awake()
    {
        _collider = GetComponentInChildren<PolygonCollider2D>();
        _carController = GetComponent<CarController>();
        _allWaypoints = FindObjectsOfType<WaypointNode>();

        _currentWaypoint = _firstWaypoint;

        _originalMaxSpeed = _maxSpeed;
    }

    private void Start()
    {
        if (_mode == AIMode.followPlayer)
            _player = FindObjectOfType<CarUpdate>().gameObject;
        
        _skillLevel = 1;
        SetMaximumSpeed(_maxSpeed);
    }

    private void FixedUpdate()
    {
        var input = Vector2.zero;

        switch (_mode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;
            case AIMode.followWaypoints:
                FollowWaypoints();
                break;
            case AIMode.followMouse:
                FollowMousePosition();
                break;
            default:
                break;
        }

        input.x = TurnToTarget();
        input.y = ApplyThrottleBrake(input.x);

        _carController.SetInput(input);
    }

    private void FollowWaypoints()
    {
        if (_currentWaypoint == null)
        {
            _currentWaypoint = FindClosestWaypoint();
        }

        if (_currentWaypoint == null) return;
        
        _targetPosition = _currentWaypoint.transform.position;

        var distanceToPoint = (_targetPosition - transform.position).magnitude;

        if (!(distanceToPoint <= _currentWaypoint.MinDistanceToWaypoint)) return;
            
        if (_currentWaypoint.MaxSpeed > 0)
            SetMaximumSpeed(_currentWaypoint.MaxSpeed);
        else
            SetMaximumSpeed(1000);

        _currentWaypoint = _currentWaypoint.NextWaypoint[Random.Range(0, _currentWaypoint.NextWaypoint.Length)];
    }

    private void FollowMousePosition()
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _targetPosition = worldPosition;
    }

    private WaypointNode FindClosestWaypoint()
    {
        return _allWaypoints.OrderBy(w => Vector3.Distance(transform.position, w.transform.position)).FirstOrDefault();
    }

    private void FollowPlayer()
    {
        _targetTransform = _player.transform;
        _targetPosition = _targetTransform.position;
    }

    private float TurnToTarget()
    {
        Vector2 toTarget = _targetPosition - transform.position;
        toTarget.Normalize();

        if (_isAvoiding)
            AvoidCars(toTarget, out toTarget);

        var angleToTarget = Vector2.SignedAngle(transform.up, toTarget);
        angleToTarget *= -1;

        var steerAmount = angleToTarget / 45;

        steerAmount = Mathf.Clamp(steerAmount, -1, 1);

        return steerAmount;
    }

    private float ApplyThrottleBrake(float inputX)
    {
        if (_carController.ReturnVelocity() > _maxSpeed)
            return 0;

        var reduceSpeed = Mathf.Abs(inputX) / 1;

        return 1.05f - reduceSpeed * _skillLevel;
    }

    private void SetMaximumSpeed(float newSpeed)
    {
        _maxSpeed = Mathf.Clamp(newSpeed, 0, _originalMaxSpeed);

        var skillBasedMaxSpeed = Mathf.Clamp(_skillLevel, .3f, 1);
        _maxSpeed = _maxSpeed * skillBasedMaxSpeed;
    }

    private bool IsCarsInFront(out Vector3 position, out Vector3 otherCarRightVector)
    {
        _collider.enabled = false;

        var raycast = Physics2D.CircleCast(transform.position + transform.up * .5f, .35f, transform.up, 12, 1 << LayerMask.NameToLayer("Car"));

        _collider.enabled = true;

        if (raycast.collider != null)
        {
            var transform2 = raycast.collider.transform;
            position = transform2.position;
            otherCarRightVector = transform2.right;

            return true;
        }

        position = Vector3.zero;
        otherCarRightVector = Vector3.zero;

        return false;
    }

    private void AvoidCars(Vector2 toTarget, out Vector2 newVectorToTarget)
    {
        if (IsCarsInFront(out Vector3 position, out Vector3 otherCarRightVector))
        {
            var avoidance = Vector2.zero;
            var avoidanceLerped = Vector2.zero;

            avoidance = Vector2.Reflect((position - transform.position).normalized, otherCarRightVector);

            var distanceToTarget = (_targetPosition - transform.position).magnitude;

            var driveToTargetInfluence = 6 / distanceToTarget;

            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, .3f, 1);

            var avoidanceInfluence = 1 - driveToTargetInfluence;

            avoidanceLerped = Vector2.Lerp(avoidanceLerped, avoidance, Time.fixedDeltaTime * 4);

            newVectorToTarget = toTarget * driveToTargetInfluence + avoidanceLerped * avoidanceInfluence;
            newVectorToTarget.Normalize();

            return;
        }
            
        newVectorToTarget = toTarget;
    }
}