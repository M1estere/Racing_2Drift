using UnityEngine;
using System.Collections;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    public float MyTime;
    
    private float _msec;
    private float _sec;
    private float _min;

    private void Start() 
    {
        StartCoroutine("StopWatch");
    }
    

    private IEnumerator StopWatch()
    {
        while(true)
        {
            MyTime += Time.deltaTime;
            _msec = (int)((MyTime - (int)MyTime) * 100);
            _sec = (int)(MyTime % 60);
            _min = (int)(MyTime / 60 % 60);

            _timer.SetText(_min.ToString("00") + ":" + _sec.ToString("00") + ":" + _msec.ToString("00"));

            yield return null;
        }
    }
}
