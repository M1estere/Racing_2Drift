using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    [SerializeField] private List<CarLapCounter> _counters = new List<CarLapCounter>();

    private void Start()
    {
        CarLapCounter[] counters = FindObjectsOfType<CarLapCounter>();

        _counters = counters.ToList<CarLapCounter>(); 

        foreach (CarLapCounter lapCounter in _counters)
            lapCounter.OnPassCheckpoint += OnPassCheckpoint;
    }

    private void OnPassCheckpoint(CarLapCounter counter)
    {
        _counters = _counters.OrderByDescending(c => c.GetNumberOfPassedPoints()).ThenBy(c => c.GetTimeAtChekpoint()).ToList(); // sort on chekpoints and time

        int carPosition = _counters.IndexOf(counter) + 1;

        counter.SetCarPosition(carPosition);
    }
}
