using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarUpdate : MonoBehaviour
{
    [Header("UI Control")]
    [SerializeField] private float _maxStamina = 100;
    [Space(2)] 
    
    [SerializeField] private GameObject _carTrail;
    [SerializeField] private ParticleSystem _trailPartcles;
    
    private TrailRenderer _carTrailRenderer;
    
    private float _currentStamina;

    [SerializeField] private float _staminaDecreaseMultiplier = 5;

    private Rigidbody2D _rigidbody;

    public float ReturnStamina() => (float)(_currentStamina / _maxStamina);

    private void Awake()
    {
        _carTrailRenderer = _carTrail.GetComponent<TrailRenderer>();
        
        _currentStamina = _maxStamina;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.TimeSlowed)
        {
            _currentStamina -= Time.unscaledDeltaTime * _staminaDecreaseMultiplier;
            
            _carTrail.SetActive(true);
            _trailPartcles.Play();
        }
        else
        {
            _carTrailRenderer.Clear();
            _carTrail.SetActive(false);
            _trailPartcles.Pause();
        }
    }

    public void RegainStamina()
    {
        _currentStamina = _maxStamina;
    }

    public void IncreaseStamina(int increment)
    {
        _currentStamina += increment;
        if (_currentStamina > _maxStamina) _currentStamina = _maxStamina;
    }
}
