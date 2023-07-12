using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WheelParticleHandle : MonoBehaviour
{
    [SerializeField] private CarController _carController;

    private float _emissionRate = 0;

    private ParticleSystem _particleSystem;

    private ParticleSystem.EmissionModule _particleSystemEmissionModule;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        _particleSystemEmissionModule = _particleSystem.emission;

        _particleSystemEmissionModule.rateOverTime = 0;
    }

    private void Update()
    {
        _emissionRate = Mathf.Lerp(_emissionRate, 0, Time.deltaTime * 5);
        _particleSystemEmissionModule.rateOverTime = _emissionRate;

        if (_carController.IsScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
                _emissionRate = 30;
            else
                _emissionRate = Mathf.Abs(lateralVelocity) * 2;
        }
    }
}
