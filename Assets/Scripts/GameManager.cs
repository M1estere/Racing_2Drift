using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject _camera;
    private Animator _cameraAnimator;

    [SerializeField] private int _nextLevelID = 2;

    [Header("Time Control")]
    [SerializeField] private CarUpdate _carUpdate;
    [Space(2)]

    [SerializeField] private KeyCode _keyCode;
    [Space(2)]

    [SerializeField] private float _slowDownFactor = .05f;
    [Space(2)]

    [Header("Post-Processing Control")]
    [SerializeField] private Volume _volume;
    [Space(2)]

    [SerializeField] private float _lerpSpeed = .2f;
    [Space(2)]

    [SerializeField] private CarLapCounter _carLapCounter;

    [HideInInspector] public bool TimeSlowed = false;

    [HideInInspector] public bool GamePaused = false;

    [HideInInspector] public int TotalLaps;

    [HideInInspector] public int CurrentLaps;

    [HideInInspector] public bool Win = false;

    [HideInInspector] public bool Finish = false;

    [HideInInspector] public bool GameStarted = false;
    
    [HideInInspector] public bool CanSlow = false;
    
    private Vignette _vignette;

    private int _carPosition;

    private void Awake()
    {
        Instance = this;

        _camera = Camera.main.gameObject;
        _cameraAnimator = _camera.GetComponent<Animator>();
    }

    public void ShakeCamera()
    {
        _cameraAnimator.SetTrigger("Shake");
    }

    private void Start()
    {
        if(_volume.profile.TryGet<Vignette>( out var tmp ) )
        {
            _vignette = tmp;
        }
    }

    private void Update()
    {
        _carPosition = _carLapCounter.GetCarPosition();

        CurrentLaps = _carLapCounter.GetLapsCompleted();
        TotalLaps = _carLapCounter.GetAllLaps();

        if (CurrentLaps >= TotalLaps + 1)
        {
            Finish = true;
            FinishLevel();

            if (_carPosition == 1)
            {
                Win = true;
                if (PlayerPrefs.GetInt("LevelReached") < _nextLevelID) 
                    PlayerPrefs.SetInt("LevelReached", _nextLevelID);
            } else
            {
                Win = false;
            }
        }

        // CanSlow = Input.GetKey(_keyCode);

        if (CanSlow == true) DoSlowMotion();
        else BreakSlowMotion();
    }

    private void FinishLevel()
    {
        Time.timeScale = 0;
    }

    private void DoSlowMotion()
    {
        if (_carUpdate.ReturnStamina() <= 0 || GamePaused || Finish || GameStarted == false)
        {
            BreakSlowMotion();
            return;
        }
            
        TimeSlowed = true;

        Time.timeScale = _slowDownFactor;

        _lerpSpeed += Time.deltaTime * 35;

        // change post processing
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, .35f, _lerpSpeed);

        _lerpSpeed = 0;
    }

    private void BreakSlowMotion()
    {
        if (GamePaused || Finish || GameStarted == false) return;
        
        TimeSlowed = false;

        Time.timeScale = 1;

        _lerpSpeed += Time.deltaTime * 2;

        // change post processing
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 0, _lerpSpeed);

        _lerpSpeed = 0;
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void UnpauseTime()
    {
        Time.timeScale = 1;
    }
}
