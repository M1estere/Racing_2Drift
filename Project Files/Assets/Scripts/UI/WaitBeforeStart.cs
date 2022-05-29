using System.Collections;
using UnityEngine;
using TMPro;

public class WaitBeforeStart : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [Space(2)]
    
    [SerializeField] private GameObject UIWhole;
    [Space(2)]
    
    [SerializeField] private Stopwatch _stopwatch;

    private void Start()
    {
        GameManager.Instance.GameStarted = false;
        
        Time.timeScale = 0;
        
        StartCoroutine(WaitCor());
    }

    private IEnumerator WaitCor()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(OpenLevel(3));
    }

    private IEnumerator OpenLevel(int seconds)
    {
        while (seconds > 1)
        {
            seconds--;
            _text.text = seconds.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        Time.timeScale = 1;
        _stopwatch.enabled = true;
        UIWhole.gameObject.SetActive(false);
        GameManager.Instance.GameStarted = true;
    }
}
