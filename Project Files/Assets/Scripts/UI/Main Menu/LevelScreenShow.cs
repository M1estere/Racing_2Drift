using UnityEngine;
using System.Collections;

public class LevelScreenShow : MonoBehaviour
{
    [SerializeField] private GameObject _levelsScreen;
    [Space(2)]

    [SerializeField] private Transform _levelsScreenTransform;
    [Space(5)]

    [Header("Camera Props")]
    [SerializeField] private GameObject _camera;
    [Space(2)]

    [SerializeField] private float _speed;
    [Space(2)]

    [SerializeField] private float _newSize;
    [Space(5)]

    [SerializeField] private InputHandler _handler;

    private Camera _cameraMain;

    private CameraFollow _follow;

    private int i = 0;

    private Transform _targetPosition;

    private bool _canMove;

    private void Awake()
    {
        _cameraMain = _camera.GetComponent<Camera>();
        _follow = _camera.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && i == 0)
        {
            _handler.enabled = false;

            _follow.enabled = false;

            i++;

            DoTransform(_levelsScreenTransform);

            _levelsScreen.SetActive(true);
        }
    }

    private void Update()
    {
        if (_canMove)
        {
            _cameraMain.orthographicSize = Mathf.Lerp(_cameraMain.orthographicSize, _newSize, Time.unscaledDeltaTime * 6);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPosition.position, _speed * Time.deltaTime);
        }
        if (_targetPosition != null)
        {
            if (_camera.transform.position == _targetPosition.position)
            {
                _canMove = false;
            }
        }
    }

    private void DoTransform(Transform newTransform)
    {
        _targetPosition = newTransform;
        _canMove = true;
    }

    public void ReturnAll()
    {
        i--;
        _canMove = false;

        _levelsScreen.SetActive(false);
        
        _follow.enabled = true;

        _handler.enabled = true;
    }
}
