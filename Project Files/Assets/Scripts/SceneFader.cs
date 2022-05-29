using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve; // to control the fading

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0)
        {
            t -= Time.unscaledDeltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color (0f, 0f, 0f, a); // setting fading in
            yield return 0; // skip to the next frame
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color (0f, 0f, 0f, a); // setting fading in
            yield return 0; // skip to the next frame
        }

        SceneManager.LoadScene(scene);
    }

}
