using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class WheelSkidmarksHandle : MonoBehaviour
{
    [SerializeField] private CarController _carController;

    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();

        _trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (_carController.IsScreeching(out float lateralVelocity, out bool isBraking))
            _trailRenderer.emitting = true;
        else
            _trailRenderer.emitting = false;
    }
}
