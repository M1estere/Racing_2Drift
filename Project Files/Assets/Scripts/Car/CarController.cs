using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController : MonoBehaviour
{

    [Header("Car Params")]
    [SerializeField] private float _driftFactor = .95f;
    [Space(2)]

    [SerializeField] private float _accelerationFactor = 30;
    [Space(2)]

    [SerializeField] private float _turnFactor = 3.5f;
    [Space(2)]

    [SerializeField] private float _maxSpeed = 15;

    private float _accelerationInput;
    private float _steerInput;

    private float _rotationAngle;

    private float _velocityUpRes;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        if (gameObject.CompareTag("AI"))
            _accelerationFactor = PlayerPrefs.GetFloat("Difficulty") * 6f;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        DestroyOrthVelocity();
        ApplySteering();
    }

    private void ApplyEngineForce()
    {
        _velocityUpRes = Vector2.Dot(transform.up, _rigidbody.velocity);

        if (_velocityUpRes > _maxSpeed && _accelerationInput > 0)
            return;

        if (_velocityUpRes < -_maxSpeed * .5f && _accelerationInput < 0)
            return;

        if (_rigidbody.velocity.sqrMagnitude > _maxSpeed * _maxSpeed && _accelerationInput > 0)
            return;

        if (_accelerationInput == 0)
            _rigidbody.drag = Mathf.Lerp(_rigidbody.drag, 3, Time.fixedDeltaTime * 3);
        else
            _rigidbody.drag = 0;

        Vector2 engineForceVector = transform.up * (_accelerationInput * _accelerationFactor);

        _rigidbody.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        float minSpeedBefore = _rigidbody.velocity.magnitude / 4;
        minSpeedBefore = Mathf.Clamp01(minSpeedBefore);

        // add if slow motioned
        _rotationAngle -= _steerInput * _turnFactor * minSpeedBefore;

        _rigidbody.MoveRotation(_rotationAngle);
    }

    private void DestroyOrthVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rigidbody.velocity, transform.right);

        _rigidbody.velocity = forwardVelocity + rightVelocity * _driftFactor;
    }

    private float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, _rigidbody.velocity);
    }

    public bool IsScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (_accelerationInput < 0 && _velocityUpRes > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4f)
            return true;

        return false;
    }

    public void SetInput(Vector2 input)
    {
        _steerInput = input.x;
        _accelerationInput = input.y;
    }

    public float ReturnVelocity()
    {
        return _rigidbody.velocity.magnitude;
    }
}
