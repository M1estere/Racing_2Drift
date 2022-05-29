using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarUpdate : MonoBehaviour
{
    [Header("UI Control")]
    [SerializeField] private float _maxStamina = 100;
    [Space(2)]

    private float _currentStamina;

    [SerializeField] private float _staminaDecreaseMultiplier = 5;

    private Rigidbody2D _rigidbody;

    public float ReturnStamina()
    {
        return (float)(_currentStamina / _maxStamina);
    }

    private void Awake()
    {
        _currentStamina = _maxStamina;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.TimeSlowed)
        {
            _currentStamina -= Time.unscaledDeltaTime * _staminaDecreaseMultiplier;
        }
    }

    public void RegainStamina()
    {
        _currentStamina = _maxStamina;
    }
}
