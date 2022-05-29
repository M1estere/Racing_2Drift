using System;
using UnityEngine;

public class CarLapCounter : MonoBehaviour
{
    [SerializeField] private int _lapsToComplete;
    [Space(5)]

    private int _lapsCompleted;

    private int _passedCheckpoint;
    private float _timeAtLastCheckpoint;

    private int _passedCheckpointsAmount;

    private bool _raceCompleted = false;

    private int _carPosition;

    public event Action<CarLapCounter> OnPassCheckpoint;

    #region Returners
    public void SetCarPosition(int position)
    {
        _carPosition = position;
    }

    public int GetNumberOfPassedPoints()
    {
        return _passedCheckpointsAmount;
    }

    public float GetTimeAtChekpoint()
    {
        return _timeAtLastCheckpoint;
    }

    public int GetLapsCompleted()
    {
        return _lapsCompleted + 1;
    }

    public int GetAllLaps()
    {
        return _lapsToComplete;
    }

    public int GetCarPosition()
    {
        return _carPosition;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Checkpoint")) return;
        
        if (_raceCompleted) return;

        Checkpoint point = collider.gameObject.GetComponent<Checkpoint>();

        if (_passedCheckpoint + 1 == point.CheckPointNumber)
        {
            _passedCheckpoint = point.CheckPointNumber;

            _passedCheckpointsAmount++;

            _timeAtLastCheckpoint = Time.time;

            if (point.IsFinishLine)
            {
                _passedCheckpoint = 0;
                _lapsCompleted++;

                if (_lapsCompleted >= _lapsToComplete)
                    _raceCompleted = true;
            }

            OnPassCheckpoint?.Invoke(this);
        }
    }
}
