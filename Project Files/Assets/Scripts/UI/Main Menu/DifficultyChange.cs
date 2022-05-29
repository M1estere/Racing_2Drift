using UnityEngine;
using UnityEngine.UI;

public class DifficultyChange : MonoBehaviour
{
    [SerializeField] private Toggle[] _toggles;

    private void Start()
    {
        var value = PlayerPrefs.GetFloat("Difficulty");

        switch (value)
        {
            case 2f:
                _toggles[2].isOn = true;
                break;
            case 1.5f:
                _toggles[1].isOn = true;
                break;
            case 1:
                _toggles[0].isOn = true;
                break;
        }
    }
    
    private void Update()
    {
        if (_toggles[0].isOn) // easy
            PlayerPrefs.SetFloat("Difficulty", 1f);

        if (_toggles[1].isOn) // medium
            PlayerPrefs.SetFloat("Difficulty", 1.5f);

        if (_toggles[2].isOn) // hard
            PlayerPrefs.SetFloat("Difficulty", 2f);
    }
}
