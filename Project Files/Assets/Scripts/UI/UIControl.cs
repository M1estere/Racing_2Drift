using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class UIControl : MonoBehaviour
{
    #region Variables
    [Header("Pause")]
    [SerializeField] private GameObject _pauseMenu;
    [Space(2)]

    [SerializeField] private Animator _pauseAnimator;
    [Space(5)]

    [Header("Main")]
    [SerializeField] private TMPro.TMP_Text _lapsText;
    [Space(5)]

    [SerializeField] private GameObject _finishUI;
    [Space(2)]

    [SerializeField] private TMPro.TMP_Text _winOrLoseText;
    [Space(2)]

    [SerializeField] private GameObject _continueButton;
    [Space(5)]

    [Header("Pause Music Control")]
    [SerializeField] private AudioMixer _default;

    [SerializeField] private SceneFader _fader;

    [SerializeField] private string _nextLevelName;
    
    private float _defaultMasterVolume;
    private float _defaultSFXVolume;

    private int _finishShower = 0;
    #endregion
    
    private void Start()
    {
        _default.GetFloat("MasterVolume", out _defaultMasterVolume);
        _default.GetFloat("SFXVolume", out _defaultSFXVolume);
    }
    
    private void Update()
    {
        bool finish = GameManager.Instance.Finish;

        int totalLaps = GameManager.Instance.TotalLaps;
        int currentLaps = GameManager.Instance.CurrentLaps;

        if (currentLaps <= totalLaps)
            _lapsText.SetText("Lap " + currentLaps + "/" + totalLaps);

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.GameStarted)
        {
            OpenPause();
        }

        if (finish)
        {
            if (_finishShower == 0)
            {
                if (GameManager.Instance.Win)
                {
                    ShowFinish("You win");
                    _finishShower++;
                } else
                {
                    ShowFinish("You lose");
                    _finishShower++;
                }
            }
        }
    }

    private void MuteSound()
    {
        _default.SetFloat("SFXVolume", -80);
        _default.SetFloat("MasterVolume", -25);
    }

    private void UnmuteSound()
    {
        _default.SetFloat("SFXVolume", _defaultSFXVolume);
        _default.SetFloat("MasterVolume", _defaultMasterVolume);
    }

    private void ShowFinish(string title)
    {
        MuteSound();
        
        _winOrLoseText.SetText(title);
        _continueButton.SetActive(GameManager.Instance.Win);

        _finishUI.SetActive(true);
    }

    public void OpenPause()
    {
        if (GameManager.Instance.TimeSlowed) return;
        
        MuteSound();

        GameManager.Instance.PauseTime();
        GameManager.Instance.GamePaused = true;

        _pauseMenu.SetActive(true);
    }

    public void ClosePause()
    {
        StartCoroutine(WaitForClose());
    }

    public void RestartLevel()
    {
        UnmuteSound();
        
        GameManager.Instance.GamePaused = false;

        _fader.FadeTo(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        _pauseMenu.SetActive(false);
    }

    public void Exit()
    {
        UnmuteSound();
        
        _fader.FadeTo("MainMenu");
    }

    public void ContinueGame()
    {
        UnmuteSound();
        
        _fader.FadeTo(_nextLevelName);
    }
    
    private IEnumerator WaitForClose()
    {
        _pauseAnimator.SetTrigger("Close");

        yield return new WaitForSecondsRealtime(.5f);

        GameManager.Instance.GamePaused = false;
        GameManager.Instance.UnpauseTime();

        UnmuteSound();

        _pauseMenu.SetActive(false);
    }
}
