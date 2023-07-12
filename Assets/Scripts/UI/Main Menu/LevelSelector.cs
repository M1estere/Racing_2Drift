using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button[] _levelButtons;

    [SerializeField] private SceneFader _fader;
    
    private void Start()
    {
        Time.timeScale = 1;
        int _levelReached = PlayerPrefs.GetInt("LevelReached"); // getting the last level from playerPrefs

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            if (i + 1 > _levelReached)
            {
                _levelButtons[i].interactable = false;
            }
        }
        if (_levelReached == 0)
            _levelButtons[0].interactable = true;
    }

    public void Select(string levelName)
    {
        _fader.FadeTo(levelName);
    }
}