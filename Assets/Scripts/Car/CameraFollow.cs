using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [Space(2)]

    [SerializeField] private Vector3 _offset;
    [Space(2)]

    [SerializeField] private CarController _carController;
    [Space(5)]

    [Header("Camera Active")]
    [SerializeField] private float _minSize = 6;
    [Space(2)]

    [SerializeField] private float _maxSize = 9;
    [Space(2)]

    [SerializeField] private float _multiplier = 4;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        float carSpeed = _carController.ReturnVelocity();

        transform.position = Vector3.Lerp(transform.position, _playerTransform.position + _offset, Time.unscaledDeltaTime * _multiplier);

        _camera.orthographicSize = Mathf.Lerp(_minSize, _maxSize, (carSpeed * 4.5f) / 100f);
    }
}
