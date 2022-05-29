using UnityEngine;

public class MenuStart : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    [SerializeField] private SceneFader _fader;
    
    public void StartMenu()
    {
        _fader.FadeTo(_sceneName);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
