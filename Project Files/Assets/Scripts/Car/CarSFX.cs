using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(CarController))]
public class CarSFX : MonoBehaviour
{
    [SerializeField] private float _audioSlowDownFactor = .1f;
    [Space(5)]

    [Header("Mixers")]
    [SerializeField] private AudioMixer _audioMixer;
    [Space(5)]

    [Header("Effects")]
    [SerializeField] private AudioSource _tiresScreeching;
    [Space(2)]

    [SerializeField] private AudioSource _engine;
    [Space(2)]

    [SerializeField] private AudioSource _carHit;
    [Space(5)]

    [SerializeField] private bool _playerCar = true;

    private CarController _carController;

    private float _desiredEnginePitch = .5f;
    private float _tiresScreechingPitch = .5f;

    private void Awake()
    {
        _carController = GetComponent<CarController>();
    }

    private void Start()
    {
        _audioMixer.SetFloat("SFXVolume", -10f);
    }

    private void Update()
    {
        if (_playerCar)
        {
            if (GameManager.Instance.TimeSlowed)
            {
                _audioMixer.SetFloat("BGMusicVolume", -20f);
                _audioMixer.SetFloat("SFXPitch", _audioSlowDownFactor);
            } else
            {
                _audioMixer.SetFloat("BGMusicVolume", 2f);
                _audioMixer.SetFloat("SFXPitch", 1f);
            }
        }
        UpdateEngineSFX();
        UpdateTiresSFX();
    }

    private void UpdateEngineSFX()
    {
        float velocity = _carController.ReturnVelocity();

        float desiredEngineVolume = velocity * 0.05f;

        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, .2f, 1);

        _engine.volume = Mathf.Lerp(_engine.volume, desiredEngineVolume, Time.deltaTime * 10);

        _desiredEnginePitch = velocity * .2f;
        _desiredEnginePitch = Mathf.Clamp(_desiredEnginePitch, .5f, 2f);
        _engine.pitch = Mathf.Lerp(_engine.pitch, _desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    private void UpdateTiresSFX()
    {
        if (_carController.IsScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
            {
                _tiresScreeching.volume = Mathf.Lerp(_tiresScreeching.volume, 1, Time.deltaTime * 10);
                _tiresScreechingPitch = Mathf.Lerp(_tiresScreechingPitch, .5f, Time.deltaTime * 10);
            } else
            {
                _tiresScreeching.volume = Mathf.Abs(lateralVelocity) * .05f;
                _tiresScreechingPitch = Mathf.Abs(lateralVelocity) * .1f;
            }
        } else
            _tiresScreeching.volume = Mathf.Lerp(_tiresScreeching.volume, 0, Time.deltaTime * 10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float relativeVelocity = collision.relativeVelocity.magnitude;

        float volume = relativeVelocity * .1f;

        _carHit.pitch = Random.Range(.95f, 1.05f);
        _carHit.volume = volume;

        if (GameManager.Instance != null && _playerCar)
            GameManager.Instance.ShakeCamera();

        if (!_carHit.isPlaying)
            _carHit.Play();
    }
}
