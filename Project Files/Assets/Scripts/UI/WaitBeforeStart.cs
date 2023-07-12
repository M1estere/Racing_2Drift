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
    [Space(5)]
    
    [SerializeField] private AudioSource _source;
    
    private void Start()
    {
        GameManager.Instance.GameStarted = false;
        
        Time.timeScale = 0;
        
        StartCoroutine(SoundCor());
        StartCoroutine(WaitCor());
    }

    private IEnumerator SoundCor()
    {
        yield return new WaitForSecondsRealtime(.75f);
        _source.Play();
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

        _text.text = "Go";

        yield return new WaitForSecondsRealtime(.5f);
        
        Time.timeScale = 1;
        _stopwatch.enabled = true;
        UIWhole.gameObject.SetActive(false);
        
        GameManager.Instance.GameStarted = true;
        
        _text.text = "";
    }
}
